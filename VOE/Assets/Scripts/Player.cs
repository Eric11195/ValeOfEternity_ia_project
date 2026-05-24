using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using voe;
using static UnityEditor.Progress;
using static voe.DecisionParameters;

namespace voe{
    public class Player
    {
        static int threshold = 60;
        
        public int idx;
        public int my_points = 0;
        public bool hand_representation_needs_update = true;
        public bool table_need_update = false;
        public CardList hand;
        public CardList table;
        public CardList chosen_at_market;
        public StoneManager stone_manager;

        //This stores the card which it can currently select from
        public CardList current_card_pool_option;

        public UnityAction<Player, stone_quant> pay_for_card_event;
        public UnityAction<Player, CardFamily> tame_card_event;
        public UnityAction<Player> card_enters_tableau_event;

        public int[] card_reduction_cost_by_family = new int[6] {0,0,0,0,0,0};

        public CardNameId favourite = CardNameId.NONE;
        public priorities player_prio = priorities.play_favourite;

        //COUNT IS SUM, COUNT+1 is best
        public List<int> enabler_ratings;
        //COUNT IS SUM, COUNT+1 is best
        public List<int> payoffs_ratings;

        public stats my_stats;

        int stones_gained_since_last_round;

        public Player()
        {
            hand = new CardList();
            table = new CardList();
            chosen_at_market = new CardList();
            stone_manager = new StoneManager();

            enabler_ratings = new List<int>(0);
            payoffs_ratings = new List<int>(0);
            for (int i = 0; i < (int)card_flags_idx.BEST + 1; ++i)
            {
                enabler_ratings.Add(0);
                payoffs_ratings.Add(0);
            }
            
            my_stats = stats.init_base_stats();
        }

        public stats retrieve_stats()
        {
            var gm = GameManager.get_instance();

            my_stats.cards_in_hand = hand.size();
            my_stats.stones_per_round /= ((float)gm.get_round());
            my_stats.mid_stones_at_end_of_round /= ((float)gm.get_round());
            my_stats.mid_stones_value_at_end_of_round /= ((float)gm.get_round());
            return my_stats;
        }

        private int get_cheapest_cost_of_card_in_hand()
        {
            throw new UnityException("unimplemented");
            return 0;
        }

        public void choose_favourite_card()
        {
            favourite = choose_best_card_in_hand(CardFamily.None, CardEffectTypes.none, (int cost)=>{ return true; }, CardNameId.NONE);
        }

        public bool has_card_with_requiriment(CardList cl, CardFamily cf, CardEffectTypes cet, cost_precondition cp)
        {
            foreach (CardNameId cni in cl.card_list)
            {
                if(DecisionParameters.check_conditions(cni, cf, cet, cp)) return true;
            }
            return false;
        }

        public bool is_playable(CardNameId cni)
        {
            if (cni == CardNameId.NONE) return false;
            var cd = CardData.get_card(cni);
            return can_pay(cd.price, cd.family) && cd.can_be_played(this);
        }
        public int free_slots_on_table()
        {
            GameManager gm = GameManager.get_instance();
            return gm.get_round() - table.size();
        }
        private bool cannot_play_cards()
        {
            if (GameManager.get_instance().get_round() <= table.size() || hand.empty()) return true;

            foreach (var card in hand.card_list)
            {
                if(is_playable(card)) return false;
            }
            return true;
        }
        private bool cannot_play_card_from_market()
        {
            if (GameManager.get_instance().get_round() <= table.size() || chosen_at_market.empty()) return true;

            foreach (var card in chosen_at_market.card_list)
            {
                if(is_playable(card)) return false;
            }
            return true;
        }
        private bool turn_can_go_on()
        {
            return is_playable(favourite) || !chosen_at_market.empty();
        }
        public void choose_priority()
        {
            GameManager gm = GameManager.get_instance();
            //var cd = CardData.get_card(favourite);
            if(favourite!=CardNameId.NONE && is_playable(favourite))
            {
                player_prio = priorities.play_favourite;
            }
            else
            {
                if (cannot_play_card_from_market())
                {
                    if (stone_manager.get_number_of_spaces_to_fill() > 0)
                    {
                        player_prio = priorities.store_stones;
                    }
                    else
                    {
                        player_prio = priorities.gain_points;
                    }
                }
                else
                {
                    player_prio = priorities.take_playable_card;
                }
            }
        }

        public IEnumerator choose_card_on_market()
        {
            Assert.IsTrue(current_card_pool_option.size() > 0);

            CardNameId cni = choose_best_card(this, current_card_pool_option, CardFamily.None, CardEffectTypes.none, (int cost) => { return true; }, player_prio, true);//current_card_pool_option.get(0);
            Logger.Log(Logger.player_log(idx, "chooses " + AssetDataBase.get_card_file_name(cni)), TextFilter.get_p_idx_message_src(idx));
            yield return cni;
        }
        public IEnumerator play_turn()
        {
            Logger.LogH3(Logger.player_log(idx,"turn:"), TextFilter.get_p_idx_message_src(idx));
            var gm = GameManager.get_instance();
            while (turn_can_go_on())
            {
                choose_priority();
                switch (player_prio)
                {
                    case priorities.gain_points:
                        yield return gm.StartCoroutine(gain_points_turn_action());
                        break;
                    case priorities.take_playable_card:
                        yield return gm.StartCoroutine(take_playable_card_turn_action());
                        break;
                    case priorities.play_favourite:
                        yield return gm.StartCoroutine(play_favourite_turn_action());
                        break;
                    case priorities.store_stones:
                        yield return gm.StartCoroutine(store_stones_turn_action());
                        break;
                    default:
                        throw new UnityException("Not valid priority value");
                }
                yield return new WaitForSeconds(gm.get_standard_enemy_action_wait_time());
            }
            yield return null;
            Assert.IsTrue(this.chosen_at_market.size()==0);
        }
        public IEnumerator gain_points_turn_action()
        {
            var cni = choose_best_card_in_personal_market_pool(CardFamily.None, CardEffectTypes.none, (int cost) => { return true; });
            add_to_hand(cni);
            yield return null;
        }
        public IEnumerator take_playable_card_turn_action()
        {
            //Put best card that we can pay from market on hand
            var cni = choose_best_playable_card_in_personal_market_pool(CardFamily.None, CardEffectTypes.none, (int cost) => { return true; });
            add_to_hand(cni);
            yield return null;
        }
        public IEnumerator play_favourite_turn_action()
        {
            //throw new UnityException("Unimplemented");
            yield return GameManager.get_instance().StartCoroutine(play_card(favourite));
            favourite = CardNameId.NONE;
        }
        public IEnumerator store_stones_turn_action()
        {
            var cni = choose_best_card_in_personal_market_pool(CardFamily.None, CardEffectTypes.none, (int cost) => { return true; });
            yield return GameManager.get_instance().StartCoroutine(sell_card(cni));
        }

        public IEnumerator activate_clocks()
        {
            var gm = GameManager.get_instance();
            //throw new UnityException("Unimplemented");
            var cal = MonteCarloClockSimulator.get_clock_order(this);
            foreach(CardNameId cni in cal.aol)
            {
                yield return gm.StartCoroutine(activate_single_clock(cni));
                yield return new WaitForSeconds(gm.get_standard_enemy_action_wait_time());
            }

            //COUNT stones at end of round;
            my_stats.mid_stones_value_at_end_of_round += stone_manager.get_total_value();
            my_stats.mid_stones_at_end_of_round += stone_manager.get_number_of_stones();
        }
        public IEnumerator activate_single_clock(CardNameId cni)
        {
            Logger.LogBold(Logger.player_log(idx, "activates " + AssetDataBase.get_card_file_name(cni) + " clock."), TextFilter.get_p_idx_message_src(idx));

            Assert.IsTrue(table.contains(cni));
            Assert.IsTrue(CardEffectTypeUtils.has_card_effect(cni, CardEffectTypes.clock));
            yield return GameManager.get_instance().StartCoroutine(CardData.get_card(cni).clockEffect(this));
        }

        public bool can_pay(int cost, CardFamily cf){
            cost -= card_reduction_cost_by_family[family_idx.get_card_family_idx(cf)];
            return cost <= stone_manager.get_total_value();
        }
        public bool can_pay(CardData cd)
        {
            return can_pay(cd.price, cd.family);
        }
        public bool can_pay(CardNameId cni)
        {
            return can_pay(CardData.get_card(cni));
        }

        public IEnumerator pay_cost(int cost, CardFamily cf){
            Assert.IsTrue(can_pay(cost, cf));
            cost -= card_reduction_cost_by_family[family_idx.get_card_family_idx(cf)];
            stone_quant payed = new stone_quant(0,0,0);
            int substracted_cost = cost;
            while (substracted_cost > 0){
                stone_type st = stone_manager.extract_highest_cost_stone();
                payed.s[(int)st] += 1;
                substracted_cost -= stone_manager.get_value(st);
            }
            my_stats.total_spent_stone += payed.get_number_of_stones();
            my_stats.total_stones_value_spent += stone_manager.get_value(payed);
            Logger.Log(Logger.player_log(idx,"payed " + stone_manager.get_value(payed)+" to cover "+cost), TextFilter.get_p_idx_message_src(idx));
            my_stats.wasted_stones_value += (stone_manager.get_value(payed) - cost);
            Assert.IsTrue(
                stone_manager.check_valid_payment(payed, cost)
            );
            pay_for_card_event?.Invoke(this, payed);

            GameManager._instance.update_all_stones_representation();
            yield return null;
        }

        private Dictionary<stone_type,int> create_stone_priority_list()
        {
            Dictionary<stone_type, int> myDict = new Dictionary<stone_type, int>();
            myDict.Add(stone_type.ST_six, DecisionParameters.ponder_single_stone(this, stone_type.ST_six, 1));
            myDict.Add(stone_type.ST_three, DecisionParameters.ponder_single_stone(this, stone_type.ST_three, 1));
            myDict.Add(stone_type.ST_one, DecisionParameters.ponder_single_stone(this, stone_type.ST_one, 1));

            var sortedDict = from entry in myDict orderby entry.Value ascending select entry;
            return myDict;
        }

        public IEnumerator choose_stones_to_add(stone_quant sq){
            //throw new UnityException("Unimplemented");
            int free_stones = stone_manager.get_number_of_spaces_to_fill();
            int total_stones = sq.get_number_of_stones();
            //ALL STONES ARE EQUAL, WE DO NOT CHOOSE
            if (total_stones == sq.s[(int)stone_type.ST_six] || total_stones == sq.s[(int)stone_type.ST_one] || total_stones == sq.s[(int)stone_type.ST_three])
            {
                int i = 0;
                while (sq.s[i] == 0) ++i;
                stone_manager.add_stones((stone_type)i, free_stones);
            }
            //STONES ARE DIFFERENT
            var sorted_dir = create_stone_priority_list();
            int stone_idx = 0;

            while ((free_stones = stone_manager.get_number_of_spaces_to_fill()) > 0)
            {
                var item = sorted_dir.ElementAt(stone_idx);
                int possible_stones = sq.s[(int)item.Key];
                int stones_to_add = Mathf.Min(possible_stones, free_stones);

                if(possible_stones < stones_to_add)
                {
                    int num_stones_wasted = stones_to_add - possible_stones;
                    my_stats.wasted_stones += num_stones_wasted;
                    my_stats.wasted_stones_value += num_stones_wasted * stone_manager.sv.s[(int)item.Key];
                }

                stone_manager.add_stones(item.Key, stones_to_add);
                ++stone_idx;
            }
            while(stone_idx < sorted_dir.Count)
            {
                var item = sorted_dir.ElementAt(stone_idx);
                int num = sq.s[(int)item.Key];
                Assert.IsTrue(num >= 0);
                my_stats.wasted_stones += num;
                my_stats.wasted_stones_value += num * stone_manager.sv.s[(int)item.Key];

                ++stone_idx;
            }


            yield return null;
        }

        public IEnumerator gain_stones(stone_quant sq){
            int i = 0;
            foreach(int val in sq.s)
            {
                if (val != 0) {
                    Logger.Log(Logger.player_log(idx ,"gains " + val + " stones with value " + stone_manager.sv.s[i]), TextFilter.get_p_idx_message_src(idx));
                }
                ++i;
            }

            my_stats.stones_per_round += Mathf.Min(sq.get_number_of_stones(), stone_manager.get_number_of_spaces_to_fill());

            if(stone_manager.get_number_of_spaces_to_fill() >= sq.get_number_of_stones()){
                stone_manager.add_stones(sq);
            }else{
                choose_stones_to_add(sq);
            }
            //Debug.Log("gain_stones right before updating stone representation");
            GameManager._instance.update_all_stones_representation();
            yield return null;
        }

        public void add_chosen_at_market(CardNameId cni)
        {
            chosen_at_market.add(cni);
        }

        public bool points_past_threshold()
        {
            return this.my_points >= threshold;
        }

        //Order is priority
        public CardNameId choose_best_card_in_tableau(CardFamily requisite, CardEffectTypes cet, cost_precondition cp, CardNameId effect_by){
            return DecisionParameters.choose_best_card(this, table, requisite, cet, cp, player_prio,false);
        }
        public CardNameId choose_worst_card_in_tableau(CardFamily requisite, CardEffectTypes cet, cost_precondition cp)
        {
            return DecisionParameters.choose_worst_card(this, table, requisite, cet, cp, player_prio,false);
        }
        public CardNameId choose_best_card_in_hand(CardFamily requisite, CardEffectTypes cet, cost_precondition cp, CardNameId effect_by)
        {
            return DecisionParameters.choose_best_card(this, hand, requisite, cet, cp, player_prio,false);
        }
        public CardNameId choose_worst_card_in_hand(CardFamily requisite, CardEffectTypes cet, cost_precondition cp)
        {
            return DecisionParameters.choose_worst_card(this, hand, requisite, cet, cp, player_prio,false);
        }
        public CardNameId choose_best_card_in_personal_market_pool(CardFamily requisite, CardEffectTypes cet, cost_precondition cp)
        {
            return DecisionParameters.choose_best_card(this, chosen_at_market, requisite, cet, cp, player_prio,true);
        }
        public CardNameId choose_best_playable_card_in_personal_market_pool(CardFamily requisite, CardEffectTypes cet, cost_precondition cp)
        {
            return DecisionParameters.choose_best_playable_card(this, chosen_at_market, requisite, cet, cp, player_prio, true);
        }
        public CardNameId choose_worst_card_in_personal_market_pool(CardFamily requisite, CardEffectTypes cet, cost_precondition cp)
        {
            return DecisionParameters.choose_worst_card(this, chosen_at_market, requisite, cet, cp, player_prio,true);
        }

        public void bounce_card(CardNameId card_name_id){
            table.extract(card_name_id);
            hand.add(card_name_id);
            hand_representation_needs_update = true;
            table_need_update = true;
        }

        public void gain_points(int points){
            Assert.IsTrue(points >= 0);
            Logger.Log(Logger.player_log(idx,"gains "+points+" points"), TextFilter.get_p_idx_message_src(idx));
            my_points += points;
            //GameManager.get_instance().update_player_points_representation();
        }
        public void loose_points(int points){
            Assert.IsTrue(points > 0);
            Logger.Log(Logger.player_log(idx, "looses " + points + " points"), TextFilter.get_p_idx_message_src(idx));
            my_points -= points;
            my_points = Mathf.Max(points, 1);
        }

        private int get_total_rating(List<int> list)
        {
            return list[(int)card_flags_idx.COUNT];
        }
        private void add_to_flags_ratings(CardNameId cni)
        {
            var card = CardData.get_card(cni);
            add_to_flags_generic_ratings(card.enabler, enabler_ratings);
            add_to_flags_generic_ratings(card.payoff, payoffs_ratings);
        }
        private void substract_from_flags_ratings(CardNameId cni)
        {
            var card = CardData.get_card(cni);
            add_to_flags_generic_ratings(card.enabler, enabler_ratings, -1);
            add_to_flags_generic_ratings(card.payoff, payoffs_ratings, -1);
        }
        public int get_simulated_sinergy_rating_with_new_card(card_flags flags, CardNameId cni)
        {
            add_to_flags_ratings(cni);
            int value = get_sinergy_complete_rating(flags);
            substract_from_flags_ratings(cni);
            return value;
        }
        public int get_simulated_sinergy_rating_with_new_card(card_flags_idx flags, CardNameId cni)
        {
            add_to_flags_ratings(cni);
            int value = get_sinergy_complete_rating(flags);
            substract_from_flags_ratings(cni);
            return value;
        }
        public int get_sinergies_rating_delta_with_new_card(card_flags flags, CardNameId cni)
        {
            return get_sinergy_complete_rating(flags) - get_simulated_sinergy_rating_with_new_card(flags, cni);
        }
        public int get_sinergies_rating_delta_with_new_card(card_flags_idx flags, CardNameId cni)
        {
            return get_sinergy_complete_rating(flags) - get_simulated_sinergy_rating_with_new_card(flags, cni);
        }
        public int get_simulated_sinergy_rating_without_card(card_flags_idx flags, CardNameId cni)
        {
            Assert.IsTrue(table.contains(cni));

            substract_from_flags_ratings(cni);
            int value = get_sinergy_complete_rating(flags);
            add_to_flags_ratings(cni);
            return value;
        }
        public int get_sinergies_rating_delta_without_card(card_flags_idx flags, CardNameId cni)
        {
            return get_sinergy_complete_rating(flags) - get_simulated_sinergy_rating_without_card(flags, cni);
        }
        public int get_sinergy_complete_rating(card_flags flags)
        {
            int payoffs_value = get_payoff_sinergy_rating(flags);
            int enabler_value = get_enablers_sinergy_rating(flags);
            if (payoffs_value == 0 || enabler_value == 0) return 0;
            return Mathf.CeilToInt(Mathf.Pow(payoffs_value, enabler_value));
        }
        public int get_sinergy_complete_rating_for_all_flags()
        {
            return payoffs_ratings[(int)card_flags_idx.COUNT] + enabler_ratings[(int)card_flags_idx.COUNT];
        }
        public int get_sinergy_complete_rating_for_all_flags_with_new_card(CardNameId cni)
        {
            add_to_flags_ratings(cni);
            int val = payoffs_ratings[(int)card_flags_idx.COUNT] + enabler_ratings[(int)card_flags_idx.COUNT];
            substract_from_flags_ratings(cni);
            return val;
        }
        public int get_sinergy_delta_rating_for_all_flags_with_new_card(CardNameId cni)
        {
            int val_before = payoffs_ratings[(int)card_flags_idx.COUNT] + enabler_ratings[(int)card_flags_idx.COUNT];
            int val_after = get_sinergy_complete_rating_for_all_flags_with_new_card(cni);
            return val_after - val_before;
        }
        public int get_sinergy_complete_rating(card_flags_idx flags)
        {
            return Mathf.CeilToInt(Mathf.Pow(get_payoff_sinergy_rating(flags), get_enablers_sinergy_rating(flags)));
        }
        public int get_enablers_sinergy_rating(card_flags flags)
        {
            return get_enablers_sinergy_rating((card_flags_idx)(int)Mathf.Log((float)flags, 2));
        }
        public int get_payoff_sinergy_rating(card_flags flags)
        {
            return get_payoff_sinergy_rating((card_flags_idx)(int)Mathf.Log((float)flags, 2));
        }
        public int get_enablers_sinergy_rating(card_flags_idx flags)
        {
            return enabler_ratings[(int)flags];
        }
        public int get_payoff_sinergy_rating(card_flags_idx flags)
        {
            //Debug.Log("Get payoff sinergy flag: " + (int)flags);
            return payoffs_ratings[(int)flags];
        }
        private void add_to_flags_generic_ratings(card_flags cf, List<int> list, int mult = 1)
        {
            int best = int.MinValue;
            for(int i = 0; i < (int)card_flags_idx.COUNT; ++i)
            {
                if(((int)cf & (1<<i)) != 0)
                {
                    int value = (mult * 1);
                    list[i] += value;
                    if (list[i] > best) best = value;
                    list[(int)card_flags_idx.COUNT] += value;
                }
            }
            list[(int)card_flags_idx.BEST] = best;
        }

        public IEnumerator play_card(CardNameId card_name_id){
            Assert.IsTrue(is_playable(card_name_id));
            var gm = GameManager.get_instance();
            CardData card = CardData.get_card(card_name_id);
            yield return gm.StartCoroutine(pay_cost(card.price, card.family));
            yield return gm.StartCoroutine(play_card_without_paying(card_name_id));
        }
        public IEnumerator play_card_without_paying(CardNameId card_name_id)
        {
            Logger.LogBold(Logger.player_log(idx,"playing "+AssetDataBase.get_card_file_name(card_name_id)), TextFilter.get_p_idx_message_src(idx));
            Assert.IsTrue(hand.contains(card_name_id));
            CardData card = CardData.get_card(card_name_id);

            hand.extract(card_name_id);
            table.add(card_name_id);
            var gm = GameManager.get_instance();
            card_enters_tableau_event?.Invoke(this);
            yield return gm.StartCoroutine(card.enterEffect(this));

            add_to_flags_ratings(card_name_id);

            hand_representation_needs_update = true;
            table_need_update = true;

            if((card.enabler & card_flags.removal)!=0 || (card.payoff & card_flags.removal) != 0)
            {
                ++my_stats.removal_played;
            }
        }
        public void add_to_hand(CardNameId cni){
            Assert.IsTrue(chosen_at_market.contains(cni));
            ++my_stats.cards_put_into_hand;
            add_to_hand_unchecked(cni);
            PlayCardsRound.remove_card_from_market(cni);
            chosen_at_market.extract(cni);
            tame_card_event?.Invoke(this, CardData.get_card(cni).family);
        }
        public void add_to_hand_unchecked(CardNameId cni)
        {
            Logger.LogBold(Logger.player_log(idx, "puts into his hand " + AssetDataBase.get_card_file_name(cni)), TextFilter.get_p_idx_message_src(idx));
            hand.add(cni);

            hand_representation_needs_update = true;

            choose_favourite_card();
        }

        public IEnumerator sell_card(CardNameId cni){
            Logger.LogBold(Logger.player_log(idx, "selling "+ AssetDataBase.get_card_file_name(cni)), TextFilter.get_p_idx_message_src(idx));
            Assert.IsTrue(chosen_at_market.contains(cni));
            ++my_stats.cards_sold;
            chosen_at_market.extract(cni);
            GameManager._instance.deck.discard(cni);
            PlayCardsRound.remove_card_from_market(cni);
            yield return GameManager._instance.StartCoroutine(gain_stones(stone_value.get_value_per_family(CardData.get_card(cni).family)));
        }

        public int count_families()
        {
            return table.count_families();
        }
        public int count_cards_in_family(CardFamily cf)
        {
            return table.count_card_of_family(cf);
        }
        public int count_cards_in_hand()
        {
            return hand.size();
        }
        public int count_cards_in_table()
        {
            return table.size();
        }
        public int count_cards_with_clocks()
        {
            return table.count_cards_with_clocks();
        }

        public void draw()
        {
            var cni = GameManager._instance.deck.draw();
            hand.add(cni);
            Logger.Log(Logger.player_log(idx, "draws "+AssetDataBase.get_card_file_name(cni)), TextFilter.get_p_idx_message_src(idx));
            hand_representation_needs_update = true;
            choose_favourite_card();
        }
        public IEnumerator discard_card_from_hand(CardNameId cni)
        {
            Assert.IsTrue(hand.contains(cni));
            hand.extract(cni);
            GameManager.get_instance().deck.discard(cni);
            hand_representation_needs_update = true;
            yield return null;
        }
        public IEnumerator discard_card_by_type_from_table(CardFamily cf)
        {
            var card = choose_worst_card_in_tableau(
                cf, CardEffectTypes.none, (int cost) => { return true; }
            );
            if(card != CardNameId.NONE)
            {
                discard_card_from_table(card);
            }

            yield return null;
        }
        public void discard_card_from_table(CardNameId cni)
        {
            Logger.Log(Logger.player_log(idx, " discards " +AssetDataBase.get_card_file_name(cni)+ "from his table"), TextFilter.get_p_idx_message_src(idx));

            Assert.IsTrue(table.contains(cni));
            table.extract(cni);
            GameManager._instance.deck.discard(cni);

            substract_from_flags_ratings(cni);

            table_need_update = true;
        }

        public Player choose_enemy()
        {
            var enemy = OpponentChoosing.choose_best_opponent(this, CardFamily.None);
            Assert.IsTrue(enemy != null);
            return enemy;
        }
        public Player choose_enemy_with_card_of_family(CardFamily cf)
        {
            var enemy = OpponentChoosing.choose_best_opponent(this,cf);
            Assert.IsTrue(enemy != null);
            return enemy;
        }
    }
}
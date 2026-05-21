using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using System.Linq;
using voe;
using static voe.DecisionParameters;

namespace voe{
    public class Player
    {
        static int threshold = 60;
        
        public int idx;
        public int points = 0;
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
        }

        private int get_cheapest_cost_of_card_in_hand()
        {
            throw new UnityException("unimplemented");
            return 0;
        }

        public void choose_favourite_card(CardList c)
        {
            favourite = choose_best_card(c);
        }
        public CardNameId choose_best_card(CardList c)
        {
            throw new UnityException("Unimplemented");
        }
        private bool cannot_play_cards()
        {
            if (GameManager.get_instance().get_round() <= table.size() || hand.empty()) return true;

            foreach (var card in hand.card_list)
            {
                var cd = CardData.get_card(card);
                if (can_pay(cd.price, cd.family)) return false;
            }
            return true;
        }
        private bool turn_can_go_on()
        {
            return !cannot_play_cards() || !chosen_at_market.empty();
        }
        public void choose_priority()
        {
            GameManager gm = GameManager.get_instance();
            //var cd = CardData.get_card(favourite);
            CardData cd;
            if(favourite!=CardNameId.NONE && can_pay((cd = CardData.get_card(favourite)).price, cd.family))
            {
                player_prio = priorities.play_favourite;
            }
            else
            {
                if(cannot_play_cards())
                {
                    if(stone_manager.get_number_of_spaces_to_fill() > 0)
                    {
                        player_prio = priorities.store_stones;
                    }
                    else 
                    { 
                        player_prio = priorities.take_playable_card;
                    }
                }
                else
                {
                    player_prio = priorities.gain_points;
                }
            }
        }

        public IEnumerator choose_card_on_market()
        {
            Assert.IsTrue(current_card_pool_option.size() > 0);

            yield return current_card_pool_option.get(0);
        }
        public IEnumerator play_turn()
        {
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
                        yield return gm.StartCoroutine(stock_up_hand_turn_action());
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
            }
            yield return null;
        }
        public IEnumerator gain_points_turn_action()
        {
            throw new UnityException("Unimplemented");
        }
        public IEnumerator stock_up_hand_turn_action()
        {
            throw new UnityException("Unimplemented");
        }
        public IEnumerator play_favourite_turn_action()
        {
            throw new UnityException("Unimplemented");
        }
        public IEnumerator store_stones_turn_action()
        {
            //throw new UnityException("Unimplemented");
            var cni = choose_best_card_in_personal_market_pool(CardFamily.None, CardEffectTypes.none, (int cost) => { return true; });
            yield return GameManager.get_instance().StartCoroutine(sell_card(cni));
        }

        public IEnumerator activate_clocks()
        {
            yield return null;
            throw new UnityException("Unimplemented");
        }
        public IEnumerator activate_single_clock(CardNameId cni)
        {
            Assert.IsTrue(table.contains(cni));
            yield return GameManager.get_instance().StartCoroutine(CardData.get_card(cni).clockEffect(this));
        }

        public bool can_pay(int cost, CardFamily cf){
            cost -= card_reduction_cost_by_family[family_idx.get_card_family_idx(cf)];
            return cost <= stone_manager.get_total_value();
        }

        public IEnumerator pay_cost(int cost, CardFamily cf){
            Assert.IsTrue(can_pay(cost, cf));

            stone_quant payed = new stone_quant(0,0,0);
            int substracted_cost = cost - card_reduction_cost_by_family[family_idx.get_card_family_idx(cf)];
            while (substracted_cost > 0){
                stone_type st = stone_manager.extract_highest_cost_stone();
                payed.s[(int)st] += 1;
                substracted_cost -= stone_manager.get_value(st);
            }

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
                int stones_to_add = Mathf.Min(sq.s[(int)item.Key], free_stones);

                stone_manager.add_stones(item.Key, stones_to_add);

                ++stone_idx;
            }

            yield return null;
        }

        public IEnumerator gain_stones(stone_quant sq){
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
            return this.points >= threshold;
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
            points += points;
        }
        public void loose_points(int points){
            Assert.IsTrue(points <= 0);
            points -= points;
            points = Mathf.Max(points, 1);
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
        public int get_sinergy_complete_rating(card_flags flags)
        {
            int payoffs_value = get_payoff_sinergy_rating(flags);
            int enabler_value = get_enablers_sinergy_rating(flags);
            if (payoffs_value == 0 || enabler_value == 0) return 0;
            return Mathf.CeilToInt(Mathf.Pow(payoffs_value, enabler_value));
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
            CardData card = CardData.get_card(card_name_id);
            Assert.IsTrue(can_pay(card.price, card.family));
            var gm = GameManager.get_instance();
            yield return gm.StartCoroutine(pay_cost(card.price, card.family));
            yield return gm.StartCoroutine(play_card_without_paying(card_name_id));
        }
        public IEnumerator play_card_without_paying(CardNameId card_name_id)
        {
            Debug.Log("Player "+idx+" playing "+AssetDataBase.get_card_file_name(card_name_id));
            Assert.IsTrue(hand.contains(card_name_id));
            CardData card = CardData.get_card(card_name_id);

            hand.extract(card_name_id);
            table.add(card_name_id);
            var gm = GameManager.get_instance();
            yield return gm.StartCoroutine(card.enterEffect(this));

            card_enters_tableau_event?.Invoke(this);

            add_to_flags_ratings(card_name_id);

            hand_representation_needs_update = true;
            table_need_update = true;
        }
        public void add_to_hand(CardNameId cni){
            Assert.IsTrue(chosen_at_market.contains(cni));
            chosen_at_market.extract(cni);
            hand.add(cni);
            PlayCardsRound.remove_card_from_market(cni);

            tame_card_event?.Invoke(this, CardData.get_card(cni).family);

            hand_representation_needs_update = true;
        }

        public IEnumerator sell_card(CardNameId cni){
            Debug.Log("Player "+idx+" selling "+ AssetDataBase.get_card_file_name(cni));
            Assert.IsTrue(chosen_at_market.contains(cni));
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
            hand.add(GameManager._instance.deck.draw());
            hand_representation_needs_update = true;
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
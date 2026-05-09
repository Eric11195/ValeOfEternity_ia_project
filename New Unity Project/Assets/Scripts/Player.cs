using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
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

        public int[] card_reduction_cost_by_family = new int[5] {0,0,0,0,0};

        public List<priorities> player_priorities = new List<priorities>((int)priorities.COUNT);
        //COUNT IS SUM, COUNT+1 is best
        public List<int> enabler_ratings = new List<int>((int)card_flags.COUNT+2);
        //COUNT IS SUM, COUNT+1 is best
        public List<int> payoffs_ratings = new List<int>((int)card_flags.COUNT+2);

        public Player()
        {
            hand = new CardList();
            table = new CardList();
            chosen_at_market = new CardList();
            stone_manager = new StoneManager();
        }

        private int get_cheapest_cost_of_card_in_hand()
        {
            throw new UnityException("unimplemented");
            return 0;
        }

        public void create_list_of_priorities()
        {
            GameManager gm = GameManager.get_instance();
            List<priorities> prio = new List<priorities>(0);
            if(stone_manager.get_total_value() < get_cheapest_cost_of_card_in_hand())
            {
                prio.Add(priorities.store_stones);
                if (gm.is_this_player_not_leading(this))
                {
                    prio.Add(priorities.gain_points);
                }
                else
                {
                    prio.Add(priorities.set_up_engine);
                }
            }
            else
            {
                if (gm.is_this_player_not_leading(this))
                {
                    prio.Add(priorities.gain_points);
                }
                else
                {
                    prio.Add(priorities.set_up_engine);
                }
                prio.Add(priorities.store_stones);
            }

        }

        public IEnumerator choose_card_on_market()
        {
            Assert.IsTrue(current_card_pool_option.size() > 0);

            yield return current_card_pool_option.get(0);
        }
        public IEnumerator play_turn()
        {
            //FOR the moment this will sell one card and play the other one, regardless of if it can be played
            CardNameId to_sell = chosen_at_market.peek();
            yield return GameManager._instance.StartCoroutine(sell_card(to_sell));
            
            CardNameId to_play = chosen_at_market.peek();
            add_to_hand(to_play);
            yield return GameManager._instance.StartCoroutine(play_card(to_play));

            yield return null;
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

        public bool can_pay(int cost){
            return cost <= stone_manager.get_total_value();
        }

        public IEnumerator pay_cost(int cost){
            Assert.IsTrue(can_pay(cost));

            stone_quant payed = new stone_quant(0,0,0);
            int substracted_cost = cost;
            while(cost > 0){
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

        public IEnumerator choose_stones_to_add(stone_quant sq){
            throw new UnityException("Unimplemented");
        }

        public IEnumerator gain_stones(stone_quant sq){
            if(stone_manager.get_number_of_spaces_to_fill() >= sq.get_number_of_stones()){
                stone_manager.add_stones(sq);
            }else{
                choose_stones_to_add(sq);
            }
            Debug.Log("gain_stones right before updating stone representation");
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
        public CardNameId choose_best_card_in_tableau(DecisionParameters.scale scale, CardFamily requisite, CardEffectTypes cet, cost_precondition cp, params DecisionParameters.prms[] my_params){
            return DecisionParameters.choose_best_card(this, table, requisite, cet, cp, scale, my_params);
        }
        public CardNameId choose_worst_card_in_tableau(DecisionParameters.scale scale, CardFamily requisite, CardEffectTypes cet, cost_precondition cp, params DecisionParameters.prms[] my_params)
        {
            return DecisionParameters.choose_worst_card(this, table, requisite, cet, cp, scale, my_params);
        }
        public CardNameId choose_best_card_in_hand(DecisionParameters.scale scale, CardFamily requisite, CardEffectTypes cet, cost_precondition cp, params DecisionParameters.prms[] my_params)
        {
            return DecisionParameters.choose_best_card(this, hand, requisite, cet, cp, scale, my_params);
        }
        public CardNameId choose_worst_card_in_hand(DecisionParameters.scale scale, CardFamily requisite, CardEffectTypes cet, cost_precondition cp, params DecisionParameters.prms[] my_params)
        {
            return DecisionParameters.choose_worst_card(this, hand, requisite, cet, cp, scale, my_params);
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
            return list[(int)card_flags.COUNT];
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
        private void add_to_flags_generic_ratings(card_flags cf, List<int> list, int mult = 1)
        {
            for(int i = 0; i < (int)card_flags.COUNT; ++i)
            {
                if(((int)cf & (1<<i)) != 0)
                {
                    int value = (mult * 1);
                    list[i] += value;
                    list[(int)card_flags.COUNT] += value;
                }
            }
        }

        public IEnumerator play_card(CardNameId card_name_id){
            Assert.IsTrue(hand.contains(card_name_id));
            CardData card = CardData.get_card(card_name_id);
            if(can_pay(card.price)){
                hand.extract(card_name_id);
                table.add(card_name_id);
                card.enterEffect(this);
            }else{
                throw new UnityException("Tried playing card whose cost could not be paid");
            }

            card_enters_tableau_event?.Invoke(this);

            add_to_flags_ratings(card_name_id);

            hand_representation_needs_update= true;
            table_need_update = true;
            yield return null;
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
                DecisionParameters.scale.constant, cf, CardEffectTypes.none, (int cost) => { return true; },
                DecisionParameters.prms.points
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

        public Player choose_enemy(DecisionParameters.scale s, params OpponentChoosing.prms[] prms)
        {
            var enemy = OpponentChoosing.choose_best_opponent(this, s, prms);
            Assert.IsTrue(enemy != null);
            return enemy;
        }
    }
}
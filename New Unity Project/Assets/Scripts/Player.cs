using UnityEngine;

using voe;

using UnityEngine.Assertions;
using System.Collections.Generic;
using System;
using System.Collections;

namespace voe{
    public class Player
    {
        static int threshold = 60;

        int points = 0;
        public CardList hand;
        public CardList table;
        public CardList chosen_at_market;
        public StoneManager stone_manager;

        //This stores the card which it can currently select from
        public CardList current_card_pool_option;

        public Player()
        {
            hand = new CardList();
            table = new CardList();
            chosen_at_market = new CardList();
            stone_manager = new StoneManager();
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

        public bool can_pay(int cost){
            return cost <= stone_manager.get_total_value();
        }

        public IEnumerator pay_cost(int cost){
            Assert.IsTrue(can_pay(cost));

            stone_quant sc = new stone_quant(0,0,0);
            int substracted_cost = cost;
            while(cost > 0){
                stone_type st = stone_manager.extract_highest_cost_stone();
                stone_manager.sa.s[(int)st] += 1;
                substracted_cost -= stone_manager.get_value(st);
            }

            Assert.IsTrue(
                stone_manager.check_valid_payment(sc, cost)
            );
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
        public CardNameId choose_card_in_tableau(DecisionParameters.scale scale, params DecisionParameters.prms[] my_params){
            int[] ponderated_value = new int[table.size()];
            for(int i=0; i < table.size(); ++i){
                ponderated_value[i]=0;
            }
            int param_using = 0;
            foreach(DecisionParameters.prms prm in my_params){
                var values_for_this_params = DecisionParameters.doFunc(prm, table);
                for(int i = 0; i < table.size(); ++i){
                    ponderated_value[i] += 
                        DecisionParameters.get_scale_value_min_to_max(scale, param_using)* 
                        values_for_this_params[i];
                }
                ++param_using;
            }

            int best_value=-1,best_index=-1;
            for(int i = 0; i < table.size(); ++i){
                if(ponderated_value[i] > best_value){
                    best_index = i;
                    best_value = ponderated_value[i];
                }
            }
            Assert.IsTrue(best_index > -1 && best_index < table.size(), "Chosen card outside valid range");
            return table.get(best_index);
        }

        public void bounce_card(CardNameId card_name_id){
            table.extract(card_name_id);
            hand.add(card_name_id);
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

        public IEnumerator play_card(CardNameId card_name_id){
            Assert.IsTrue(hand.contains(card_name_id));
            CardData card = CardData.get_card(card_name_id);
            if(can_pay(card.price)){
                hand.extract(card_name_id);
                table.add(card_name_id);
                card.enterEffect(this);
            }else{
                throw new UnityException("Tried playing card whose cost could no be payed");
            }
            yield return null;
        }
        public void add_to_hand(CardNameId cni){
            Assert.IsTrue(chosen_at_market.contains(cni));
            chosen_at_market.extract(cni);
            hand.add(cni);
            PlayCardsRound.remove_card_from_market(cni);
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
        }
        public IEnumerator discard_card()
        {
            throw new UnityException("Unimplemented");
        }
    }
}
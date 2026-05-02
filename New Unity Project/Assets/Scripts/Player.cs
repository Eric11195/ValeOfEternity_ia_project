using UnityEngine;

using voe;

using UnityEngine.Assertions;
using System.Collections;

namespace voe{
    public class Player
    {
        static int threshold = 60;

        int points = 0;
        CardList hand;
        CardList table;
        CardList chosen_at_market;
        StoneManager stone_manager;

        //This stores the card which it can currently select from
        public CardList current_card_pool_option;

        public Player()
        {
            hand = new CardList();
            table = new CardList();
            chosen_at_market = new CardList();
        }

        public IEnumerator choose_card_on_market()
        {
            Assert.IsTrue(current_card_pool_option.size() > 0);

            yield return current_card_pool_option.get(0);
        }
        public IEnumerator play_turn()
        {
            yield return null;
            throw new UnityException("Unimplemented");
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

            stone_cuant sc = new stone_cuant(0,0,0);
            int substracted_cost = cost;
            while(cost > 0){
                stone_type st = stone_manager.extract_highest_cost_stone();
                stone_manager.sa.s[(int)st] += 1;
                substracted_cost -= stone_manager.get_value(st);
            }

            Assert.IsTrue(
                stone_manager.check_valid_payment(sc, cost)
            );

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
    }
}
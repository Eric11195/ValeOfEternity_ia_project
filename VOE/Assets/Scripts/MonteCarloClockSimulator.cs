using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

namespace voe {
    public static class MonteCarloClockSimulator
    {
        private struct state
        {
            public StoneManager sm;
            public int cards_in_hand;
            public int points;
            public CardList table;
            public clock_activation_list cal;
            public state(StoneManager _sm, int _cards_in_hand, CardList _table)
            {
                sm = _sm;
                cards_in_hand = _cards_in_hand;
                table = _table;
                points = 0;
                cal = new clock_activation_list();
            }
            public state(state st)
            {
                sm = st.sm.get_deep_copy();
                cards_in_hand = st.cards_in_hand;
                table = st.table.get_deep_copy();
                cal = st.cal.get_deep_copy(st.cal);
                points = st.points;
            }
            public state(Player p)
            {
                sm = p.stone_manager.get_deep_copy();
                cards_in_hand = p.hand.size();
                table = p.table.get_deep_copy();
                cal = new clock_activation_list();
                points = 0;
            }
            public int ponder(Player p)
            {
                priorities pp = p.player_prio;

                //throw new UnityException("Unimplemented");
                int card_in_hand_value = this.cards_in_hand * 5 *
                    ((pp ==priorities.take_playable_card) ? 3 : 1);
                int stones_value = DecisionParameters.ponder_stones_gain(p, sm.sa) *
                    ((pp == priorities.store_stones) ? 3 : 1);
                int points_value = this.points *
                    ((pp == priorities.gain_points) ? 3 : 1);

                int result = card_in_hand_value + stones_value + points_value;

                if (p.favourite != CardNameId.NONE)
                {
                    int is_favourite_playable = p.can_pay(p.favourite) ? 10 : 0 *
                        ((pp == priorities.play_favourite) ? 3 : 1);
                    result += is_favourite_playable;
                }
                return result;
            }
        }
        public static clock_activation_list get_clock_order(Player p)
        {
            //Initialize relevant values
            //throw new UnityException("unimplemented: needs to call recursive monte carlo rollout");
            var aux = recursive_monte_carlo_rollout(new state(p), p);
            return aux.cal;
        }
        public class clock_activation_list
        {
            //activation order list
            public List<CardNameId> aol;
            public clock_activation_list()
            {
                aol = new(0);
            }
            public clock_activation_list get_deep_copy(clock_activation_list _cal)
            {
                clock_activation_list cal = new();
                for (int i = 0; i < _cal.aol.Count; ++i)
                {
                    cal.aol.Add(_cal.aol[i]);
                }
                return cal;
            }
            public clock_activation_list(clock_activation_list _cal)
            {
                aol = new(0);
                for(int i = 0; i < _cal.aol.Count; ++i)
                {
                    aol.Add(_cal.aol[i]);
                }
            }
        }

        private static state recursive_monte_carlo_rollout(state st, Player p)
        {
            //int activated_cards = 0;
            state best_outcome = new state(st);
            bool first_time = true;
            for (int i = 0; i < st.table.size(); ++i)
            {
                CardNameId cni = st.table.get(i);
                if (st.cal.aol.Contains(cni)) continue;
                if (!CardEffectTypeUtils.has_card_effect(cni, CardEffectTypes.clock)) continue;

                activate_clock(st, cni);
                state st_aux = recursive_monte_carlo_rollout(st, p);
                deactivate_clock(st, cni);

                if (first_time || st_aux.ponder(p) > best_outcome.ponder(p))
                {
                    best_outcome = st_aux;
                    first_time = false;
                }

                //++activated_cards;
            }
            return best_outcome;
        }

        private static void activate_clock(state st, CardNameId cni)
        {
            st.cal.aol.Add(cni);
            activate_clock_with_mult(st, cni, 1);
        }
        private static void deactivate_clock(state st, CardNameId cni)
        {
            activate_clock_with_mult(st, cni, -1);
            st.cal.aol.Remove(cni);
        }
        private static void activate_clock_with_mult(state st, CardNameId cni, int mult)
        {
            Assert.IsTrue(CardEffectTypeUtils.has_card_effect(cni, CardEffectTypes.clock));

            CardData cd = CardData.get_card(cni);
            var cost = cd.clock_expect.input;
            if(
                cost.cards_in_hand <= st.cards_in_hand || 
                cost.points <= st.points ||
                st.sm.has(cost.sq)
                )
            {
                st.sm.discard_stones(cost.sq);
                st.points -= cost.points;
                st.cards_in_hand -= cost.cards_in_hand;


                st.sm.add_stones_unchecked(cost.sq);
                st.points += cost.points;
                st.cards_in_hand += cost.cards_in_hand;
            }

        }
    }
}
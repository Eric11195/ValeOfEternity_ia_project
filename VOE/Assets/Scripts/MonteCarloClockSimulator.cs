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
            public int ponderation;
            public state(StoneManager _sm, int _cards_in_hand, CardList _table)
            {
                sm = _sm;
                cards_in_hand = _cards_in_hand;
                table = _table;
                points = 0;
                cal = new clock_activation_list();
                ponderation = -1;
            }
            public state(state st)
            {
                sm = st.sm.get_deep_copy();
                cards_in_hand = st.cards_in_hand;
                table = st.table.get_deep_copy();
                cal = new clock_activation_list(st.cal);
                points = st.points;
                ponderation = -1;
            }
            public state(Player p)
            {
                sm = p.stone_manager.get_deep_copy();
                cards_in_hand = p.hand.size();
                table = p.table.get_deep_copy();
                cal = new clock_activation_list();
                points = 0;
                ponderation = -1;
            }
            public int ponder()
            {
                if(ponderation != -1)
                {
                    return ponderation;
                }
                else
                {
                    throw new UnityException("Unimplemented");
                    //Ponder resources
                }
            }
        }
        public static clock_activation_list get_clock_order(Player p)
        {
            //Initialize relevant values
            //throw new UnityException("unimplemented: needs to call recursive monte carlo rollout");
            var aux = recursive_monte_carlo_rollout(new state(p));
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
            public clock_activation_list(clock_activation_list _cal)
            {
                aol = new(0);
                for(int i = 0; i < _cal.aol.Count; ++i)
                {
                    aol.Add(_cal.aol[i]);
                }
            }
        }

        private static state recursive_monte_carlo_rollout(state st)
        {
            int activated_cards = 0;
            state best_outcome = new state(st);
            best_outcome.ponderation = int.MinValue;
            for (int i = 0; i < st.table.size(); ++i)
            {
                CardNameId cni = st.table.get(i);
                if (st.cal.aol.Contains(cni)) continue;
                if (!CardEffectTypeUtils.has_card_effect(cni, CardEffectTypes.clock)) continue;

                activate_clock(st, cni);
                state st_aux = recursive_monte_carlo_rollout(st);
                deactivate_clock(st, cni);

                if(st_aux.ponder() > best_outcome.ponder()) best_outcome = st_aux;

                ++activated_cards;
            }
            if (activated_cards == 0) return st;
            else return best_outcome;
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


                st.sm.add_stones(cost.sq);
                st.points += cost.points;
                st.cards_in_hand += cost.cards_in_hand;
            }

        }
    }
}
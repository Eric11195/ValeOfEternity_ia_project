using UnityEngine;
using static voe.DecisionParameters;

namespace voe {
    public static class OpponentChoosing
    {
        public enum prms{
            points,
            cards_in_hand,
            cards_in_table,
            cards_with_clocks,
            families,
            cards_in_family,
            important_R_card,
            important_G_card,
            important_B_card,
            important_P_card,
            important_D_card
        }

        public delegate int param_chosing_function(Player p);

        public static param_chosing_function[] chosers = {
        };
        public static int doFunc(prms prm, Player p)
        {
            return chosers[(int)prm](p);
        }

        public static Player choose_best_opponent(Player self, CardFamily cf)
        {
            var opponents_points = choose_oponent(self,cf);
            int idx = DecisionParameters.choose_best(opponents_points);
            if(idx >= self.idx) ++idx;
            return GameManager.get_instance().players[idx];
        }
        public static bool opponent_has_card_with_card_family(Player self, CardFamily cf)
        {
            GameManager gm = GameManager.get_instance();
            foreach(Player p in gm.players)
            {
                if(p==self) continue;

                foreach(CardNameId cni in p.table.card_list)
                {
                    CardData cd = CardData.get_card(cni);
                    if(cf == CardFamily.None || cd.family == cf)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static Player choose_worst_opponent(Player self, CardFamily cf)
        {
            var opponents_points = choose_oponent(self,cf);
            int idx = DecisionParameters.choose_worst(opponents_points);
            if (idx >= self.idx) ++idx;
            return GameManager.get_instance().players[idx];
        }
        public static int[] choose_oponent(Player self, CardFamily cf)
        {
            GameManager gm = GameManager.get_instance();
            int[] opponents_points = new int[gm.players.Count - 1];
            int idx = 0;
            foreach (Player p in gm.players)
            {
                if (!p.has_card_with_requiriment(p.table, cf, CardEffectTypes.none, (int cost) => { return true; })) continue;
                if (p.idx == self.idx) continue;
                opponents_points[idx++] = ponderate_player(p);
            }
            return opponents_points;
        }
        private static int ponderate_player(Player ponder)
        {
            int result = 0;

            int param_using = 0;
            //throw new UnityException("Unimplemented: choose for most points");

            return ponder.my_points;

            //foreach (OpponentChoosing.prms prm in my_params)
            //{
            //    var values_for_this_params = OpponentChoosing.doFunc(prm, ponder);
            //    result +=
            //        DecisionParameters.get_scale_value_min_to_max(scale, param_using) *
            //        values_for_this_params;
            //    ++param_using;
            //}
            //return result;
        }
    }
}
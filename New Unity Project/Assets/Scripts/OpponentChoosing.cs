using UnityEngine;

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
        public static Player choose_best_opponent(Player self, DecisionParameters.scale scale, params DecisionParameters.prms[] my_params)
        {
            var opponents_points = choose_oponent(self, scale, my_params);
            int idx = DecisionParameters.choose_best(opponents_points);
            if(idx >= self.idx) ++idx;
            return GameManager.get_instance().players[idx];
        }
        public static Player choose_worst_opponent(Player self, DecisionParameters.scale scale, params DecisionParameters.prms[] my_params)
        {
            var opponents_points = choose_oponent(self, scale, my_params);
            int idx = DecisionParameters.choose_worst(opponents_points);
            if (idx >= self.idx) ++idx;
            return GameManager.get_instance().players[idx];
        }
        public static int[] choose_oponent(Player self, DecisionParameters.scale scale, params DecisionParameters.prms[] my_params)
        {
            GameManager gm = GameManager.get_instance();
            int[] opponents_points = new int[gm.players.Count - 1];
            int idx = 0;
            foreach (Player p in gm.players)
            {
                if (p.idx == self.idx) continue;
                opponents_points[idx++] = ponderate_player(p, my_params);
            }
            return opponents_points;
        }
        private static int ponderate_player(Player ponder, params DecisionParameters.prms[] my_params)
        {
            throw new UnityException("Unimplemented");
        }
    }
}
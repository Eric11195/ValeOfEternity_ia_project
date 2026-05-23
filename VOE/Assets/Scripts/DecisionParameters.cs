using UnityEngine;
using UnityEngine.Assertions;

namespace voe{
    public static class DecisionParameters
    {
        //public enum prms{
        //    greater_cost,
        //    lower_cost,
        //    good_bounce_target,
        //    playable,
        //    sinergy,
        //    points
        //};
        ////Return points for each card in the same order as given
        //public delegate int param_chosing_function(Player p, CardNameId cl);
        ////Functions in same order as params
        //public static param_chosing_function[] chosers = {
        //};
        //public static int doFunc(prms prm, Player p, CardNameId cl){
        //    return chosers[(int)prm](p, cl);
        //}

        public enum scale{
            linear,
            pot2,
            fibonacci,
            constant,
            COUNT
        };
        private const int max_scale_idx = 20;
        public static int get_scale_value_max_to_min(scale s, int idx){
            Assert.IsTrue(max_scale_idx > idx && (int)s < (int)scale.COUNT);
            return scales_values[max_scale_idx-(int)s,idx];
        }
        public static int get_scale_value_min_to_max(scale s, int idx){
            Assert.IsTrue(max_scale_idx > idx && (int)s < (int)scale.COUNT);
            return scales_values[(int)s,idx];
        }
        public static int[,] scales_values= new int[(int)scale.COUNT,max_scale_idx]{
            {1,2,3,4,5,6,7,8,9,10,
                11,12,13,14,15,16,17,18,19,20},
            {1,2,4,8,16,32,64,128,256,512,
                1024,2048,4096,8192,16384,32768,65536,131072,262144,524288},
            {1,1,2,3,5,8,13,21,34,55,
                89,144,233,377,610,987,1597,2584,4181,6765},
            {1,1,1,1,1,1,1,1,1,1,
                1,1,1,1,1,1,1,1,1,1 }
        };

        public static int choose_best(int[] values)
        {
            int best = int.MinValue;
            int best_idx = -1;
            Assert.IsTrue(values.Length > 0);
            int i = 0;
            foreach (int v in values){
                if(best < v)
                {
                    best_idx = i;
                    best = v;
                }
                ++i;
            }
            Assert.IsTrue(best_idx != -1);
            return best_idx;
        }
        public static int choose_worst(int[] values)
        {
            int best = int.MaxValue;
            int best_idx = -1;
            Assert.IsTrue(values.Length > 0);
            int i = 0;
            foreach (int v in values)
            {
                if (best > v)
                {
                    best_idx = i;
                    best = v;
                }
                ++i;
            }
            Assert.IsTrue(best_idx != -1);
            return best_idx;
        }
        public static bool check_conditions(CardNameId cni, CardFamily cf, CardEffectTypes cet, cost_precondition cp)
        {
            var card = CardData.get_card(cni);
            return (cp(card.price) && CardEffectTypeUtils.has_card_effect(cni, cet) && ((card.family & cf) != 0 || cf == CardFamily.None));
        }

        public delegate bool cost_precondition(int cost);
        public static CardNameId choose_best_card(Player p, CardList cl, CardFamily cf, CardEffectTypes cet, cost_precondition cp, priorities player_prio, bool market)
        {
            var opponents_points = choose_card(p, cl, cf, cet, cp, player_prio, market);
            int idx = choose_best(opponents_points);

            if (check_conditions(cl.get(idx), cf, cet, cp))
                return cl.get(idx);
            else 
                return CardNameId.NONE;
        }
        public static CardNameId choose_worst_card(Player p, CardList cl, CardFamily cf, CardEffectTypes cet, cost_precondition cp, priorities player_prio, bool market)
        {
            var opponents_points = choose_card(p, cl, cf, cet, cp, player_prio,market);
            int idx = choose_best(opponents_points);

            if (check_conditions(cl.get(idx), cf, cet, cp))
                return cl.get(idx);
            else
                return CardNameId.NONE;
        }

        public static int[] choose_card(Player p, CardList cl, CardFamily cf, CardEffectTypes cet, cost_precondition cp, priorities player_prio, bool market)
        {
            int[] card_points = new int[cl.size()];
            int idx = 0;
            foreach (CardNameId cni in cl.card_list)
            {
                var card = CardData.get_card(cni);
                if (cp(card.price) && ((card.family & cf) != 0 || cf == CardFamily.None))
                    card_points[idx++] = ponderate_card(p, cni, player_prio,market);
                else
                    card_points[idx++] = int.MinValue;
            }
            return card_points;
        }
        public static float multiplier_for_priority = 5.0f;
        public static int ponderate_card(Player p, CardNameId cni, priorities my_params, bool market)
        {
            float result = 0;

            result += ponder_stones_gain(p, cni, market) * (my_params == priorities.store_stones ? multiplier_for_priority : 1.0f) ;
            result += ponder_point_gain(p, cni, market) * (my_params == priorities.gain_points ? multiplier_for_priority : 1.0f);
            result += ponder_hand_card_gain(p, cni, market) *(my_params == priorities.take_playable_card ? multiplier_for_priority : 1.0f);

            return Mathf.CeilToInt(result);
        }
        const int stone_ponderation_sinergy_multiplier = 5;
        public static int ponder_stones_gain(Player p, CardNameId cni, bool market)
        {
            CardData cd = CardData.get_card(cni);
            int result = 0;
            int free = p.stone_manager.get_number_of_spaces_to_fill();
            //ADD MARKET COST * rock synergies
            if (market)
            {
                stone_quant sq = stone_value.get_value_per_family(cd.family);
                result += ponder_stones_gain(p, sq);
            }

            //ADD stone sinergy
            result += stone_ponderation_sinergy_multiplier * p.get_simulated_sinergy_rating_with_new_card(card_flags.stones1, cni);
            result += stone_ponderation_sinergy_multiplier * p.get_simulated_sinergy_rating_with_new_card(card_flags.stones3, cni);
            result += stone_ponderation_sinergy_multiplier * p.get_simulated_sinergy_rating_with_new_card(card_flags.stones6, cni);

            // ADD Rock generation capabilities
            Debug.LogWarning("Stone ponderation only takes into account price on table or sinergy on hand. Does not check whether that card produces stones");

            return result;
        }
        public static int ponder_stones_gain(Player p, stone_quant sq)
        {
            int result = 0;

            result += ponder_single_stone(p, stone_type.ST_six, sq.s[(int)stone_type.ST_six]);

            result += ponder_single_stone(p, stone_type.ST_three, sq.s[(int)stone_type.ST_three]);

            result += ponder_single_stone(p, stone_type.ST_one, sq.s[(int)stone_type.ST_one]);

            return result;
        }
        public static int ponder_single_stone(Player p, stone_type st, int number_of_stones)
        {
            int quant =
                Mathf.Min(number_of_stones, p.stone_manager.get_number_of_spaces_to_fill()) * //number
                p.stone_manager.sv.s[(int)st] //value
                ;
            if (quant == 0) return 0;
            switch (st)
            {
                case stone_type.ST_six:
                    quant += stone_ponderation_sinergy_multiplier * p.get_sinergy_complete_rating(card_flags.stones6);
                    break;
                case stone_type.ST_three:
                    quant += stone_ponderation_sinergy_multiplier * p.get_sinergy_complete_rating(card_flags.stones3);
                    break;
                case stone_type.ST_one:
                    quant += stone_ponderation_sinergy_multiplier * p.get_sinergy_complete_rating(card_flags.stones1);
                    break;
            }
            return quant;
        }
        public static int ponder_point_gain(Player p, CardNameId cni, bool market)
        {
            //Synergies
            int quant = 0;
            for(int i = 0; i < (int)card_flags_idx.COUNT; ++i)
            {
                quant += 5* p.get_sinergies_rating_delta_with_new_card((card_flags_idx)i,cni);
            }
            return quant;
        }
        public static int ponder_hand_card_gain(Player p, CardNameId cni, bool market)
        {
            int result = 0;
            if (market) result += 5 * p.get_sinergy_complete_rating(card_flags.big_hand);
            Debug.LogWarning("Ponder hand card gain does not take into account cards whose effects are drawing");
            
            return result;
        }
    }
}
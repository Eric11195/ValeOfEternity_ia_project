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
        public static CardNameId choose_best_card(Player p, CardList cl, CardFamily cf, CardEffectTypes cet, cost_precondition cp, priorities player_prio)
        {
            var opponents_points = choose_card(p, cl, cf, cet, cp, player_prio);
            int idx = choose_best(opponents_points);

            if (check_conditions(cl.get(idx), cf, cet, cp))
                return cl.get(idx);
            else 
                return CardNameId.NONE;
        }
        public static CardNameId choose_worst_card(Player p, CardList cl, CardFamily cf, CardEffectTypes cet, cost_precondition cp, priorities player_prio)
        {
            var opponents_points = choose_card(p, cl, cf, cet, cp, player_prio);
            int idx = choose_best(opponents_points);

            if (check_conditions(cl.get(idx), cf, cet, cp))
                return cl.get(idx);
            else
                return CardNameId.NONE;
        }

        public static int[] choose_card(Player p, CardList cl, CardFamily cf, CardEffectTypes cet, cost_precondition cp, priorities player_prio)
        {
            int[] card_points = new int[cl.size()];
            int idx = 0;
            foreach (CardNameId cni in cl.card_list)
            {
                var card = CardData.get_card(cni);
                if (cp(card.price) && ((card.family & cf) != 0 || cf == CardFamily.None))
                    card_points[idx++] = ponderate_card(p, cni, player_prio);
                else
                    card_points[idx++] = int.MinValue;
            }
            return card_points;
        }
        public static int ponderate_card(Player p, CardNameId cni, priorities my_params)
        {
            int result = 0;

            int param_using = 0;
            throw new UnityException("Unimplemented: take corresponding func");
            //foreach (DecisionParameters.prms prm in my_params)
            //{
            //    var values_for_this_params = DecisionParameters.doFunc(prm, p, cni);
            //    result +=
            //        DecisionParameters.get_scale_value_min_to_max(scale, param_using) *
            //        values_for_this_params;
            //    ++param_using;
            //}
            //return result;
        }
    }
}
using UnityEngine;
using UnityEngine.Assertions;

namespace voe{
    public static class DecisionParameters
    {
        public enum prms{
            greater_cost,
            lower_cost,
            good_bounce_target,
            playable,
            family_sinergy
        };
        //Return points for each card in the same order as given
        public delegate int[] param_chosing_function(CardList cl);
        //Functions in same order as params
        public static param_chosing_function[] chosers = {
        };
        public static int[] doFunc(prms prm, CardList cl){
            return chosers[(int)prm](cl);
        }

        public enum scale{
            linear,
            pot2,
            fibonacci,
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
                89,144,233,377,610,987,1597,2584,4181,6765}
        };
    }
}
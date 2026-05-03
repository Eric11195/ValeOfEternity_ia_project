using UnityEngine;

namespace voe{
    public enum CardFamily
    {
        R,
        G,
        B,
        P,
        D
    }

    public static class stone_value{
        public static stone_quant get_value_per_family(CardFamily cf){
            return per_family[(int)cf];
        }
        public static stone_quant[] per_family = {
            new stone_quant(3,0,0),
            new stone_quant(4,0,0),
            new stone_quant(0,1,0),
            new stone_quant(1,1,0),
            new stone_quant(0,0,1),
        };
    }
}
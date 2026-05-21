using UnityEngine;

namespace voe{
    public enum CardFamily
    {
        None = 0,
        R=1<<0,
        G=1<<1,
        B=1<<2,
        P=1<<3,
        D=1<<4,
    }
    public static class family_idx
    {
        public static int get_card_family_idx(CardFamily cf)
        {
            switch (cf)
            {
                case CardFamily.None:
                    return 0;
                case CardFamily.R:
                    return 1;
                case CardFamily.G:
                    return 2;
                case CardFamily.B:
                    return 3;
                case CardFamily.P:
                    return 4;
                case CardFamily.D:
                    return 5;
                default:
                    throw new UnityException("Non valid card family");
            }
        }
    }
    public static class stone_value{
        public static stone_quant get_value_per_family(CardFamily cf){
            switch (cf)
            {
                case CardFamily.R:
                    return per_family[0];
                case CardFamily.G:
                    return per_family[1];
                case CardFamily.B:
                    return per_family[2];
                case CardFamily.P:
                    return per_family[3];
                case CardFamily.D:
                    return per_family[4];
                default:
                    throw new UnityException("Non valid card family");
            }
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
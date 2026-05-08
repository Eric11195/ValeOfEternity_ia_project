using UnityEngine;
namespace voe
{
    public enum CardEffectTypes
    {
        enter,
        clock,
        exit,
        none
    }
    public static class CardEffectTypeUtils
    {
        public static bool has_card_effect(CardNameId cni, CardEffectTypes cet)
        {
            var card = CardData.get_card(cni);

            if((cet & CardEffectTypes.enter) != 0)
            {
                if (card.enterEffect == CardFuncs.void_func) return false;
            }
            if ((cet & CardEffectTypes.clock) != 0)
            {
                if (card.clockEffect == CardFuncs.void_func) return false;
            }
            if ((cet & CardEffectTypes.exit) != 0)
            {
                if (card.exitEffect == CardFuncs.void_func) return false;
            }
            return true;
        }
    }

}
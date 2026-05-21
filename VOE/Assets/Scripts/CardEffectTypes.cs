using UnityEngine;
namespace voe
{
    public enum CardEffectTypes
    {
        enter = 1 << 0,
        clock = 1 << 1,
        infinite = 1 << 2,
        none = 0
    }
    public static class CardEffectTypeUtils
    {
        public static bool has_card_effect(CardNameId cni, CardEffectTypes cet)
        {
            var card = CardData.get_card(cni);

            return cet == CardEffectTypes.none || (card.effect_type & cet) != 0;
        }
    }

}
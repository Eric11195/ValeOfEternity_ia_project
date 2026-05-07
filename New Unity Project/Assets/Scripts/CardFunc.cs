using UnityEngine;
using System.Collections;

namespace voe{
    public static class CardFuncs
    {
        public static IEnumerator unimplemented_func(Player p)
        {
            throw new UnityException("Unimplemented");
        }
        public static IEnumerator void_func(Player p){
            return null;
        }
        public static IEnumerator aerie_enter_func(Player p){
            var card_chosen = 
                p.choose_card_in_tableau(
                    DecisionParameters.scale.fibonacci,
                    DecisionParameters.prms.good_bounce_target,
                    DecisionParameters.prms.greater_cost,
                    DecisionParameters.prms.playable
                );
            p.bounce_card(card_chosen);
            p.gain_points(CardData.get_card(card_chosen).price);
            throw new UnityException("Unimplemented");
        }
        #region Agni
        public static IEnumerator agni_enter_func(Player p)
        {
            p.stone_manager.add_stone_value(stone_type.ST_one, 1);
            yield return null;
        }
        public static IEnumerator agni_exit_func(Player p)
        {
            p.stone_manager.add_stone_value(stone_type.ST_one, -1);
            yield return null;
        }
        #endregion
        #region Behemoth
        public static IEnumerator behemoth_enter_func(Player p)
        {
            p.gain_points(3 * p.count_families());
            yield return null;
        }
        #endregion
        #region Boreas
        public static IEnumerator boreas_enter_func(Player p)
        {
            p.gain_points(p.count_cards_in_family(CardFamily.P));
            p.bounce_card(CardNameId.Boreas);
            yield return null;
        }
        #endregion
    }
}
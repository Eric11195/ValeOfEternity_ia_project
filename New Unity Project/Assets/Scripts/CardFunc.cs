using UnityEngine;
using System.Collections;

namespace voe{
    public static class CardFuncs
    {
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
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace voe
{
    public static class PlayCardsRound
    {
        public static IEnumerator play_cards_round()
        {
            Debug.Log("Started Play Phase");
            GameManager gm = GameManager.get_instance();
            foreach (Player p in gm.players)
            {
                yield return p.play_turn();
            }
            yield return null;
        }
    
        public static IEnumerator sell_card(Player p, CardNameId card){
            p.sell_card(card);
            yield return null;
        }
        public static void remove_card_from_market(CardNameId card){
            GameManager gm = GameManager.get_instance();
            gm.market_area.remove(card);
        }
    }
}

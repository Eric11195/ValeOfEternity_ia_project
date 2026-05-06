using UnityEngine;
using System.Collections;

namespace voe
{
    public static class PlayCardsRound
    {
        public static IEnumerator play_cards_round()
        {
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
    }
}

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
    
        public static void sell_card(Player p, CardNameId card){
            p.gain_stones(stone_value.get_value_per_family(CardData.get_card(card).family));
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace voe
{
    public static class PlayCardsRound
    {
        public static IEnumerator play_cards_round()
        {
            Logger.LogH2("Started Play Phase", TextFilter.message_src.general);
            GameManager gm = GameManager.get_instance();

            for (int i = 0; i < gm.players.Count; i++)
            {
                yield return gm.StartCoroutine(gm.get_player_with_idx_i_from_turn_player(i).play_turn());
            }
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

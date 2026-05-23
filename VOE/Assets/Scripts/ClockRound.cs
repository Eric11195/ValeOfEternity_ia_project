using UnityEngine;
using System.Collections;

namespace voe
{
    static class ClockRound
    {
        public static IEnumerator clock_round()
        {
            Logger.LogH2("Started clock round", TextFilter.message_src.general);
            GameManager gm = GameManager.get_instance();
            for (int i = 0; i < gm.players.Count; i++)
            {
                yield return gm.StartCoroutine(gm.get_player_with_idx_i_from_turn_player(i).activate_clocks());
            }
            yield return null;
        }
    }
}
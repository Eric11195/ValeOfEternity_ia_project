using UnityEngine;
using System.Collections;

namespace voe
{
    static class ClockRound
    {
        public static IEnumerator clock_round()
        {
            GameManager gm = GameManager.get_instance();
            foreach (Player p in gm.players)
            {
                yield return p.activate_clocks();
            }
            yield return null;
        }
    }
}
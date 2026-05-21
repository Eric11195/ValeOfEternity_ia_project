using UnityEngine;
using System.Collections;

namespace voe
{
    static class ClockRound
    {
        public static IEnumerator clock_round()
        {
            Debug.Log("Started clock round");
            GameManager gm = GameManager.get_instance();
            foreach (Player p in gm.players)
            {
                yield return p.activate_clocks();
            }
            yield return null;
        }
    }
}
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace voe
{
    public class PlayerStatSetter : MonoBehaviour
    {
        List<PlayerStatSetterInPannel> psspl;
        private void Start()
        {
            psspl = new List<PlayerStatSetterInPannel>();
            foreach(Transform tr in this.transform)
            {
                psspl.Add(tr.GetComponent<PlayerStatSetterInPannel>());
                tr.gameObject.SetActive(false);
            }
        }
        public void set_stats(stats[] st)
        {
            Assert.IsTrue(st.Length==psspl.Count);

            for (int i = 0; i < st.Length; i++)
            {
                psspl[i].gameObject.SetActive(true);
                psspl[i].set_stats(st[i]);
            }
        }
    }
}
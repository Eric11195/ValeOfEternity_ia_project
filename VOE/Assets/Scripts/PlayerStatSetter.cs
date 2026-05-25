using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace voe
{
    public class PlayerStatSetter : MonoBehaviour
    {
        List<PlayerStatSetterInPannel> psspl;
        private GameObject activator_button;
        private bool is_visible = false;
        private void Start()
        {
            psspl = new List<PlayerStatSetterInPannel>();
            int i = 0;
            foreach(Transform tr in this.transform)
            {
                psspl.Add(tr.GetComponent<PlayerStatSetterInPannel>());
                tr.gameObject.SetActive(false);
                ++i;
                if (i == 4) break;
            }
            activator_button = GameObject.Find("Activator");
            activator_button.SetActive(false);
            is_visible = false;
        }

        public void show_hide()
        {
            is_visible = !is_visible;
            foreach (var p in psspl)
            {
                p.gameObject.SetActive(is_visible);
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
            activator_button.SetActive(true);
            is_visible = true ;
        }
    }
}
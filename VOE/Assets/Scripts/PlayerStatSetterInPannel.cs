using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace voe {
    public class PlayerStatSetterInPannel : MonoBehaviour
    {
        [SerializeField]
        Transform parent_tr;
        List<TextMeshProUGUI> list;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            list = new(0);
            foreach (Transform tr in parent_tr)
            {
                list.Add(tr.GetComponent<TextMeshProUGUI>());
            }
            Assert.IsTrue(list.Count==14);
        }

        public void set_stats(stats st)
        {
            list[0].text = st.most_points_in_single_round.ToString();
            list[1].text = st.stones_per_round.ToString();
            list[2].text = st.total_spent_stone.ToString();
            list[3].text = st.total_stones_value_spent.ToString();
            list[4].text = st.mid_stones_value_at_end_of_round.ToString();
            list[5].text = st.mid_stones_at_end_of_round.ToString();
            list[6].text = st.points_per_enter_effect.ToString();
            list[7].text = st.points_per_clock_effect.ToString();
            list[8].text = st.wasted_stones.ToString();
            list[9].text = st.wasted_stones_value.ToString();
            list[10].text = st.removal_played.ToString();
            list[11].text = st.cards_in_hand.ToString();
            list[12].text = st.cards_sold.ToString();
            list[13].text = st.cards_put_into_hand.ToString();
        }
    }
}
using UnityEngine;

namespace voe {
    public struct stats
    {
        public int most_points_in_single_round;
        public float stones_per_round;
        public int total_spent_stone;
        public int total_stones_value_spent;
        public float mid_stones_value_at_end_of_round;
        public float mid_stones_at_end_of_round;
        public int points_per_enter_effect;
        public int points_per_clock_effect;
        public int wasted_stones;
        public int wasted_stones_value;
        public int removal_played;
        public int cards_in_hand;
        public int cards_sold;
        public int cards_put_into_hand;
        public static stats init_base_stats()
        {
            stats s = new stats();
            s.most_points_in_single_round = 0;
            s.stones_per_round = 0;
            s.total_spent_stone = 0;
            s.total_stones_value_spent = 0;
            s.mid_stones_value_at_end_of_round = 0;
            s.mid_stones_at_end_of_round = 0;
            s.points_per_enter_effect = 0;
            s.points_per_clock_effect = 0;
            s.wasted_stones = 0;
            s.wasted_stones_value = 0;
            s.removal_played = 0;
            s.cards_in_hand = 0;
            s.cards_sold = 0;
            s.cards_put_into_hand = 0;
            return s;
        }
    }
}
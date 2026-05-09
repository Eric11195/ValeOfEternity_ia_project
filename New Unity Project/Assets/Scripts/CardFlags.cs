using UnityEngine;

namespace voe
{
    public enum card_flags
    {
        none = 0,
        big_hand = 1 << 0,
        familyR = 1 << 1,
        familyB = 1 << 2,
        familyG = 1 << 3,
        familyP = 1 << 4,
        familyD = 1 << 5,
        stones1 = 1 << 6,
        stones3 = 1 << 7,
        stones6 = 1 << 8,
        number_of_families = 1 << 9,
        clocks = 1 << 10,
        etbs = 1 << 11,
        recursion = 1 << 12, //similar to recursion
        removal = 1 << 13,
        space_free = 1 << 14,
        high_costs = 1 << 15,
        low_costs = 1 << 16,
        loose_points = 1 << 17,
        cost_reduction = 1 << 18,
        multicast = 1 << 19,
        tableau_width = 1 << 20,
        COUNT = 21,
        BEST = 22
    }

}
using UnityEngine;

namespace voe
{
    public enum card_flags_idx
    {
        big_hand ,
        familyR ,
        familyB ,
        familyG ,
        familyP ,
        familyD ,
        stones1 ,
        stones3 ,
        stones6 ,
        number_of_families,
        clocks,
        etbs,
        recursion, //similar to recursion
        removal,
        space_free,
        high_costs,
        low_costs,
        loose_points,
        cost_reduction,
        multicast,
        tableau_width,
        COUNT,
        BEST
    }

    public enum card_flags
    {
        none = 0,
        big_hand = 1 << card_flags_idx.big_hand,
        familyR = 1 << card_flags_idx.familyR,
        familyB = 1 << card_flags_idx.familyB,
        familyG = 1 << card_flags_idx.familyG,
        familyP = 1 << card_flags_idx.familyP,
        familyD = 1 << card_flags_idx.familyD,
        stones1 = 1 << card_flags_idx.stones1,
        stones3 = 1 << card_flags_idx.stones3,
        stones6 = 1 << card_flags_idx.stones6,
        number_of_families = 1 << card_flags_idx.number_of_families,
        clocks = 1 << card_flags_idx.clocks,
        etbs = 1 << card_flags_idx.etbs,
        recursion = 1 << card_flags_idx.recursion,
        removal = 1 << card_flags_idx.removal,
        space_free = 1 << card_flags_idx.space_free,
        high_costs = 1 << card_flags_idx.high_costs,
        low_costs = 1 << card_flags_idx.low_costs,
        loose_points = 1 << card_flags_idx.loose_points,
        cost_reduction = 1 << card_flags_idx.cost_reduction,
        multicast = 1 << card_flags_idx.multicast,
        tableau_width = 1 << card_flags_idx.tableau_width
    }

}
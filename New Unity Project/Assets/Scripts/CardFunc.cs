using UnityEngine;
using System.Collections;
using NUnit.Framework;

namespace voe{
    public static class CardFuncs
    {
        public static IEnumerator unimplemented_func(Player p)
        {
            throw new UnityException("Unimplemented");
        }
        public static IEnumerator void_func(Player p){
            return null;
        }
        public static IEnumerator aerie_enter_func(Player p){
            var card_chosen = 
                p.choose_card_in_tableau(
                    DecisionParameters.scale.fibonacci,
                    DecisionParameters.prms.good_bounce_target,
                    DecisionParameters.prms.greater_cost,
                    DecisionParameters.prms.playable
                );
            p.bounce_card(card_chosen);
            p.gain_points(CardData.get_card(card_chosen).price);
            throw new UnityException("Unimplemented");
        }
        #region Agni
        public static IEnumerator agni_enter_func(Player p)
        {
            p.stone_manager.add_stone_value(stone_type.ST_one, 1);
            yield return null;
        }
        public static IEnumerator agni_exit_func(Player p)
        {
            p.stone_manager.add_stone_value(stone_type.ST_one, -1);
            yield return null;
        }
        #endregion
        #region Behemoth
        public static IEnumerator behemoth_enter_func(Player p)
        {
            p.gain_points(3 * p.count_families());
            yield return null;
        }
        #endregion
        #region Boreas
        public static IEnumerator boreas_enter_func(Player p)
        {
            p.gain_points(p.count_cards_in_family(CardFamily.P));
            p.bounce_card(CardNameId.Boreas);
            yield return null;
        }
        #endregion
        #region DandelionSpirit
        public static IEnumerator dandelion_enter_func(Player p)
        {
            p.draw();
            yield return null;
        }
        public static IEnumerator dandelion_clock_func(Player p)
        {
            p.bounce_card(CardNameId.Dandelionspirit);
            yield return null;
        }
        #endregion
        #region Eternity
        public static IEnumerator eternity_enter_func(Player p)
        {
            p.gain_points(4 * p.count_families());
            yield return null;
        }
        #endregion
        #region Firefox
        public static IEnumerator firefox_enter_func(Player p)
        {
            p.gain_points(1 * p.count_cards_in_hand());
            yield return null;
        }
        #endregion
        #region Freyja
        public static IEnumerator freyja_clock_func(Player p)
        {
            p.gain_points(1 * p.count_cards_with_clocks());
            yield return null;
        }
        #endregion
        #region Genie
        public static IEnumerator genie_enter_func(Player p)
        {
            p.activate_clocks();
            yield return null;
        }
        #endregion
        #region Gi-rin
        public static IEnumerator girin_enter_func(Player p)
        {
            p.gain_points(2 * p.count_cards_in_table());
            yield return null;
        }
        #endregion
        # region Griffon
        public static IEnumerator griffon_clock_func(Player p)
        {
            p.draw();
            yield return null;
        }
        #endregion
        #region Hae-tae
        public static IEnumerator haetae_enter_n_exit_func(Player p) 
        {
            //swap three and six
            (p.stone_manager.sv.s[(int)stone_type.ST_three],
                p.stone_manager.sv.s[(int)stone_type.ST_six]) = (
                p.stone_manager.sv.s[(int)stone_type.ST_six],
                p.stone_manager.sv.s[(int)stone_type.ST_three] );
            yield return null;
        }
        #endregion
        #region Harpy
        public static IEnumerator harpy_clock_func(Player p)
        {
            if (p.count_cards_in_hand() == p.count_cards_in_table())
            {
                p.gain_points(3);
            }
            yield return null;
        }
        #endregion
        #region Hestia
        public static IEnumerator hestia_enter_func(Player p)
        {
            p.stone_manager.max_stones += 2;
            Assert.IsTrue(p.stone_manager.get_number_of_stones()==6);
            yield return null;
        }
        public static IEnumerator hestia_exit_func(Player p)
        {
            p.stone_manager.max_stones -= 2;
            Assert.IsTrue(p.stone_manager.get_number_of_stones() == 4);
            yield return null;
        }
        #endregion
        #region Horned Salamander
        public static IEnumerator horned_salamander_clock_func(Player p)
        {
            p.gain_stones(new stone_quant(4, 0, 0));
            yield return null;
        }
        #endregion
        #region Ifrit
        public static IEnumerator ifrit_enter_func(Player p)
        {
            p.gain_points(1 * p.count_cards_in_table());
            yield return null;
        }
        #endregion
        #region Imp
        public static IEnumerator imp_enter_func(Player p)
        {
            p.gain_stones(new stone_quant(2, 0, 0));
            yield return null;
        }
        public static IEnumerator imp_clock_func(Player p)
        {
            p.bounce_card(CardNameId.Imp);
            yield return null;
        }
        #endregion
        #region Incubus
        public static int incubus_count(Player p)
        {
            int count = 0;
            foreach (var c in p.table.card_list)
            {
                if (CardData.get_card(c).price == 2)
                {
                    ++count;
                }
            }return count;
        }
        public static IEnumerator incubus_enter_func(Player p)
        {
            p.gain_points(2*incubus_count(p));
            yield return null;
        }
        #endregion
        #region Lava Giant
        public static IEnumerator lava_giant_enter_func(Player p)
        {
            p.gain_points(2 * p.count_cards_in_family(CardFamily.R));
            yield return null;
        }
        #endregion
        #region Medusa
        public static IEnumerator medusa_clock_func(Player p)
        {
            if (p.count_cards_in_hand() > 0)
            {
                yield return p.discard_card();
                p.gain_stones(new stone_quant(0, 0, 1));
            }
            yield return null;
        }
        #endregion
        #region MudSlime
        public static IEnumerator mud_slime_enter_func(Player p)
        {
            p.gain_points(6);
            yield return null;
        }
        public static IEnumerator mud_slime_clock_func(Player p)
        {
            p.bounce_card(CardNameId.Mudslime);
            yield return null;
        }
        #endregion
        #region Nessie
        public static IEnumerator nessie_clock_func(Player p)
        {
            if (p.count_cards_in_family(CardFamily.D) == 0)
            {
                p.gain_points(2);
            }
            yield return null;
        }
        #endregion
        #region Odin
        public static IEnumerator odin_clock_func(Player p)
        {
            if (p.count_cards_in_hand() < 6)
            {
                p.gain_points(2);
            }
            else
            {
                p.gain_stones(new stone_quant(0, 0, 1));
            }
            yield return null;
        }
        #endregion
        #region Poseidon
        public static IEnumerator poseidon_enter_func(Player p)
        {
            p.gain_points(3 * p.count_cards_in_family(CardFamily.B));
            yield return null;
        }
        #endregion
        #region Rock Golem
        public static IEnumerator rock_golem_enter_func(Player p)
        {
            p.gain_points(
                p.stone_manager.sa.s[(int)stone_type.ST_six]*
                p.stone_manager.get_value(stone_type.ST_six)
            );
            yield return null;
        }
        #endregion
        #region Rudra
        public static IEnumerator rudra_enter_effect(Player p)
        {
            p.gain_points(2 * p.count_cards_in_hand());
            yield return null;
        }
        #endregion
        #region Salamander
        public static IEnumerator salamander_clock_func(Player p)
        {
            p.gain_stones(new stone_quant(1, 0, 0));
            p.gain_points(1);
            yield return null;
        }
        #endregion
        #region Sand Giant
        public static IEnumerator sand_giant_enter_effect(Player p)
        {
            p.gain_points(4*p.count_cards_in_family(CardFamily.G));
            yield return null;
        }
        #endregion
    }
}
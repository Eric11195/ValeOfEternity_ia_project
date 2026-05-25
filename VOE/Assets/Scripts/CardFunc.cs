using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace voe{
    public static class CardFuncs
    {
        #region defaults
        //public static bool cost_func(Player p, CardNameId cni)
        //{
        //    return p.can_pay(cni);
        //}
        public static bool bool_true_func(Player p) { return true; }
        public static IEnumerator unimplemented_func(Player p)
        {
            throw new UnityException("Unimplemented");  
        }
        public static IEnumerator void_func(Player p){
            yield return null;
        }
        public static IEnumerator dragon_enter_func(Player p, CardFamily cf, int points)
        {
            var enemy = p.choose_enemy_with_card_of_family(
                cf
            );
            Assert.IsTrue(enemy != null);
            p.gain_points(points, Player.points_src.etb);
            enemy.discard_card_by_type_from_table(cf);
            yield return null;
        }
        #endregion
        #region Aerie
        public static IEnumerator aerie_enter_func(Player p) {
            var card_chosen =
                p.choose_best_card_in_tableau(
                    CardFamily.None,
                    CardEffectTypes.none,
                    (int cost) =>{return true;},
                    CardNameId.Aeris
                );
            p.bounce_card(card_chosen);
            p.gain_points(CardData.get_card(card_chosen).price, Player.points_src.etb);
            yield return null;
        }
        #endregion
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
        #region Asmodeus
        public static IEnumerator asmodeus_clock_func(Player p)
        {
            var card_chosen =
                p.choose_best_card_in_tableau(
                    CardFamily.None,
                    CardEffectTypes.enter,
                    (int cost) => { return cost <= 2; },
                    CardNameId.Asmodeus
                );
            if(card_chosen != CardNameId.NONE)
            {
                p.bounce_card(card_chosen);
            }
            yield return null;
        }
        #endregion
        #region Balog
        public static IEnumerator balog_clock_func(Player p)
        {
            var card_chosen =
                p.choose_best_card_in_tableau(
                    CardFamily.R,
                    CardEffectTypes.enter,
                    (int cost) => { return true; },
                    CardNameId.Balog
                );
            if (card_chosen != CardNameId.NONE)
            {
                p.bounce_card(card_chosen);
            }
            yield return null;
        }
        #endregion
        #region Basilisk
        public static int basilisk_choosing_func(Player p)
        {
            int[] ponderated_options;

            if(p.stone_manager.get_number_of_spaces_to_fill() == 0)
                ponderated_options = new int[3]{1,0,0};
            else
                ponderated_options = new int[3]
                {
                    DecisionParameters.ponder_single_stone(p, stone_type.ST_one, 1),
                    DecisionParameters.ponder_single_stone(p, stone_type.ST_three, 1),
                    DecisionParameters.ponder_single_stone(p, stone_type.ST_six, 1)
                };
            int option = DecisionParameters.choose_best(ponderated_options);

            Logger.Log(Logger.player_log(p.idx,"choose option " + option.ToString() + " of basilisk clock action"), TextFilter.get_p_idx_message_src(p.idx));
            return option;
        }
        public static IEnumerator basilisk_clock_func(Player p)
        {
            switch (basilisk_choosing_func(p))
            {
                case 0:
                    p.gain_stones(new stone_quant(1, 0, 0));
                    break;
                case 1:
                    p.loose_points(1, Player.points_src.clock);
                    p.gain_stones(new stone_quant(0, 1, 0));
                    break;
                case 2:
                    p.loose_points(2, Player.points_src.clock);
                    p.gain_stones(new stone_quant(0, 0, 1));
                    break;
            }
            yield return new WaitForSeconds(GameManager.get_instance().get_standard_enemy_action_wait_time());
        }
        #endregion
        #region Behemoth
        public static IEnumerator behemoth_enter_func(Player p)
        {
            p.gain_points(3 * p.count_families(), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Boulder
        public static IEnumerator boulder_enter_func(Player p)
        {
            yield return GameManager._instance.StartCoroutine(dragon_enter_func(p, CardFamily.P, 8));
        }
        #endregion
        #region BurningSkull
        public static IEnumerator burning_skull_clock_func(Player p)
        {
            if (p.stone_manager.get_number_of_stones(stone_type.ST_one) > 0)
            {
                p.gain_points(3, Player.points_src.clock);
            }
            yield return null;
        }
        #endregion
        #region Cerberus
        public static List<CardNameId> cerberus_choose_to_discard(Player p)
        {
            int[] card_ponderations = new int[p.table.size()];

            for(int i = 0; i < p.table.size(); ++i)
            {
                var cni = p.table.get(i);
                if (cni == CardNameId.Cerberus ||
                    !CardEffectTypeUtils.has_card_effect(cni, CardEffectTypes.enter)) 
                {
                    card_ponderations[i] = 500;
                    continue;
                }

                card_ponderations[i] = 0;
                for(int j = 0; j < (int)card_flags_idx.COUNT; ++j)
                {
                    //This should return negative values if the card was synergistic
                    card_ponderations[i] += p.get_sinergies_rating_delta_without_card((card_flags_idx)j, cni);
                }
            }

            List<CardNameId> cnil = new(0);
            int best = DecisionParameters.choose_best(card_ponderations);
            while (cnil.Count < 3 && card_ponderations[best] <= 0)
            {
                cnil.Add(p.table.get(best));
                card_ponderations[best] = 500;
            }
            return cnil;
        }
        public static IEnumerator cerberus_enter_func(Player p)
        {
            var cnil = cerberus_choose_to_discard(p);
            foreach(var cni in cnil)
            {
                Logger.LogBold(Logger.player_log(p.idx, " Cerberus effect discards:"), TextFilter.get_p_idx_message_src(p.idx));
                p.discard_card_from_table(cni);
                yield return new WaitForSeconds(GameManager.get_instance().get_standard_enemy_action_wait_time());
            }
        }
        #endregion
        #region Boreas
        public static IEnumerator boreas_enter_func(Player p)
        {
            p.gain_points(p.count_cards_in_family(CardFamily.P), Player.points_src.etb);
            p.bounce_card(CardNameId.Boreas);
            yield return null;
        }
        #endregion
        #region Charybdis
        public static IEnumerator charybdis_clock_func(Player p)
        {
            if (p.stone_manager.get_number_of_stones(stone_type.ST_three) > 0)
            {
                p.gain_points(5, Player.points_src.clock);
            }
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
        #region Dragon Egg
        public static IEnumerator dragon_egg_enter_func(Player p)
        {
            var dragon_chosen = p.choose_best_card_in_hand(
                
                CardFamily.D,
                CardEffectTypes.none,
                (int cost) => { return true; },
                CardNameId.Dragonegg
            );
            p.discard_card_from_table(CardNameId.Dragonegg);
            if(dragon_chosen != CardNameId.NONE)
            {
                yield return GameManager._instance.StartCoroutine(p.play_card_without_paying(dragon_chosen));
            }
            yield return null;
        }
        #endregion
        #region Ember
        public static IEnumerator ember_enter_func(Player p)
        {
            yield return GameManager.get_instance().StartCoroutine(dragon_enter_func(p, CardFamily.B, 7));
        }
        #endregion
        #region Eternity
        public static IEnumerator eternity_enter_func(Player p)
        {
            p.gain_points(4 * p.count_families(), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Firefox
        public static IEnumerator firefox_enter_func(Player p)
        {
            p.gain_points(1 * p.count_cards_in_hand(), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Forest spirit
        public static CardNameId forest_spirit_choose_card(Player p)
        {
            int[] values = new int[p.hand.size()];
            int i = 0;
            foreach(var cni in p.hand.card_list)
            {
                values[i] = 0;
                var cd = CardData.get_card(cni);
                values[i] += cd.price * 4;
                values[i] -= p.get_sinergy_delta_rating_for_all_flags_with_new_card(cni);
            }
            int idx = DecisionParameters.choose_best(values);
            return p.hand.get(idx);
        }
        public static IEnumerator forest_spirit_enter_func(Player p)
        {
            yield return new WaitForSeconds(GameManager.get_instance().get_standard_enemy_action_wait_time());
            var cni = forest_spirit_choose_card(p);
            p.discard_card_from_hand(cni);
            p.gain_points(CardData.get_card(cni).price, Player.points_src.etb);
        }
        #endregion
        #region Freyja
        public static IEnumerator freyja_clock_func(Player p)
        {
            p.gain_points(1 * p.count_cards_with_clocks(), Player.points_src.clock);
            yield return null;
        }
        #endregion
        #region Gargoyle
        public static void gargoyle_trigger_func(Player p, stone_quant q)
        {
            if (q.s[(int)stone_type.ST_six] > 0)
            {
                p.gain_points(3, Player.points_src.infinite);
            }
        }
        public static IEnumerator gargoyle_enter_func(Player p)
        {
            p.pay_for_card_event += gargoyle_trigger_func;
            yield return null;
        }
        public static IEnumerator gargoyle_exit_func(Player p)
        {
            p.pay_for_card_event -= gargoyle_trigger_func;
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
        #region Genie Exalted
        public static IEnumerator genie_exalted_clock_func(Player p)
        {
            var card = p.choose_best_card_in_tableau(
                
                CardFamily.None,
                CardEffectTypes.clock,
                (int cost) => { return true; },
                CardNameId.Genieexalted
            );
            if(card != CardNameId.NONE)
            {
                yield return GameManager.get_instance().StartCoroutine(p.activate_single_clock(card));
            }
            yield return null;
        }
        #endregion
        #region Gi-rin
        public static IEnumerator girin_enter_func(Player p)
        {
            p.gain_points(2 * p.count_cards_in_table(), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Goblin
        public static IEnumerator goblin_clock_func(Player p)
        {
            var enemy = p.choose_enemy();
            p.gain_points(1, Player.points_src.clock);
            enemy.loose_points(1, Player.points_src.clock);
            yield return null;
        }
        #endregion
        #region Goblin soldier
        public static IEnumerator goblin_soldier_clock_func(Player p)
        {
            var gm = GameManager.get_instance();
            if (gm.is_this_player_not_leading(p))
            {
                Logger.Log(Logger.player_log(p.idx, "Goblin soldier asks is this player leading. NO"), TextFilter.get_p_idx_message_src(p.idx));
                p.gain_points(4, Player.points_src.clock);
            }
            else
            {
                Logger.Log(Logger.player_log(p.idx, "Goblin soldier asks: is this player leading? YES"), TextFilter.get_p_idx_message_src(p.idx));
                p.loose_points(4, Player.points_src.clock);
            }
            yield return null;
        }
        #endregion
        #region Griffon
        public static IEnumerator griffon_clock_func(Player p)
        {
            p.draw();
            yield return null;
        }
        #endregion
        #region Gust
        public static IEnumerator gust_enter_func(Player p)
        {
            yield return GameManager.get_instance().StartCoroutine(dragon_enter_func(p, CardFamily.G, 8));
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
                p.gain_points(3, Player.points_src.etb);
            }
            yield return null;
        }
        #endregion
        #region Hestia
        public static IEnumerator hestia_enter_func(Player p)
        {
            p.stone_manager.max_stones += 2;
            Assert.IsTrue(p.stone_manager.get_max_num_of_stones()==6);
            yield return null;
        }
        public static IEnumerator hestia_exit_func(Player p)
        {
            p.stone_manager.max_stones -= 2;
            Assert.IsTrue(p.stone_manager.get_max_num_of_stones() == 4);
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
        #region Hydra
        public static int[] hydra_option_choser(Player p)
        {
            int[] opts = new int[4];
            for (int opt = 0; opt < 4; ++opt) opts[opt] = 0;

            opts[0] = DecisionParameters.ponder_single_stone(p, stone_type.ST_six, 1) * 
                (p.player_prio==priorities.store_stones?3:1);
            opts[1] = 3 * p.get_sinergy_complete_rating(card_flags.big_hand) *
                (p.player_prio == priorities.store_stones ? 3 : 1);
            opts[2] = DecisionParameters.ponder_single_stone(p, stone_type.ST_three, 1) *
                (p.player_prio == priorities.store_stones ? 3 : 1);
            opts[3] = 4 * (p.player_prio == priorities.gain_points ? 3 : 1);

            int[] final_chosen = new int[2];
            for (int i = 0; i < final_chosen.Length; ++i)
            {
                int best = DecisionParameters.choose_best(opts);
                opts[best] = -100;
                final_chosen[i] = best;
            }
            return final_chosen;
        }
        public static IEnumerator hydra_enter_func(Player p)
        {
            var gm = GameManager.get_instance();

            var hydra_options = hydra_option_choser(p);
            Assert.IsTrue(hydra_options.Length==2);
            foreach(int h in hydra_options)
            {
                Logger.Log(Logger.player_log(p.idx, " chooses option " + h + " from hydra enter func"), TextFilter.get_p_idx_message_src(p.idx));
                switch (h)
                {
                    case 0:
                        p.gain_stones(new stone_quant(0, 0, 1)); break;
                    case 1:
                        p.draw() ; break;
                    case 2:
                        p.gain_stones(new stone_quant(0,2,0)); break;
                    case 3:
                        p.gain_points(4, Player.points_src.etb); break;
                }
                yield return new WaitForSeconds(gm.get_standard_enemy_action_wait_time());
            }
        }
        #endregion
        #region Ifrit
        public static IEnumerator ifrit_enter_func(Player p)
        {
            p.gain_points(1 * p.count_cards_in_table(), Player.points_src.etb);
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
            p.gain_points(2*incubus_count(p), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Kappa
        public static void kappa_trigger_func(Player p, stone_quant sq)
        {
            if (sq.s[(int)stone_type.ST_three] > 0)
            {
                p.gain_points(2, Player.points_src.infinite);
            }
        }
        public static IEnumerator kappa_enter_func(Player p)
        {
            p.pay_for_card_event += kappa_trigger_func;
            yield return null;
        }
        public static IEnumerator kappa_exit_func(Player p)
        {
            p.pay_for_card_event -= kappa_trigger_func;
            yield return null;
        }
        #endregion
        #region Lava Giant
        public static IEnumerator lava_giant_enter_func(Player p)
        {
            p.gain_points(2 * p.count_cards_in_family(CardFamily.R), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Leviathan
        public static IEnumerator leviathan_enter_func(Player p)
        { 
            yield return GameManager.get_instance().StartCoroutine(dragon_enter_func(p, CardFamily.D, 7));
        }
        #endregion
        #region Marina
        public static IEnumerator marina_enter_func(Player p)
        {
            yield return GameManager.get_instance().StartCoroutine(dragon_enter_func(p, CardFamily.R, 7));
        }
        #endregion
        #region Medusa
        public static IEnumerator medusa_clock_func(Player p)
        {
            if (p.count_cards_in_hand() > 0)
            {
                var card = p.choose_worst_card_in_hand(
                    
                    CardFamily.None,
                    CardEffectTypes.none,
                    (int cost) => { return true; }
                );
                yield return p.discard_card_from_hand(card);
                p.gain_stones(new stone_quant(0, 0, 1));
            }
            yield return null;
        }
        #endregion
        #region Mimic
        public static CardNameId mimic_choser_func(Player p)
        {
            var gm = GameManager.get_instance();
            CardNameId cni = DecisionParameters.choose_best_card(p, gm.deck.discard_pile, CardFamily.G, CardEffectTypes.none, (int cost) => { return true; }, p.player_prio,false);
            return cni;
        }
        public static IEnumerator mimic_clock_func(Player p)
        {
            var cni = mimic_choser_func(p);
            if(cni!=CardNameId.NONE)
                p.add_to_hand_unchecked(cni);
            yield return new WaitForSeconds(GameManager.get_instance().get_standard_enemy_action_wait_time());
        }
        #endregion
        #region MudSlime
        public static IEnumerator mud_slime_enter_func(Player p)
        {
            p.gain_points(6, Player.points_src.etb);
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
                p.gain_points(2, Player.points_src.clock);
            }
            yield return null;
        }
        #endregion
        #region Odin
        public static IEnumerator odin_clock_func(Player p)
        {
            if (p.count_cards_in_hand() < 6)
            {
                p.gain_points(2, Player.points_src.clock);
            }
            else
            {
                p.gain_stones(new stone_quant(0, 0, 1));
            }
            yield return null;
        }
        #endregion
        #region Pegasus
        public static IEnumerator pegasus_enter_func(Player p)
        {
            p.draw();
            for (int i = 1; i < p.card_reduction_cost_by_family.Length; i++)
            {
                p.card_reduction_cost_by_family[i] += 1;
            }
            yield return null;
        }
        public static IEnumerator pegasus_exit_func(Player p)
        {
            for (int i = 0; i < p.card_reduction_cost_by_family.Length; i++)
            {
                p.card_reduction_cost_by_family[family_idx.get_card_family_idx(CardFamily.P)] -= 1;
            }
            yield return null;
        }
        #endregion
        #region Phoenix
        public static void phoenix_trigger_func(Player p, stone_quant q)
        {
            int number_of_1s = q.s[(int)stone_type.ST_one];
            p.gain_points(number_of_1s, Player.points_src.infinite);
        }
        public static IEnumerator phoenix_enter_func(Player p)
        {
            p.pay_for_card_event += phoenix_trigger_func;
            yield return null;
        }
        public static IEnumerator phoenix_exit_func(Player p)
        {
            p.pay_for_card_event -= phoenix_trigger_func;
            yield return null;
        }
        #endregion
        #region Poseidon
        public static IEnumerator poseidon_enter_func(Player p)
        {
            p.gain_points(3 * p.count_cards_in_family(CardFamily.B), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Rock Golem
        public static IEnumerator rock_golem_enter_func(Player p)
        {
            p.gain_points(
                p.stone_manager.sa.s[(int)stone_type.ST_six]*
                p.stone_manager.get_value(stone_type.ST_six),
                Player.points_src.etb
            );
            yield return null;
        }
        #endregion
        #region Rudra
        public static IEnumerator rudra_enter_effect(Player p)
        {
            p.gain_points(2 * p.count_cards_in_hand(), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Salamander
        public static IEnumerator salamander_clock_func(Player p)
        {
            p.gain_stones(new stone_quant(1, 0, 0));
            p.gain_points(1, Player.points_src.clock);
            yield return null;
        }
        #endregion
        #region Sand Giant
        public static IEnumerator sand_giant_enter_effect(Player p)
        {
            p.gain_points(4*p.count_cards_in_family(CardFamily.G), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Scorch
        public static IEnumerator scorch_enter_func(Player p)
        {
            var card = p.choose_best_card_in_tableau(
                
                CardFamily.None,
                CardEffectTypes.enter,
                (int cost) => { return true; },
                CardNameId.Scorch
            );
            if(card != CardNameId.NONE)
            {
                var card_data = CardData.get_card(card);
                yield return GameManager.get_instance().StartCoroutine(card_data.enterEffect(p));
            }
            yield return null;
        }
        #endregion
        #region Sea Spirit
        public static IEnumerator sea_spirit_clock_func(Player p)
        {
            p.gain_points(p.stone_manager.sa.s[(int)stone_type.ST_three], Player.points_src.clock);
            yield return null;
        }
        #endregion
        #region Snail Maiden
        public static int snail_maiden_choose_func(Player p)
        {
            if(!p.stone_manager.has(new stone_quant(0, 1, 0)) && !p.stone_manager.has(new stone_quant(0, 0, 1))){
                return -1;
            }
            if(p.stone_manager.has(new stone_quant(0, 1, 0)) && p.stone_manager.has(new stone_quant(0, 0, 1)))
            {
                //Choose
                int[] ponder = new int[2];

                ponder[0] = DecisionParameters.ponder_stones_gain(p, new stone_quant(0,-1,1));
                ponder[1] = DecisionParameters.ponder_stones_gain(p, new stone_quant(0, 3, -1));

                return DecisionParameters.choose_best(ponder);
            }
            if(p.stone_manager.has(new stone_quant(0, 1, 0)))
            {
                return 0;
            }
            else if (p.stone_manager.has(new stone_quant(0, 0, 1)))
            {
                return 1;
            }
            else
            {
                throw new UnityException("Non valid state reached in snail maiden choose func");
            }

        }
        public static IEnumerator snail_maiden_clock_func(Player p)
        {
            int option = snail_maiden_choose_func(p);

            if (option == 0)
            {
                Assert.IsTrue(p.stone_manager.has(new stone_quant(0,1,0)));
                Logger.Log(Logger.player_log(p.idx, " exchanging a '3' stone for a '6' stone via snail maiden clock effect"), TextFilter.get_p_idx_message_src(p.idx));
                p.stone_manager.sa.s[(int)stone_type.ST_three] -= 1;
                p.gain_stones(new stone_quant(0, 0, 1));
            }else if(option == 1)
            {
                Assert.IsTrue(p.stone_manager.has(new stone_quant(0, 0, 1)));
                Logger.Log(Logger.player_log(p.idx, " exchanging a '6' stone for three '3' stones via snail maiden clock effect"), TextFilter.get_p_idx_message_src(p.idx));
                p.stone_manager.sa.s[(int)stone_type.ST_six] -= 1;
                p.gain_stones(new stone_quant(0, 0, 1));
            }else if (option == -1)
            {
                Assert.IsFalse(p.stone_manager.has(new stone_quant(0, 1, 0)) || p.stone_manager.has(new stone_quant(0, 0, 1)));
                Logger.Log(Logger.player_log(p.idx, " snail maiden clock effect does nothing. Requirements not met."), TextFilter.get_p_idx_message_src(p.idx));
            }
            else
            {
                throw new UnityException("Snail maiden is chosing a non valid option");
            }
            yield return null;
        }
        #endregion
        #region Stone Golem
        public static IEnumerator stone_golem_enter_func(Player p)
        {
            int n = p.stone_manager.get_number_of_stones();
            p.stone_manager.sa = new stone_quant(0, 0, n);
            yield return null;
        }
        #endregion
        #region Succubus
        public static bool check_succubus(Player p)
        {
            long count = 0;
            foreach (var c in p.table.card_list)
            {
                int price = CardData.get_card(c).price;
                price -= 1;
                count |= (1L << price);
            }
            //15 L is 1111 in binary, so this checks if there are cards of price 1,2,3 and 4 in the tableau
            return ((count&15L)==15L);
        }
        public static IEnumerator succubus_enter_effect(Player p)
        {
            if(check_succubus(p))
            {
                p.gain_points(10, Player.points_src.etb);
            }
            yield return null;
        }
        #endregion
        #region Surtr
        public static IEnumerator srtr_enter_effect(Player p)
        {
            p.gain_points(2 * p.count_families(), Player.points_src.clock);
            yield return null;
        }
        #endregion
        #region Sylph
        public static void sylph_trigger_func(Player p)
        {
            p.gain_points(1, Player.points_src.infinite);
        }
        public static IEnumerator sylph_enter_func(Player p)
        {
            p.draw();
            p.card_enters_tableau_event += sylph_trigger_func;
            yield return null;
        }
        public static IEnumerator sylph_exit_func(Player p)
        {
            p.card_enters_tableau_event -= sylph_trigger_func;
            yield return null;
        }
        #endregion
        #region Tengu
        public static IEnumerator tengu_enter_effect(Player p)
        {
            p.gain_points(6, Player.points_src.etb);
            Assert.IsTrue(p.table.contains(CardNameId.Tengu));
            p.table.extract(CardNameId.Tengu);
            GameManager._instance.deck.put_card_on_top(CardNameId.Tengu);
            yield return null;
        }
        #endregion
        #region Tidal
        public static IEnumerator tidal_enter_effect(Player p)
        {
            p.gain_points(5 * p.count_cards_in_family(CardFamily.D), Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Triton
        public static void triton_trigger_effect(Player p, CardFamily cf)
        {
            if (cf == CardFamily.B)
            {
                p.gain_stones(new stone_quant( 0,2,0));
            }
        }
        public static IEnumerator triton_enter_effect(Player p)
        {
            p.tame_card_event += triton_trigger_effect;
            yield return null;
        }
        public static IEnumerator triton_exit_effect(Player p)
        {
            p.tame_card_event -= triton_trigger_effect;
            yield return null;
        }
        #endregion
        #region Troll
        public static IEnumerator troll_clock_effect(Player p)
        {
            if(p.stone_manager.sa.s[(int)stone_type.ST_six] > 0)
            {
                p.gain_points(3, Player.points_src.clock);
            }
            yield return null;
        }
        #endregion
        #region Undine
        public static IEnumerator undine_enter_effect(Player p)
        {
            p.gain_stones(new stone_quant(0, 1, 0));
            yield return null;
        }
        public static IEnumerator undine_clock_effect(Player p)
        {
            p.bounce_card(CardNameId.Undine);
            yield return null;
        }
        #endregion
        #region Undine Queen
        public static IEnumerator undine_queen_clock_effect(Player p)
        {
            p.gain_stones(new stone_quant(0, 1, 0));
            yield return null;
        }
        #endregion
        #region Valkyrie
        public static IEnumerator valkyrie_clock_effect(Player p)
        {
            p.gain_points(p.count_families(), Player.points_src.clock);
            yield return null;
        }
        #endregion
        #region Water Giant
        public static IEnumerator water_giant_enter_effect(Player p)
        {
            p.gain_stones(new stone_quant(0, 2, 0));
            p.stone_manager.sv.s[(int)stone_type.ST_three] += 1;
            p.stone_manager.sv.s[(int)stone_type.ST_six] += 1;
            yield return null;
        }
        public static IEnumerator water_giant_exit_effect(Player p)
        {
            p.stone_manager.sv.s[(int)stone_type.ST_three] -= 1;
            p.stone_manager.sv.s[(int)stone_type.ST_six] -= 1;
            yield return null;
        }
        #endregion
        #region Willow
        public static IEnumerator willow_enter_effect(Player p)
        {
            p.gain_stones(new stone_quant(1, 1, 1));
            p.gain_points(3, Player.points_src.etb);
            yield return null;
        }
        #endregion
        #region Young Forest Spirit
        public static IEnumerator young_forest_spirit_enter_effect(Player p)
        {
            if (p.hand.size() > 1)
            {
                var worst_card = p.choose_worst_card_in_hand(
                    
                    CardFamily.None,
                    CardEffectTypes.none,
                    (int cost) => { return true; }
                );
                var best_card = p.choose_best_card_in_hand(
                    
                    CardFamily.None,
                    CardEffectTypes.enter,
                    (int cost) => { return true; },
                    CardNameId.Youngforestspirit
                );
                p.discard_card_from_hand(worst_card);
                p.play_card(best_card);
            }
            yield return null;
        }
        #endregion
        #region Yuki Onna
        public static IEnumerator yuki_onna_enter_effect(Player p)
        {
            p.gain_points(p.stone_manager.get_total_value(), Player.points_src.etb);
            p.stone_manager.clear_stones();
            yield return null;
        }
        #endregion
        #region Yuki Onna Exalted
        public static IEnumerator yuki_onna_exalted_effect(Player p)
        {
            p.gain_points(
                p.stone_manager.sa.s[(int)stone_type.ST_three] * 
                p.stone_manager.sv.s[(int)stone_type.ST_three],
                Player.points_src.etb
            );
            yield return null;
        }
        #endregion
    }
}
using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace voe{
    public delegate IEnumerator CardFunc(Player p);
    public delegate bool can_be_played_func(Player p);

    public class PlayerResources
    {
        public int points;
        public int cards_in_hand;
        public stone_quant sq;
        public PlayerResources(int _points, int _cards_in_hand, stone_quant _sq)
        {
            points = _points;
            cards_in_hand = _cards_in_hand;
            sq = _sq;
        }
        public PlayerResources( stone_quant _sq)
        {
            points = 0;
            cards_in_hand = 0;
            sq = _sq;
        }
        public PlayerResources(int _points, int _cards_in_hand)
        {
            points = _points;
            cards_in_hand = _cards_in_hand;
            sq = new stone_quant(0,0,0);
        }
        public PlayerResources()
        {
            points = 0;
            cards_in_hand = 0;
            sq = new stone_quant(0, 0, 0);
        }
    }
    public class CardInputOutput
    {
        public PlayerResources input;
        public PlayerResources output;
        public CardInputOutput()
        {
            input = new PlayerResources();
            output = new PlayerResources();
        }
        public CardInputOutput(PlayerResources input, PlayerResources output)
        {
            this.input = input;
            this.output = output;
        }
    }
    public class CardData
    {   
        public int price;
        public CardFamily family;
        public CardFunc enterEffect;
        public CardFunc clockEffect;
        public CardFunc exitEffect;
        public card_flags enabler;
        public card_flags payoff;
        public CardEffectTypes effect_type;
        public can_be_played_func can_be_played;
        public CardInputOutput clock_expect;
        public CardData(int _price, CardFamily cf, CardFunc _enter, CardFunc _clock, CardFunc _exit, card_flags _enabler, card_flags _payoff, CardEffectTypes _ce, can_be_played_func _can_be_played, CardInputOutput _clock_expect){
            price = _price; family = cf;
            enterEffect = _enter;
            clockEffect = _clock;
            exitEffect = _exit;
            enabler = _enabler;
            payoff = _payoff;
            effect_type = _ce;
            can_be_played = _can_be_played;
            clock_expect = _clock_expect;
        }
        public CardData(int _price, CardFamily cf, CardFunc _enter, CardFunc _clock, CardFunc _exit, card_flags _enabler, card_flags _payoff, CardEffectTypes _ce, can_be_played_func _can_be_played)
        {
            price = _price; family = cf;
            enterEffect = _enter;
            clockEffect = _clock;
            exitEffect = _exit;
            enabler = _enabler;
            payoff = _payoff;
            effect_type = _ce;
            can_be_played = _can_be_played;
            clock_expect = new CardInputOutput();
        }

        public static CardData get_card(CardNameId cid){
            Assert.IsTrue((int)cid >= 0 && (int)cid< (int)CardNameId.NUMBER_OF_CARDS);
            return cd[(int)cid];
        }

        private static CardData[] cd = {
            //X 0 Aeris,
            new CardData(9, CardFamily.D,
                CardFuncs.aerie_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.recursion | card_flags.etbs | card_flags.space_free,
                card_flags.high_costs,
                CardEffectTypes.enter,
                (Player p)=>{
                    return
                        p.has_card_with_requiriment(
                            p.table,
                            CardFamily.None,
                            CardEffectTypes.none,
                            (int cost)=>{return true; }
                        );
                }
            ),
            //X 1 Agni,
            new CardData(4,CardFamily.R,
                CardFuncs.agni_enter_func,
                CardFuncs.void_func,
                CardFuncs.agni_exit_func,
                card_flags.stones1 | card_flags.high_costs,
                card_flags.none,
                CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 2 Asmodeus,
            new CardData(4,CardFamily.R,
                CardFuncs.void_func,
                CardFuncs.asmodeus_clock_func,
                CardFuncs.void_func,
                card_flags.clocks | card_flags.recursion | card_flags.low_costs | card_flags.space_free,
                card_flags.none,
                CardEffectTypes.clock,
                (Player p)=>{
                    return
                        p.has_card_with_requiriment(
                            p.table,
                            CardFamily.None,
                            CardEffectTypes.enter,
                            (int cost)=>{return cost<=2; }
                        );
                },
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(0,1)
                    )
            ),
            //X 3 Balog,
            new CardData(4,CardFamily.R,
                CardFuncs.void_func,
                CardFuncs.balog_clock_func,
                CardFuncs.void_func,
                card_flags.clocks | card_flags.recursion | card_flags.space_free | card_flags.familyR | card_flags.space_free,
                card_flags.none,
                CardEffectTypes.clock,
                (Player p)=>{
                    return
                        p.has_card_with_requiriment(
                            p.table,
                            CardFamily.R,
                            CardEffectTypes.enter,
                            (int cost)=>{return true; }
                        );
                },
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(0,1)
                    )
            ),
            // 4 Basilisk,
            new CardData(3,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.clocks | card_flags.stones1 | card_flags.stones3 | card_flags.stones6 | card_flags.loose_points,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,2),
                    new PlayerResources(new stone_quant(1,1,1))
                    )
            ),
            //X 5 Behemoth,
            new CardData(9,CardFamily.G,
                CardFuncs.behemoth_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.number_of_families,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 6 Boreas,
            new CardData(4,CardFamily.P,
                CardFuncs.boreas_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.multicast,
                card_flags.cost_reduction,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 7 Boulder,
            new CardData(8,CardFamily.D,
                CardFuncs.boulder_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.removal | card_flags.etbs,
                card_flags.none,
                CardEffectTypes.enter,
                (Player p)=>{
                    return OpponentChoosing.opponent_has_card_with_card_family(p, CardFamily.P);
                }
            ),
            //X 8 Burningskull,
            new CardData(3,CardFamily.R,
                CardFuncs.void_func,
                CardFuncs.burning_skull_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.stones1,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(new stone_quant(1,0,0)),
                    new PlayerResources(3,0)
                    )

            ),
            // 9 Cerberus,
            new CardData(5,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.space_free,
                card_flags.none,
                CardEffectTypes.enter,
                (Player p)=>{
                    return p.table.size() > 1;
                }
            ),
            //X 10 Charybdis,
            new CardData(5,CardFamily.B,
                CardFuncs.void_func,
                CardFuncs.charybdis_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.stones3,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(new stone_quant(0,1,0)),
                    new PlayerResources(5,0)
                    )
            ),
            //X 11 Dandelionspirit,
            new CardData(3,CardFamily.P,
                CardFuncs.dandelion_enter_func,
                CardFuncs.dandelion_clock_func,
                CardFuncs.void_func,
                card_flags.clocks | card_flags.space_free | card_flags.multicast | card_flags.big_hand,
                card_flags.none,
                CardEffectTypes.enter | CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(),
                    new PlayerResources(0,1)
                    )
            ),
            //X 12 Dragonegg,
            new CardData(3,CardFamily.D,
                CardFuncs.dragon_egg_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.multicast | card_flags.high_costs | card_flags.familyD,
                card_flags.none,
                CardEffectTypes.enter,
                (Player p)=>{
                    return p.free_slots_on_table() >= 2 &&
                        p.has_card_with_requiriment(p.hand, CardFamily.D, CardEffectTypes.none, (int cost)=>{return true; });
                }
            ),
            //X 13 Ember,
            new CardData(7,CardFamily.D,
                CardFuncs.ember_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.removal | card_flags.etbs,
                card_flags.none,
                CardEffectTypes.enter,
                (Player p)=>{
                    return OpponentChoosing.opponent_has_card_with_card_family(p, CardFamily.B);
                }
            ),
            //X 14 Eternity,
            new CardData(12,CardFamily.D,
                CardFuncs.eternity_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.number_of_families,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 15 Firefox,
            new CardData(1,CardFamily.R,
                CardFuncs.firefox_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.big_hand | card_flags.recursion,
                CardEffectTypes.enter,
                (Player p) =>
                {
                    return p.hand.size() >= 3;
                }
            ),
            // 16 Forestspirit,
            new CardData(2,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.etbs,
                card_flags.high_costs | card_flags.big_hand | card_flags.recursion,
                CardEffectTypes.enter,
                (Player p) =>
                {
                    return p.hand.size()>0;
                }

            ),
            //X 17 Freyja,
            new CardData(7,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.freyja_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.clocks,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(),
                    new PlayerResources(0,3)
                    )
            ),
            //X 18 Gargoyle,
            new CardData(2,CardFamily.G,
                CardFuncs.gargoyle_enter_func,
                CardFuncs.void_func,
                CardFuncs.gargoyle_exit_func,
                card_flags.none,
                card_flags.stones6,
                CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 19 Genie,
            new CardData(4,CardFamily.P,
                CardFuncs.genie_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.clocks | card_flags.recursion,
                CardEffectTypes.enter,
                (Player p)=>{
                    return p.has_card_with_requiriment(p.table, CardFamily.None, CardEffectTypes.clock, (int cost)=>{return true; });
                }
            ),
            //X 20 Genieexalted,
            new CardData(5,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.genie_exalted_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.clocks,
                CardEffectTypes.clock,
                (Player p)=>{
                    return p.has_card_with_requiriment(p.table, CardFamily.None, CardEffectTypes.clock, (int cost)=>{return true; });
                }
            ),
            //X 21 Gi_rin,
            new CardData(10,CardFamily.P,
                CardFuncs.girin_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.number_of_families,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 22 Goblin,
            new CardData(1,CardFamily.G,
                CardFuncs.void_func,
                CardFuncs.goblin_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(1,0)
                    )
            ),
            // 23 Goblinsoldier,
            new CardData(4,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.loose_points | card_flags.clocks,
                card_flags.loose_points,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(4,0),
                    new PlayerResources(4,0)
                    )
            ),
            //X 24 Griffon,
            new CardData(7,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.griffon_clock_func,
                CardFuncs.void_func,
                card_flags.big_hand,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(0,1)
                    )
            ),
            //X 25 Gust,
            new CardData(8,CardFamily.D,
                CardFuncs.gust_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.removal | card_flags.etbs,
                card_flags.none,
                CardEffectTypes.enter,
                (Player p)=>{
                    return OpponentChoosing.opponent_has_card_with_card_family(p, CardFamily.G);
                }
            ),
            //X 26 Hae_tae,
            new CardData(3,CardFamily.B,
                CardFuncs.haetae_enter_n_exit_func,
                CardFuncs.void_func,
                CardFuncs.haetae_enter_n_exit_func,
                card_flags.stones3,
                card_flags.none,
                CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 27 Harpy,
            new CardData(3,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.harpy_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.big_hand,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(3,0)
                    )
            ),
            //X 28 Hestia,
            new CardData(0,CardFamily.R,
                CardFuncs.hestia_enter_func,
                CardFuncs.void_func,
                CardFuncs.hestia_exit_func,
                card_flags.stones1 | card_flags.stones3 | card_flags.stones6,
                card_flags.none,
                CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 29 Hornedsalamander,
            new CardData(2,CardFamily.R,
                CardFuncs.void_func,
                CardFuncs.horned_salamander_clock_func,
                CardFuncs.void_func,
                card_flags.stones1 | card_flags.clocks,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(),
                    new PlayerResources(new stone_quant(4,0,0))
                    )
            ),
            // 30 Hydra,
            new CardData(4,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.stones6 | card_flags.stones3 | card_flags.big_hand | card_flags.etbs,
                card_flags.none | card_flags.recursion,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 31 Ifrit,
            new CardData(2,CardFamily.R,
                CardFuncs.ifrit_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs | card_flags.recursion,
                card_flags.tableau_width ,
                CardEffectTypes.enter,
                (Player p)=>{
                    return p.table.size() > 3;
                }
            ),
            //X 32 Imp,
            new CardData(0,CardFamily.R,
                CardFuncs.imp_enter_func,
                CardFuncs.imp_clock_func,
                CardFuncs.void_func,
                card_flags.space_free | card_flags.clocks | card_flags.stones1 | card_flags.multicast,
                card_flags.none,
                CardEffectTypes.enter | CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(),
                    new PlayerResources(0,1)
                    )
            ),
            //X 33 Incubus,
            new CardData(2,CardFamily.R,
                CardFuncs.incubus_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs | card_flags.recursion,
                card_flags.low_costs | card_flags.tableau_width,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            // 34 Kappa,
            new CardData(1,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.none,
                card_flags.stones3,
                CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 35 Lavagiant,
            new CardData(3,CardFamily.R,
                CardFuncs.lava_giant_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.familyR | card_flags.tableau_width | card_flags.recursion,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 36 Leviathan,
            new CardData(4,CardFamily.B,
                CardFuncs.leviathan_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.removal | card_flags.etbs,
                card_flags.recursion,
                CardEffectTypes.enter,
                (Player p)=>{
                    return OpponentChoosing.opponent_has_card_with_card_family(p, CardFamily.D);
                }
            ),
            //X 37 Marina,
            new CardData(7,CardFamily.D,
                CardFuncs.marina_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.removal | card_flags.etbs,
                card_flags.none,
                CardEffectTypes.enter,
                (Player p)=>{
                    return OpponentChoosing.opponent_has_card_with_card_family(p, CardFamily.R);
                }
            ),
            //X 38 Medusa,
            new CardData(4,CardFamily.G,
                CardFuncs.medusa_clock_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.stones6 | card_flags.clocks,
                card_flags.none,
                CardEffectTypes.clock,
                (Player p)=>{
                    return p.hand.size() > 1;
                },
                new CardInputOutput(
                    new PlayerResources(0,1),
                    new PlayerResources(new stone_quant(0,0,1))
                    )
            ),
            // 39 Mimic,
            new CardData(6,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.big_hand | card_flags.clocks | card_flags.familyG,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(0,1)
                    )
            ),
            //X 40 Mudslime,
            new CardData(6,CardFamily.G,
                CardFuncs.mud_slime_enter_func,
                CardFuncs.mud_slime_clock_func,
                CardFuncs.void_func,
                card_flags.multicast | card_flags.etbs | card_flags.space_free,
                card_flags.none,
                CardEffectTypes.enter | CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(0,1)
                    )
            ),
            //X 41 Nessie,
            new CardData(2,CardFamily.B,
                CardFuncs.void_func,
                CardFuncs.nessie_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.none,
                CardEffectTypes.clock,
                (Player p)=>{
                    return !p.has_card_with_requiriment(p.table, CardFamily.D, CardEffectTypes.none, (int cost)=>{return true; });
                },
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(2,0)
                    )
            ),
            //X 42 Odin,
            new CardData(6,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.odin_clock_func,
                CardFuncs.void_func,
                card_flags.stones6,
                card_flags.big_hand,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(2,0,new stone_quant(0,0,1))
                    )
            ),
            //X 43 Pegasus,
            new CardData(3,CardFamily.P,
                CardFuncs.pegasus_enter_func,
                CardFuncs.void_func,
                CardFuncs.pegasus_exit_func,
                card_flags.cost_reduction | card_flags.big_hand,
                card_flags.none,
                CardEffectTypes.infinite |CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            // 44 Phoenix,
            new CardData(3,CardFamily.R,
                CardFuncs.phoenix_enter_func,
                CardFuncs.void_func,
                CardFuncs.phoenix_exit_func,
                card_flags.none,
                card_flags.multicast | card_flags.stones1,
                CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 45 Poseidon,
            new CardData(7,CardFamily.B,
                CardFuncs.poseidon_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.familyB | card_flags.tableau_width,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 46 Rockgolem,
            new CardData(6,CardFamily.G,
                CardFuncs.rock_golem_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.stones6,
                CardEffectTypes.enter,
                (Player p) =>
                {
                    return p.stone_manager.sa.s[(int)stone_type.ST_six] > 2;
                }
            ),
            //X 47 Rudra,
            new CardData(8,CardFamily.P,
                CardFuncs.rudra_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.big_hand,
                CardEffectTypes.enter,
                (Player p)=>{
                    return p.hand.size()>4;
                }
            ),
            //X 48 Salamander,
            new CardData(1,CardFamily.R,
                CardFuncs.void_func,
                CardFuncs.salamander_clock_func,
                CardFuncs.void_func,
                card_flags.clocks | card_flags.stones1,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(0,0),
                    new PlayerResources(1,0, new stone_quant(1,0,0))
                    )
            ),
            //X 49 Sandgiant,
            new CardData(10,CardFamily.G,
                CardFuncs.sand_giant_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.familyG | card_flags.tableau_width,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 50 Scorch,
            new CardData(9,CardFamily.D,
                CardFuncs.scorch_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.none,
                card_flags.etbs,
                CardEffectTypes.enter,
                (Player p) =>
                {
                    return p.has_card_with_requiriment(p.table, CardFamily.None, CardEffectTypes.enter, (int cost)=>{return true; });
                }
            ),
            //X 51 Seaspirit,
            new CardData(1,CardFamily.B,
                CardFuncs.void_func,
                CardFuncs.sea_spirit_clock_func,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.stones3,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(new stone_quant(0,1,0)),
                    new PlayerResources(6,0)
                    )
            ),
            // 52 Snailmaiden,
            new CardData(3,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.clocks | card_flags.stones3 | card_flags.stones6,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(new stone_quant(0,1,1)),
                    new PlayerResources(new stone_quant(0,3,1))
                    )
            ),
            // 53 Stonegolem,
            new CardData(6,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                card_flags.stones6 | card_flags.etbs,
                card_flags.none,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 54 Succubus,
            new CardData(0,CardFamily.R,
                CardFuncs.succubus_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.tableau_width | card_flags.low_costs | card_flags.recursion,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func

            ),
            //X 55 Surtr,
            new CardData(4,CardFamily.R,
                CardFuncs.srtr_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.number_of_families | card_flags.recursion,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 56 Sylph,
            new CardData(4,CardFamily.P,
                CardFuncs.sylph_enter_func,
                CardFuncs.void_func,
                CardFuncs.sylph_exit_func,
                card_flags.big_hand,
                card_flags.multicast,
                CardEffectTypes.enter | CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 57 Tengu,
            new CardData(3,CardFamily.P,
                CardFuncs.tengu_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.space_free,
                card_flags.none,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 58 Tidal,
            new CardData(5,CardFamily.D,
                CardFuncs.tidal_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.etbs,
                card_flags.familyD | card_flags.recursion,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(new stone_quant(0,0,1)),
                    new PlayerResources(6,0)
                    )
            ),
            //X 59 Triton,
            new CardData(4,CardFamily.B,
                CardFuncs.triton_enter_effect,
                CardFuncs.void_func,
                CardFuncs.triton_exit_effect,
                card_flags.stones3,
                card_flags.familyB,
                CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 60 Troll,
            new CardData(3,CardFamily.G,
                CardFuncs.void_func,
                CardFuncs.troll_clock_effect,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.stones6,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func
            ),
            //X 61 Undine,
            new CardData(1,CardFamily.B,
                CardFuncs.undine_enter_effect,
                CardFuncs.undine_clock_effect,
                CardFuncs.void_func,
                card_flags.stones3 | card_flags.multicast | card_flags.clocks,
                card_flags.none,
                CardEffectTypes.enter |CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(),
                    new PlayerResources(0,1)
                    )
            ),
            //X 62 Undinequeen,
            new CardData(3,CardFamily.B,
                CardFuncs.void_func,
                CardFuncs.undine_queen_clock_effect,
                CardFuncs.void_func,
                card_flags.clocks | card_flags.stones3,
                card_flags.none,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(),
                    new PlayerResources(new stone_quant(0,1,0))
                    )
            ),
            //X 63 Valkyrie,
            new CardData(5,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.valkyrie_clock_effect,
                CardFuncs.void_func,
                card_flags.clocks,
                card_flags.number_of_families,
                CardEffectTypes.clock,
                CardFuncs.bool_true_func,
                new CardInputOutput(
                    new PlayerResources(),
                    new PlayerResources(3,0)
                    )
            ),
            //X 64 Watergiant,
            new CardData(4,CardFamily.B,
                CardFuncs.water_giant_enter_effect,
                CardFuncs.void_func,
                CardFuncs.water_giant_exit_effect,
                card_flags.stones3 | card_flags.stones6,
                card_flags.none,
                CardEffectTypes.enter |CardEffectTypes.infinite,
                CardFuncs.bool_true_func
            ),
            //X 65 Willow,
            new CardData(10,CardFamily.D,
                CardFuncs.willow_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.stones3 | card_flags.stones1 | card_flags.stones6 | card_flags.big_hand | card_flags.etbs,
                card_flags.none,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 66 Youngforestspirit,
            new CardData(0,CardFamily.G,
                CardFuncs.young_forest_spirit_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.high_costs ,
                card_flags.none,
                CardEffectTypes.enter,
                (Player p) =>
                {
                    return p.free_slots_on_table()>=2;
                }
            ),
            //X 67 Yukionna,
            new CardData(0,CardFamily.B,
                CardFuncs.yuki_onna_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.none,
                card_flags.stones3 | card_flags.stones1 | card_flags.stones6,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
            //X 68 Yukionnaexalted
            new CardData(3,CardFamily.B,
                CardFuncs.yuki_onna_exalted_effect,
                CardFuncs.void_func,
                CardFuncs.void_func,
                card_flags.none,
                card_flags.stones3,
                CardEffectTypes.enter,
                CardFuncs.bool_true_func
            ),
        };
    }
}
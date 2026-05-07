using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace voe{
    public delegate IEnumerator CardFunc(Player p);

    public class CardData
    {   
        public int price;
        public CardFamily family;
        public CardFunc enterEffect;
        public CardFunc clockEffect;
        public CardFunc exitEffect;
        public CardData(int _price, CardFamily cf, CardFunc _enter, CardFunc _clock, CardFunc _exit){
            price = _price; family = cf;
            enterEffect = _enter;
            clockEffect = _clock;
            exitEffect = _exit;
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
                CardFuncs.void_func
            ),
            //X 1 Agni,
            new CardData(4,CardFamily.R,
                CardFuncs.agni_enter_func,
                CardFuncs.void_func,
                CardFuncs.agni_exit_func
            ),
            // 2 Asmodeus,
            new CardData(4,CardFamily.R,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 3 Balog,
            new CardData(4,CardFamily.R,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 4 Basilisk,
            new CardData(3,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 5 Behemoth,
            new CardData(9,CardFamily.G,
                CardFuncs.behemoth_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 6 Boreas,
            new CardData(4,CardFamily.P,
                CardFuncs.boreas_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 7 Boulder,
            new CardData(8,CardFamily.D,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 8 Burningskull,
            new CardData(3,CardFamily.R,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 9 Cerberus,
            new CardData(5,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 10 Charybdis,
            new CardData(5,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 11 Dandelionspirit,
            new CardData(3,CardFamily.P,
                CardFuncs.dandelion_enter_func,
                CardFuncs.dandelion_clock_func,
                CardFuncs.void_func
            ),
            // 12 Dragonegg,
            new CardData(3,CardFamily.D,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 13 Ember,
            new CardData(7,CardFamily.D,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 14 Eternity,
            new CardData(12,CardFamily.D,
                CardFuncs.eternity_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 15 Firefox,
            new CardData(1,CardFamily.R,
                CardFuncs.firefox_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 16 Forestspirit,
            new CardData(2,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 17 Freyja,
            new CardData(7,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.freyja_clock_func,
                CardFuncs.void_func
            ),
            // 18 Gargoyle,
            new CardData(2,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 19 Genie,
            new CardData(4,CardFamily.P,
                CardFuncs.genie_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 20 Genieexalted,
            new CardData(5,CardFamily.P,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 21 Gi_rin,
            new CardData(10,CardFamily.P,
                CardFuncs.girin_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 22 Goblin,
            new CardData(1,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 23 Goblinsoldier,
            new CardData(4,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 24 Griffon,
            new CardData(7,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.griffon_clock_func,
                CardFuncs.void_func
            ),
            // 25 Gust,
            new CardData(8,CardFamily.D,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 26 Hae_tae,
            new CardData(3,CardFamily.B,
                CardFuncs.haetae_enter_n_exit_func,
                CardFuncs.void_func,
                CardFuncs.haetae_enter_n_exit_func
            ),
            //X 27 Harpy,
            new CardData(3,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.harpy_clock_func,
                CardFuncs.void_func
            ),
            //X 28 Hestia,
            new CardData(0,CardFamily.R,
                CardFuncs.hestia_enter_func,
                CardFuncs.void_func,
                CardFuncs.hestia_exit_func
            ),
            //X 29 Hornedsalamander,
            new CardData(2,CardFamily.R,
                CardFuncs.void_func,
                CardFuncs.horned_salamander_clock_func,
                CardFuncs.void_func
            ),
            // 30 Hydra,
            new CardData(4,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 31 Ifrit,
            new CardData(2,CardFamily.R,
                CardFuncs.ifrit_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 32 Imp,
            new CardData(0,CardFamily.R,
                CardFuncs.imp_enter_func,
                CardFuncs.imp_clock_func,
                CardFuncs.void_func
            ),
            //X 33 Incubus,
            new CardData(2,CardFamily.R,
                CardFuncs.incubus_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 34 Kappa,
            new CardData(1,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 35 Lavagiant,
            new CardData(3,CardFamily.R,
                CardFuncs.lava_giant_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 36 Leviathan,
            new CardData(4,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 37 Marina,
            new CardData(7,CardFamily.D,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 38 Medusa,
            new CardData(4,CardFamily.G,
                CardFuncs.medusa_clock_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 39 Mimic,
            new CardData(6,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 40 Mudslime,
            new CardData(6,CardFamily.G,
                CardFuncs.mud_slime_enter_func,
                CardFuncs.mud_slime_clock_func,
                CardFuncs.void_func
            ),
            //X 41 Nessie,
            new CardData(2,CardFamily.B,
                CardFuncs.void_func,
                CardFuncs.nessie_clock_func,
                CardFuncs.void_func
            ),
            //X 42 Odin,
            new CardData(6,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.odin_clock_func,
                CardFuncs.void_func
            ),
            // 43 Pegasus,
            new CardData(3,CardFamily.P,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 44 Phoenix,
            new CardData(3,CardFamily.R,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 45 Poseidon,
            new CardData(7,CardFamily.B,
                CardFuncs.poseidon_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 46 Rockgolem,
            new CardData(6,CardFamily.G,
                CardFuncs.rock_golem_enter_func,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 47 Rudra,
            new CardData(8,CardFamily.P,
                CardFuncs.rudra_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 48 Salamander,
            new CardData(1,CardFamily.R,
                CardFuncs.void_func,
                CardFuncs.salamander_clock_func,
                CardFuncs.void_func
            ),
            //X 49 Sandgiant,
            new CardData(10,CardFamily.G,
                CardFuncs.sand_giant_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 50 Scorch,
            new CardData(9,CardFamily.D,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 51 Seaspirit,
            new CardData(1,CardFamily.B,
                CardFuncs.void_func,
                CardFuncs.sea_spirit_clock_func,
                CardFuncs.void_func
            ),
            // 52 Snailmaiden,
            new CardData(3,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            // 53 Stonegolem,
            new CardData(6,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 54 Succubus,
            new CardData(0,CardFamily.R,
                CardFuncs.succubus_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 55 Surtr,
            new CardData(4,CardFamily.R,
                CardFuncs.srtr_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 56 Sylph,
            new CardData(4,CardFamily.P,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 57 Tengu,
            new CardData(3,CardFamily.P,
                CardFuncs.tengu_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 58 Tidal,
            new CardData(5,CardFamily.D,
                CardFuncs.tidal_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 59 Triton,
            new CardData(4,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 60 Troll,
            new CardData(3,CardFamily.G,
                CardFuncs.void_func,
                CardFuncs.troll_clock_effect,
                CardFuncs.void_func
            ),
            //X 61 Undine,
            new CardData(1,CardFamily.B,
                CardFuncs.undine_enter_effect,
                CardFuncs.undine_clock_effect,
                CardFuncs.void_func
            ),
            //X 62 Undinequeen,
            new CardData(3,CardFamily.B,
                CardFuncs.void_func,
                CardFuncs.undine_queen_clock_effect,
                CardFuncs.void_func
            ),
            //X 63 Valkyrie,
            new CardData(5,CardFamily.P,
                CardFuncs.void_func,
                CardFuncs.valkyrie_clock_effect,
                CardFuncs.void_func
            ),
            // 64 Watergiant,
            new CardData(4,CardFamily.B,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 65 Willow,
            new CardData(10,CardFamily.D,
                CardFuncs.willow_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            // 66 Youngforestspirit,
            new CardData(0,CardFamily.G,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func,
                CardFuncs.unimplemented_func
            ),
            //X 67 Yukionna,
            new CardData(0,CardFamily.B,
                CardFuncs.yuki_onna_enter_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
            //X 68 Yukionnaexalted
            new CardData(3,CardFamily.B,
                CardFuncs.yuki_onna_exalted_effect,
                CardFuncs.void_func,
                CardFuncs.void_func
            ),
        };
    }
}
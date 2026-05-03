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
            // Aeris,
            new CardData(9, CardFamily.D, 
                CardFuncs.aerie_enter_func, 
                CardFuncs.void_func, 
                CardFuncs.void_func)
            // Agni,
            // Asmodeus,
            // Balog,
            // Basilisk,
            // Behemoth,
            // Boreas,
            // Boulder,
            // Burningskull,
            // Cerberus,
            // Charybdis,
            // Dandelionspirit,
            // Dragonegg,
            // Ember,
            // Eternity,
            // Firefox,
            // Forestspirit,
            // Freyja,
            // Gargoyle,
            // Genie,
            // Genieexalted,
            // Gi_rin,
            // Goblin,
            // Goblinsoldier,
            // Griffon,
            // Gust,
            // Hae_tae,
            // Harpy,
            // Hestia,
            // Hornedsalamander,
            // Hydra,
            // Ifrit,
            // Imp,
            // Incubus,
            // Kappa,
            // Lavagiant,
            // Leviathan,
            // Marina,
            // Medusa,
            // Mimic,
            // Mudslime,
            // Nessie,
            // Odin,
            // Pegasus,
            // Phoenix,
            // Poseidon,
            // Rockgolem,
            // Rudra,
            // Salamander,
            // Sandgiant,
            // Scorch,
            // Seaspirit,
            // Snailmaiden,
            // Stonegolem,
            // Succubus,
            // Surtr,
            // Sylph,
            // Tengu,
            // Tidal,
            // Triton,
            // Troll,
            // Undine,
            // Undinequeen,
            // Valkyrie,
            // Watergiant,
            // Willow,
            // Youngforestspirit,
            // Yukionna,
            // Yukionnaexalted
        };
    }
}
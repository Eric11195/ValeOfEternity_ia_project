namespace voe{
    public static class AssetDataBase{
        static string[] assetFiles = new string[]
        {
            "Aeris",
            "Agni",
            "Asmodeus",
            "Balog",
            "Basilisk",
            "Behemoth",
            "Boreas",
            "Boulder",
            "Burningskull",
            "Cerberus",
            "Charybdis",
            "Dandelionspirit",
            "Dragonegg",
            "Ember",
            "Eternity",
            "Firefox",
            "Forestspirit",
            "Freyja",
            "Gargoyle",
            "Genie",
            "Genieexalted",
            "Gi-rin",
            "Goblin",
            "Goblinsoldier",
            "Griffon",
            "Gust",
            "Hae-tae",
            "Harpy",
            "Hestia",
            "Hornedsalamander",
            "Hydra",
            "Ifrit",
            "Imp",
            "Incubus",
            "Kappa",
            "Lavagiant",
            "Leviathan",
            "Marina",
            "Medusa",
            "Mimic",
            "Mudslime",
            "Nessie",
            "Odin",
            "Pegasus",
            "Phoenix",
            "Poseidon",
            "Rockgolem",
            "Rudra",
            "Salamander",
            "Sandgiant",
            "Scorch",
            "Seaspirit",
            "Snailmaiden",
            "Stonegolem",
            "Succubus",
            "Surtr",
            "Sylph",
            "Tengu",
            "Tidal",
            "Triton",
            "Troll",
            "Undine",
            "Undinequeen",
            "Valkyrie",
            "Watergiant",
            "Willow",
            "Youngforestspirit",
            "Yukionna",
            "Yukionnaexalted"
        };

        public static string get_card_file_name(CardNameId card_id)
        {
            return assetFiles[(int)card_id];
        }

        static string[] rock_assets = new string[]{
            "Symb_1stone","Symb_3stone","Symb_6stone"
        };
        public static string get_stone_file_name(stone_type st)
        {
            return rock_assets[(int)st];
        }
    }
}
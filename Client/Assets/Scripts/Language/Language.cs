namespace DevelopersHub.ClashOfWhatecer
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class Language : MonoBehaviour
    {

        private static LanguageID defaultLanguage = LanguageID.english;
        private static Language _instance = null; public static Language instanse { get { Initialize(); return _instance; } }

        [Serializable] public class Translation
        {
            public LanguageID language;
            public string text;
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            _instance = this;
        }

        private static void Initialize()
        {
            if(_instance != null) { return; }
            _instance = FindFirstObjectByType<Language>();
            if(_instance == null)
            {
                _instance = new GameObject("Language").AddComponent<Language>();
            }
            LanguageID stored = defaultLanguage;
            if (PlayerPrefs.HasKey("language"))
            {
                try
                {
                    stored = (LanguageID)PlayerPrefs.GetInt("language");
                }
                catch (Exception)
                {
                    stored = defaultLanguage;
                    PlayerPrefs.SetInt("language", (int)stored);
                }
            }
            else
            {
                PlayerPrefs.SetInt("language", (int)stored);
            }
            _instance.language = stored;
        }

        private LanguageID _language = LanguageID.persian; public LanguageID language { get { return _language; } set { SetLanguage(value); } }

        public enum LanguageID
        {
            english = 0, persian = 1, russian = 2, arabic = 3, spanish = 4, french =  5, italian = 6, german = 7
        }

        public bool IsRTL
        {
            get
            {
                if (_language == LanguageID.persian || _language == LanguageID.arabic)
                {
                    return true;
                }
                return false;
            }
        }

        private void SetLanguage(LanguageID id)
        {
            _language = id;
        }

        public string GetLanguageName(LanguageID id)
        {
            switch (id)
            {
                case LanguageID.english: return "English";
                case LanguageID.persian: return "فارسی";
                case LanguageID.russian: return "Русский";
                case LanguageID.arabic: return "العربية";
                case LanguageID.spanish: return "Española";
                case LanguageID.french: return "Français";
                case LanguageID.italian: return "Italiana";
                case LanguageID.german: return "Deutsch";
                default: return "English";
            }
        }

        public string GetBuildingName(Data.BuildingID id, int level = 1)
        {
            switch (language)
            {
                case LanguageID.english:
                    switch (id)
                    {
                        case Data.BuildingID.townhall: return "Town Hall";
                        case Data.BuildingID.goldmine: return "Gold Mine";
                        case Data.BuildingID.goldstorage: return "Gold Storage";
                        case Data.BuildingID.elixirmine: return "Elixir Collector";
                        case Data.BuildingID.elixirstorage: return "Elixir Storage";
                        case Data.BuildingID.darkelixirmine: return "Dark Elixir Drill";
                        case Data.BuildingID.darkelixirstorage: return "Dark Elixir Storage";
                        case Data.BuildingID.buildershut: return "Builders Hut";
                        case Data.BuildingID.armycamp: return "Army Camp";
                        case Data.BuildingID.barracks: return "Barracks";
                        case Data.BuildingID.darkbarracks: return "Dark Barracks";
                        case Data.BuildingID.wall: return "Wall";
                        case Data.BuildingID.cannon: return "Cannon";
                        case Data.BuildingID.archertower: return "Archer Tower";
                        case Data.BuildingID.mortor: return "Mortor";
                        case Data.BuildingID.airdefense: return "Air Defense";
                        case Data.BuildingID.wizardtower: return "Lightning Tower";
                        case Data.BuildingID.hiddentesla: return "Hidden Tesla";
                        case Data.BuildingID.bombtower: return "Bomb Tower";
                        case Data.BuildingID.xbow: return "X Bow";
                        case Data.BuildingID.infernotower: return "Inferno Tower";
                        case Data.BuildingID.decoration: return "Decoration";
                        case Data.BuildingID.obstacle:
                            switch (level)
                            {
                                case 1: case 2: case 3: return "Tree";
                                case 4: case 5: return "Rock";
                                default: return "Obstacle";
                            }
                        case Data.BuildingID.boomb: return "Boomb";
                        case Data.BuildingID.springtrap: return "Spring Trap";
                        case Data.BuildingID.airbomb: return "Air Bomb";
                        case Data.BuildingID.giantbomb: return "Giant Bomb";
                        case Data.BuildingID.seekingairmine: return "Seeking Air Mine";
                        case Data.BuildingID.skeletontrap: return "Skeleton Trap";
                        case Data.BuildingID.clancastle: return "Clan Center";
                        case Data.BuildingID.spellfactory: return "Spell Factory";
                        case Data.BuildingID.darkspellfactory: return "Dark Spell Factory";
                        case Data.BuildingID.laboratory: return "Laboratory";
                    }
                    break;
                case LanguageID.persian:
                    switch (id)
                    {
                        case Data.BuildingID.townhall: return "قلعه";
                        case Data.BuildingID.goldmine: return "معدن طلا";
                        case Data.BuildingID.goldstorage: return "انبار طلا";
                        case Data.BuildingID.elixirmine: return "اکسیر ساز";
                        case Data.BuildingID.elixirstorage: return "منبع اکسیر";
                        case Data.BuildingID.darkelixirmine: return "Dark Elixir Drill";
                        case Data.BuildingID.darkelixirstorage: return "Dark Elixir Storage";
                        case Data.BuildingID.buildershut: return "ایستگاه ساخت";
                        case Data.BuildingID.armycamp: return "کمپ نظامی";
                        case Data.BuildingID.barracks: return "سرباز خانه";
                        case Data.BuildingID.darkbarracks: return "Dark Barracks";
                        case Data.BuildingID.wall: return "دیوار";
                        case Data.BuildingID.cannon: return "توپ";
                        case Data.BuildingID.archertower: return "برج کماندار";
                        case Data.BuildingID.mortor: return "خمپاره انداز";
                        case Data.BuildingID.airdefense: return "ضد هوایی";
                        case Data.BuildingID.wizardtower: return "برج جادوگر";
                        case Data.BuildingID.hiddentesla: return "Hidden Tesla";
                        case Data.BuildingID.bombtower: return "Bomb Tower";
                        case Data.BuildingID.xbow: return "X Bow";
                        case Data.BuildingID.infernotower: return "برج جهنمی";
                        case Data.BuildingID.decoration: return "Decoration";
                        case Data.BuildingID.obstacle:
                            switch (level)
                            {
                                case 1: case 2: case 3: return "درخت";
                                case 4: case 5: return "سنگ";
                                default: return "مانع";
                            }
                        case Data.BuildingID.boomb: return "Boomb";
                        case Data.BuildingID.springtrap: return "Spring Trap";
                        case Data.BuildingID.airbomb: return "Air Bomb";
                        case Data.BuildingID.giantbomb: return "Giant Bomb";
                        case Data.BuildingID.seekingairmine: return "Seeking Air Mine";
                        case Data.BuildingID.skeletontrap: return "Skeleton Trap";
                        case Data.BuildingID.clancastle: return "مرکز قبیله";
                        case Data.BuildingID.spellfactory: return "کارخانه جادو";
                        case Data.BuildingID.darkspellfactory: return "Dark Spell Factory";
                        case Data.BuildingID.laboratory: return "آزمایشگاه";
                    }
                    break;
            }
            return "";
        }

        public string GetUnitName(Data.UnitID id)
        {
            switch (language)
            {
                case LanguageID.english:
                    switch (id)
                    {
                        case Data.UnitID.barbarian: return "Soldier";
                        case Data.UnitID.archer: return "Archer";
                        case Data.UnitID.goblin: return "Goblin";
                        case Data.UnitID.healer: return "Healer";
                        case Data.UnitID.wallbreaker: return "Wall Breaker";
                        case Data.UnitID.giant: return "Giant";
                        case Data.UnitID.miner: return "Miner";
                        case Data.UnitID.balloon: return "Balloon";
                        case Data.UnitID.wizard: return "Wizard";
                        case Data.UnitID.dragon: return "Dragon Archer";
                        case Data.UnitID.pekka: return "Knight";
                        case Data.UnitID.babydragon: return "Terminaror";
                        case Data.UnitID.electrodragon: return "Electro Dragon";
                        case Data.UnitID.yeti: return "Yeti";
                        case Data.UnitID.dragonrider: return "Dragon Rider";
                        case Data.UnitID.electrotitan: return "Town Hall";
                        case Data.UnitID.minion: return "Minion";
                        case Data.UnitID.hogrider: return "Gog Rider";
                        case Data.UnitID.valkyrie: return "Valkyrie";
                        case Data.UnitID.golem: return "Golem";
                        case Data.UnitID.witch: return "Witch";
                        case Data.UnitID.lavahound: return "Lava Hound";
                        case Data.UnitID.bowler: return "Bowler";
                        case Data.UnitID.icegolem: return "Ice Golem";
                        case Data.UnitID.headhunter: return "Head Hunter";
                        case Data.UnitID.skeleton: return "Skeleton";
                        case Data.UnitID.bat: return "Bat";
                    }
                    break;
                case LanguageID.persian:
                    switch (id)
                    {
                        case Data.UnitID.barbarian: return "سرباز";
                        case Data.UnitID.archer: return "کماندار";
                        case Data.UnitID.goblin: return "گوبلین";
                        case Data.UnitID.healer: return "درمانگر";
                        case Data.UnitID.wallbreaker: return "دیوار شکن";
                        case Data.UnitID.giant: return "غول";
                        case Data.UnitID.miner: return "معدنچی";
                        case Data.UnitID.balloon: return "بالن";
                        case Data.UnitID.wizard: return "جادوگر";
                        case Data.UnitID.dragon: return "کماندار اژدها";
                        case Data.UnitID.pekka: return "شوالیه";
                        case Data.UnitID.babydragon: return "ترمیناتور";
                        case Data.UnitID.electrodragon: return "Electro Dragon";
                        case Data.UnitID.yeti: return "Yeti";
                        case Data.UnitID.dragonrider: return "Dragon Rider";
                        case Data.UnitID.electrotitan: return "Electro Titan";
                        case Data.UnitID.minion: return "Minion";
                        case Data.UnitID.hogrider: return "Gog Rider";
                        case Data.UnitID.valkyrie: return "Valkyrie";
                        case Data.UnitID.golem: return "Golem";
                        case Data.UnitID.witch: return "Witch";
                        case Data.UnitID.lavahound: return "Lava Hound";
                        case Data.UnitID.bowler: return "Bowler";
                        case Data.UnitID.icegolem: return "Ice Golem";
                        case Data.UnitID.headhunter: return "Head Hunter";
                        case Data.UnitID.skeleton: return "Skeleton";
                        case Data.UnitID.bat: return "Bat";
                    }
                    break;
            }
            return "";
        }
        
        public string GetSpellName(Data.SpellID id)
        {
            switch (language)
            {
                case LanguageID.english:
                    switch (id)
                    {
                        case Data.SpellID.lightning: return "Lightning";
                        case Data.SpellID.healing: return "Healing";
                        case Data.SpellID.rage: return "Rage";
                        case Data.SpellID.jump: return "Jump";
                        case Data.SpellID.freeze: return "Freeze";
                        case Data.SpellID.invisibility: return "Invisibility";
                        case Data.SpellID.recall: return "Recall";
                        case Data.SpellID.earthquake: return "Earthquake";
                        case Data.SpellID.haste: return "Haste";
                        case Data.SpellID.skeleton: return "Skeleton";
                        case Data.SpellID.bat: return "Bat";
                    }
                    break;
                case LanguageID.persian:
                    switch (id)
                    {
                        case Data.SpellID.lightning: return "صاعقه";
                        case Data.SpellID.healing: return "شفا";
                        case Data.SpellID.rage: return "خشم";
                        case Data.SpellID.jump: return "پرش";
                        case Data.SpellID.freeze: return "انجماد";
                        case Data.SpellID.invisibility: return "نامرعی";
                        case Data.SpellID.recall: return "Recall";
                        case Data.SpellID.earthquake: return "زمین لرزه";
                        case Data.SpellID.haste: return "عجله";
                        case Data.SpellID.skeleton: return "اسلت";
                        case Data.SpellID.bat: return "خفاش";
                    }
                    break;
            }
            return "";
        }

    }
}
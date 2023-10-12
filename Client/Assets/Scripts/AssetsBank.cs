namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AssetsBank : MonoBehaviour
    {

        private static AssetsBank _instance = null; public static AssetsBank instanse { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
        }

        [Header("Resources")]
        public Sprite goldIcon = null;
        public Sprite elixirIcon = null;
        public Sprite darkIcon = null;
        public Sprite gemsIcon = null;

        [Header("Buildings")]
        public Sprite townhallIcon = null;
        public Sprite buildershutIcon = null;
        public Sprite[] wallIcons = null;
        public Sprite goldmineIcon = null;
        public Sprite elixirmineIcon = null;
        public Sprite darkmineIcon = null;
        public Sprite goldstorageIcon = null;
        public Sprite elixirstorageIcon = null;
        public Sprite darkstorageIcon = null;
        public Sprite barracksIcon = null;
        public Sprite darkbarracksIcon = null;
        public Sprite spellfacroryIcon = null;
        public Sprite darkspellfacroryIcon = null;
        public Sprite armycampIcon = null;
        public Sprite clancastleIcon = null;
        public Sprite cannonIcon = null;
        public Sprite archertowerIcon = null;
        public Sprite mortorIcon = null;
        public Sprite airdefenseIcon = null;
        public Sprite wizardTowerIcon = null;
        public Sprite infernoTowerIcon = null;
        public Sprite labIcon = null;
        public Sprite obsticle01Icon = null;
        public Sprite obsticle02Icon = null;
        public Sprite obsticle03Icon = null;
        public Sprite obsticle04Icon = null;
        public Sprite obsticle05Icon = null;

        [Header("Units")]
        public Sprite barbarianIcon = null;
        public Sprite archerIcon = null;
        public Sprite goblinIcon = null;
        public Sprite healerIcon = null;
        public Sprite wallbreakerIcon = null;
        public Sprite giantIcon = null;
        public Sprite minerIcon = null;
        public Sprite balloonIcon = null;
        public Sprite wizardIcon = null;
        public Sprite dragonIcon = null;
        public Sprite pekkaIcon = null;
        public Sprite babydragonIcon = null;
        public Sprite electrodragonIcon = null;
        public Sprite yetiIcon = null;
        public Sprite dragonriderIcon = null;
        public Sprite electrotitanIcon = null;
        public Sprite minionIcon = null;
        public Sprite hogriderIcon = null;
        public Sprite valkyrieIcon = null;
        public Sprite golemIcon = null;
        public Sprite witchIcon = null;
        public Sprite lavahoundIcon = null;
        public Sprite bowlerIcon = null;
        public Sprite icegolemIcon = null;
        public Sprite headhunterIcon = null;
        public Sprite skeletonIcon = null;
        public Sprite batIcon = null;

        [Header("Spells")]
        public Sprite lightningIcon = null;
        public Sprite healingIcon = null;
        public Sprite rageIcon = null;
        public Sprite jumpIcon = null;
        public Sprite freezeIcon = null;
        public Sprite invisibilityIcon = null;
        public Sprite recallIcon = null;
        public Sprite earthquakeIcon = null;
        public Sprite hasteIcon = null;
        public Sprite skeletonSommonIcon = null;
        public Sprite batSommonIcon = null;

        [Header("Spells")]
        public Sprite englishIcon = null;
        public Sprite persianIcon = null;
        public Sprite arabicIcon = null;
        public Sprite russianIcon = null;
        public Sprite spanishIcon = null;
        public Sprite frenchIcon = null;
        public Sprite italianIcon = null;
        public Sprite germanIcon = null;

        public static Sprite GetBuildingIcon(Data.BuildingID id, int level = 1)
        {
            if(instanse != null)
            {
                return instanse._GetBuildingIcon(id, level);
            }
            return null;
        }

        private Sprite _GetBuildingIcon(Data.BuildingID id, int level = 1)
        {
            switch (id)
            {
                case Data.BuildingID.townhall: return townhallIcon;
                case Data.BuildingID.goldmine: return goldmineIcon;
                case Data.BuildingID.goldstorage: return goldstorageIcon;
                case Data.BuildingID.elixirmine: return elixirmineIcon;
                case Data.BuildingID.elixirstorage: return elixirstorageIcon;
                case Data.BuildingID.darkelixirmine: return darkmineIcon;
                case Data.BuildingID.darkelixirstorage: return darkstorageIcon;
                case Data.BuildingID.buildershut: return buildershutIcon;
                case Data.BuildingID.armycamp: return armycampIcon;
                case Data.BuildingID.barracks: return barracksIcon;
                case Data.BuildingID.darkbarracks: return darkstorageIcon;
                case Data.BuildingID.wall: return wallIcons[level];
                case Data.BuildingID.cannon: return cannonIcon;
                case Data.BuildingID.archertower: return archertowerIcon;
                case Data.BuildingID.mortor: return mortorIcon;
                case Data.BuildingID.airdefense: return airdefenseIcon;
                case Data.BuildingID.wizardtower: return wizardTowerIcon;
                case Data.BuildingID.hiddentesla:
                    break;
                case Data.BuildingID.bombtower:
                    break;
                case Data.BuildingID.xbow:
                    break;
                case Data.BuildingID.infernotower: return infernoTowerIcon;
                case Data.BuildingID.decoration:
                    break;
                case Data.BuildingID.obstacle:
                    switch (level)
                    {
                        case 1: return obsticle01Icon;
                        case 2: return obsticle02Icon;
                        case 3: return obsticle03Icon;
                        case 4: return obsticle04Icon;
                        case 5: return obsticle05Icon;
                    }
                    break;
                case Data.BuildingID.boomb:
                    break;
                case Data.BuildingID.springtrap:
                    break;
                case Data.BuildingID.airbomb:
                    break;
                case Data.BuildingID.giantbomb:
                    break;
                case Data.BuildingID.seekingairmine:
                    break;
                case Data.BuildingID.skeletontrap:

                    break;
                case Data.BuildingID.clancastle: return clancastleIcon;
                case Data.BuildingID.spellfactory: return spellfacroryIcon;
                case Data.BuildingID.darkspellfactory: return darkspellfacroryIcon;
                case Data.BuildingID.laboratory: return labIcon;
            }
            return null;
        }

        public static Sprite GetUnitIcon(Data.UnitID id)
        {
            if (instanse != null)
            {
                return instanse._GetUnitIcon(id);
            }
            return null;
        }

        private Sprite _GetUnitIcon(Data.UnitID id)
        {
            switch (id)
            {
                case Data.UnitID.barbarian: return barbarianIcon;
                case Data.UnitID.archer: return archerIcon;
                case Data.UnitID.goblin: return goblinIcon;
                case Data.UnitID.healer: return healerIcon;
                case Data.UnitID.wallbreaker: return wallbreakerIcon;
                case Data.UnitID.giant: return giantIcon;
                case Data.UnitID.miner: return minerIcon;
                case Data.UnitID.balloon: return balloonIcon;
                case Data.UnitID.wizard: return wizardIcon;
                case Data.UnitID.dragon: return dragonIcon;
                case Data.UnitID.pekka: return pekkaIcon;
                case Data.UnitID.babydragon: return babydragonIcon;
                case Data.UnitID.electrodragon: return electrodragonIcon;
                case Data.UnitID.yeti: return yetiIcon;
                case Data.UnitID.dragonrider: return dragonriderIcon;
                case Data.UnitID.electrotitan: return electrotitanIcon;
                case Data.UnitID.minion: return minionIcon;
                case Data.UnitID.hogrider: return hogriderIcon;
                case Data.UnitID.valkyrie: return valkyrieIcon;
                case Data.UnitID.golem: return golemIcon;
                case Data.UnitID.witch: return witchIcon;
                case Data.UnitID.lavahound: return lavahoundIcon;
                case Data.UnitID.bowler: return bowlerIcon;
                case Data.UnitID.icegolem: return icegolemIcon;
                case Data.UnitID.headhunter: return headhunterIcon;
                case Data.UnitID.skeleton: return skeletonIcon;
                case Data.UnitID.bat: return batIcon;
            }
            return null;
        }

        public static Sprite GetSpellIcon(Data.SpellID id)
        {
            if (instanse != null)
            {
                return instanse._GetSpellIcon(id);
            }
            return null;
        }

        private Sprite _GetSpellIcon(Data.SpellID id)
        {
            switch (id)
            {
                case Data.SpellID.lightning: return lightningIcon;
                case Data.SpellID.healing: return healingIcon;
                case Data.SpellID.rage: return rageIcon;
                case Data.SpellID.jump: return jumpIcon;
                case Data.SpellID.freeze: return freezeIcon;
                case Data.SpellID.invisibility: return invisibilityIcon;
                case Data.SpellID.recall: return recallIcon;
                case Data.SpellID.earthquake: return earthquakeIcon;
                case Data.SpellID.haste: return hasteIcon;
                case Data.SpellID.skeleton: return skeletonSommonIcon;
                case Data.SpellID.bat: return batSommonIcon;
            }
            return null;
        }

        public static Sprite GetLanguageIcon(Language.LanguageID id)
        {
            if (instanse != null)
            {
                return instanse._GetLanguageIcon(id);
            }
            return null;
        }

        private Sprite _GetLanguageIcon(Language.LanguageID id)
        {
            switch (id)
            {
                case Language.LanguageID.english: return englishIcon;
                case Language.LanguageID.persian: return persianIcon;
                case Language.LanguageID.russian: return russianIcon;
                case Language.LanguageID.arabic: return arabicIcon;
                case Language.LanguageID.spanish: return spanishIcon;
                case Language.LanguageID.french: return frenchIcon;
                case Language.LanguageID.italian: return italianIcon;
                case Language.LanguageID.german: return germanIcon;
            }
            return null;
        }

    }
}
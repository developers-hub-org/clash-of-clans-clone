namespace DevelopersHub.ClashOfWhatecer
{
    using System.Xml.Serialization;
    using System.IO;
    using System.Collections.Generic;
    using System;
    using System.Threading.Tasks;
    using System.Text;
    using System.Security.Cryptography;
    using System.IO.Compression;

    public static class Data
    {

        public const int maxTownHallLevel = 10;
        public const int minGoldCollect = 10;
        public const int minElixirCollect = 10;
        public const int minDarkElixirCollect = 10;
        public static readonly int battleDuration = 120;
        public static readonly int battlePrepDuration = 30;
        public static readonly int gridSize = 45;
        public static readonly float gridCellSize = 1;

        public static readonly float battleFrameRate = 0.05f;
        public static readonly int battleTilesWorthOfOneWall = 15;
        public static readonly int battleGroupWallAttackRadius = 5;
        public static readonly int battleGridOffset = 2;
        public static readonly int shieldMinutesAmountToBattleLost = 180;

        public static readonly int clanMaxMembers = 50;
        public static readonly int clansPerPage = 20;
        public static readonly int clanNameMinLength = 3;
        public static readonly int clanJoinTimeGapHours = 24;
        public static readonly int clanCreatePrice = 40000;
        public static readonly int clanWarAttacksPerPlayer = 2;
        public static readonly int clanWarPrepHours = 24;
        public static readonly int clanWarBattleHours = 24;
        public static readonly double clanWarMatchMinPercentage = 0.70d;

        public static readonly double clanWarMatchTownHallEffectPercentage = 0.60d;
        public static readonly double clanWarMatchSpellFactoryEffectPercentage = 0.05d;
        public static readonly double clanWarMatchDarkSpellFactoryEffectPercentage = 0.05d;
        public static readonly double clanWarMatchBarracksEffectPercentage = 0.05d;
        public static readonly double clanWarMatchDarkBarracksEffectPercentage = 0.05d;
        public static readonly double clanWarMatchCampsEffectPercentage = 0.20d;

        public static readonly int[] clanRanksWithEditPermission = { 1, 2 };
        public static readonly int[] clanRanksWithWarPermission = { 1, 2 };
        public static readonly int[] clanRanksWithKickMembersPermission = { 1, 2 };
        public static readonly int[] clanRanksWithAcceptJoinRequstsPermission = { 1, 2 };
        public static readonly int[] clanRanksWithPromoteMembersPermission = { 1, 2 };
        public static readonly int[] clanWarAvailableCounts = { 5, 10, 15, 20 };

        public static readonly int globalChatArchiveMaxMessages = 30;
        public static readonly int clanChatArchiveMaxMessages = 30;
        public static readonly int chatSyncPeriod = 2;

        public static readonly string mysqlDateTimeFormat = "%Y-%m-%d %H:%i:%s";

        public static readonly int recoveryCodeExpiration = 300;
        public static readonly int confirmationCodeExpiration = 300;
        public static readonly int recoveryCodeLength = 6;
        
        public class PlayersRanking
        {
            public int page = 1;
            public int pagesCount = 1;
            public List<PlayerRank> players = new List<PlayerRank>();
        }

        public class PlayerRank
        {
            public long id = 0;
            public int rank = 0;
            public string name = "";
            public int trophies = 0;
            public int xp = 0;
            public int level = 0;
        }

        public static string EncodeString(string input)
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(input));
            }
            catch (Exception)
            {
                return input;
            }       
        }

        public static string DecodeString(string input)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(input));
            }
            catch (Exception)
            {
                return input;
            }
        }

        public static bool IsEmailValid(string email)
        {
            email = email.Trim();
            if (email.EndsWith(".")) { return false; }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        public static string RandomCode(int length)
        {
            if (length <= 0)
            {
                return "";
            }
            Random random = new Random();
            const string chars = "0123456789";
            string value = "";
            while (value.Length < length)
            {
                value += chars[random.Next(0, chars.Length)].ToString();
            }
            return value;
        }

        public static string EncrypteToMD5(string data)
        {
            UTF8Encoding ue = new UTF8Encoding();
            byte[] bytes = ue.GetBytes(data);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString = hashString + Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            return hashString.PadLeft(32, '0');
        }

        public enum ResearchType
        {
            unit = 1, spell = 2
        }

        public class Research
        {
            public long id;
            public ResearchType type;
            public string globalID;
            public int level;
            public bool researching;
            public DateTime end;
        }

        public enum ChatType
        {
            global = 1, clan = 2
        }

        public enum ClanRank
        {
            member = 0, leader = 1, coleader = 2, elder = 3
        }

        public enum ClanJoinType
        {
            AnyoneCanJoin = 0, NotAcceptingNewMembers = -1, TakingJoinRequests = 1
        }

        public static bool IsMessageGoodToSend(string message)
        {
            // if (message.Contains("fork")) { return false; }
            return true;
        }

        public class CharMessage
        {
            public long id = 0;
            public long accountID = 0;
            public string name = "";
            public Data.ChatType type = 0;
            public long globalID = 0;
            public long clanID = 0;
            public string message = "";
            public string color = "";
            public string time = "";
        }

        public class JoinRequest
        {
            public long id = 0;
            public long accountID = 0;
            public string name = "";
            public int level = 1;
            public int trophies = 0;
            public DateTime time;
        }

        public class ClansList
        {
            public int page = 1;
            public int pagesCount = 1;
            public List<Data.Clan> clans = new List<Clan>();
        }

        public class ClanWarSearch
        {
            public long id = 0;
            public long clan = 0;
            public long player = 0;
            public DateTime time;
            public List<ClanWarSearchMember> members = null;
            public List<long> notMatch = new List<long>();
            public int match = -1;
            public bool handled = false;
        }

        public class ClanWarData
        {
            public long id = 0;
            public bool searching = false;
            public int count = 0;
            public string starter = "";
            public long clan1ID = 0;
            public long clan2ID = 0;
            public long winnerID = 0;
            public int size = 0;
            public bool hasReport = false;
            public int clan1Stars = 0;
            public int clan2Stars = 0;
            public int maxStars = 0;
            public DateTime startTime;
            public Clan clan1 = null;
            public Clan clan2 = null;
        }

        public class ClanWarAttack
        {
            public long id = 0;
            public DateTime start;
            public long attacker = 0;
            public long defender = 0;
            public int stars = 0;
            public int gold = 0;
            public int elixir = 0;
            public int dark = 0;
            public bool starsCounted = false;
        }

        public class ClanWarSearchMember
        {
            public int tempPosition = -1;
            public int warPosition = -1;
            public ClanMember data = new ClanMember();
            public List<Building> Buildings = new List<Building>();

            public int wallsPower = 0;
            public int defencePower = 0;

            public int townHall = 0;
            public int spellFactory = 0;
            public int darkSpellFactory = 0;
            public int barracks = 0;
            public int darkBarracks = 0;
            public int campsCapacity = 0;
        }

        public class Clan
        {
            public long id = 0;
            public string name = "Clan";
            public ClanJoinType joinType = ClanJoinType.AnyoneCanJoin;
            public int level = 1;
            public int xp = 0;
            public int rank = 0;
            public int trophies = 0;
            public int minTrophies = 0;
            public int minTownhallLevel = 0;
            public int pattern = 0;
            public int background = 0;
            public string patternColor = "";
            public string backgroundColor = "";
            public List<ClanMember> members = new List<ClanMember>();
            public ClanWar war = new ClanWar();
        }

        public class ClanWar
        {
            public long id = 0;
            public long clan1 = 0;
            public long clan2 = 0;
            public int stage = 0;
            public DateTime start;
            public List<ClanWarAttack> attacks = new List<ClanWarAttack>();
        }

        public class ClanMember
        {
            public long id = 0;
            public string name = "Player";
            public int level = 1;
            public int xp = 0;
            public int rank = 0;
            public int trophies = 0;
            public int townHallLevel = 1;
            public bool online = false;
            public long clanID = 0;
            public int clanRank = 0;
            public long warID = 0;
            public int warPos = 0;
        }

        public class Player
        {
            public long id = 0;
            public string name = "Player";
            public int gems = 0;
            public int trophies = 0;
            public bool banned = false;
            public DateTime nowTime;
            public DateTime shield;
            public int xp = 0;
            public int level = 1;
            public DateTime clanTimer;
            public long clanID = 0;
            public int clanRank = 0;
            public long warID = 0;
            public string email = "";
            public int layout = 0;
            public DateTime shield1;
            public DateTime shield2;
            public DateTime shield3;
            public List<Building> buildings = new List<Building>();
            public List<Unit> units = new List<Unit>();
            public List<Spell> spells = new List<Spell>();
        }

        public enum UnitID
        {
            barbarian = 0, archer = 1, goblin = 2, healer = 3, wallbreaker = 4, giant = 5, miner = 6, balloon = 7, wizard = 8, dragon = 9, pekka = 10, babydragon = 11, electrodragon = 12, yeti = 13, dragonrider = 14, electrotitan = 15, minion = 16, hogrider = 17, valkyrie = 18, golem = 19, witch = 20, lavahound = 21, bowler = 22, icegolem = 23, headhunter = 24, skeleton = 25, bat = 26
        }

        // Spells that their effects have been applied to the project: lightning, healing, rage, freeze, invisibility, haste
        public enum SpellID
        {
            lightning = 0, healing = 1, rage = 2, jump = 3, freeze = 4, invisibility = 5, recall = 6, earthquake = 7, haste = 8, skeleton = 9, bat = 10
        }

        public class ServerSpell
        {
            public long databaseID = 0;
            public SpellID id = SpellID.lightning;
            public int level = 0;
            public int requiredGold = 0;
            public int requiredElixir = 0;
            public int requiredGems = 0;
            public int requiredDarkElixir = 0;
            public int brewTime = 0;
            public int housing = 1;
            public float radius = 0;
            public int pulsesCount = 0;
            public float pulsesDuration = 0;
            public float pulsesValue = 0;
            public float pulsesValue2 = 0;
            public int researchTime = 0;
            public int researchGold = 0;
            public int researchElixir = 0;
            public int researchDarkElixir = 0;
            public int researchGems = 0;
            public int researchXp = 0;
        }

        public class Spell
        {
            public long databaseID = 0;
            public SpellID id = SpellID.lightning;
            public int level = 0;
            public int hosing = 1;
            public bool brewed = false;
            public bool ready = false;
            public int brewTime = 0;
            public float brewedTime = 0;
            public int housing = 1;
            public ServerSpell server = null;
        }

        public static int GetClanWarGainedXP(int gainedStars, int enemyGainedStars, int maxStars, bool didWonFirstAttack)
        {
            int xp = 0;
            double percentage = (double)gainedStars / (double)enemyGainedStars;

            if (percentage >= 0.4d)
            {
                xp += 10;
            }

            if (percentage >= 0.6d)
            {
                xp += 25;
            }

            if (gainedStars > enemyGainedStars)
            {
                xp += 50;
            }

            if (didWonFirstAttack)
            {
                xp += 10;
            }

            return xp;
        }

        public static int GetClanNexLevelRequiredXp(int currentLevel)
        {
            switch (currentLevel)
            {
                case 1: return 0;
                case 2: return 500;
                case 3: return 1200;
                case 4: return 1900;
                case 5: return 3100;
                case 6: return 3800;
                case 7: return 4500;
                case 8: return 5200;
                case 9: return 5900;
                case 10: return 7900;
                case 11: return 8600;
                case 12: return 9300;
                case 13: return 10000;
                case 14: return 10700;
                case 15: return 15700;
                case 16: return 16400;
                case 17: return 17100;
                case 18: return 17800;
                case 19: return 18500;
                case 20: return 23500;
                case 21: return 24200;
                case 22: return 24900;
                case 23: return 25600;
                case 24: return 26300;
                case 25: return 31300;
                case 26: return 32000;
                case 27: return 32700;
                case 28: return 33400;
                case 29: return 34100;
                case 30: return 39100;
                case 31: return 39800;
                case 32: return 40500;
                case 33: return 41200;
                case 34: return 41900;
                case 35: return 46900;
                case 36: return 47600;
                case 37: return 48300;
                case 38: return 49000;
                case 39: return 49700;
                case 40: return 54700;
                case 41: return 55400;
                case 42: return 56100;
                case 43: return 56800;
                case 44: return 57500;
                case 45: return 62500;
                case 46: return 63200;
                case 47: return 63900;
                case 48: return 64600;
                case 49: return 65300;
                case 50: return 70300;
                case 51: return 71000;
                default: return 99999999;
            }
        }

        public static bool IsUnitUnlocked(UnitID id, int barracksLevel, int darkBarracksLevel)
        {
            switch (id)
            {
                case UnitID.barbarian: return barracksLevel >= 1;
                case UnitID.archer: return barracksLevel >= 2;
                case UnitID.giant: return barracksLevel >= 3;
                case UnitID.goblin: return barracksLevel >= 4;
                case UnitID.wallbreaker: return barracksLevel >= 5;
                case UnitID.balloon: return barracksLevel >= 6;
                case UnitID.wizard: return barracksLevel >= 7;
                case UnitID.healer: return barracksLevel >= 8;
                case UnitID.dragon: return barracksLevel >= 9;
                case UnitID.pekka: return barracksLevel >= 10;
                case UnitID.babydragon: return barracksLevel >= 11;
                case UnitID.miner: return barracksLevel >= 12;
                case UnitID.electrodragon: return barracksLevel >= 13;
                case UnitID.yeti: return barracksLevel >= 14;
                case UnitID.dragonrider: return barracksLevel >= 15;
                case UnitID.electrotitan: return barracksLevel >= 16;
                case UnitID.minion: return darkBarracksLevel >= 1;
                case UnitID.hogrider: return darkBarracksLevel >= 2;
                case UnitID.valkyrie: return darkBarracksLevel >= 3;
                case UnitID.golem: return darkBarracksLevel >= 4;
                case UnitID.witch: return darkBarracksLevel >= 5;
                case UnitID.lavahound: return darkBarracksLevel >= 6;
                case UnitID.bowler: return darkBarracksLevel >= 7;
                case UnitID.icegolem: return darkBarracksLevel >= 8;
                case UnitID.headhunter: return darkBarracksLevel >= 9;
                default: return false;
            }
        }

        public static bool IsSpellUnlocked(SpellID id, int spellFactoryLevel, int darkSpellFactoryLevel)
        {
            switch (id)
            {
                case SpellID.lightning: return spellFactoryLevel >= 1;
                case SpellID.healing: return spellFactoryLevel >= 2;
                case SpellID.rage: return spellFactoryLevel >= 3;
                //case SpellID.jump: return spellFactoryLevel >= 4;
                case SpellID.freeze: return spellFactoryLevel >= 4;
                case SpellID.invisibility: return spellFactoryLevel >= 5;
                case SpellID.earthquake: return darkSpellFactoryLevel >= 1;
                case SpellID.haste: return darkSpellFactoryLevel >= 2;
                case SpellID.skeleton: return darkSpellFactoryLevel >= 3;
                case SpellID.bat: return darkSpellFactoryLevel >= 4;
                default: return false;
            }
        }

        public static int GetNexLevelRequiredXp(int currentLevel)
        {
            if (currentLevel == 1) { return 30; }
            else if (currentLevel <= 200) { return (currentLevel - 1) * 50; }
            else if (currentLevel <= 299) { return ((currentLevel - 200) * 500) + 9500; }
            else { return ((currentLevel - 300) * 1000) + 60000; }
        }

        public static int GetBattleSearchCost(int townHallLevel)
        {
            switch (townHallLevel)
            {
                case 1: return 10;
                case 2: return 25;
                case 3: return 50;
                case 4: return 100;
                case 5: return 200;
                case 6: return 380;
                case 7: return 420;
                case 8: return 580;
                case 9: return 850;
                case 10: return 1000;
                case 11: return 1500;
                case 12: return 2000;
                case 13: return 4000;
                case 14: return 6000;
                case 15: return 10000;
                default: return 999999;
            }
        }

        public static int GetTotalXpEarned(int currentLevel)
        {
            if (currentLevel == 1) { return 0; }
            else if (currentLevel <= 201) { return ((currentLevel - 1) * (currentLevel - 2) * 25) + 30; }
            else if (currentLevel <= 299) { return ((currentLevel - 200) * (currentLevel - 200) * 250) + (9250 * (currentLevel - 200)) + 985530; }
            else { return ((currentLevel - 300) * (currentLevel - 300) * 500) + (59500 * (currentLevel - 300)) + 4410530; }
        }

        public enum TargetPriority
        {
            none = 0, all = 1, defenses = 2, resources = 3, walls = 4
        }

        public enum BuildingTargetType
        {
            none = 0, ground = 1, air = 2, all = 3
        }

        public enum UnitMoveType
        {
            ground = 0, jump = 1, fly = 2, underground = 3
        }

        public enum BuildingID
        {
            townhall = 0, goldmine = 1, goldstorage = 2, elixirmine = 3, elixirstorage = 4, darkelixirmine = 5, darkelixirstorage = 6, buildershut = 7, armycamp = 8, barracks = 9, darkbarracks = 10, wall = 11, cannon = 12, archertower = 13, mortor = 14, airdefense = 15, wizardtower = 16, hiddentesla = 19, bombtower = 20, xbow = 21, infernotower = 22, decoration = 23, obstacle = 24, boomb = 25, springtrap = 26, airbomb = 27, giantbomb = 28, seekingairmine = 29, skeletontrap = 30, clancastle = 31, spellfactory = 32, darkspellfactory = 33, laboratory = 34, airsweeper = 35, kingaltar = 36, qeenaltar = 37
        }

        public static int GetStorageGoldAndElixirLoot(int townhallLevel, float storage)
        {
            double p = 0;
            switch (townhallLevel)
            {
                case 1: case 2: case 3: case 4: case 5: case 6: p = 0.2d; break;
                case 7: p = 0.18d; break;
                case 8: p = 0.16d; break;
                case 9: p = 0.14d; break;
                case 10: p = 0.12d; break;
                default: p = 0.1d; break;
            }
            return (int)Math.Floor(storage * p);
        }

        public static int GetStorageDarkElixirLoot(int townhallLevel, float storage)
        {
            double p = 0;
            switch (townhallLevel)
            {
                case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8: p = 0.06d; break;
                case 9: p = 0.05d; break;
                default: p = 0.04d; break;
            }
            return (int)Math.Floor(storage * p);
        }

        public static int GetMinesGoldAndElixirLoot(int townhallLevel, float storage)
        {
            return (int)Math.Floor(storage * 0.5d);
        }

        public static int GetMinesDarkElixirLoot(int townhallLevel, float storage)
        {
            return (int)Math.Floor(storage * 0.75d);
        }

        public static (int, int) GetBattleTrophies(int attackerTrophies, int defendderTrophies)
        {
            int win = 0;
            int lose = 0;
            if (attackerTrophies == defendderTrophies)
            {
                win = 30;
                lose = 20;
            }
            else
            {
                double delta = Math.Abs(attackerTrophies - defendderTrophies);
                if (attackerTrophies > defendderTrophies)
                {
                    win = 30 - (int)Math.Floor(delta * (28d / 600d));
                    lose = 20 + (int)Math.Floor(delta * (19d / 600d));
                    if (win < 2)
                    {
                        win = 2;
                    }
                }
                else
                {
                    win = 30 + (int)Math.Floor(delta * (28d / 600d));
                    lose = 20 - (int)Math.Floor(delta * (19d / 600d));
                    if (lose < 1)
                    {
                        lose = 1;
                    }
                }
            }
            return (win, lose);
        }

        public static (int, int) GetWarTrophies(int clan1Trophies, int clan2Trophies, int clan1Stars, int clan2Stars, int maxStars)
        {
            int clan1 = 0;
            int clan2 = 0;
            if (clan1Stars != clan2Stars && (clan1Stars > 0 || clan2Stars > 0))
            {
                double delta = Math.Abs(clan1Trophies - clan2Trophies);
                if (clan1Stars > clan2Stars)
                {
                    double percentage = (double)clan1Stars / (double)maxStars;
                    clan1 = (int)Math.Floor((20 + (clan1Trophies < clan2Trophies ? delta * 0.05d : 0)) * percentage);
                    clan2 = -clan1;
                }
                else
                {
                    double percentage = (double)clan2Stars / (double)maxStars;
                    clan2 = (int)Math.Floor((20 + (clan2Trophies < clan1Trophies ? delta * 0.05d : 0)) * percentage);
                    clan1 = -clan2;
                }
            }
            else
            {
                clan1 = -5;
                clan2 = -5;
            }
            return (clan1, clan2);
        }

        public class BattleFrame
        {
            public int frame = 0;
            public List<BattleFrameUnit> units = new List<BattleFrameUnit>();
            public List<BattleFrameSpell> spells = new List<BattleFrameSpell>();
        }

        public class BattleReport
        {
            public long attacker = 0;
            public long defender = 0;
            public int totalFrames = 0;
            public BattleType type = BattleType.normal;
            public List<Building> buildings = new List<Building>();
            public List<BattleFrame> frames = new List<BattleFrame>();
        }

        public class BattleReportItem
        {
            public long id = 0;
            public long attacker = 0;
            public long defender = 0;
            public string username = "";
            public DateTime time;
            public int stars = 0;
            public int trophies = 0;
            public int gold = 0;
            public int elixir = 0;
            public int dark = 0;
            public bool seen = false;
            public bool hasReply = false;
        }

        public static List<Battle.Building> BuildingsToBattleBuildings(List<Building> buildings, BattleType type)
        {
            List<Battle.Building> battleBuildings = new List<Battle.Building>();
            int townhallLevel = 1;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].id == Data.BuildingID.townhall)
                {
                    townhallLevel = buildings[i].level;
                    break;
                }
            }

            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].databaseID != buildings[i].databaseID || buildings[i].id != buildings[i].id || buildings[i].health != buildings[i].health || buildings[i].damage != buildings[i].damage || buildings[i].percentage != buildings[i].percentage)
                {
                    return null;
                }

                Battle.Building building = new Battle.Building();
                building.building = buildings[i];
                if (type == Data.BattleType.war)
                {
                    building.building.x = building.building.warX;
                    building.building.y = building.building.warY;
                }

                if (building.building.x < 0 || building.building.y < 0)
                {
                    continue;
                }

                building.building.x += Data.battleGridOffset;
                building.building.y += Data.battleGridOffset;

                // bool storage = false;
                switch (building.building.id)
                {
                    case Data.BuildingID.townhall:
                        building.lootGoldStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.goldStorage);
                        building.lootElixirStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.elixirStorage);
                        building.lootDarkStorage = Data.GetStorageDarkElixirLoot(townhallLevel, building.building.darkStorage);
                        // storage = true;
                        break;
                    case Data.BuildingID.goldmine:
                        building.lootGoldStorage = Data.GetMinesGoldAndElixirLoot(townhallLevel, building.building.goldStorage);
                        // storage = true;
                        break;
                    case Data.BuildingID.goldstorage:
                        building.lootGoldStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.goldStorage);
                        // storage = true;
                        break;
                    case Data.BuildingID.elixirmine:
                        building.lootElixirStorage = Data.GetMinesGoldAndElixirLoot(townhallLevel, building.building.elixirStorage);
                        // storage = true;
                        break;
                    case Data.BuildingID.elixirstorage:
                        building.lootElixirStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.elixirStorage);
                        // storage = true;
                        break;
                    case Data.BuildingID.darkelixirmine:
                        building.lootDarkStorage = Data.GetMinesDarkElixirLoot(townhallLevel, building.building.darkStorage);
                        // storage = true;
                        break;
                    case Data.BuildingID.darkelixirstorage:
                        building.lootDarkStorage = Data.GetStorageDarkElixirLoot(townhallLevel, building.building.darkStorage);
                        // storage = true;
                        break;
                }
                /*
                if (storage)
                {
                    Data.BattleStartBuildingData st = new Data.BattleStartBuildingData();
                    st.id = building.building.id;
                    st.databaseID = building.building.databaseID;
                    st.lootGoldStorage = building.building.goldStorage;
                    st.lootElixirStorage = building.building.elixirStorage;
                    st.lootDarkStorage = building.building.darkStorage;
                    startData.Add(st);
                }
                */
                battleBuildings.Add(building);
            }
            return battleBuildings;
        }

        public class BattleFrameUnit
        {
            public long id = 0;
            public int x = 0;
            public int y = 0;
            public Unit unit = null;
        }

        public class BattleFrameSpell
        {
            public long id = 0;
            public int x = 0;
            public int y = 0;
            public Spell spell = null;
        }

        public enum BattleType
        {
            normal = 1, war = 2, quest = 3 
        }

        public class BattleData
        {
            public Battle battle = null;
            public BattleType type = BattleType.normal;
            public List<BattleFrame> savedFrames = new List<BattleFrame>();
            public List<BattleFrame> frames = new List<BattleFrame>();
        }

        public class OpponentData
        {
            public long id = 0;
            public Data.Player data = null;
            public List<Building> buildings = null;
        }

        public class BattleStartBuildingData
        {
            public BuildingID id = BuildingID.townhall;
            public long databaseID = 0;
            public int lootGoldStorage = 0;
            public int lootElixirStorage = 0;
            public int lootDarkStorage = 0;
        }

        public class InitializationData
        {
            public long accountID = 0;
            public string password = "";
            public string[] versions;
            public List<ServerBuilding> serverBuildings = new List<ServerBuilding>();
            public List<ServerUnit> serverUnits = new List<ServerUnit>();
            public List<ServerSpell> serverSpells = new List<ServerSpell>();
            public List<Research> research = new List<Research>();
        }

        public class ServerUnit
        {
            public UnitID id = UnitID.barbarian;
            public int level = 0;
            public int requiredGold = 0;
            public int requiredElixir = 0;
            public int requiredGems = 0;
            public int requiredDarkElixir = 0;
            public int trainTime = 0;
            public int health = 0;
            public int housing = 0;
            public int researchTime = 0;
            public int researchGold = 0;
            public int researchElixir = 0;
            public int researchDarkElixir = 0;
            public int researchGems = 0;
            public int researchXp = 0;
        }

        public class Unit
        {
            public UnitID id = UnitID.barbarian;
            public int level = 0;
            public long databaseID = 0;
            public int hosing = 1;
            public bool trained = false;
            public bool ready = false;
            public int health = 0;
            public int trainTime = 0;
            public float trainedTime = 0;
            public float moveSpeed = 1;
            public float attackSpeed = 1;
            public float attackRange = 1;
            public float damage = 1;
            public float splashRange = 0;
            public float rangedSpeed = 5;
            public TargetPriority priority = TargetPriority.none;
            public UnitMoveType movement = UnitMoveType.ground;
            public float priorityMultiplier = 1;
        }

        public class Building
        {
            public BuildingID id = BuildingID.townhall;
            public int level = 0;
            public long databaseID = 0;
            public int x = 0;
            public int y = 0;
            public int warX = -1;
            public int warY = -1;
            public int columns = 0;
            public int rows = 0;
            public int goldStorage = 0;
            public int elixirStorage = 0;
            public int darkStorage = 0;
            public DateTime boost;
            public int health = 100;
            public float damage = 0;
            public int capacity = 0;
            public int goldCapacity = 0;
            public int elixirCapacity = 0;
            public int darkCapacity = 0;
            public float speed = 0;
            public float radius = 0;
            public DateTime constructionTime;
            public bool isConstructing = false;
            public int buildTime = 0;
            public BuildingTargetType targetType = BuildingTargetType.none;
            public float blindRange = 0;
            public float splashRange = 0;
            public float rangedSpeed = 5;
            public double percentage = 0;
        }

        public class ServerBuilding
        {
            public string id = "";
            public int level = 0;
            public long databaseID = 0;
            public int requiredGold = 0;
            public int requiredElixir = 0;
            public int requiredGems = 0;
            public int requiredDarkElixir = 0;
            public int columns = 0;
            public int rows = 0;
            public int buildTime = 0;
            public int gainedXp = 0;
        }

        public static int GetBoostResourcesCost(Data.BuildingID id, int level)
        {
            return 20;
        }

        public static T CloneClass<T>(this T target)
        {
            return Desrialize<T>(Serialize<T>(target));
        }

        public static string Serialize<T>(this T target)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter();
            xml.Serialize(writer, target);
            return writer.ToString();
        }

        public static T Desrialize<T>(this string target)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(target);
            return (T)xml.Deserialize(reader);
        }

        public async static Task<string> SerializeAsync<T>(this T target)
        {
            Task<string> task = Task.Run(() =>
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                StringWriter writer = new StringWriter();
                xml.Serialize(writer, target);
                return writer.ToString();
            });
            return await task;
        }

        public async static Task<T> DesrializeAsync<T>(this string target)
        {
            Task<T> task = Task.Run(() =>
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));
                StringReader reader = new StringReader(target);
                return (T)xml.Deserialize(reader);
            });
            return await task;
        }

        public static void CopyTo(Stream source, Stream target)
        {
            byte[] bytes = new byte[4096]; int count;
            while ((count = source.Read(bytes, 0, bytes.Length)) != 0)
            {
                target.Write(bytes, 0, count);
            }
        }

        public async static Task<byte[]> CompressAsync(string target)
        {
            Task<byte[]> task = Task.Run(() =>
            {
                return Compress(target);
            });
            return await task;
        }

        public static byte[] Compress(string target)
        {
            var bytes = Encoding.UTF8.GetBytes(target);
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        CopyTo(msi, gs);
                    }
                    return mso.ToArray();
                }
            }
        }

        public async static Task<string> DecompressAsync(byte[] bytes)
        {
            Task<string> task = Task.Run(() =>
            {
                return Decompress(bytes);
            });
            return await task;
        }

        public static string Decompress(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        CopyTo(gs, mso);
                    }
                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
        }

        public static BuildingCount GetBuildingLimits(int townHallLevel, string globalID)
        {
            if(townHallLevel > 0 && townHallLevel < buildingAvailability.Length)
            {
                for (int i = 0; i < buildingAvailability.Length; i++)
                {
                    if(buildingAvailability[i].level == townHallLevel)
                    {
                        for (int j = 0; j < buildingAvailability[i].buildings.Length; j++)
                        {
                            if (buildingAvailability[i].buildings[j].id == globalID)
                            {
                                return CloneClass(buildingAvailability[i].buildings[j]);
                            }
                        }
                        break;
                    }
                }
            }
            return null;
        }

        public static BuildingAvailability GetTownHallLimits(int targetTownHallLevel)
        {
            for (int i = 0; i < buildingAvailability.Length; i++)
            {
                if (buildingAvailability[i].level == targetTownHallLevel)
                {
                    return CloneClass(buildingAvailability[i]);
                }
            }
            return null;
        }

        public static int GetInstantBuildRequiredGems(int remainedSeconds)
        {
            int gems = 0;
            if (remainedSeconds > 0)
            {
                if (remainedSeconds <= 60)
                {
                    gems = 1;
                }
                else if (remainedSeconds <= 3600)
                {
                    gems = (int)(0.00537f * ((float)remainedSeconds - 60f)) + 1;
                }
                else if (remainedSeconds <= 86400)
                {
                    gems = (int)(0.00266f * ((float)remainedSeconds - 3600f)) + 20;
                }
                else
                {
                    gems = (int)(0.00143f * ((float)remainedSeconds - 86400f)) + 260;
                }
            }
            return gems;
        }

        public static int GetResourceGemCost(int gold, int elixir, int dark)
        {
            if (gold < 0){gold = 0; }
            if (elixir < 0) { elixir = 0; }
            if (dark < 0) { dark = 0; }
            if (gold <= 0 && elixir <= 0 && dark <= 0)
            {
                return 0;
            }
            else
            {
                return (int)Math.Ceiling(((double)(gold + elixir) * 0.001d + (double)dark * 0.1d));
            }
        }

        public enum BuyResourcePack
        {
            gold_10 = 0, gold_50 = 1, gold_100 = 2, elixir_10 = 3, elixir_50 = 4, elixir_100 = 5, dark_10 = 6, dark_50 = 7, dark_100 = 8
        }

        public class BuildingAvailability
        {
            public int level = 1;
            public BuildingCount[] buildings = null;
        }

        public class BuildingCount
        {
            public string id = "global_id";
            public int count = 0;
            public int maxLevel = 1;
            public int have = 0;
        }

        public static BuildingAvailability[] buildingAvailability =
        {
            new BuildingAvailability
            {
                level = 0,
                buildings = {}
            },
            new BuildingAvailability
            {
                level = 1,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 1, maxLevel = 2},
                    new BuildingCount { id = "elixirmine", count = 1, maxLevel = 2},
                    new BuildingCount { id = "goldstorage", count = 1, maxLevel = 1},
                    new BuildingCount { id = "elixirstorage", count = 1, maxLevel = 1},
                    new BuildingCount { id = "armycamp", count = 1, maxLevel = 1},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 3},
                    new BuildingCount { id = "cannon", count = 2, maxLevel = 2},
                }
            },
            new BuildingAvailability
            {
                level = 2,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 2, maxLevel = 4},
                    new BuildingCount { id = "elixirmine", count = 2, maxLevel = 4},
                    new BuildingCount { id = "goldstorage", count = 1, maxLevel = 3},
                    new BuildingCount { id = "elixirstorage", count = 1, maxLevel = 3},
                    new BuildingCount { id = "armycamp", count = 1, maxLevel = 2},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 4},
                    new BuildingCount { id = "cannon", count = 2, maxLevel = 3},
                    new BuildingCount { id = "archertower", count = 1, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 25, maxLevel = 2},
                }
            },
            new BuildingAvailability
            {
                level = 3,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 3, maxLevel = 6},
                    new BuildingCount { id = "elixirmine", count = 3, maxLevel = 6},
                    new BuildingCount { id = "goldstorage", count = 2, maxLevel = 6},
                    new BuildingCount { id = "elixirstorage", count = 2, maxLevel = 6},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 1},
                    new BuildingCount { id = "armycamp", count = 2, maxLevel = 3},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 5},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 2, maxLevel = 4},
                    new BuildingCount { id = "archertower", count = 1, maxLevel = 3},
                    new BuildingCount { id = "mortor", count = 1, maxLevel = 1},
                    new BuildingCount { id = "wall", count = 50, maxLevel = 3},
                    new BuildingCount { id = "boomb", count = 2, maxLevel = 2},
                }
            },
            new BuildingAvailability
            {
                level = 4,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 4, maxLevel = 8},
                    new BuildingCount { id = "elixirmine", count = 4, maxLevel = 8},
                    new BuildingCount { id = "goldstorage", count = 2, maxLevel = 8},
                    new BuildingCount { id = "elixirstorage", count = 2, maxLevel = 8},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 2},
                    new BuildingCount { id = "armycamp", count = 2, maxLevel = 4},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 6},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 2},
                    new BuildingCount { id = "cannon", count = 2, maxLevel = 5},
                    new BuildingCount { id = "archertower", count = 2, maxLevel = 4},
                    new BuildingCount { id = "mortor", count = 1, maxLevel = 2},
                    new BuildingCount { id = "airdefense", count = 1, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 75, maxLevel = 4},
                    new BuildingCount { id = "boomb", count = 2, maxLevel = 2},
                    new BuildingCount { id = "springtrap", count = 2, maxLevel = 1},
                }
            },
            new BuildingAvailability
            {
                level = 5,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 5, maxLevel = 10},
                    new BuildingCount { id = "elixirmine", count = 5, maxLevel = 10},
                    new BuildingCount { id = "goldstorage", count = 2, maxLevel = 9},
                    new BuildingCount { id = "elixirstorage", count = 2, maxLevel = 9},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 2},
                    new BuildingCount { id = "armycamp", count = 3, maxLevel = 5},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 7},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 3},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 3, maxLevel = 6},
                    new BuildingCount { id = "archertower", count = 3, maxLevel = 6},
                    new BuildingCount { id = "mortor", count = 1, maxLevel = 3},
                    new BuildingCount { id = "airdefense", count = 1, maxLevel = 3},
                    new BuildingCount { id = "wizardtower", count = 1, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 100, maxLevel = 5},
                    new BuildingCount { id = "boomb", count = 4, maxLevel = 3},
                    new BuildingCount { id = "springtrap", count = 2, maxLevel = 1},
                    new BuildingCount { id = "airbomb", count = 2, maxLevel = 2},
                }
            },
            new BuildingAvailability
            {
                level = 6,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 6, maxLevel = 10},
                    new BuildingCount { id = "elixirmine", count = 6, maxLevel = 10},
                    new BuildingCount { id = "goldstorage", count = 2, maxLevel = 10},
                    new BuildingCount { id = "elixirstorage", count = 2, maxLevel = 10},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 3},
                    new BuildingCount { id = "armycamp", count = 3, maxLevel = 6},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 8},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 4},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 2},
                    new BuildingCount { id = "cannon", count = 3, maxLevel = 7},
                    new BuildingCount { id = "archertower", count = 3, maxLevel = 7},
                    new BuildingCount { id = "mortor", count = 2, maxLevel = 4},
                    new BuildingCount { id = "airdefense", count = 2, maxLevel = 4},
                    new BuildingCount { id = "wizardtower", count = 2, maxLevel = 3},
                    new BuildingCount { id = "airsweeper", count = 1, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 125, maxLevel = 6},
                    new BuildingCount { id = "boomb", count = 4, maxLevel = 3},
                    new BuildingCount { id = "springtrap", count = 4, maxLevel = 1},
                    new BuildingCount { id = "airbomb", count = 2, maxLevel = 2},
                    new BuildingCount { id = "giantbomb", count = 1, maxLevel = 2},
                }
            },
            new BuildingAvailability
            {
                level = 7,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 6, maxLevel = 11},
                    new BuildingCount { id = "elixirmine", count = 6, maxLevel = 11},
                    new BuildingCount { id = "darkelixirmine", count = 1, maxLevel = 3},
                    new BuildingCount { id = "goldstorage", count = 2, maxLevel = 11},
                    new BuildingCount { id = "elixirstorage", count = 2, maxLevel = 11},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 2},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 3},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 6},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 9},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 2},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 3},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 5, maxLevel = 8},
                    new BuildingCount { id = "archertower", count = 4, maxLevel = 8},
                    new BuildingCount { id = "mortor", count = 3, maxLevel = 5},
                    new BuildingCount { id = "airdefense", count = 3, maxLevel = 5},
                    new BuildingCount { id = "wizardtower", count = 2, maxLevel = 4},
                    new BuildingCount { id = "airsweeper", count = 1, maxLevel = 3},
                    new BuildingCount { id = "hiddentesla", count = 2, maxLevel = 3},
                    new BuildingCount { id = "wall", count = 175, maxLevel = 6},
                    new BuildingCount { id = "boomb", count = 6, maxLevel = 4},
                    new BuildingCount { id = "springtrap", count = 4, maxLevel = 2},
                    new BuildingCount { id = "airbomb", count = 2, maxLevel = 3},
                    new BuildingCount { id = "giantbomb", count = 2, maxLevel = 2},
                    new BuildingCount { id = "seekingairmine", count = 1, maxLevel = 1},
                }
            },
            new BuildingAvailability
            {
                level = 8,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 6, maxLevel = 12},
                    new BuildingCount { id = "elixirmine", count = 6, maxLevel = 12},
                    new BuildingCount { id = "darkelixirmine", count = 2, maxLevel = 3},
                    new BuildingCount { id = "goldstorage", count = 3, maxLevel = 11},
                    new BuildingCount { id = "elixirstorage", count = 3, maxLevel = 11},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 4},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 4},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 6},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 10},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 4},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 6},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 3},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 2},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 5, maxLevel = 10},
                    new BuildingCount { id = "archertower", count = 5, maxLevel = 10},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 6},
                    new BuildingCount { id = "airdefense", count = 3, maxLevel = 6},
                    new BuildingCount { id = "wizardtower", count = 3, maxLevel = 6},
                    new BuildingCount { id = "airsweeper", count = 1, maxLevel = 4},
                    new BuildingCount { id = "hiddentesla", count = 3, maxLevel = 6},
                    new BuildingCount { id = "bombtower", count = 1, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 225, maxLevel = 8},
                    new BuildingCount { id = "boomb", count = 6, maxLevel = 5},
                    new BuildingCount { id = "springtrap", count = 6, maxLevel = 3},
                    new BuildingCount { id = "airbomb", count = 4, maxLevel = 3},
                    new BuildingCount { id = "giantbomb", count = 3, maxLevel = 3},
                    new BuildingCount { id = "seekingairmine", count = 2, maxLevel = 1},
                    new BuildingCount { id = "skeletontrap", count = 2, maxLevel = 2},
                }
            },
            new BuildingAvailability
            {
                level = 9,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 7, maxLevel = 12},
                    new BuildingCount { id = "elixirmine", count = 7, maxLevel = 12},
                    new BuildingCount { id = "darkelixirmine", count = 3, maxLevel = 6},
                    new BuildingCount { id = "goldstorage", count = 4, maxLevel = 11},
                    new BuildingCount { id = "elixirstorage", count = 4, maxLevel = 11},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 6},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 5},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 7},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 11},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 6},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 7},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 4},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 4},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "qeenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 5, maxLevel = 11},
                    new BuildingCount { id = "archertower", count = 6, maxLevel = 11},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 7},
                    new BuildingCount { id = "airdefense", count = 4, maxLevel = 7},
                    new BuildingCount { id = "wizardtower", count = 4, maxLevel = 7},
                    new BuildingCount { id = "airsweeper", count = 2, maxLevel = 5},
                    new BuildingCount { id = "hiddentesla", count = 4, maxLevel = 7},
                    new BuildingCount { id = "bombtower", count = 1, maxLevel = 3},
                    new BuildingCount { id = "xbow", count = 2, maxLevel = 3},
                    new BuildingCount { id = "wall", count = 250, maxLevel = 10},
                    new BuildingCount { id = "boomb", count = 6, maxLevel = 6},
                    new BuildingCount { id = "springtrap", count = 6, maxLevel = 4},
                    new BuildingCount { id = "airbomb", count = 4, maxLevel = 4},
                    new BuildingCount { id = "giantbomb", count = 4, maxLevel = 3},
                    new BuildingCount { id = "seekingairmine", count = 4, maxLevel = 2},
                    new BuildingCount { id = "skeletontrap", count = 2, maxLevel = 3},
                }
            },
            new BuildingAvailability
            {
                level = 10,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 7, maxLevel = 13},
                    new BuildingCount { id = "elixirmine", count = 7, maxLevel = 13},
                    new BuildingCount { id = "darkelixirmine", count = 3, maxLevel = 7},
                    new BuildingCount { id = "goldstorage", count = 4, maxLevel = 11},
                    new BuildingCount { id = "elixirstorage", count = 4, maxLevel = 11},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 6},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 6},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 8},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 12},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 7},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 8},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "qeenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 6, maxLevel = 13},
                    new BuildingCount { id = "archertower", count = 7, maxLevel = 13},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 8},
                    new BuildingCount { id = "airdefense", count = 4, maxLevel = 8},
                    new BuildingCount { id = "wizardtower", count = 4, maxLevel = 9},
                    new BuildingCount { id = "airsweeper", count = 2, maxLevel = 6},
                    new BuildingCount { id = "hiddentesla", count = 4, maxLevel = 8},
                    new BuildingCount { id = "bombtower", count = 2, maxLevel = 4},
                    new BuildingCount { id = "xbow", count = 3, maxLevel = 4},
                    new BuildingCount { id = "infernotower", count = 2, maxLevel = 3},
                    new BuildingCount { id = "wall", count = 275, maxLevel = 11},
                    new BuildingCount { id = "boomb", count = 6, maxLevel = 7},
                    new BuildingCount { id = "springtrap", count = 6, maxLevel = 5},
                    new BuildingCount { id = "airbomb", count = 5, maxLevel = 4},
                    new BuildingCount { id = "giantbomb", count = 5, maxLevel = 4},
                    new BuildingCount { id = "seekingairmine", count = 5, maxLevel = 3},
                    new BuildingCount { id = "skeletontrap", count = 3, maxLevel = 4},
                }
            },
            new BuildingAvailability
            {
                level = 11,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 7, maxLevel = 14},
                    new BuildingCount { id = "elixirmine", count = 7, maxLevel = 14},
                    new BuildingCount { id = "darkelixirmine", count = 3, maxLevel = 8},
                    new BuildingCount { id = "goldstorage", count = 4, maxLevel = 12},
                    new BuildingCount { id = "elixirstorage", count = 4, maxLevel = 12},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 6},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 7},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 9},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 13},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 8},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 9},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 6},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "qeenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "wardenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 7, maxLevel = 15},
                    new BuildingCount { id = "archertower", count = 8, maxLevel = 15},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 10},
                    new BuildingCount { id = "airdefense", count = 4, maxLevel = 9},
                    new BuildingCount { id = "wizardtower", count = 5, maxLevel = 10},
                    new BuildingCount { id = "airsweeper", count = 2, maxLevel = 7},
                    new BuildingCount { id = "hiddentesla", count = 4, maxLevel = 9},
                    new BuildingCount { id = "bombtower", count = 2, maxLevel = 6},
                    new BuildingCount { id = "xbow", count = 4, maxLevel = 5},
                    new BuildingCount { id = "infernotower", count = 2, maxLevel = 5},
                    new BuildingCount { id = "eagleartillery", count = 1, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 300, maxLevel = 12},
                    new BuildingCount { id = "boomb", count = 6, maxLevel = 8},
                    new BuildingCount { id = "springtrap", count = 6, maxLevel = 5},
                    new BuildingCount { id = "airbomb", count = 5, maxLevel = 5},
                    new BuildingCount { id = "giantbomb", count = 5, maxLevel = 5},
                    new BuildingCount { id = "seekingairmine", count = 5, maxLevel = 3},
                    new BuildingCount { id = "skeletontrap", count = 3, maxLevel = 4},
                    new BuildingCount { id = "tornadotrap", count = 1, maxLevel = 2},
                }
            },
            new BuildingAvailability
            {
                level = 12,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "elixirmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "darkelixirmine", count = 3, maxLevel = 9},
                    new BuildingCount { id = "goldstorage", count = 4, maxLevel = 13},
                    new BuildingCount { id = "elixirstorage", count = 4, maxLevel = 13},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 7},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 8},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 10},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 14},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 9},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 10},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 6},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "workshop", count = 1, maxLevel = 3},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "qeenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "wardenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 7, maxLevel = 17},
                    new BuildingCount { id = "archertower", count = 8, maxLevel = 17},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 12},
                    new BuildingCount { id = "airdefense", count = 4, maxLevel = 10},
                    new BuildingCount { id = "wizardtower", count = 5, maxLevel = 11},
                    new BuildingCount { id = "airsweeper", count = 2, maxLevel = 7},
                    new BuildingCount { id = "hiddentesla", count = 5, maxLevel = 10},
                    new BuildingCount { id = "bombtower", count = 2, maxLevel = 7},
                    new BuildingCount { id = "xbow", count = 4, maxLevel = 6},
                    new BuildingCount { id = "infernotower", count = 3, maxLevel = 6},
                    new BuildingCount { id = "eagleartillery", count = 1, maxLevel = 3},
                    new BuildingCount { id = "wall", count = 300, maxLevel = 13},
                    new BuildingCount { id = "boomb", count = 6, maxLevel = 8},
                    new BuildingCount { id = "springtrap", count = 8, maxLevel = 5},
                    new BuildingCount { id = "airbomb", count = 6, maxLevel = 6},
                    new BuildingCount { id = "giantbomb", count = 6, maxLevel = 5},
                    new BuildingCount { id = "seekingairmine", count = 6, maxLevel = 3},
                    new BuildingCount { id = "skeletontrap", count = 3, maxLevel = 4},
                    new BuildingCount { id = "tornadotrap", count = 1, maxLevel = 3},
                }
            },
            new BuildingAvailability
            {
                level = 13,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "elixirmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "darkelixirmine", count = 3, maxLevel = 9},
                    new BuildingCount { id = "goldstorage", count = 4, maxLevel = 14},
                    new BuildingCount { id = "elixirstorage", count = 4, maxLevel = 14},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 8},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 9},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 11},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 15},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 9},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 11},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 7},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "workshop", count = 1, maxLevel = 5},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "qeenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "wardenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "championaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 7, maxLevel = 19},
                    new BuildingCount { id = "archertower", count = 8, maxLevel = 19},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 13},
                    new BuildingCount { id = "airdefense", count = 4, maxLevel = 11},
                    new BuildingCount { id = "wizardtower", count = 5, maxLevel = 13},
                    new BuildingCount { id = "airsweeper", count = 2, maxLevel = 7},
                    new BuildingCount { id = "hiddentesla", count = 5, maxLevel = 12},
                    new BuildingCount { id = "bombtower", count = 2, maxLevel = 8},
                    new BuildingCount { id = "xbow", count = 4, maxLevel = 8},
                    new BuildingCount { id = "infernotower", count = 3, maxLevel = 7},
                    new BuildingCount { id = "eagleartillery", count = 1, maxLevel = 4},
                    new BuildingCount { id = "scattershot", count = 2, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 300, maxLevel = 14},
                    new BuildingCount { id = "boomb", count = 7, maxLevel = 9},
                    new BuildingCount { id = "springtrap", count = 9, maxLevel = 5},
                    new BuildingCount { id = "airbomb", count = 6, maxLevel = 8},
                    new BuildingCount { id = "giantbomb", count = 6, maxLevel = 7},
                    new BuildingCount { id = "seekingairmine", count = 7, maxLevel = 4},
                    new BuildingCount { id = "skeletontrap", count = 3, maxLevel = 4},
                    new BuildingCount { id = "tornadotrap", count = 1, maxLevel = 3},
                }
            },
            new BuildingAvailability
            {
                level = 14,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "elixirmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "darkelixirmine", count = 3, maxLevel = 9},
                    new BuildingCount { id = "goldstorage", count = 4, maxLevel = 15},
                    new BuildingCount { id = "elixirstorage", count = 4, maxLevel = 15},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 9},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 10},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 11},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 16},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 9},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 12},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 7},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "workshop", count = 1, maxLevel = 6},
                    new BuildingCount { id = "pethouse", count = 1, maxLevel = 4},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "qeenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "wardenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "championaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 7, maxLevel = 20},
                    new BuildingCount { id = "archertower", count = 8, maxLevel = 20},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 14},
                    new BuildingCount { id = "airdefense", count = 4, maxLevel = 12},
                    new BuildingCount { id = "wizardtower", count = 5, maxLevel = 14},
                    new BuildingCount { id = "airsweeper", count = 2, maxLevel = 7},
                    new BuildingCount { id = "hiddentesla", count = 5, maxLevel = 13},
                    new BuildingCount { id = "bombtower", count = 2, maxLevel = 9},
                    new BuildingCount { id = "xbow", count = 4, maxLevel = 9},
                    new BuildingCount { id = "infernotower", count = 3, maxLevel = 8},
                    new BuildingCount { id = "eagleartillery", count = 1, maxLevel = 5},
                    new BuildingCount { id = "scattershot", count = 2, maxLevel = 3},
                    new BuildingCount { id = "wall", count = 325, maxLevel = 15},
                    new BuildingCount { id = "boomb", count = 8, maxLevel = 10},
                    new BuildingCount { id = "springtrap", count = 9, maxLevel = 5},
                    new BuildingCount { id = "airbomb", count = 7, maxLevel = 9},
                    new BuildingCount { id = "giantbomb", count = 7, maxLevel = 8},
                    new BuildingCount { id = "seekingairmine", count = 8, maxLevel = 4},
                    new BuildingCount { id = "skeletontrap", count = 4, maxLevel = 4},
                    new BuildingCount { id = "tornadotrap", count = 1, maxLevel = 3},
                }
            },
            new BuildingAvailability
            {
                level = 15,
                buildings = new BuildingCount[]
                {
                    new BuildingCount { id = "townhall", count = 1, maxLevel = 15},
                    new BuildingCount { id = "buildershut", count = 5, maxLevel = 1},
                    new BuildingCount { id = "goldmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "elixirmine", count = 7, maxLevel = 15},
                    new BuildingCount { id = "darkelixirmine", count = 3, maxLevel = 9},
                    new BuildingCount { id = "goldstorage", count = 4, maxLevel = 16},
                    new BuildingCount { id = "elixirstorage", count = 4, maxLevel = 16},
                    new BuildingCount { id = "darkelixirstorage", count = 1, maxLevel = 10},
                    new BuildingCount { id = "clancastle", count = 1, maxLevel = 11},
                    new BuildingCount { id = "armycamp", count = 4, maxLevel = 12},
                    new BuildingCount { id = "barracks", count = 1, maxLevel = 16},
                    new BuildingCount { id = "darkbarracks", count = 1, maxLevel = 9},
                    new BuildingCount { id = "laboratory", count = 1, maxLevel = 13},
                    new BuildingCount { id = "spellfactory", count = 1, maxLevel = 7},
                    new BuildingCount { id = "darkspellfactory", count = 1, maxLevel = 5},
                    new BuildingCount { id = "workshop", count = 1, maxLevel = 7},
                    new BuildingCount { id = "pethouse", count = 1, maxLevel = 8},
                    new BuildingCount { id = "kingaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "qeenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "wardenaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "championaltar", count = 1, maxLevel = 1},
                    new BuildingCount { id = "cannon", count = 7, maxLevel = 21},
                    new BuildingCount { id = "archertower", count = 8, maxLevel = 21},
                    new BuildingCount { id = "mortor", count = 4, maxLevel = 15},
                    new BuildingCount { id = "airdefense", count = 4, maxLevel = 13},
                    new BuildingCount { id = "wizardtower", count = 5, maxLevel = 15},
                    new BuildingCount { id = "airsweeper", count = 2, maxLevel = 7},
                    new BuildingCount { id = "hiddentesla", count = 5, maxLevel = 13},
                    new BuildingCount { id = "bombtower", count = 2, maxLevel = 10},
                    new BuildingCount { id = "xbow", count = 4, maxLevel = 10},
                    new BuildingCount { id = "infernotower", count = 3, maxLevel = 9},
                    new BuildingCount { id = "eagleartillery", count = 1, maxLevel = 5},
                    new BuildingCount { id = "scattershot", count = 2, maxLevel = 3},
                    new BuildingCount { id = "spelltower", count = 2, maxLevel = 3},
                    new BuildingCount { id = "monolith", count = 1, maxLevel = 2},
                    new BuildingCount { id = "wall", count = 325, maxLevel = 16},
                    new BuildingCount { id = "boomb", count = 8, maxLevel = 11},
                    new BuildingCount { id = "springtrap", count = 9, maxLevel = 5},
                    new BuildingCount { id = "airbomb", count = 7, maxLevel = 10},
                    new BuildingCount { id = "giantbomb", count = 7, maxLevel = 8},
                    new BuildingCount { id = "seekingairmine", count = 8, maxLevel = 4},
                    new BuildingCount { id = "skeletontrap", count = 4, maxLevel = 4},
                    new BuildingCount { id = "tornadotrap", count = 1, maxLevel = 3},
                }
            },
        };

    }
}

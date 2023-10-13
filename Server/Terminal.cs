using System;
using System.Numerics;

namespace DevelopersHub.RealtimeNetworking.Server
{
    class Terminal
    {

        #region Update
        public const int updatesPerSecond = 30;
        public static void Update()
        {
            Database.Update();
        }
        #endregion

        #region Connection
        public static string[] clientVersions = { "1.0.0", "1.0.1" };
        public const int maxPlayers = 100000;
        public const int portNumber = 5555;
        public static int onlinePlayers = 0;
        public static readonly string dataFolderPath = "C:\\Clash Of Whatever\\";
        public static readonly string logFolderPath = "C:\\Clash Of Whatever\\Errors\\";

        public static void OnClientConnected(int id, string ip)
        {
            if (!Server.clients[id].connected)
            {
                Server.clients[id].connected = true;
                UpdateLog(1);
            }
        }

        public static void OnClientDisconnected(int id, string ip)
        {
            Database.PlayerDisconnected(id);
            if (Server.clients[id].connected)
            {
                Server.clients[id].connected = false;
                UpdateLog(-1);
            }
        }

        public static void UpdateLog(int amount)
        {
            onlinePlayers += amount;
            Console.Clear();
            Console.WriteLine("Online Players: " + onlinePlayers.ToString());
        }

        public static void OnServerStarted()
        {
            Database.Initialize();
        }
        #endregion

        #region Data
        public enum RequestsID
        {
            AUTH = 1, SYNC = 2, BUILD = 3, REPLACE = 4, COLLECT = 5, PREUPGRADE = 6, UPGRADE = 7, INSTANTBUILD = 8, TRAIN = 9, CANCELTRAIN = 10, BATTLEFIND = 11, BATTLESTART = 12, BATTLEFRAME = 13, BATTLEEND = 14, OPENCLAN = 15, GETCLANS = 16, JOINCLAN = 17, LEAVECLAN = 18, EDITCLAN = 19, CREATECLAN = 20, OPENWAR = 21, STARTWAR = 22, CANCELWAR = 23, WARSTARTED = 24, WARATTACK = 25, WARREPORTLIST = 26, WARREPORT = 27, JOINREQUESTS = 28, JOINRESPONSE = 29, GETCHATS = 30, SENDCHAT = 31, SENDCODE = 32, CONFIRMCODE = 33, EMAILCODE = 34, EMAILCONFIRM = 35, LOGOUT = 36, KICKMEMBER = 37, BREW = 38, CANCELBREW = 39, RESEARCH = 40, PROMOTEMEMBER = 41, DEMOTEMEMBER = 42, SCOUT = 43, BUYSHIELD = 44, BUYGEM = 45, BYUGOLD = 46, REPORTCHAT = 47, PLAYERSRANK = 48, BOOST = 49, BUYRESOURCE = 50, BATTLEREPORTS = 51, BATTLEREPORT = 52, RENAME = 53
        }

        public static void ReceivedPacket(int clientID, Packet packet)
        {
            try
            {
                int id = packet.ReadInt();
                string device = "";
                long databaseID = 0;
                switch ((RequestsID)id)
                {
                    case RequestsID.AUTH:
                        device = packet.ReadString();
                        string pass = packet.ReadString();
                        string user = packet.ReadString();
                        Database.AuthenticatePlayer(clientID, device, pass, user);
                        break;
                    case RequestsID.SYNC:
                        device = packet.ReadString();
                        Database.SyncPlayerData(clientID, device);
                        break;
                    case RequestsID.BUILD:
                        device = packet.ReadString();
                        string building = packet.ReadString();
                        int x = packet.ReadInt();
                        int y = packet.ReadInt();
                        int layoutBuild = packet.ReadInt();
                        databaseID = packet.ReadLong();
                        Database.PlaceBuilding(clientID, device, building, x, y, layoutBuild, databaseID);
                        break;
                    case RequestsID.REPLACE:
                        databaseID = packet.ReadLong();
                        int replaceX = packet.ReadInt();
                        int replaceY = packet.ReadInt();
                        int layoutReplace = packet.ReadInt();
                        Database.ReplaceBuilding(clientID, databaseID, replaceX, replaceY, layoutReplace);
                        break;
                    case RequestsID.COLLECT:
                        long dbid = packet.ReadLong();
                        Database.Collect(clientID, dbid);
                        break;
                    case RequestsID.PREUPGRADE:
                        //databaseID = packet.ReadLong();
                        //Database.GetNextLevelRequirements(clientID, databaseID);
                        break;
                    case RequestsID.UPGRADE:
                        databaseID = packet.ReadLong();
                        Database.UpgradeBuilding(clientID, databaseID);
                        break;
                    case RequestsID.INSTANTBUILD:
                        databaseID = packet.ReadLong();
                        Database.InstantBuild(clientID, databaseID);
                        break;
                    case RequestsID.TRAIN:
                        string globalID = packet.ReadString();
                        Database.TrainUnit(clientID, globalID);
                        break;
                    case RequestsID.CANCELTRAIN:
                        databaseID = packet.ReadLong();
                        Database.CancelTrainUnit(clientID, databaseID);
                        break;
                    case RequestsID.BATTLEFIND:
                        Database.FindBattleTarget(clientID);
                        break;
                    case RequestsID.BATTLESTART:
                        int bytesLength = packet.ReadInt();
                        byte[] bytes = packet.ReadBytes(bytesLength);
                        int battleType = packet.ReadInt();
                        Database.StartBattle(clientID, bytes, (Data.BattleType)battleType);
                        break;
                    case RequestsID.BATTLEFRAME:
                        int bfl = packet.ReadInt();
                        byte[] bf = packet.ReadBytes(bfl);
                        Database.AddBattleFrame(clientID, bf);
                        break;
                    case RequestsID.BATTLEEND:
                        databaseID = Server.clients[clientID].account;
                        bool surrender = packet.ReadBool();
                        int frame = packet.ReadInt();
                        Database.EndBattle(databaseID, surrender, frame);
                        break;
                    case RequestsID.OPENCLAN:
                        databaseID = packet.ReadLong();
                        long clanID = packet.ReadLong();
                        Database.OpenClan(clientID, databaseID, clanID);
                        break;
                    case RequestsID.GETCLANS:
                        int page = packet.ReadInt();
                        Database.GetClans(clientID, page);
                        break;
                    case RequestsID.CREATECLAN:
                        string clanName = packet.ReadString();
                        int minTrophies = packet.ReadInt();
                        int minHall = packet.ReadInt();
                        int pattern = packet.ReadInt();
                        int background = packet.ReadInt();
                        string patternColor = packet.ReadString();
                        string backgroundColor = packet.ReadString();
                        int joinType = packet.ReadInt();
                        Database.CreateClan(clientID, clanName, minTrophies, minHall, pattern, background, patternColor, backgroundColor, joinType);
                        break;
                    case RequestsID.JOINCLAN:
                        int clan_id = packet.ReadInt();
                        Database.JoinClan(clientID, clan_id);
                        break;
                    case RequestsID.LEAVECLAN:
                        Database.LeaveClan(clientID);
                        break;
                    case RequestsID.EDITCLAN:
                        string clanNameEdit = packet.ReadString();
                        int minTrophiesEdit = packet.ReadInt();
                        int minHallEdit = packet.ReadInt();
                        int patternEdit = packet.ReadInt();
                        int backgroundEdit = packet.ReadInt();
                        string patternColorEdit = packet.ReadString();
                        string backgroundColorEdit = packet.ReadString();
                        int joinTypeEdit = packet.ReadInt();
                        Database.EditClan(clientID, clanNameEdit, minTrophiesEdit, minHallEdit, patternEdit, backgroundEdit, patternColorEdit, backgroundColorEdit, joinTypeEdit);
                        break;
                    case RequestsID.OPENWAR:
                        Database.OpenClanWar(clientID);
                        break;
                    case RequestsID.STARTWAR:
                        string warMembersData = packet.ReadString();
                        Database.StartClanWar(clientID, warMembersData);
                        break;
                    case RequestsID.CANCELWAR:
                        Database.CancelClanWar(clientID);
                        break;
                    case RequestsID.WARATTACK:
                        databaseID = packet.ReadLong();
                        Database.StartWarAttack(clientID, databaseID);
                        break;
                    case RequestsID.WARREPORTLIST:
                        Database.GetWarReportsList(clientID);
                        break;
                    case RequestsID.WARREPORT:
                        databaseID = packet.ReadLong();
                        Database.GetWarReport(clientID, databaseID);
                        break;
                    case RequestsID.JOINREQUESTS:
                        Database.GetClanJoinRequests(clientID);
                        break;
                    case RequestsID.JOINRESPONSE:
                        databaseID = packet.ReadLong();
                        bool accepted = packet.ReadBool();
                        Database.ClanJoinRequestResponse(clientID, databaseID, accepted);
                        break;
                    case RequestsID.SENDCHAT:
                        string message = packet.ReadString();
                        int sendType = packet.ReadInt();
                        databaseID = packet.ReadLong();
                        Database.SendChatMessage(clientID, message, (Data.ChatType)sendType, databaseID);
                        break;
                    case RequestsID.GETCHATS:
                        int msgType = packet.ReadInt();
                        databaseID = packet.ReadLong();
                        Database.SyncMessages(clientID, (Data.ChatType)msgType, databaseID);
                        break;
                    case RequestsID.SENDCODE:
                        device = packet.ReadString();
                        string targetEmail = packet.ReadString();
                        Database.SendRecoveryCode(clientID, device, targetEmail);
                        break;
                    case RequestsID.CONFIRMCODE:
                        device = packet.ReadString();
                        string confirmEmail = packet.ReadString();
                        string code = packet.ReadString();
                        Database.ConfirmRecoveryCode(clientID, device, confirmEmail, code);
                        break;
                    case RequestsID.EMAILCODE:
                        device = packet.ReadString();
                        string sendEmail = packet.ReadString();
                        Database.SendEmailCode(clientID, device, sendEmail);
                        break;
                    case RequestsID.EMAILCONFIRM:
                        device = packet.ReadString();
                        string coEmail = packet.ReadString();
                        string codeEmail = packet.ReadString();
                        Database.ConfirmEmailCode(clientID, device, coEmail, codeEmail);
                        break;
                    case RequestsID.LOGOUT:
                        device = packet.ReadString();
                        Database.LogOut(clientID, device);
                        break;
                    case RequestsID.KICKMEMBER:
                        databaseID = packet.ReadLong();
                        Database.KickOutClanMember(clientID, databaseID);
                        break;
                    case RequestsID.BREW:
                        string spellID = packet.ReadString();
                        Database.BrewSpell(clientID, spellID);
                        break;
                    case RequestsID.CANCELBREW:
                        databaseID = packet.ReadLong();
                        Database.CancelBrewSpell(clientID, databaseID);
                        break;
                    case RequestsID.RESEARCH:
                        int type = packet.ReadInt();
                        string global_id = packet.ReadString();
                        Database.DoResearch(clientID, (Data.ResearchType)type, global_id);
                        break;
                    case RequestsID.PROMOTEMEMBER:
                        databaseID = packet.ReadLong();
                        Database.PromoteClanMember(clientID, databaseID);
                        break;
                    case RequestsID.DEMOTEMEMBER:
                        databaseID = packet.ReadLong();
                        Database.DemoteClanMember(clientID, databaseID);
                        break;
                    case RequestsID.SCOUT:
                        databaseID = packet.ReadLong();
                        int scoutType = packet.ReadInt();
                        Database.Scout(clientID, databaseID, scoutType);
                        break;
                    case RequestsID.BUYGEM:
                        int gemPack = packet.ReadInt();
                        string ps = packet.ReadString();
                        string pr = packet.ReadString();
                        string or = packet.ReadString();
                        string pk = packet.ReadString();
                        string mk = packet.ReadString();
                        Database.BuyGem(clientID, gemPack, or, pr, ps, pk, mk);
                        break;
                    case RequestsID.BUYSHIELD:
                        int shieldPack = packet.ReadInt();
                        Database.BuyShield(clientID, shieldPack);
                        break;
                    case RequestsID.BYUGOLD:
                        int goldPack = packet.ReadInt();
                        Database.BuyGold(clientID, goldPack);
                        break;
                    case RequestsID.REPORTCHAT:
                        databaseID = packet.ReadLong();
                        Database.ReportChatMessage(clientID, databaseID);
                        break;
                    case RequestsID.PLAYERSRANK:
                        int p = packet.ReadInt();
                        Database.GetPlayersRanking(clientID, p);
                        break;
                    case RequestsID.BOOST:
                        databaseID = packet.ReadLong();
                        Database.BoostResource(clientID, databaseID);
                        break;
                    case RequestsID.BUYRESOURCE:
                        int resPack = packet.ReadInt();
                        Database.BuyResources(clientID, resPack);
                        break;
                    case RequestsID.BATTLEREPORTS:
                        Database.GetBattlesList(clientID);
                        break;
                    case RequestsID.BATTLEREPORT:
                        long repId = packet.ReadLong();
                        Database.GetBattleReport(clientID, repId);
                        break;
                    case RequestsID.RENAME:
                        string nm = packet.ReadString();
                        Database.ChangePlayerName(clientID, nm);
                        break;
                }
            }
            catch (Exception ex)
            {
                Tools.LogError(ex.Message, ex.StackTrace);
            }
        }

        public static void ReceivedBytes(int clientID, int packetID, byte[] data)
        {

        }

        public static void ReceivedString(int clientID, int packetID, string data)
        {

        }

        public static void ReceivedInteger(int clientID, int packetID, int data)
        {

        }

        public static void ReceivedFloat(int clientID, int packetID, float data)
        {

        }

        public static void ReceivedBoolean(int clientID, int packetID, bool data)
        {

        }

        public static void ReceivedVector3(int clientID, int packetID, Vector3 data)
        {

        }

        public static void ReceivedQuaternion(int clientID, int packetID, Quaternion data)
        {

        }

        public static void ReceivedLong(int clientID, int packetID, long data)
        {

        }

        public static void ReceivedShort(int clientID, int packetID, short data)
        {

        }

        public static void ReceivedByte(int clientID, int packetID, byte data)
        {

        }

        public static void ReceivedEvent(int clientID, int packetID)
        {

        }
        #endregion

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DevelopersHub.RealtimeNetworking.Client;
    using UnityEngine.SceneManagement;
    using System;

    public class Player : MonoBehaviour
    {

        public Data.Player data = new Data.Player();
        private static Player _instance = null; public static Player instanse { get { return _instance; } }
        public Data.InitializationData initializationData = new Data.InitializationData();
        private bool _inBattle = false; public static bool inBattle { get { return instanse._inBattle; } set { instanse._inBattle = value; } }

        public Data.ServerBuilding GetServerBuilding(Data.BuildingID id, int level)
        {
            for (int i = 0; i < initializationData.serverBuildings.Count; i++)
            {
                if (initializationData.serverBuildings[i].id == id.ToString() && initializationData.serverBuildings[i].level == level)
                {
                    return initializationData.serverBuildings[i];
                }
            }
            return null;
        }

        public enum RequestsID
        {
            AUTH = 1, SYNC = 2, BUILD = 3, REPLACE = 4, COLLECT = 5, PREUPGRADE = 6, UPGRADE = 7, INSTANTBUILD = 8, TRAIN = 9, CANCELTRAIN = 10, BATTLEFIND = 11, BATTLESTART = 12, BATTLEFRAME = 13, BATTLEEND = 14, OPENCLAN = 15, GETCLANS = 16, JOINCLAN = 17, LEAVECLAN = 18, EDITCLAN = 19, CREATECLAN = 20, OPENWAR = 21, STARTWAR = 22, CANCELWAR = 23, WARSTARTED = 24, WARATTACK = 25, WARREPORTLIST = 26, WARREPORT = 27, JOINREQUESTS = 28, JOINRESPONSE = 29, GETCHATS = 30, SENDCHAT = 31, SENDCODE = 32, CONFIRMCODE = 33, EMAILCODE = 34, EMAILCONFIRM = 35, LOGOUT = 36, KICKMEMBER = 37, BREW = 38, CANCELBREW = 39, RESEARCH = 40, PROMOTEMEMBER = 41, DEMOTEMEMBER = 42, SCOUT = 43, BUYSHIELD = 44, BUYGEM = 45, BYUGOLD = 46, REPORTCHAT = 47, PLAYERSRANK = 48, BOOST = 49, BUYRESOURCE = 50, BATTLEREPORTS = 51, BATTLEREPORT = 52, RENAME = 53
        }

        public enum Panel
        {
            main = 0, clan = 1
        }

        public static readonly string username_key = "username";
        public static readonly string password_key = "password";

        private int _unreadBattleReports = 0; public int unreadBattleReports { get { return _gold; } }
        private int _gold = 0; public int gold { get { return _gold; } set { _gold = value; } }
        private int _maxGold = 0; public int maxGold { get { return _maxGold; } }

        private int _elixir = 0; public int elixir { get { return _elixir; } set { _elixir = value; } }
        private int _maxElixir = 0; public int maxElixir { get { return _maxElixir; } }

        private int _darkElixir = 0; public int darkElixir { get { return _darkElixir; } set { _darkElixir = value; } }
        private int _maxDarkElixir = 0; public int maxDarkElixir { get { return _maxDarkElixir; }  }

        private int _townHallLevel = 1; public int townHallLevel { get { return _townHallLevel; } }
        private int _spellFactoryLevel = 0; public int spellFactoryLevel { get { return _spellFactoryLevel; } }
        private int _darkSpellFactoryLevel = 0; public int darkSpellFactoryLevel { get { return _darkSpellFactoryLevel; } }
        private int _barracksLevel = 0; public int barracksLevel { get { return _barracksLevel; } }
        private int _darkBarracksLevel = 0; public int darkBarracksLevel { get { return _townHallLevel; } }
        private bool _callDisconnectError = true;

        private void Start()
        {
            // Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.Full);
            RealtimeNetworking.OnPacketReceived += ReceivedPaket;
            RealtimeNetworking.OnDisconnectedFromServer += DisconnectedFromServer;
            string device = SystemInfo.deviceUniqueIdentifier;
            string password = "";
            string username = "";
            if (PlayerPrefs.HasKey(password_key))
            {
                password = PlayerPrefs.GetString(password_key);
            }
            if (PlayerPrefs.HasKey(username_key))
            {
                username = PlayerPrefs.GetString(username_key);
            }
            Packet packet = new Packet();
            packet.Write((int)RequestsID.AUTH);
            packet.Write(device);
            packet.Write(password);
            packet.Write(username);
            Sender.TCP_Send(packet);
        }

        private void Awake()
        {
            _instance = this;
            Application.runInBackground = true;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void OnDestroy()
        {
            RealtimeNetworking.OnPacketReceived -= ReceivedPaket;
            RealtimeNetworking.OnDisconnectedFromServer -= DisconnectedFromServer;
        }

        private bool connected = false;
        private float timer = 0;
        private bool updating = false;
        private float syncTime = 5;
        [HideInInspector] public DateTime lastUpdate = DateTime.Now;
        [HideInInspector] public DateTime lastUpdateSent = DateTime.Now;

        private void Update()
        {
            if (connected)
            {
                if (!_inBattle)
                {
                    if (timer <= 0)
                    {
                        if(updating == false)
                        {
                            updating = true;
                            timer = syncTime;
                            SendSyncRequest();
                        }
                    }
                    else
                    {
                        timer -= Time.deltaTime;
                    }
                }
                data.nowTime = data.nowTime.AddSeconds(Time.deltaTime);
            }
        }

        private void ReceivedPaket(Packet packet)
        {
            try
            {
                int id = packet.ReadInt();
                long databaseID = 0;
                int response = 0;
                int bytesLength = 0;
                byte[] bytes;

                switch ((RequestsID)id)
                {
                    case RequestsID.AUTH:
                        response = packet.ReadInt();
                        if(response == 1)
                        {
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            _unreadBattleReports = packet.ReadInt();
                            UI_Main.instanse.ChangeUnreadBattleReports(_unreadBattleReports);
                            initializationData = Data.Desrialize<Data.InitializationData>(Data.Decompress(bytes));
                            bool versionValid = false;
                            bool isThereNewerVersion = true;
                            for (int i = 0; i < initializationData.versions.Length; i++)
                            {
                                if(initializationData.versions[i] == Application.version)
                                {
                                    versionValid = true;
                                    isThereNewerVersion = (i < (initializationData.versions.Length - 1));
                                    break;
                                }
                            }
                            if(!versionValid)
                            {
                                switch (Language.instanse.language)
                                {
                                    case Language.LanguageID.persian:
                                        MessageBox.Open(1, 0.9f, false, MessageResponded, new string[] { "این ورژن منقضی شده. لطفاً ورژن جدید بازی را دانلود کنید." }, new string[] { "خروج" });
                                        break;
                                    default:
                                        MessageBox.Open(1, 0.9f, false, MessageResponded, new string[] { "This version is expired. Please download the new version of the game." }, new string[] { "Exit" });
                                        break;
                                }
                            }
                            else
                            {
                                if(isThereNewerVersion)
                                {
                                    /*
                                    switch (Language.instanse.language)
                                    {
                                        case Language.LanguageID.persian:
                                            MessageBox.Open(1, 0.9f, false, MessageResponded, new string[] { "ورژن جدیدی از بازی منتشر شده و پیشنهاد میشود بازی را بروزرسانی کنید." }, new string[] { "باشه" });
                                            break;
                                        default:
                                            MessageBox.Open(1, 0.9f, false, MessageResponded, new string[] { "There is a new version available. We recommend you to update the game." }, new string[] { "OK" });
                                            break;
                                    }
                                    */
                                }
                                connected = true;
                                updating = true;
                                timer = 0;
                                PlayerPrefs.SetString(password_key, initializationData.password);
                                SendSyncRequest();
                            }
                        }
                        else
                        {
                            switch (Language.instanse.language)
                            {
                                case Language.LanguageID.persian:
                                    MessageBox.Open(0, 0.8f, false, MessageResponded, new string[] { "احراز حویت شما با خطا مواجه شد." }, new string[] { "باشه" });
                                    break;
                                default:
                                    MessageBox.Open(0, 0.8f, false, MessageResponded, new string[] { "Failed to authenticate your account." }, new string[] { "OK" });
                                    break;
                            }
                            Client.instance.Disconnect(false);
                            // RestartGame();
                        }
                        break;
                    case RequestsID.SYNC:
                        response = packet.ReadInt();
                        if(response == 1)
                        {
                            int playerBytesLength = packet.ReadInt();
                            byte[] playerBytes = packet.ReadBytes(playerBytesLength);
                            string playerData = Data.Decompress(playerBytes);
                            Data.Player playerSyncData = Data.Desrialize<Data.Player>(playerData);
                            SyncData(playerSyncData);
                            if (playerSyncData.banned)
                            {
                                switch (Language.instanse.language)
                                {
                                    case Language.LanguageID.persian:
                                        MessageBox.Open(1, 0.9f, false, MessageResponded, new string[] { "حساب شما مسدود شده است. لطفاً جهت کسب اطلاعات بیشتر با ما تماس بگیرید." }, new string[] { "خروج" });
                                        break;
                                    default:
                                        MessageBox.Open(1, 0.9f, false, MessageResponded, new string[] { "Your account has been banned. Please contact us for more information." }, new string[] { "Exit" });
                                        break;
                                }
                                break;
                            }
                            lastUpdate = DateTime.Now;
                        }
                        updating = false;
                        break;
                    case RequestsID.BUILD:
                        response = packet.ReadInt();
                        switch (response)
                        {
                            case 0:
                                Debug.Log("Unknown");
                                break;
                            case 1:
                                RushSyncRequest();
                                break;
                            case 2:
                                Debug.Log("No resources");
                                break;
                            case 3:
                                Debug.Log("Max level");
                                break;
                            case 4:
                                Debug.Log("Place taken");
                                break;
                            case 5:
                                Debug.Log("No builder");
                                break;
                            case 6:
                                Debug.Log("Max limit reached");
                                break;
                        }
                        break;
                    case RequestsID.REPLACE:
                        int replaceResponse = packet.ReadInt();
                        int replaceX = packet.ReadInt();
                        int replaceY = packet.ReadInt();
                        long replaceID = packet.ReadLong();
                        for (int i = 0; i < UI_Main.instanse._grid.buildings.Count; i++)
                        {
                            if (UI_Main.instanse._grid.buildings[i].databaseID == replaceID)
                            {
                                switch (replaceResponse)
                                {
                                    case 0:
                                        Debug.Log("No building");
                                        break;
                                    case 1:
                                        UI_Main.instanse._grid.buildings[i].PlacedOnGrid(replaceX, replaceY, true);
                                        if (UI_Main.instanse._grid.buildings[i] != Building.selectedInstanse)
                                        {

                                        }
                                        RushSyncRequest();
                                        break;
                                    case 2:
                                        Debug.Log("Place taken");
                                        break;
                                }
                                UI_Main.instanse._grid.buildings[i].waitingReplaceResponse = false;
                                break;
                            }
                        }
                        break;
                    case RequestsID.COLLECT:
                        long db = packet.ReadLong();
                        int collected = packet.ReadInt();
                        for (int i = 0; i < UI_Main.instanse._grid.buildings.Count; i++)
                        {
                            if (db == UI_Main.instanse._grid.buildings[i].data.databaseID)
                            {
                                UI_Main.instanse._grid.buildings[i].collecting = false;
                                switch (UI_Main.instanse._grid.buildings[i].id)
                                {
                                    case Data.BuildingID.goldmine:
                                        UI_Main.instanse._grid.buildings[i].data.goldStorage -= collected;
                                        break;
                                    case Data.BuildingID.elixirmine:
                                        UI_Main.instanse._grid.buildings[i].data.elixirStorage -= collected;
                                        break;
                                    case Data.BuildingID.darkelixirmine:
                                        UI_Main.instanse._grid.buildings[i].data.darkStorage -= collected;
                                        break;
                                }
                                UI_Main.instanse._grid.buildings[i].AdjustUI();
                                break;
                            }
                        }
                        RushSyncRequest();
                        break;
                    case RequestsID.PREUPGRADE:
                        /*
                        databaseID = packet.ReadLong();
                        string re = packet.ReadString();
                        Data.ServerBuilding sr = Data.Desrialize<Data.ServerBuilding>(re);
                        UI_BuildingUpgrade.instanse.Open(sr, databaseID);
                        */
                        break;
                    case RequestsID.UPGRADE:
                        response = packet.ReadInt();
                        switch (response)
                        {
                            case 0:
                                Debug.Log("Unknown");
                                break;
                            case 1:
                                Debug.Log("Upgrade started");
                                RushSyncRequest();
                                break;
                            case 2:
                                Debug.Log("No resources");
                                break;
                            case 3:
                                Debug.Log("Max level");
                                break;
                            case 5:
                                Debug.Log("No builder");
                                break;
                            case 6:
                                Debug.Log("Max limit reached");
                                break;
                        }
                        break;
                    case RequestsID.INSTANTBUILD:
                        response = packet.ReadInt();
                        if (response == 2)
                        {
                            Debug.Log("No gems.");
                        }
                        else if (response == 1)
                        {
                            Debug.Log("Instant built.");
                            RushSyncRequest();
                        }
                        else
                        {
                            Debug.Log("Nothing happend.");
                        }
                        break;
                    case RequestsID.TRAIN:
                        response = packet.ReadInt();
                        if (response == 4)
                        {
                            Debug.Log("Server unit not found.");
                        }
                        if (response == 3)
                        {
                            Debug.Log("No capacity.");
                        }
                        if (response == 2)
                        {
                            Debug.Log("No resources.");
                        }
                        else if (response == 1)
                        {
                            RushSyncRequest();
                        }
                        else
                        {
                            Debug.Log("Nothing happend.");
                        }
                        break;
                    case RequestsID.CANCELTRAIN:
                        response = packet.ReadInt();
                        if (response == 1)
                        {
                            RushSyncRequest();
                        }
                        break;
                    case RequestsID.BATTLEFIND:
                        long target = packet.ReadLong();
                        Data.OpponentData opponent = null;
                        if (target > 0)
                        {
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            opponent = Data.Desrialize<Data.OpponentData>(Data.Decompress(bytes));
                        }
                        UI_Search.instanse.FindResponded(target, opponent);
                        break;
                    case RequestsID.BATTLESTART:
                        bool matched = packet.ReadBool();
                        bool attack = packet.ReadBool();
                        bool confirmed = matched && attack;
                        List<Data.BattleStartBuildingData> buildings = null;
                        int wt = 0;
                        int lt = 0;
                        if (confirmed)
                        {
                            wt = packet.ReadInt();
                            lt = packet.ReadInt();
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            buildings = Data.Desrialize<List<Data.BattleStartBuildingData>>(Data.Decompress(bytes));
                        }
                        UI_Battle.instanse.StartBattleConfirm(confirmed, buildings, wt, lt);
                        break;
                    case RequestsID.BATTLEEND:
                        int stars = packet.ReadInt();
                        int unitsDeployed = packet.ReadInt();
                        int lootedGold = packet.ReadInt();
                        int lootedElixir = packet.ReadInt();
                        int lootedDark = packet.ReadInt();
                        int trophies = packet.ReadInt();
                        int frame = packet.ReadInt();
                        UI_Battle.instanse.BattleEnded(stars, unitsDeployed, lootedGold, lootedElixir, lootedDark, trophies, frame);
                        break;
                    case RequestsID.OPENCLAN:
                        bool haveClan = packet.ReadBool();
                        Data.Clan clan = null;
                        List<Data.ClanMember> warMembers = null;
                        if (haveClan)
                        {
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            clan = Data.Desrialize<Data.Clan>(Data.Decompress(bytes));
                            if (clan.war != null && clan.war.id > 0)
                            {
                                bytesLength = packet.ReadInt();
                                bytes = packet.ReadBytes(bytesLength);
                                warMembers = Data.Desrialize<List<Data.ClanMember>>(Data.Decompress(bytes));
                            }
                        }
                        UI_Clan.instanse.ClanOpen(clan, warMembers);
                        break;
                    case RequestsID.GETCLANS:
                        bytesLength = packet.ReadInt();
                        bytes = packet.ReadBytes(bytesLength);
                        Data.ClansList clans = Data.Desrialize<Data.ClansList>(Data.Decompress(bytes));
                        UI_Clan.instanse.ClansListOpen(clans);
                        break;
                    case RequestsID.CREATECLAN:
                        response = packet.ReadInt();
                        UI_Clan.instanse.CreateResponse(response);
                        break;
                    case RequestsID.JOINCLAN:
                        response = packet.ReadInt();
                        UI_Clan.instanse.JoinResponse(response);
                        break;
                    case RequestsID.LEAVECLAN:
                        response = packet.ReadInt();
                        UI_Clan.instanse.LeaveResponse(response);
                        break;
                    case RequestsID.EDITCLAN:
                        response = packet.ReadInt();
                        UI_Clan.instanse.EditResponse(response);
                        break;
                    case RequestsID.OPENWAR:
                        bytesLength = packet.ReadInt();
                        bytes = packet.ReadBytes(bytesLength);
                        Data.ClanWarData war = Data.Desrialize<Data.ClanWarData>(Data.Decompress(bytes));
                        UI_Clan.instanse.WarOpen(war);
                        break;
                    case RequestsID.STARTWAR:
                        response = packet.ReadInt();
                        UI_Clan.instanse.WarStartResponse(response);
                        break;
                    case RequestsID.CANCELWAR:
                        response = packet.ReadInt();
                        UI_Clan.instanse.WarSearchCancelResponse(response);
                        break;
                    case RequestsID.WARSTARTED:
                        databaseID = packet.ReadInt();
                        UI_Clan.instanse.WarStarted(databaseID);
                        break;
                    case RequestsID.WARATTACK:
                        databaseID = packet.ReadLong();
                        Data.OpponentData warOpponent = null;
                        if (databaseID > 0)
                        {
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            warOpponent = Data.Desrialize<Data.OpponentData>(Data.Decompress(bytes));
                        }
                        UI_Clan.instanse.AttackResponse(databaseID, warOpponent);
                        break;
                    case RequestsID.WARREPORTLIST:
                        string warReportsData = packet.ReadString();
                        List<Data.ClanWarData> warReports = Data.Desrialize<List<Data.ClanWarData>>(warReportsData);
                        UI_Clan.instanse.OpenWarHistoryList(warReports);
                        break;
                    case RequestsID.WARREPORT:
                        bool hasReport = packet.ReadBool();
                        Data.ClanWarData warReport = null;
                        if (hasReport)
                        {
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            warReport = Data.Desrialize<Data.ClanWarData>(Data.Decompress(bytes));
                        }
                        UI_Clan.instanse.WarOpen(warReport, true);
                        break;
                    case RequestsID.JOINREQUESTS:
                        bytesLength = packet.ReadInt();
                        bytes = packet.ReadBytes(bytesLength);
                        List<Data.JoinRequest> requests = Data.Desrialize<List<Data.JoinRequest>>(Data.Decompress(bytes));
                        UI_Clan.instanse.OpenRequestsList(requests);
                        break;
                    case RequestsID.JOINRESPONSE:
                        response = packet.ReadInt();
                        if (UI_ClanJoinRequest.active != null)
                        {
                            UI_ClanJoinRequest.active.Response(response);
                            UI_ClanJoinRequest.active = null;
                        }
                        break;
                    case RequestsID.SENDCHAT:
                        response = packet.ReadInt();
                        UI_Chat.instanse.ChatSendResponse(response);
                        break;
                    case RequestsID.GETCHATS:
                        bytesLength = packet.ReadInt();
                        bytes = packet.ReadBytes(bytesLength);
                        List<Data.CharMessage> messages = Data.Desrialize<List<Data.CharMessage>>(Data.Decompress(bytes));
                        int chatType = packet.ReadInt();
                        UI_Chat.instanse.ChatSynced(messages, (Data.ChatType)chatType);
                        break;
                    case RequestsID.EMAILCODE:
                        response = packet.ReadInt();
                        int expTime = packet.ReadInt();
                        UI_Settings.instanse.EmailSendResponse(response, expTime);
                        break;
                    case RequestsID.EMAILCONFIRM:
                        response = packet.ReadInt();
                        string confEmail = packet.ReadString();
                        UI_Settings.instanse.EmailConfirmResponse(response, confEmail);
                        break;
                    case RequestsID.KICKMEMBER:
                        databaseID = packet.ReadLong();
                        response = packet.ReadInt();
                        if (response == -1)
                        {
                            string kicker = packet.ReadString();
                            if (UI_Clan.instanse.isActive)
                            {
                                UI_Clan.instanse.Close();
                            }
                        }
                        else
                        {
                            UI_Clan.instanse.kickResponse(databaseID, response);
                        }
                        break;
                    case RequestsID.BREW:
                        response = packet.ReadInt();
                        if (response == 3)
                        {
                            Debug.Log("Server spell not found.");
                        }
                        else if (response == 4)
                        {
                            Debug.Log("No capacity.");
                        }
                        else if (response == 2)
                        {
                            Debug.Log("No resources.");
                        }
                        else if (response == 1)
                        {
                            Debug.Log("Train started.");
                            RushSyncRequest();
                        }
                        else
                        {
                            Debug.Log("Nothing happend.");
                        }
                        break;
                    case RequestsID.CANCELBREW:
                        response = packet.ReadInt();
                        if (response == 1)
                        {
                            RushSyncRequest();
                        }
                        break;
                    case RequestsID.RESEARCH:
                        response = packet.ReadInt();
                        Data.Research research = null;
                        if (response == 1)
                        {
                            bool found = false;

                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            research = Data.Desrialize<Data.Research>(Data.Decompress(bytes));
                            for (int i = 0; i < initializationData.research.Count; i++)
                            {
                                if (initializationData.research[i].id == research.id)
                                {
                                    initializationData.research[i] = research;
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                            {
                                initializationData.research.Add(research);
                            }
                        }
                        UI_Research.instanse.ResearchResponse(response, research);
                        break;
                    case RequestsID.PROMOTEMEMBER:
                        databaseID = packet.ReadLong();
                        response = packet.ReadInt();
                        if (response == -1)
                        {
                            string promoter = packet.ReadString();
                            MessageBox.Open(1, 0.8f, false, MessageResponded, new string[] { promoter + " promoted your clan rank." }, new string[] { "OK" });
                        }
                        else
                        {
                            UI_Clan.instanse.PromoteResponse(databaseID, response);
                        }
                        break;
                    case RequestsID.DEMOTEMEMBER:
                        databaseID = packet.ReadLong();
                        response = packet.ReadInt();
                        if (response == -1)
                        {
                            string demoter = packet.ReadString();
                            MessageBox.Open(1, 0.8f, false, MessageResponded, new string[] { demoter + " demoted your clan rank." }, new string[] { "OK" });
                        }
                        else
                        {
                            UI_Clan.instanse.DemoteResponse(databaseID, response);
                        }
                        break;
                    case RequestsID.SCOUT:
                        response = packet.ReadInt();
                        if (response == 1)
                        {
                            int scoutType = packet.ReadInt();
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            Data.Player scoutTarget = Data.Desrialize<Data.Player>(Data.Decompress(bytes));
                            UI_Scout.instanse.Open(scoutTarget, (Data.BattleType)scoutType, null);
                        }
                        break;
                    case RequestsID.BUYGEM:
                        response = packet.ReadInt();
                        if (response == 1)
                        {
                            int gemPack = packet.ReadInt();
                            RushSyncRequest();
                            UI_Store.instanse.GemPurchased();
                        }
                        break;
                    case RequestsID.BUYSHIELD:
                        response = packet.ReadInt();
                        if (response == 1)
                        {
                            int shieldPack = packet.ReadInt();
                            RushSyncRequest();
                            UI_Store.instanse.ShieldPurchased(true, shieldPack);
                        }
                        else
                        {
                            UI_Store.instanse.ShieldPurchased(false, 0);
                        }
                        break;
                    case RequestsID.REPORTCHAT:
                        response = packet.ReadInt();
                        UI_Chat.instanse.ReportResult(response);
                        break;
                    case RequestsID.PLAYERSRANK:
                        bytesLength = packet.ReadInt();
                        bytes = packet.ReadBytes(bytesLength);
                        Data.PlayersRanking players = Data.Desrialize<Data.PlayersRanking>(Data.Decompress(bytes));
                        UI_PlayersRanking.instanse.OpenResponse(players);
                        break;
                    case RequestsID.BOOST:
                        response = packet.ReadInt();
                        RushSyncRequest();
                        break;
                    case RequestsID.BUYRESOURCE:
                        response = packet.ReadInt();
                        int resPack = packet.ReadInt();
                        if (response == 1)
                        {
                            RushSyncRequest();
                            UI_Store.instanse.ResourcePurchased(true, resPack);
                        }
                        else
                        {
                            UI_Store.instanse.ResourcePurchased(false, resPack);
                        }
                        break;
                    case RequestsID.BATTLEREPORTS:
                        response = packet.ReadInt();
                        List<Data.BattleReportItem> reports = new List<Data.BattleReportItem>();
                        if (response == 1)
                        {
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            reports = Data.Desrialize<List<Data.BattleReportItem>>(Data.Decompress(bytes));
                            if (reports != null && reports.Count > 0)
                            {
                                UI_Main.instanse.ChangeUnreadBattleReports(0);
                            }
                        }
                        UI_BattleReports.instanse.OpenResponse(reports);
                        break;
                    case RequestsID.BATTLEREPORT:
                        response = packet.ReadInt();
                        if (response == 1)
                        {
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            Data.BattleReport report = Data.Desrialize<Data.BattleReport>(Data.Decompress(bytes));
                            bytesLength = packet.ReadInt();
                            bytes = packet.ReadBytes(bytesLength);
                            Data.Player reportP = Data.Desrialize<Data.Player>(Data.Decompress(bytes));
                            UI_BattleReports.instanse.PlayReply(report, reportP);
                        }
                        break;
                    case RequestsID.RENAME:
                        response = packet.ReadInt();
                        if (response == 1)
                        {
                            RushSyncRequest();
                        }
                        break;
                }
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex.Message + "\n" + ex.StackTrace);
            }
        }

        public void SendSyncRequest()
        {
            Debug.Log("Request Sent."); // For some wierd reason if I remove this debug then buildings upgrade button will delay showing for a few seconds until next update
            lastUpdateSent = DateTime.Now;
            Packet p = new Packet();
            p.Write((int)RequestsID.SYNC);
            p.Write(SystemInfo.deviceUniqueIdentifier);
            Sender.TCP_Send(p);
        }

        public void SyncData(Data.Player player)
        {
            data = player;

            _gold = 0;
            _maxGold = 0;

            _elixir = 0;
            _maxElixir = 0;

            _darkElixir = 0;
            _maxDarkElixir = 0;

            if (player.buildings != null && player.buildings.Count > 0)
            {
                for (int i = 0; i < player.buildings.Count; i++)
                {
                    switch (player.buildings[i].id)
                    {
                        case Data.BuildingID.townhall:
                            _townHallLevel = player.buildings[i].level;
                            _maxGold += player.buildings[i].goldCapacity;
                            _gold += player.buildings[i].goldStorage;
                            _maxElixir += player.buildings[i].elixirCapacity;
                            _elixir += player.buildings[i].elixirStorage;
                            _maxDarkElixir += player.buildings[i].darkCapacity;
                            _darkElixir += player.buildings[i].darkStorage;
                            break;
                        case Data.BuildingID.goldstorage:
                            _maxGold += player.buildings[i].goldCapacity;
                            _gold += player.buildings[i].goldStorage;
                            break;
                        case Data.BuildingID.elixirstorage:
                            _maxElixir += player.buildings[i].elixirCapacity;
                            _elixir += player.buildings[i].elixirStorage;
                            break;
                        case Data.BuildingID.darkelixirstorage:
                            _maxDarkElixir += player.buildings[i].darkCapacity;
                            _darkElixir += player.buildings[i].darkStorage;
                            break;
                        case Data.BuildingID.barracks:
                            _barracksLevel = player.buildings[i].level;
                            break;
                        case Data.BuildingID.darkbarracks:
                            _darkBarracksLevel = player.buildings[i].level;
                            break;
                        case Data.BuildingID.spellfactory:
                            _spellFactoryLevel = player.buildings[i].level;
                            break;
                        case Data.BuildingID.darkspellfactory:
                            _darkSpellFactoryLevel = player.buildings[i].level;
                            break;
                    }
                }
            }

            /*
            for (int i = 0; i < player.units.Count; i++)
            {

            }
            */
            _gold = Mathf.Clamp(_gold, 0, _maxGold);
            _elixir = Mathf.Clamp(_elixir, 0, _maxElixir);
            _darkElixir = Mathf.Clamp(_darkElixir, 0, _maxDarkElixir);

            UpdateResourcesUI();

            UI_Main.instanse._usernameText.text = Data.DecodeString(data.name);
            UI_Main.instanse._trophiesText.text = data.trophies.ToString();
            UI_Main.instanse._levelText.text = data.level.ToString();
            UI_Main.instanse._xpText.text = data.xp.ToString();
            
            int reqXp = Data.GetNexLevelRequiredXp(data.level);
            UI_Main.instanse._xpBar.fillAmount = (reqXp > 0 ? ((float)data.xp / (float)reqXp) : 0);

            if (UI_Main.instanse.isActive && !UI_WarLayout.instanse.isActive && !UI_Scout.instanse.isActive)
            {
                UI_Main.instanse.DataSynced();
                if (UI_Main.instanse._grid.unidentifiedBuildings != null && UI_Main.instanse._grid.unidentifiedBuildings.Count > 0)
                {
                    for (int i = 0; i < UI_Main.instanse._grid.unidentifiedBuildings.Count; i++)
                    {
                        _gold -= UI_Main.instanse._grid.unidentifiedBuildings[i].placeGoldCost;
                        _elixir -= UI_Main.instanse._grid.unidentifiedBuildings[i].placeElixirCost;
                        _darkElixir -= UI_Main.instanse._grid.unidentifiedBuildings[i].placeDarkElixirCost;
                        data.gems -= UI_Main.instanse._grid.unidentifiedBuildings[i].placeGemCost;
                    }
                    _gold = Mathf.Clamp(_gold, 0, _maxGold);
                    _elixir = Mathf.Clamp(_elixir, 0, _maxElixir);
                    _darkElixir = Mathf.Clamp(_darkElixir, 0, _maxDarkElixir);
                    UpdateResourcesUI();
                }
            }
            else if (UI_WarLayout.instanse.isActive)
            {
                UI_WarLayout.instanse.DataSynced();
            }
            else if (UI_Train.instanse.isActive)
            {
                UI_Train.instanse.Sync();
            }
            else if (UI_Spell.instanse.isOpen)
            {
                UI_Spell.instanse.Sync();
            }
            if (UI_Store.instanse.isActive)
            {
                UI_Store.instanse.Sync();
            }
            UI_Main.instanse._usernameText.ForceMeshUpdate(true);
            UI_Main.instanse._trophiesText.ForceMeshUpdate(true);
            UI_Main.instanse._levelText.ForceMeshUpdate(true);
            UI_Main.instanse._xpText.ForceMeshUpdate(true);
        }

        private void UpdateResourcesUI()
        {
            UI_Main.instanse._goldText.text = _gold.ToString();
            UI_Main.instanse._elixirText.text = _elixir.ToString();
            UI_Main.instanse._darkText.text = _darkElixir.ToString();
            UI_Main.instanse._gemsText.text = data.gems.ToString();

            UI_Main.instanse._goldBar.fillAmount = (_maxGold > 0 ? ((float)_gold / (float)_maxGold) : 0);
            UI_Main.instanse._elixirBar.fillAmount = (_maxElixir > 0 ? ((float)_elixir / (float)_maxElixir) : 0);
            UI_Main.instanse._darkBar.fillAmount = (_maxDarkElixir > 0 ? ((float)_darkElixir / (float)_maxDarkElixir) : 0);
        }

        public void RushSyncRequest()
        {
            timer = 0;
        }

        private void DisconnectedFromServer()
        {
            ThreadDispatcher.instance.Enqueue(() => Desconnected());
        }

        private void Desconnected()
        {
            connected = false;
            if (_callDisconnectError)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(0, 0.8f, false, MessageResponded, new string[] { "اتصال به سرور برقرار نشد. لطفاً اینترنت خود را چک و مجدداً تلاش کنید." }, new string[] { "تلاش مجدد" });
                        break;
                    default:
                        MessageBox.Open(0, 0.8f, false, MessageResponded, new string[] { "Failed to connect to server. Please check you internet connection and try again." }, new string[] { "Try Again" });
                        break;
                }
            }
            RealtimeNetworking.OnDisconnectedFromServer -= DisconnectedFromServer;
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 0)
            {
                RestartGame();
            }
            /*
            else if (layoutIndex == 0)
            {
                Application.Quit();
            }*/
        }

        public void AssignServerSpell(ref Data.Spell spell)
        {
            if (spell != null)
            {
                for (int i = 0; i < initializationData.serverSpells.Count; i++)
                {
                    if (initializationData.serverSpells[i].id == spell.id && initializationData.serverSpells[i].level == spell.level)
                    {
                        spell.server = initializationData.serverSpells[i];
                        break;
                    }
                }
            }
        }

        public static void RestartGame()
        {
            Time.timeScale = 1f;
            if (_instance != null)
            {
                RealtimeNetworking.OnDisconnectedFromServer -= _instance.DisconnectedFromServer;
                RealtimeNetworking.OnPacketReceived -= _instance.ReceivedPaket;
            }
            Destroy(RealtimeNetworking.instance.gameObject);
            SceneManager.LoadScene(0);
        }

    }
}
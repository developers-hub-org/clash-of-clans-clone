namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Battle : MonoBehaviour
    {

        private Battle battle = null;
        private bool isStarted = false;
        private bool readyToStart = false;
        [SerializeField] private GameObject _endPanel = null;
        [SerializeField] private GameObject _infoPanel = null;
        [SerializeField] public TextMeshProUGUI _playerNameText = null;
        [SerializeField] public TextMeshProUGUI _timerText = null;
        [SerializeField] public TextMeshProUGUI _timerDescription = null;
        [SerializeField] public TextMeshProUGUI _percentageText = null;
        [SerializeField] public TextMeshProUGUI _lootGoldText = null;
        [SerializeField] public TextMeshProUGUI _lootElixirText = null;
        [SerializeField] public TextMeshProUGUI _lootDarkText = null;
        [SerializeField] public TextMeshProUGUI _winTrophiesText = null;
        [SerializeField] public TextMeshProUGUI _looseTrophiesText = null;
        [SerializeField] public TextMeshProUGUI _endGoldText = null;
        [SerializeField] public TextMeshProUGUI _endElixirText = null;
        [SerializeField] public TextMeshProUGUI _endDarkText = null;
        [SerializeField] public TextMeshProUGUI _endTrophiesText = null;
        [SerializeField] public UI_Bar healthBarPrefab = null;
        [SerializeField] private RectTransform healthBarGrid = null;
        [SerializeField] private BattleUnit[] battleUnits = null;
        [SerializeField] private Button _findButton = null;
        [SerializeField] private GameObject _damagePanel = null;
        [SerializeField] private GameObject _star1 = null;
        [SerializeField] private GameObject _star2 = null;
        [SerializeField] private GameObject _star3 = null;
        [SerializeField] private GameObject _endStar1 = null;
        [SerializeField] private GameObject _endStar2 = null;
        [SerializeField] private GameObject _endStar3 = null;
        [SerializeField] private GameObject _endWinEffects = null;
        [SerializeField] public TextMeshProUGUI _endText = null;
        [SerializeField] public TextMeshProUGUI _findCostText = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private Button _okButton = null;
        [SerializeField] private Button _surrenderButton = null;
        [SerializeField] public UI_SpellEffect spellEffectPrefab = null;
        private List<BattleUnit> unitsOnGrid = new List<BattleUnit>();
        public List<BuildingOnGrid> buildingsOnGrid = new List<BuildingOnGrid>();
        private DateTime baseTime;
        private List<ItemToAdd> toAddUnits = new List<ItemToAdd>();
        private List<ItemToAdd> toAddSpells = new List<ItemToAdd>();
        private long target = 0;
        private bool surrender = false;
        private Data.BattleType _battleType = Data.BattleType.normal;
        private float itemHeight = 1;
        private byte[] opponentBytes = null;

        public class BuildingOnGrid
        {
            public long id = 0;
            public int index = -1;
            public Building building = null;
        }

        private class ItemToAdd
        {
            public ItemToAdd(long id, int x, int y)
            {
                this.id = id;
                this.x = x;
                this.y = y;
            }
            public long id;
            public int x;
            public int y;
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _findButton.onClick.AddListener(Find);
            _okButton.onClick.AddListener(CloseEndPanel);
            _surrenderButton.onClick.AddListener(Surrender);
            itemHeight = (battleItemsGridRoot.anchorMax.y - battleItemsGridRoot.anchorMin.y) * Screen.height;
        }

        private void CloseEndPanel()
        {
            Close();
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 1)
            {
                MessageBox.Close();
            }
        }

        private void Close()
        {
            UI_Main.instanse._grid.Clear();
            Player.instanse.SyncData(Player.instanse.data);
            isStarted = false;
            readyToStart = false;
            SetStatus(false);
            UI_Main.instanse.SetStatus(true);
        }

        public void Find()
        {
            readyToStart = false;
            _findButton.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            UI_Search.instanse.Find();
        }

        List<Data.Building> startbuildings = new List<Data.Building>();
        List<Battle.Building> battleBuildings = new List<Battle.Building>();

        public void NoTarget()
        {
            Close();
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "در حال حاضر هدفی برای حمله پیدا نشد." }, new string[] { "باشه" });
                    break;
                default:
                    MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "There is no target to attack at this moment. Please try again later." }, new string[] { "OK" });
                    break;
            }
        }

        public bool Display(Data.Player player, List<Data.Building> buildings, long defender, Data.BattleType battleType)
        {
            opponentBytes = null;
            
            ClearSpells();
            ClearUnits();
            _damagePanel.SetActive(false);
            _star1.SetActive(false);
            _star2.SetActive(false);
            _star3.SetActive(false);

            _playerNameText.text = Data.DecodeString(player.name);
            _playerNameText.ForceMeshUpdate(true);
            
            for (int i = 0; i < Player.instanse.data.units.Count; i++)
            {
                if (!Player.instanse.data.units[i].ready)
                {
                    continue;
                }
                int k = -1;
                for (int j = 0; j < units.Count; j++)
                {
                    if (units[j].id == Player.instanse.data.units[i].id)
                    {
                        k = j;
                        break;
                    }
                }
                if (k < 0)
                {
                    k = units.Count;
                    UI_BattleUnit bu = Instantiate(unitsPrefab, battleItemsGrid);
                    bu.Initialize(Player.instanse.data.units[i].id);
                    RectTransform rect = bu.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(itemHeight, itemHeight);
                    units.Add(bu);
                }
                units[k].Add(Player.instanse.data.units[i].databaseID);
            }

            for (int i = 0; i < Player.instanse.data.spells.Count; i++)
            {
                if (!Player.instanse.data.spells[i].ready)
                {
                    continue;
                }
                int k = -1;
                for (int j = 0; j < spells.Count; j++)
                {
                    if (spells[j].id == Player.instanse.data.spells[i].id)
                    {
                        k = j;
                        break;
                    }
                }
                if (k < 0)
                {
                    k = spells.Count;
                    UI_BattleSpell bs = Instantiate(spellsPrefab, battleItemsGrid);
                    bs.Initialize(Player.instanse.data.spells[i].id);
                    RectTransform rect = bs.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(itemHeight, itemHeight);
                    spells.Add(bs);
                }
                spells[k].Add(Player.instanse.data.spells[i].databaseID);
            }

            if (units.Count <= 0)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "هیچ سربازی برای حمله ندارید." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "You do not have any units for battle." }, new string[] { "OK" });
                        break;
                }
                return false;
            }

            _battleType = battleType;
            int townhallLevel = 1;
            for (int i = 0; i < buildings.Count; i++)
            {
                if (buildings[i].id == Data.BuildingID.townhall)
                {
                    townhallLevel = buildings[i].level;
                    if (_battleType != Data.BattleType.war)
                    {
                        break;
                    }
                }
                if (_battleType == Data.BattleType.war)
                {
                    buildings[i].x = buildings[i].warX;
                    buildings[i].y = buildings[i].warY;
                }
            }

            target = defender;
            startbuildings = buildings;
            SetOpData();
            battleBuildings.Clear();
            spellEffects.Clear();

            for (int i = 0; i < buildings.Count; i++)
            {

                if (buildings[i].x < 0 || buildings[i].y < 0)
                {
                    continue;
                }

                Battle.Building building = new Battle.Building();
                building.building = buildings[i];
                switch (building.building.id)
                {
                    case Data.BuildingID.townhall:
                        building.lootGoldStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.goldStorage);
                        building.lootElixirStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.elixirStorage);
                        building.lootDarkStorage = Data.GetStorageDarkElixirLoot(townhallLevel, building.building.darkStorage);
                        break;
                    case Data.BuildingID.goldmine:
                        building.lootGoldStorage = Data.GetMinesGoldAndElixirLoot(townhallLevel, building.building.goldStorage);
                        break;
                    case Data.BuildingID.goldstorage:
                        building.lootGoldStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.goldStorage);
                        break;
                    case Data.BuildingID.elixirmine:
                        building.lootElixirStorage = Data.GetMinesGoldAndElixirLoot(townhallLevel, building.building.elixirStorage);
                        break;
                    case Data.BuildingID.elixirstorage:
                        building.lootElixirStorage = Data.GetStorageGoldAndElixirLoot(townhallLevel, building.building.elixirStorage);
                        break;
                    case Data.BuildingID.darkelixirmine:
                        building.lootDarkStorage = Data.GetMinesDarkElixirLoot(townhallLevel, building.building.darkStorage);
                        break;
                    case Data.BuildingID.darkelixirstorage:
                        building.lootDarkStorage = Data.GetStorageDarkElixirLoot(townhallLevel, building.building.darkStorage);
                        break;
                }
                battleBuildings.Add(building);
            }

            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    _timerDescription.text = "زمان تا شروع حمله:";
                    break;
                default:
                    _timerDescription.text = "Battle Starts In:";
                    break;
            }
            _timerDescription.ForceMeshUpdate(true);
            _timerText.text = TimeSpan.FromSeconds(Data.battlePrepDuration).ToString(@"mm\:ss");
            _timerText.ForceMeshUpdate(true);
            
            ClearBuildingsOnGrid();
            ClearUnitsOnGrid();

            UI_Main.instanse._grid.Clear();
            for (int i = 0; i < battleBuildings.Count; i++)
            {
                var prefab = UI_Main.instanse.GetBuildingPrefab(battleBuildings[i].building.id);
                if (prefab.Item1 != null)
                {
                    BuildingOnGrid building = new BuildingOnGrid();
                    building.building = Instantiate(prefab.Item1, Vector3.zero, Quaternion.identity);
                    building.building.rows = prefab.Item2.rows;
                    building.building.columns = prefab.Item2.columns;
                    building.building.databaseID = battleBuildings[i].building.databaseID;
                    building.building.PlacedOnGrid(battleBuildings[i].building.x, battleBuildings[i].building.y, true);
                    if (building.building._baseArea)
                    {
                        building.building._baseArea.gameObject.SetActive(false);
                    }
                    building.building.healthBar = Instantiate(healthBarPrefab, healthBarGrid);
                    building.building.healthBar.bar.fillAmount = 1;
                    building.building.healthBar.gameObject.SetActive(false);

                    building.building.data = battleBuildings[i].building;
                    building.id = battleBuildings[i].building.databaseID;
                    building.index = i;
                    buildingsOnGrid.Add(building);
                    UI_Main.instanse._grid.buildings.Add(building.building);
                }

                battleBuildings[i].building.x += Data.battleGridOffset;
                battleBuildings[i].building.y += Data.battleGridOffset;
            }

            for (int i = 0; i < buildingsOnGrid.Count; i++)
            {
                buildingsOnGrid[i].building.AdjustUI(true);
            }

            _findButton.gameObject.SetActive(_battleType == Data.BattleType.normal);
            if(_battleType == Data.BattleType.normal)
            {
                int townHallLevel = 1;
                for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
                {
                    if(Player.instanse.data.buildings[i].id == Data.BuildingID.townhall)
                    {
                        townHallLevel = Player.instanse.data.buildings[i].level;
                    }
                }
                int cost = Data.GetBattleSearchCost(townHallLevel);
                _findCostText.text = cost.ToString();
                if (cost > Player.instanse.gold)
                {
                    _findButton.interactable = false;
                    _findCostText.color = Color.red;
                }
                else
                {
                    _findButton.interactable = true;
                    _findCostText.color = Color.white;
                }
                _findCostText.ForceMeshUpdate(true);
            }
            _closeButton.gameObject.SetActive(true);
            _surrenderButton.gameObject.SetActive(false);
            baseTime = DateTime.Now;
            SetStatus(true);

            toAddSpells.Clear();
            toAddUnits.Clear();
            battle = new Battle();
            battle.Initialize(battleBuildings, DateTime.Now, BuildingAttackCallBack, BuildingDestroyedCallBack, BuildingDamageCallBack, StarGained);

            _percentageText.text = Mathf.RoundToInt((float)(battle.percentage * 100f)).ToString() + "%";
            _percentageText.ForceMeshUpdate(true);
            UpdateLoots();

            var trophies = Data.GetBattleTrophies(Player.instanse.data.trophies, player.trophies);
            _winTrophiesText.text = trophies.Item1.ToString();
            _winTrophiesText.ForceMeshUpdate(true);
            _looseTrophiesText.text = "-" + trophies.Item2.ToString();
            _looseTrophiesText.ForceMeshUpdate(true);
            surrender = false;
            readyToStart = true;
            isStarted = false;

            return true;
        }

        private async void SetOpData()
        {
            Data.OpponentData opponent = new Data.OpponentData();
            opponent.id = target;
            opponent.buildings = startbuildings;
            string data = await Data.SerializeAsync<Data.OpponentData>(opponent);
            opponentBytes = await Data.CompressAsync(data);
        }

        private void UpdateLoots()
        {
            var looted = battle.GetlootedResources();
            _lootGoldText.text = (looted.Item4 - looted.Item1).ToString();
            _lootGoldText.ForceMeshUpdate(true);
            _lootElixirText.text = (looted.Item5 - looted.Item2).ToString();
            _lootElixirText.ForceMeshUpdate(true);
            _lootDarkText.text = (looted.Item6 - looted.Item3).ToString();
            _lootDarkText.ForceMeshUpdate(true);
        }

        private void StartBattle()
        {
            if (SoundManager.instanse.musicSource.clip != SoundManager.instanse.battleMusic)
            {
                SoundManager.instanse.PlayMusic(SoundManager.instanse.battleMusic);
            }
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    _timerDescription.text = "زمان تا پایان حمله:";
                    break;
                default:
                    _timerDescription.text = "Battle Ends In:";
                    break;
            }
            _timerDescription.ForceMeshUpdate(true);
            _timerText.text = TimeSpan.FromSeconds(Data.battleDuration).ToString(@"mm\:ss");
            _timerText.ForceMeshUpdate(true);
            _findButton.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            _surrenderButton.gameObject.SetActive(true);
            _damagePanel.SetActive(true);
            readyToStart = false;
            baseTime = DateTime.Now;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.BATTLESTART);
            packet.Write(opponentBytes.Length);
            packet.Write(opponentBytes);
            packet.Write((int)_battleType);
            Sender.TCP_Send(packet);
        }

        public void BattleEnded(int stars, int unitsDeployed, int lootedGold, int lootedElixir, int lootedDark, int trophies, int frame)
        {
            _findButton.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            _surrenderButton.gameObject.SetActive(false);
            var looted = battle.GetlootedResources();
            //Debug.Log("Battle Ended.");
            //Debug.Log("Frame -> Client:" + battle.frameCount + " Server:" + frame);
            //Debug.Log("Stars -> Client:" + battle.stars + " Server:" + stars);
            //Debug.Log("Units Deployed -> Client:" + battle.unitsDeployed + " Server:" + unitsDeployed);
            //Debug.Log("Looted Gold -> Client:" + looted.Item1 + " Server:" + lootedGold);
            //Debug.Log("Looted Elixir -> Client:" + looted.Item2 + " Server:" + lootedElixir);
            //Debug.Log("Looted Dark Elixir -> Client:" + looted.Item3 + " Server:" + lootedDark);
            //Debug.Log("Trophies -> Client:" + battle.GetTrophies() + " Server:" + trophies);
            _endTrophiesText.text = trophies.ToString();
            _endTrophiesText.ForceMeshUpdate(true);
            _endGoldText.text = lootedGold.ToString();
            _endGoldText.ForceMeshUpdate(true);
            _endElixirText.text = lootedElixir.ToString();
            _endElixirText.ForceMeshUpdate(true);
            _endDarkText.text = lootedDark.ToString();
            _endDarkText.ForceMeshUpdate(true);
            if(_endWinEffects != null)
            {
                _endWinEffects.SetActive(stars > 0);
            }
            _endStar1.SetActive(stars > 0);
            _endStar2.SetActive(stars > 1);
            _endStar3.SetActive(stars > 2);
            if(stars > 0)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        _endText.text = "پیروزی";
                        break;
                    default:
                        _endText.text = "Victory";
                        break;
                }
            }
            else
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        _endText.text = "شکست";
                        break;
                    default:
                        _endText.text = "Defeat";
                        break;
                }
            }
            _endText.ForceMeshUpdate(true);
            for (int i = healthBarGrid.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(healthBarGrid.transform.GetChild(i).gameObject);
            }
            _infoPanel.SetActive(false);
            _endPanel.SetActive(true);
        }

        public void StartBattleConfirm(bool confirmed, List<Data.BattleStartBuildingData> buildings, int winTrophies, int loseTrophies)
        {
            if (confirmed)
            {
                battle.winTrophies = winTrophies;
                battle.loseTrophies = loseTrophies;
                for (int i = 0; i < battle._buildings.Count; i++)
                {
                    bool resource = false;
                    switch (battle._buildings[i].building.id)
                    {
                        case Data.BuildingID.townhall:
                        case Data.BuildingID.goldmine:
                        case Data.BuildingID.goldstorage:
                        case Data.BuildingID.elixirmine:
                        case Data.BuildingID.elixirstorage:
                        case Data.BuildingID.darkelixirmine:
                        case Data.BuildingID.darkelixirstorage:
                            resource = true;
                            break;
                    }
                    if (!resource)
                    {
                        continue;
                    }
                    for (int j = 0; j < buildings.Count; j++)
                    {
                        if (battle._buildings[i].building.databaseID != buildings[j].databaseID)
                        {
                            continue;
                        }
                        battle._buildings[i].lootGoldStorage = buildings[j].lootGoldStorage;
                        battle._buildings[i].lootElixirStorage = buildings[j].lootElixirStorage;
                        battle._buildings[i].lootDarkStorage = buildings[j].lootDarkStorage;
                        break;
                    }
                }
                isStarted = true;
            }
            else
            {
                Debug.Log("Battle is not confirmed by the server.");
            }
        }

        private void Surrender()
        {
            surrender = true;
        }

        public void EndBattle(bool surrender, int surrenderFrame)
        {
            _findButton.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            _surrenderButton.gameObject.SetActive(false);
            battle.end = true;
            battle.surrender = surrender;
            isStarted = false;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.BATTLEEND);
            packet.Write(surrender);
            packet.Write(surrenderFrame);
            Sender.TCP_Send(packet);
        }

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private RectTransform battleItemsGrid = null;
        [SerializeField] private RectTransform battleItemsGridRoot = null;
        [SerializeField] public UI_BattleUnit unitsPrefab = null;
        [SerializeField] public UI_BattleSpell spellsPrefab = null;
        private static UI_Battle _instance = null; public static UI_Battle instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }

        [HideInInspector] public int selectedUnit = -1;
        [HideInInspector] public int selectedSpell = -1;

        private List<UI_BattleUnit> units = new List<UI_BattleUnit>();
        private List<UI_BattleSpell> spells = new List<UI_BattleSpell>();

        public void SpellSelected(Data.SpellID id)
        {
            if (selectedUnit >= 0)
            {
                units[selectedUnit].Deselect();
                selectedUnit = -1;
            }
            if (selectedSpell >= 0)
            {
                spells[selectedSpell].Deselect();
                selectedSpell = -1;
            }
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i].id == id)
                {
                    selectedSpell = i;
                    break;
                }
            }
            if (selectedSpell >= 0 && spells[selectedSpell].count <= 0)
            {
                spells[selectedSpell].Deselect();
                selectedSpell = -1;
            }
        }

        public void UnitSelected(Data.UnitID id)
        {
            if (selectedUnit >= 0)
            {
                units[selectedUnit].Deselect();
                selectedUnit = -1;
            }
            if (selectedSpell >= 0)
            {
                spells[selectedSpell].Deselect();
                selectedSpell = -1;
            }
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i].id == id)
                {
                    selectedUnit = i;
                    break;
                }
            }
            if (selectedUnit >= 0 && units[selectedUnit].count <= 0)
            {
                units[selectedUnit].Deselect();
                selectedUnit = -1;
            }
        }

        public void PlaceUnit(int x, int y)
        {
            if (battle != null && opponentBytes != null)
            {
                if (selectedUnit >= 0 && units[selectedUnit].count > 0 && battle.CanAddUnit(x, y))
                {
                    SoundManager.instanse.PlaySound(SoundManager.instanse.placeUnitSound);
                    if (!isStarted)
                    {
                        if (!readyToStart)
                        {
                            return;
                        }
                        StartBattle();
                    }
                    long id = units[selectedUnit].Get();
                    if (id >= 0)
                    {
                        if (units[selectedUnit].count <= 0)
                        {
                            units[selectedUnit].Deselect();
                            selectedUnit = -1;
                        }
                        toAddUnits.Add(new ItemToAdd(id, x, y));
                    }
                }
                else if (selectedSpell >= 0 && spells[selectedSpell].count > 0 && battle.CanAddSpell(x, y))
                {
                    if (!isStarted)
                    {
                        if (!readyToStart)
                        {
                            return;
                        }
                        StartBattle();
                    }
                    long id = spells[selectedSpell].Get();
                    if (id >= 0)
                    {
                        if (spells[selectedSpell].count <= 0)
                        {
                            spells[selectedSpell].Deselect();
                            selectedSpell = -1;
                        }
                        toAddSpells.Add(new ItemToAdd(id, x, y));
                    }
                }
            }
        }

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void ClearUnits()
        {
            for (int i = 0; i < units.Count; i++)
            {
                if (units[i])
                {
                    Destroy(units[i].gameObject);
                }
            }
            units.Clear();
        }

        private void ClearSpells()
        {
            for (int i = 0; i < spells.Count; i++)
            {
                if (spells[i])
                {
                    Destroy(spells[i].gameObject);
                }
            }
            spells.Clear();
        }

        public void SetStatus(bool status)
        {
            if (!status)
            {
                ClearSpells();
                ClearBuildingsOnGrid();
                ClearUnitsOnGrid();
                ClearUnits();
            }
            else
            {
                _endPanel.SetActive(false);
                _infoPanel.SetActive(true);
            }
            Player.inBattle = status;
            _active = status;
            _elements.SetActive(status);
        }

        private void Update()
        {
            if (battle != null && battle.end == false)
            {
                if (isStarted)
                {
                    TimeSpan span = DateTime.Now - baseTime;

                    if (_timerText != null)
                    {
                        _timerText.text = TimeSpan.FromSeconds(Data.battleDuration - span.TotalSeconds).ToString(@"mm\:ss");
                        _timerText.ForceMeshUpdate(true);
                    }

                    int frame = (int)Math.Floor(span.TotalSeconds / Data.battleFrameRate);
                    if (frame > battle.frameCount)
                    {
                        if (toAddUnits.Count > 0 || toAddSpells.Count > 0)
                        {
                            Data.BattleFrame battleFrame = new Data.BattleFrame();
                            battleFrame.frame = battle.frameCount + 1;

                            if (toAddUnits.Count > 0)
                            {
                                for (int i = toAddUnits.Count - 1; i >= 0; i--)
                                {
                                    for (int j = 0; j < Player.instanse.data.units.Count; j++)
                                    {
                                        if (Player.instanse.data.units[j].databaseID == toAddUnits[i].id)
                                        {
                                            battle.AddUnit(Player.instanse.data.units[j], toAddUnits[i].x, toAddUnits[i].y, UnitSpawnCallBack, UnitAttackCallBack, UnitDiedCallBack, UnitDamageCallBack, UnitHealCallBack, UnitTargetSelectedCallBack);
                                            Data.BattleFrameUnit bfu = new Data.BattleFrameUnit();
                                            bfu.id = Player.instanse.data.units[j].databaseID;
                                            bfu.x = toAddUnits[i].x;
                                            bfu.y = toAddUnits[i].y;
                                            battleFrame.units.Add(bfu);
                                            break;
                                        }
                                    }
                                    toAddUnits.RemoveAt(i);
                                }
                            }

                            if (toAddSpells.Count > 0)
                            {
                                for (int i = toAddSpells.Count - 1; i >= 0; i--)
                                {
                                    for (int j = 0; j < Player.instanse.data.spells.Count; j++)
                                    {
                                        if (Player.instanse.data.spells[j].databaseID == toAddSpells[i].id)
                                        {
                                            Data.Spell spell = Player.instanse.data.spells[j];
                                            Player.instanse.AssignServerSpell(ref spell);
                                            battle.AddSpell(spell, toAddSpells[i].x, toAddSpells[i].y, SpellSpawnCallBack, SpellPalseCallBack, SpellEndCallBack);
                                            Data.BattleFrameSpell bfs = new Data.BattleFrameSpell();
                                            bfs.id = spell.databaseID;
                                            bfs.x = toAddSpells[i].x;
                                            bfs.y = toAddSpells[i].y;
                                            battleFrame.spells.Add(bfs);
                                            break;
                                        }
                                    }
                                    toAddSpells.RemoveAt(i);
                                }
                            }

                            Packet packet = new Packet();
                            packet.Write((int)Player.RequestsID.BATTLEFRAME);
                            byte[] bytes = Data.Compress(Data.Serialize<Data.BattleFrame>(battleFrame));
                            packet.Write(bytes.Length);
                            packet.Write(bytes);
                            Sender.TCP_Send(packet);
                        }
                        battle.ExecuteFrame();
                        if ((float)battle.frameCount * Data.battleFrameRate >= battle.duration || Math.Abs(battle.percentage - 1d) <= 0.0001d)
                        {
                            EndBattle(false, battle.frameCount);
                        }
                        else if (surrender || (!battle.IsAliveUnitsOnGrid() && !HaveUnitLeftToPlace()))
                        {
                            EndBattle(true, battle.frameCount);
                        }
                    }
                }
                else if (readyToStart)
                {
                    TimeSpan span = DateTime.Now - baseTime;
                    if (span.TotalSeconds >= Data.battlePrepDuration)
                    {
                        StartBattle();
                    }
                    else
                    {
                        _timerText.text = TimeSpan.FromSeconds(Data.battlePrepDuration - span.TotalSeconds).ToString(@"mm\:ss");
                        _timerText.ForceMeshUpdate(true);
                    }
                }
                UpdateUnits();
                UpdateBuildings();
            }
        }

        private bool HaveUnitLeftToPlace()
        {
            if (units.Count > 0)
            {
                for (int i = 0; i < units.Count; i++)
                {
                    if (units[i].count > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public BattleUnit GetUnitPrefab(Data.UnitID id)
        {
            for (int i = 0; i < battleUnits.Length; i++)
            {
                if (battleUnits[i].id == id)
                {
                    return battleUnits[i];
                }
            }
            return null;
        }

        public void ClearUnitsOnGrid()
        {
            for (int i = 0; i < unitsOnGrid.Count; i++)
            {
                if (unitsOnGrid[i])
                {
                    Destroy(unitsOnGrid[i].gameObject);
                }
            }
            unitsOnGrid.Clear();
        }

        public void ClearBuildingsOnGrid()
        {
            for (int i = 0; i < buildingsOnGrid.Count; i++)
            {
                if (buildingsOnGrid[i].building != null)
                {
                    Destroy(buildingsOnGrid[i].building.gameObject);
                }
            }
            buildingsOnGrid.Clear();
        }

        public static Vector3 BattlePositionToWorldPosotion(Battle.BattleVector2 position)
        {
            Vector3 result = new Vector3(position.x * UI_Main.instanse._grid.cellSize, position.y * UI_Main.instanse._grid.cellSize, 0);
            result = UI_Main.instanse._grid.xDirection * result.x + UI_Main.instanse._grid.yDirection * result.y;
            return result;
        }

        #region Events
        private void UpdateUnits()
        {
            for (int i = 0; i < unitsOnGrid.Count; i++)
            {
                if (battle._units[unitsOnGrid[i].index].health > 0)
                {
                    unitsOnGrid[i].moving = battle._units[unitsOnGrid[i].index].moving;
                    unitsOnGrid[i].positionTarget = BattlePositionToWorldPosotion(battle._units[unitsOnGrid[i].index].positionOnGrid);
                    if (battle._units[unitsOnGrid[i].index].health < battle._units[unitsOnGrid[i].index].unit.health)
                    {
                        unitsOnGrid[i].healthBar.gameObject.SetActive(true);
                        unitsOnGrid[i].healthBar.bar.fillAmount = battle._units[unitsOnGrid[i].index].health / battle._units[unitsOnGrid[i].index].unit.health;
                        unitsOnGrid[i].healthBar.rect.anchoredPosition = GetUnitBarPosition(unitsOnGrid[i].transform.position);
                    }
                    else
                    {
                        unitsOnGrid[i].healthBar.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void UpdateBuildings()
        {
            for (int i = 0; i < buildingsOnGrid.Count; i++)
            {
                if (battle._buildings[buildingsOnGrid[i].index].health > 0)
                {
                    if (battle._buildings[buildingsOnGrid[i].index].health < battle._buildings[buildingsOnGrid[i].index].building.health)
                    {
                        buildingsOnGrid[i].building.healthBar.gameObject.SetActive(true);
                        buildingsOnGrid[i].building.healthBar.bar.fillAmount = battle._buildings[buildingsOnGrid[i].index].health / battle._buildings[buildingsOnGrid[i].index].building.health;
                        buildingsOnGrid[i].building.healthBar.rect.anchoredPosition = GetUnitBarPosition(UI_Main.instanse._grid.GetEndPosition(buildingsOnGrid[i].building));
                    }
                }
            }
        }

        private Vector2 GetUnitBarPosition(Vector3 position)
        {
            Vector3 planDownLeft = CameraController.instanse.planDownLeft;
            Vector3 planTopRight = CameraController.instanse.planTopRight;

            float w = planTopRight.x - planDownLeft.x;
            float h = planTopRight.y - planDownLeft.y;

            float endW = position.x - planDownLeft.x;
            float endH = position.y - planDownLeft.y;

            return new Vector2(endW / w * Screen.width, endH / h * Screen.height);
        }

        private List<UI_SpellEffect> spellEffects = new List<UI_SpellEffect>();

        public void SpellSpawnCallBack(long databaseID, Data.SpellID id, Battle.BattleVector2 target, float radius)
        {
            Vector3 position = BattlePositionToWorldPosotion(target);
            //Vector3 position = new Vector3(target.x, 0, target.y);
            position = UI_Main.instanse._grid.transform.TransformPoint(position);
            UI_SpellEffect effect = Instantiate(spellEffectPrefab, position, Quaternion.identity);
            effect.Initialize(id, databaseID, radius * UI_Main.instanse._grid.cellSize);
            spellEffects.Add(effect);
        }

        public void SpellPalseCallBack(long id)
        {
            for (int i = 0; i < spellEffects.Count; i++)
            {
                if (spellEffects[i].DatabaseID == battle._spells[i].spell.databaseID)
                {
                    spellEffects[i].Pulse();
                    break;
                }
            }
        }

        public void SpellEndCallBack(long id)
        {
            for (int i = 0; i < spellEffects.Count; i++)
            {
                if (spellEffects[i].DatabaseID == id)
                {
                    spellEffects[i].End();
                    spellEffects.RemoveAt(i);
                    break;
                }
            }
        }

        public void UnitSpawnCallBack(long id)
        {
            int u = -1;
            for (int i = 0; i < battle._units.Count; i++)
            {
                if (battle._units[i].unit.databaseID == id)
                {
                    u = i;
                    break;
                }
            }
            if (u >= 0)
            {
                BattleUnit prefab = GetUnitPrefab(battle._units[u].unit.id);
                if (prefab)
                {
                    BattleUnit unit = Instantiate(prefab, UI_Main.instanse._grid.transform);
                    unit.transform.localPosition = BattlePositionToWorldPosotion(battle._units[u].positionOnGrid);
                    //unit.transform.rotation = Quaternion.LookRotation(new Vector3(0, unit.transform.position.y, 0) - unit.transform.position);
                    unit.transform.localEulerAngles = Vector3.zero;
                    unit.positionTarget = unit.transform.localPosition;
                    unit.Initialize(u, battle._units[u].unit.databaseID, battle._units[u].unit);
                    unit.healthBar = Instantiate(healthBarPrefab, healthBarGrid);
                    unit.healthBar.bar.fillAmount = 1;
                    unit.healthBar.gameObject.SetActive(false);
                    unitsOnGrid.Add(unit);
                }
            }
        }

        public void StarGained()
        {
            if(battle.stars == 1)
            {
                _star1.SetActive(true);
            }
            else if (battle.stars == 2)
            {
                _star2.SetActive(true);
            }
            else if (battle.stars == 3)
            {
                _star3.SetActive(true);
            }
        }

        public void UnitAttackCallBack(long id, long target)
        {
            int u = -1;
            int b = -1;
            for (int i = 0; i < unitsOnGrid.Count; i++)
            {
                if (unitsOnGrid[i].databaseID == id)
                {
                    u = i;
                    break;
                }
            }
            if (u >= 0)
            {
                for (int i = 0; i < buildingsOnGrid.Count; i++)
                {
                    if (buildingsOnGrid[i].building.databaseID == target)
                    {
                        b = i;
                        break;
                    }
                }
                if (b >= 0)
                {
                    if (unitsOnGrid[u].projectilePrefab && unitsOnGrid[u].shootPoint && unitsOnGrid[u].data.attackRange > 0f && unitsOnGrid[u].data.rangedSpeed > 0f)
                    {
                        UI_Projectile projectile = Instantiate(unitsOnGrid[u].projectilePrefab);
                        projectile.Initialize(unitsOnGrid[u].shootPoint.position, buildingsOnGrid[b].building.shootTarget, unitsOnGrid[u].data.rangedSpeed * UI_Main.instanse._grid.cellSize);
                    }
                    unitsOnGrid[u].Attack(buildingsOnGrid[b].building.transform.position);
                }
                else
                {
                    unitsOnGrid[u].Attack();
                }
            }
        }

        public void UnitDiedCallBack(long id)
        {
            for (int i = 0; i < unitsOnGrid.Count; i++)
            {
                if (unitsOnGrid[i].databaseID == id)
                {
                    Destroy(unitsOnGrid[i].healthBar.gameObject);
                    Destroy(unitsOnGrid[i].gameObject);
                    unitsOnGrid.RemoveAt(i);
                    break;
                }
            }
        }

        public void UnitTargetSelectedCallBack(long id)
        {
            int targetIndex = -1;
            for (int i = 0; i < battle._units.Count; i++)
            {
                if (battle._units[i].unit.databaseID == id)
                {
                    targetIndex = battle._units[i].target;
                    break;
                }
            }

            if(targetIndex >= 0 && battle._buildings.Count > targetIndex)
            {
                long buildingID = battle._buildings[targetIndex].building.databaseID;
                for (int i = 0; i < buildingsOnGrid.Count; i++)
                {
                    if (buildingsOnGrid[i].building.databaseID == buildingID)
                    {
                        //Vector3 pos = buildingsOnGrid[i].transform.position; // This is the target
                        // You can instantiate target point here and delete it after a few seconds for example:
                        // Transform tp = Instantiate(prefab, ...)
                        // Destroy(tp.gameObject, 2f);
                        break;
                    }
                }
            }
        }

        public void UnitDamageCallBack(long id, float damage)
        {

        }

        public void UnitHealCallBack(long id, float health)
        {

        }

        public void BuildingAttackCallBack(long id, long target)
        {
            int u = -1;
            int b = -1;
            for (int i = 0; i < buildingsOnGrid.Count; i++)
            {
                if (buildingsOnGrid[i].id == id)
                {
                    if (buildingsOnGrid[i].building.data.radius > 0 && buildingsOnGrid[i].building.data.rangedSpeed > 0)
                    {
                        b = i;
                    }
                    break;
                }
            }
            if (b >= 0)
            {
                for (int i = 0; i < unitsOnGrid.Count; i++)
                {
                    if (unitsOnGrid[i].databaseID == target)
                    {
                        u = i;
                        break;
                    }
                }
                if (u >= 0)
                {
                    buildingsOnGrid[b].building.LookAt(unitsOnGrid[u].transform.position);
                    UI_Projectile projectile = buildingsOnGrid[b].building.GetProjectile();
                    Transform muzzle = buildingsOnGrid[b].building.GetMuzzle();
                    if (projectile != null && muzzle != null)
                    {
                        projectile = Instantiate(projectile, muzzle.position, Quaternion.LookRotation(unitsOnGrid[u].transform.position - muzzle.position, Vector3.up));
                        projectile.Initialize(muzzle.position, unitsOnGrid[u].targetPoint != null ? unitsOnGrid[u].targetPoint : unitsOnGrid[u].transform, buildingsOnGrid[b].building.data.rangedSpeed * UI_Main.instanse._grid.cellSize, UI_Projectile.GetCutveHeight(buildingsOnGrid[b].building.id));
                    }
                }
            }
        }

        public void BuildingDamageCallBack(long id, float damage)
        {
            UpdateLoots();
        }

        public void BuildingDestroyedCallBack(long id, double percentage)
        {
            if (percentage > 0)
            {
                _percentageText.text = Mathf.RoundToInt((float)(battle.percentage * 100f)).ToString() + "%";
                _percentageText.ForceMeshUpdate(true);
            }
            for (int i = 0; i < buildingsOnGrid.Count; i++)
            {
                if (buildingsOnGrid[i].id == id)
                {
                    Destroy(buildingsOnGrid[i].building.gameObject);
                    buildingsOnGrid.RemoveAt(i);
                    break;
                }
            }
        }

        #endregion

    }
}
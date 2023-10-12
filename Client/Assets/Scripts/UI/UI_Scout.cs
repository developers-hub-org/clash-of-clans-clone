namespace DevelopersHub.ClashOfWhatecer
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Scout : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _backButton = null;
        [SerializeField] public GameObject _panelReport = null;
        [SerializeField] public GameObject _panelScout = null;
        [SerializeField] public TextMeshProUGUI _playerNameText = null;
        [SerializeField] public TextMeshProUGUI _timerText = null;
        [SerializeField] public TextMeshProUGUI _percentageText = null;
        [SerializeField] private GameObject _damagePanel = null;
        [SerializeField] private GameObject _star1 = null;
        [SerializeField] private GameObject _star2 = null;
        [SerializeField] private GameObject _star3 = null;
        [SerializeField] private RectTransform healthBarGrid = null;
        [SerializeField] private Button _playButton = null;
        [SerializeField] private Button _pauseButton = null;
        [SerializeField] private Button _replayButton = null;

        private Player.Panel _backPanel = Player.Panel.main;
        private static UI_Scout _instance = null; public static UI_Scout instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }
        private Data.Player _player = null;
        private Data.BattleType _type = Data.BattleType.normal;

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _backButton.onClick.AddListener(Back);
            _playButton.onClick.AddListener(PlayReplyFirstTime);
            _pauseButton.onClick.AddListener(PauseReply);
            _replayButton.onClick.AddListener(ReplayReort);
        }

        public void Open(Data.Player player, Data.BattleType type, Data.BattleReport report)
        {
            _type = type;
            if (report != null)
            {
                _player = player;
                _report = report;
                _panelScout.SetActive(false);
                _panelReport.SetActive(true);
                Display();
            }
            else if (player != null)
            {
                _player = player;
                _panelScout.SetActive(true);
                _panelReport.SetActive(false);
                PlaceBuildings();
            }
            if (UI_Clan.instanse.isActive)
            {
                _backPanel = Player.Panel.clan;
                UI_Clan.instanse.Close();
            }
            else
            {
                _backPanel = Player.Panel.main;
            }
            if (UI_Main.instanse.isActive)
            {
                UI_Main.instanse.SetStatus(false);
            }
            _active = true;
            _elements.SetActive(true);
        }

        private void PlaceBuildings()
        {
            UI_Main.instanse._grid.Clear();
            for (int i = 0; i < _player.buildings.Count; i++)
            {
                if(_type == Data.BattleType.war && (_player.buildings[i].warX < 0 || _player.buildings[i].warY < 0))
                {
                    continue;
                }
                var prefab = UI_Main.instanse.GetBuildingPrefab(_player.buildings[i].id);
                if (prefab.Item1 != null)
                {
                    Building building = Instantiate(prefab.Item1, Vector3.zero, Quaternion.identity);
                    building.scout = true;
                    building.data = _player.buildings[i];
                    building.rows = prefab.Item2.rows;
                    building.columns = prefab.Item2.columns;
                    building.databaseID = _player.buildings[i].databaseID;
                    if (_type == Data.BattleType.war)
                    {
                        building.PlacedOnGrid(_player.buildings[i].warX, _player.buildings[i].warY, true, true);
                    }
                    else
                    {
                        building.PlacedOnGrid(_player.buildings[i].x, _player.buildings[i].y, true, true);
                    }
                    if (building._baseArea)
                    {
                        building._baseArea.gameObject.SetActive(false);
                    }
                    UI_Main.instanse._grid.buildings.Add(building);
                }
            }
            UI_Main.instanse._grid.RefreshBuildings();
        }

        private void PlayReply()
        {
            isStarted = true;
            baseTime.AddMilliseconds((DateTime.Now - pauseTime).TotalMilliseconds);
            Time.timeScale = 1f;
            _playButton.gameObject.SetActive(false);
            _pauseButton.gameObject.SetActive(true);
            _replayButton.gameObject.SetActive(false);
        }

        private void PlayReplyFirstTime()
        {
            baseTime = DateTime.Now;
            PlayReply();
        }

        private void ReplayReort()
        {
            Display();
            PlayReply();
        }

        private void PauseReply()
        {
            isStarted = false;
            pauseTime = DateTime.Now;
            Time.timeScale = 0f;
            _playButton.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
            _replayButton.gameObject.SetActive(false);
        }

        private void Back()
        {
            Close();
        }

        private void Close()
        {
            _active = false;
            isStarted = false;
            Time.timeScale = 1f;
            ClearSpells();
            ClearBuildingsOnGrid();
            ClearUnitsOnGrid();
            ClearUnits();
            UI_Main.instanse._grid.Clear();
            Player.instanse.SyncData(Player.instanse.data);
            if (_backPanel == Player.Panel.clan)
            {
                UI_Main.instanse.SetStatus(true);
                UI_Clan.instanse.Open();
            }
            else
            {
                UI_Main.instanse.SetStatus(true);
            }
            _elements.SetActive(false);
        }

        private Data.BattleReport _report = null;
        private Battle battle = null;
        private bool isStarted = false;
        private DateTime baseTime;
        private DateTime pauseTime;
        private List<UI_BattleUnit> units = new List<UI_BattleUnit>();
        private List<UI_BattleSpell> spells = new List<UI_BattleSpell>();
        private List<BattleUnit> unitsOnGrid = new List<BattleUnit>();
        private List<UI_Battle.BuildingOnGrid> buildingsOnGrid = new List<UI_Battle.BuildingOnGrid>();
        private List<Battle.Building> battleBuildings = new List<Battle.Building>();

        public bool Display()
        {
            _playerNameText.text = Data.DecodeString(_player.name);
            ClearSpells();
            ClearUnits();
            _damagePanel.SetActive(false);
            _star1.SetActive(false);
            _star2.SetActive(false);
            _star3.SetActive(false);
            _playButton.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(false);
            _replayButton.gameObject.SetActive(false);

            int townhallLevel = 1;
            for (int i = 0; i < _report.buildings.Count; i++)
            {
                if (_report.buildings[i].id == Data.BuildingID.townhall)
                {
                    townhallLevel = _report.buildings[i].level;
                    //break;
                }
                _report.buildings[i].x -= Data.battleGridOffset;
                _report.buildings[i].y -= Data.battleGridOffset;
            }
            /*
            for (int i = 0; i < _report.frames.Count; i++)
            {
                for (int j = 0; j < _report.frames[i].units.Count; j++)
                {
                    _report.frames[i].units[j].x -= Data.battleGridOffset;
                    _report.frames[i].units[j].y -= Data.battleGridOffset;
                }
                for (int j = 0; j < _report.frames[i].spells.Count; j++)
                {
                    _report.frames[i].spells[j].x -= Data.battleGridOffset;
                    _report.frames[i].spells[j].y -= Data.battleGridOffset;
                }
            }
            */
            battleBuildings.Clear();
            spellEffects.Clear();

            for (int i = 0; i < _report.buildings.Count; i++)
            {

                if (_report.buildings[i].x < 0 || _report.buildings[i].y < 0)
                {
                    continue;
                }

                Battle.Building building = new Battle.Building();
                building.building = _report.buildings[i];
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

            _timerText.text = TimeSpan.FromSeconds(Data.battleDuration).ToString(@"mm\:ss");

            ClearBuildingsOnGrid();
            ClearUnitsOnGrid();

            UI_Main.instanse._grid.Clear();
            for (int i = 0; i < battleBuildings.Count; i++)
            {
                var prefab = UI_Main.instanse.GetBuildingPrefab(battleBuildings[i].building.id);
                if (prefab.Item1 != null)
                {
                    UI_Battle.BuildingOnGrid building = new UI_Battle.BuildingOnGrid();
                    building.building = Instantiate(prefab.Item1, Vector3.zero, Quaternion.identity);
                    building.building.scout = true;
                    building.building.rows = prefab.Item2.rows;
                    building.building.columns = prefab.Item2.columns;
                    building.building.databaseID = battleBuildings[i].building.databaseID;
                    building.building.PlacedOnGrid(battleBuildings[i].building.x, battleBuildings[i].building.y, true);
                    if (building.building._baseArea)
                    {
                        building.building._baseArea.gameObject.SetActive(false);
                    }
                    building.building.healthBar = Instantiate(UI_Battle.instanse.healthBarPrefab, healthBarGrid);
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

            if (_type == Data.BattleType.normal)
            {
                int townHallLevel = 1;
                for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
                {
                    if (Player.instanse.data.buildings[i].id == Data.BuildingID.townhall)
                    {
                        townHallLevel = Player.instanse.data.buildings[i].level;
                    }
                }
            }

            baseTime = DateTime.Now;
            battle = new Battle();
            battle.Initialize(battleBuildings, DateTime.Now, BuildingAttackCallBack, BuildingDestroyedCallBack, BuildingDamageCallBack, StarGained);

            _percentageText.text = Mathf.RoundToInt((float)(battle.percentage * 100f)).ToString() + "%";
            //UpdateLoots();

            //var trophies = Data.GetBattleTrophies(Player.instanse.data.trophies, player.trophies);
            //_winTrophiesText.text = trophies.Item1.ToString();
            //_looseTrophiesText.text = "-" + trophies.Item2.ToString();


            isStarted = false;

            return true;
        }

        private void Update()
        {
            if (battle != null && battle.end == false && isStarted)
            {
                if (battle.frameCount < _report.totalFrames)
                {
                    TimeSpan span = DateTime.Now - baseTime;
                    if (_timerText != null)
                    {
                        _timerText.text = TimeSpan.FromSeconds(Data.battleDuration - span.TotalSeconds).ToString(@"mm\:ss");
                    }
                    int frame = (int)Math.Floor(span.TotalSeconds / Data.battleFrameRate);
                    if (frame > battle.frameCount)
                    {
                        for (int i = 0; i < _report.frames.Count; i++)
                        {
                            if(_report.frames[i].frame == battle.frameCount + 1)
                            {
                                for (int u = 0; u < _report.frames[i].units.Count; u++)
                                {
                                    battle.AddUnit(_report.frames[i].units[u].unit, _report.frames[i].units[u].x, _report.frames[i].units[u].y, UnitSpawnCallBack, UnitAttackCallBack, UnitDiedCallBack, UnitDamageCallBack, UnitHealCallBack);
                                }
                                for (int s = 0; s < _report.frames[i].spells.Count; s++)
                                {
                                    battle.AddSpell(_report.frames[i].spells[s].spell, _report.frames[i].spells[s].x, _report.frames[i].spells[s].y, SpellSpawnCallBack, SpellPalseCallBack, SpellEndCallBack);
                                }
                                break;
                            }
                        }
                        battle.ExecuteFrame();
                    }
                }
                else
                {
                    _playButton.gameObject.SetActive(false);
                    _pauseButton.gameObject.SetActive(false);
                    _replayButton.gameObject.SetActive(true);
                    isStarted = false;
                    Transform[] bars = healthBarGrid.GetComponentsInChildren<Transform>();
                    if(bars != null)
                    {
                        for (int i = 0; i < bars.Length; i++)
                        {
                            if (bars[i] != healthBarGrid.transform)
                            {
                                Destroy(bars[i].gameObject);
                            }
                        }
                    }
                }
                UpdateUnits();
                UpdateBuildings();
            }
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

        #region Events
        private void UpdateUnits()
        {
            for (int i = 0; i < unitsOnGrid.Count; i++)
            {
                if (battle._units[unitsOnGrid[i].index].health > 0)
                {
                    unitsOnGrid[i].moving = battle._units[unitsOnGrid[i].index].moving;
                    unitsOnGrid[i].positionTarget = UI_Battle.BattlePositionToWorldPosotion(battle._units[unitsOnGrid[i].index].positionOnGrid);
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
            Vector3 position = new Vector3(target.x, 0, target.y);
            position = UI_Main.instanse._grid.transform.TransformPoint(position);
            UI_SpellEffect effect = Instantiate(UI_Battle.instanse.spellEffectPrefab, position, Quaternion.identity);
            effect.Initialize(id, databaseID, radius);
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
                BattleUnit prefab = UI_Battle.instanse.GetUnitPrefab(battle._units[u].unit.id);
                if (prefab)
                {
                    BattleUnit unit = Instantiate(prefab, UI_Main.instanse._grid.transform);
                    unit.transform.localPosition = UI_Battle.BattlePositionToWorldPosotion(battle._units[u].positionOnGrid);
                    //unit.transform.rotation = Quaternion.LookRotation(new Vector3(0, unit.transform.position.y, 0) - unit.transform.position);
                    unit.transform.localEulerAngles = Vector3.zero;
                    unit.positionTarget = unit.transform.localPosition;
                    unit.Initialize(u, battle._units[u].unit.databaseID, battle._units[u].unit);
                    unit.healthBar = Instantiate(UI_Battle.instanse.healthBarPrefab, healthBarGrid);
                    unit.healthBar.bar.fillAmount = 1;
                    unit.healthBar.gameObject.SetActive(false);
                    unitsOnGrid.Add(unit);
                }
            }
        }

        public void StarGained()
        {
            if (battle.stars == 1)
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
                        projectile.Initialize(unitsOnGrid[u].shootPoint.position, buildingsOnGrid[b].building.shootTarget, unitsOnGrid[u].data.rangedSpeed * Data.gridCellSize);
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
                        projectile.Initialize(muzzle.position, unitsOnGrid[u].targetPoint != null ? unitsOnGrid[u].targetPoint : unitsOnGrid[u].transform, buildingsOnGrid[b].building.data.rangedSpeed * Data.gridCellSize, UI_Projectile.GetCutveHeight(buildingsOnGrid[b].building.id));
                    }
                }
            }
        }

        public void BuildingDamageCallBack(long id, float damage)
        {
            //UpdateLoots();
        }

        public void BuildingDestroyedCallBack(long id, double percentage)
        {
            if (percentage > 0)
            {
                _percentageText.text = Mathf.RoundToInt((float)(battle.percentage * 100f)).ToString() + "%";
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
namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using DevelopersHub.RealtimeNetworking.Client;
    using System;

    public class Building : MonoBehaviour
    {

        public Data.BuildingID id = Data.BuildingID.townhall;
        private static Building _buildInstance = null; public static Building buildInstanse { get { return _buildInstance; } set { _buildInstance = value; } }
        private static Building _selectedInstance = null; public static Building selectedInstanse { get { return _selectedInstance; } set { _selectedInstance = value; } }

        [HideInInspector] private Data.Building _data = new Data.Building(); public Data.Building data { get { return _data; }
            set
            {
                if (!(lastChange >= Player.instanse.lastUpdateSent && UI_Main.instanse.isActive))
                {
                    if(_data.level <= value.level) // TODO -> This will be an issue if you implement building downgrade in the game
                    {
                        _data = value; DataSet();
                    }
                }
            } 
        }
        public int level { get { return _data.level; } set { _data.level = value; } }
        public bool isCons { get { return _data.isConstructing; } set { _data.isConstructing = value; } }
        [HideInInspector] public UI_Button collectButton = null;
        [HideInInspector] public bool collecting = false;
        [HideInInspector] public UI_Bar buildBar = null;
        [HideInInspector] public UI_Bar healthBar = null;
        private float collectTimer = 0;
        private bool collectTimeout = false;
        public Transform _center = null;
        public int debugLevel = 1;
        private int _serverIndex = 1; public int serverIndex { get { return _serverIndex; } set { _serverIndex = value; } }

        [HideInInspector] public int placeGemCost = 0;
        [HideInInspector] public int placeElixirCost = 0;
        [HideInInspector] public int placeDarkElixirCost = 0;
        [HideInInspector] public int placeGoldCost = 0;

        [HideInInspector] public DateTime lastChange = DateTime.Now;

        [System.Serializable] public class Level
        {
            public int level = 1;
            public GameObject elements = null;
            public UI_Projectile projectile = null;
            public Wall wall = null;
            public Transform[] _angles = null;
            [HideInInspector] public Angles[] angles = null;
        }

        [System.Serializable] public class Angles
        {            
            public float angle = 0;
            public Transform renderer = null;
            public Transform muzzle = null;
            public Transform direction = null;
        }

        [System.Serializable] public class Wall
        {            
            public GameObject center = null;
            public GameObject back = null;
            public GameObject left = null;
        }

        private long _databaseID = 0; public long databaseID { get { return _databaseID; } set { _databaseID = value; } }
        private int _rows = 1; public int rows { get { return _rows; } set { _rows = value; } }
        private int _columns = 1; public int columns { get { return _columns; } set { _columns = value; } }

        [SerializeField] private GameObject _selectEffects = null;
        [SerializeField] private GameObject _rangeEffects = null;
        [SerializeField] private GameObject _blindRangeEffects = null;
        [SerializeField] private GameObject _boostEffects = null;
        [SerializeField] private GameObject _constructionEffects = null;
        [SerializeField] public SpriteRenderer _baseArea = null;
        [SerializeField] private Level[] _levels = null;

        public Transform shootTarget
        {
            get
            {
                if(_center == null)
                {
                    return transform;
                }
                else
                {
                    return _center;
                }
            }
        }

        private int _currentX = 0; public int currentX { get { return _currentX; } }
        private int _currentY = 0; public int currentY { get { return _currentY; } }
        private int _X = 0;
        private int _Y = 0;
        private int _originalX = 0; public int originalX { get { return _originalX; } }
        private int _originalY = 0; public int originalY { get { return _originalY; } }
        private int levelIndex = -1;
        private bool _scout = false; public bool scout { get { return _scout; } set { _scout = value; } }
        private float _angle = 0;
        private int _angleIndex = -1;

        public static Vector2Int GetBuildingPosition(Data.Building building, int layout = 0)
        {
            if (layout == 0) { layout = UI_WarLayout.instanse.isActive ? 2 : 1; }
            if (layout == 2)
            {
                return new Vector2Int(building.warX, building.warY);
            }
            else
            {
                return new Vector2Int(building.x, building.y);
            }
        }

        private void Awake()
        {
            if (_selectEffects)
            {
                _selectEffects.SetActive(false);
            }
            if (_boostEffects)
            {
                _boostEffects.SetActive(false);
            }
            if (_rangeEffects)
            {
                _rangeEffects.SetActive(false);
            }
            if (_blindRangeEffects)
            {
                _blindRangeEffects.SetActive(false);
            }
            if (_constructionEffects)
            {
                _constructionEffects.SetActive(false);
            }
            if(_levels != null && _levels.Length > 0)
            {
                for (int i = 0; i < _levels.Length; i++)
                {
                    if(_levels[i] != null && _levels[i]._angles != null && _levels[i]._angles.Length > 0)
                    {
                        _levels[i].angles = new Angles[_levels[i]._angles.Length];
                        for (int j = 0; j < _levels[i]._angles.Length; j++)
                        {
                            _levels[i].angles[j] = new Angles();
                            _levels[i].angles[j].renderer = _levels[i]._angles[j];
                        }
                        int closest = -1;
                        float cla = Mathf.Infinity;
                        for (int j = 0; j < _levels[i].angles.Length; j++)
                        {
                            if(_levels[i].angles[j] != null && _levels[i].angles[j].renderer != null)
                            {
                                _levels[i].angles[j].renderer.gameObject.SetActive(false);
                                if (_levels[i].angles[j].renderer.childCount > 0)
                                {
                                    _levels[i].angles[j].muzzle = _levels[i].angles[j].renderer.GetChild(0);
                                    if (_levels[i].angles[j].renderer.childCount > 1)
                                    {
                                        _levels[i].angles[j].direction = _levels[i].angles[j].renderer.GetChild(1);
                                    }
                                    else
                                    {
                                        _levels[i].angles[j].direction = _levels[i].angles[j].muzzle;
                                    }
                                }
                                if (_levels[i].angles[j].direction != null)
                                {
                                    Vector3 cntr = transform.position;
                                    if(_center != null)
                                    {
                                        cntr = _center.position;
                                    }
                                    Vector2 direction = _levels[i].angles[j].direction.position - cntr;
                                    _levels[i].angles[j].angle = Vector2.SignedAngle(Vector2.down, direction.normalized);
                                    if (_levels[i].angles[j].angle < 0)
                                    {
                                        _levels[i].angles[j].angle += 360;
                                    }
                                    _levels[i].angles[j].angle -= 45;
                                    if (_levels[i].angles[j].angle < 0)
                                    {
                                        _levels[i].angles[j].angle += 360;
                                    }
                                    if (_levels[i].angles[j].angle < cla)
                                    {
                                        cla = _levels[i].angles[j].angle;
                                        closest = j;
                                    }
                                }
                            }
                        }
                        if(closest >= 0)
                        {
                            _levels[i].angles[closest].renderer.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (buildBar && buildBar != null)
            {
                Destroy(buildBar.gameObject);
            }
            if (collectButton && collectButton != null)
            {
                Destroy(collectButton.gameObject);
            }
            if (healthBar && healthBar != null)
            {
                Destroy(healthBar.gameObject);
            }
        }

        private void Update()
        {
            AdjustUI();
        }

        private void DataSet()
        {
            lastChange = Player.instanse.lastUpdate;
            if (_rangeEffects && _data.radius > 0)
            {
                Vector3 scale = _rangeEffects.transform.localScale;
                scale.x = _data.radius;
                scale.y = _data.radius;
                _rangeEffects.transform.localScale = scale;
                if(_data.blindRange > 0 && _blindRangeEffects)
                {
                    scale = _blindRangeEffects.transform.localScale;
                    scale.x = _data.blindRange;
                    scale.y = _data.blindRange;
                    _blindRangeEffects.transform.localScale = scale;
                }
            }
        }

        public Transform GetMuzzle()
        {
            if (levelIndex >= 0 && _levels[levelIndex].angles != null)
            {
                if (_angleIndex >= 0 && _levels[levelIndex].angles[_angleIndex] != null && _levels[levelIndex].angles[_angleIndex].muzzle != null) { return _levels[levelIndex].angles[_angleIndex].muzzle; }
                
                /*
                int closest = -1;
                float distance = Mathf.infinity;
                for(int i = 0; i < _levels[levelIndex].angles.Length; i++)
                {
                    if(_levels[levelIndex].angles[i] != null)
                    {
                        float d = Mathf.Abs(_levels[levelIndex].angles[i].angle - );
                    }
                }
                if(closest >= 0)
                {
                    
                }
                if (_angle >= 337.5f || _angle <= 22.5f)
                {
                    // 0
                    if (_levels[levelIndex].rotations.r000Muzzle != null) { return _levels[levelIndex].rotations.r000Muzzle; }
                }
                else if (_angle <= 67.5f)
                {
                    // 45
                    if (_levels[levelIndex].rotations.r045Muzzle != null) { return _levels[levelIndex].rotations.r045Muzzle; }
                }
                else if (_angle <= 112.5f)
                {
                    // 90
                    if (_levels[levelIndex].rotations.r090Muzzle != null) { return _levels[levelIndex].rotations.r090Muzzle; }
                }
                else if (_angle <= 157.5f)
                {
                    // 135
                    if (_levels[levelIndex].rotations.r135Muzzle != null) { return _levels[levelIndex].rotations.r135Muzzle; }
                }
                else if (_angle <= 202.5f)
                {
                    // 180
                    if (_levels[levelIndex].rotations.r180Muzzle != null) { return _levels[levelIndex].rotations.r180Muzzle; }
                }
                else if (_angle <= 247.5f)
                {
                    // 225
                    if (_levels[levelIndex].rotations.r225Muzzle != null) { return _levels[levelIndex].rotations.r225Muzzle; }
                }
                else if (_angle <= 292.5f)
                {
                    // 270
                    if (_levels[levelIndex].rotations.r270Muzzle != null) { return _levels[levelIndex].rotations.r270Muzzle; }
                }
                else
                {
                    // 315
                    if (_levels[levelIndex].rotations.r315Muzzle != null) { return _levels[levelIndex].rotations.r315Muzzle; }
                }
            }
            if (_levels[levelIndex].rotations.r000Muzzle != null) { return _levels[levelIndex].rotations.r000Muzzle; }
            */
            }
            return null;
        }

        public UI_Projectile GetProjectile()
        {
            if (levelIndex >= 0)
            {
                return _levels[levelIndex].projectile;
            }
            return null;
        }

        public void LookAt(Vector3 target)
        {
            if (levelIndex >= 0 && _levels[levelIndex].angles != null && target != null)
            {
                Vector3 cntr = transform.position;
                if (_center != null)
                {
                    cntr = _center.position;
                }
                _angle = Vector2.SignedAngle(Vector2.down, (target - cntr).normalized);
                if(_angle < 0)
                {
                    _angle += 360;
                }
                _angle -= 45;
                if(_angle < 0) 
                {
                    _angle += 360;
                }
                int closest = -1;
                float distance = Mathf.Infinity;
                for(int i = 0; i < _levels[levelIndex].angles.Length; i++)
                {
                    if (_levels[levelIndex].angles[i] != null)
                    {
                        if (_levels[levelIndex].angles[i].renderer != null) { _levels[levelIndex].angles[i].renderer.gameObject.SetActive(false); }
                        float d = Mathf.Abs(_levels[levelIndex].angles[i].angle - _angle);
                        if(d < distance)
                        {
                            distance = d;
                            closest = i;
                        }
                    }
                }
                if (closest < 0)
                {
                    return;
                }
                _angleIndex = closest;
                if (_levels[levelIndex].angles[_angleIndex] != null && _levels[levelIndex].angles[_angleIndex].renderer != null) { _levels[levelIndex].angles[_angleIndex].renderer.gameObject.SetActive(true); }



/*
                if (_levels[levelIndex].rotations.r000 != null) { _levels[levelIndex].rotations.r000.gameObject.SetActive(false); }
                if (_levels[levelIndex].rotations.r045 != null) { _levels[levelIndex].rotations.r045.gameObject.SetActive(false); }
                if (_levels[levelIndex].rotations.r090 != null) { _levels[levelIndex].rotations.r090.gameObject.SetActive(false); }
                if (_levels[levelIndex].rotations.r135 != null) { _levels[levelIndex].rotations.r135.gameObject.SetActive(false); }
                if (_levels[levelIndex].rotations.r180 != null) { _levels[levelIndex].rotations.r180.gameObject.SetActive(false); }
                if (_levels[levelIndex].rotations.r225 != null) { _levels[levelIndex].rotations.r225.gameObject.SetActive(false); }
                if (_levels[levelIndex].rotations.r270 != null) { _levels[levelIndex].rotations.r270.gameObject.SetActive(false); }
                if (_levels[levelIndex].rotations.r315 != null) { _levels[levelIndex].rotations.r315.gameObject.SetActive(false); }

                if (_angle >= 337.5f || _angle <= 22.5f)
                {
                    // 0
                    if (_levels[levelIndex].rotations.r000 != null) { _levels[levelIndex].rotations.r000.gameObject.SetActive(true); }
                }
                else if (_angle <= 67.5f)
                {
                    // 45
                    if (_levels[levelIndex].rotations.r045 != null) { _levels[levelIndex].rotations.r045.gameObject.SetActive(true); }
                }
                else if (_angle <= 112.5f)
                {
                    // 90
                    if (_levels[levelIndex].rotations.r090 != null) { _levels[levelIndex].rotations.r090.gameObject.SetActive(true); }
                }
                else if (_angle <= 157.5f)
                {
                    // 135
                    if (_levels[levelIndex].rotations.r135 != null) { _levels[levelIndex].rotations.r135.gameObject.SetActive(true); }
                }
                else if (_angle <= 202.5f)
                {
                    // 180
                    if (_levels[levelIndex].rotations.r180 != null) { _levels[levelIndex].rotations.r180.gameObject.SetActive(true); }
                }
                else if (_angle <= 247.5f)
                {
                    // 225
                    if (_levels[levelIndex].rotations.r225 != null) { _levels[levelIndex].rotations.r225.gameObject.SetActive(true); }
                }
                else if (_angle <= 292.5f)
                {
                    // 270
                    if (_levels[levelIndex].rotations.r270 != null) { _levels[levelIndex].rotations.r270.gameObject.SetActive(true); }
                }
                else
                {
                    // 315
                    if (_levels[levelIndex].rotations.r315 != null) { _levels[levelIndex].rotations.r315.gameObject.SetActive(true); }
                }
                */
            }
        }

        public void AdjustUI(bool checkLevel = false)
        {
            if(checkLevel)
            {
                CheckLevel();
            }

            if (collectButton && !scout)
            {
                if (collectTimeout)
                {
                    if(collectTimer > 0)
                    {
                        collectTimer -= Time.deltaTime;
                    }
                    else
                    {
                        collectTimeout = false;
                    }
                }
                else
                {
                    switch (id)
                    {
                        case Data.BuildingID.townhall:
                            break;
                        case Data.BuildingID.goldmine:
                            if (_data.goldStorage >= Data.minGoldCollect)
                            {
                                collectButton.gameObject.SetActive(!collecting && _data.isConstructing == false && Player.instanse.gold < Player.instanse.maxGold);
                            }
                            else
                            {
                                collectButton.gameObject.SetActive(false);
                            }
                            break;
                        case Data.BuildingID.elixirmine:
                            if (_data.elixirStorage >= Data.minElixirCollect)
                            {
                                collectButton.gameObject.SetActive(!collecting && _data.isConstructing == false && Player.instanse.elixir < Player.instanse.maxElixir);
                            }
                            else
                            {
                                collectButton.gameObject.SetActive(false);
                            }
                            break;
                        case Data.BuildingID.darkelixirmine:
                            if (_data.darkStorage >= Data.minDarkElixirCollect)
                            {
                                collectButton.gameObject.SetActive(!collecting && _data.isConstructing == false && Player.instanse.darkElixir < Player.instanse.maxDarkElixir);
                            }
                            else
                            {
                                collectButton.gameObject.SetActive(false);
                            }
                            break;
                        case Data.BuildingID.goldstorage:
                            break;
                    }
                    Vector3 end = UI_Main.instanse._grid.GetEndPosition(this);

                    Vector3 planDownLeft = CameraController.instanse.planDownLeft;
                    Vector3 planTopRight = CameraController.instanse.planTopRight;

                    float w = planTopRight.x - planDownLeft.x;
                    float h = planTopRight.y - planDownLeft.y;

                    float endW = end.x - planDownLeft.x;
                    float endH = end.y - planDownLeft.y;

                    Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height);
                    collectButton.rect.anchoredPosition = screenPoint;
                }
            }

            bool isConstructing = _data.isConstructing && _data.buildTime > 0;

            if (_boostEffects)
            {
                _boostEffects.SetActive(_data.boost > Player.instanse.data.nowTime);
            }

            bool showBar = false;

            if (isConstructing)
            {
                System.TimeSpan span = System.TimeSpan.Zero;
                if (Player.instanse.data.nowTime < _data.constructionTime)
                {
                    span = _data.constructionTime - Player.instanse.data.nowTime;
                }

                if (span.TotalSeconds <= 0)
                {
                    isConstructing = false;
                    isCons = false;
                    level = level + 1;
                    checkLevel = true;
                    lastChange = System.DateTime.Now;
                    Player.instanse.RushSyncRequest();
                }
                else
                {
                    showBar = true;
                }

                if (buildBar && showBar)
                {
                    buildBar.texts[0].text = Tools.SecondsToTimeFormat(span);
                    buildBar.texts[0].ForceMeshUpdate(true);
                    buildBar.bar.fillAmount = Mathf.Abs(1f - ((float)span.TotalSeconds / (float)_data.buildTime));

                    if (span.TotalSeconds <= 0)
                    {
                        buildBar.gameObject.SetActive(false);
                    }
                    else
                    {
                        buildBar.gameObject.SetActive(true);
                        Vector3 end = UI_Main.instanse._grid.GetEndPosition(this);

                        Vector3 planDownLeft = CameraController.instanse.planDownLeft;
                        Vector3 planTopRight = CameraController.instanse.planTopRight;

                        float w = planTopRight.x - planDownLeft.x;
                        float h = planTopRight.y - planDownLeft.y;

                        float endW = end.x - planDownLeft.x;
                        float endH = end.y - planDownLeft.y;

                        Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height);
                        buildBar.rect.anchoredPosition = screenPoint;
                    }
                }
            }

            if (buildBar)
            {
                buildBar.gameObject.SetActive(showBar);
            }

            if (_constructionEffects)
            {
                _constructionEffects.SetActive(isConstructing);
            }
        }

        public void Collect()
        {
            collectButton.gameObject.SetActive(false);
            collecting = true;
            collectTimeout = true;
            collectTimer = 60;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.COLLECT);
            packet.Write(_data.databaseID);
            Sender.TCP_Send(packet);
            if (id == Data.BuildingID.elixirmine)
            {
                SoundManager.instanse.PlaySound(SoundManager.instanse.elixirCollect);
            }
            else if (id == Data.BuildingID.goldmine)
            {
                SoundManager.instanse.PlaySound(SoundManager.instanse.goldCollect);
            }
        }

        public void PlacedOnGrid(int x, int y, bool notMovingIntended = false, bool scout = false)
        {
            if (collectButton)
            {
                collectButton.gameObject.SetActive(false);
            }
            _currentX = x;
            _currentY = y;
            _X = x;
            _Y = y;
            _originalX = x;
            _originalY = y;
            SetPosition(x, y);
            if (notMovingIntended)
            {
                if (_baseArea)
                {
                    _baseArea.gameObject.SetActive(false);
                }
            }
            else
            {
                SetBaseColor();
            }
            CheckLevel();
        }

        public void SetPosition(int x, int y)
        {
            transform.position = UI_Main.instanse._grid.CellToWorld(x, y);
        }

        public void StartMovingOnGrid()
        {
            _X = _currentX;
            _Y = _currentY;
        }

        public void RemovedFromGrid()
        {
            _buildInstance = null;
            UI_Build.instanse.SetStatus(false);
            CameraController.instanse.isPlacingBuilding = false;
            Destroy(gameObject);
        }

        public void BuildForFirstTimeStarted()
        {
            if(_constructionEffects != null && UI_WarLayout.instanse != null && data.buildTime > 0)
            {
                _constructionEffects.SetActive(true);
            }
            UI_Main.instanse._grid.AddUnidentifiedBuilding(buildInstanse);
            _buildInstance = null;
            UI_Build.instanse.SetStatus(false);
            CameraController.instanse.isPlacingBuilding = false;
            if (_baseArea)
            {
                _baseArea.gameObject.SetActive(false);
            }
        }

        public void UpdateGridPosition(Vector3 basePosition, Vector3 currentPosition)
        {

            Vector3 dir = currentPosition - basePosition;
            Vector3 original = UI_Main.instanse._grid.CellToWorld(_X, _Y);
            Vector3 position = original + dir;
            Vector2Int p = UI_Main.instanse._grid.WorldToCell(position);
            _currentX = p.x;
            _currentY = p.y;
            SetPosition(_currentX, _currentY);
            if(_X != _currentX || _Y != _currentY)
            {
                if (_baseArea)
                {
                    _baseArea.gameObject.SetActive(true);
                }
            }
            SetBaseColor();
        }

        private void SetBaseColor()
        {
            if(UI_Main.instanse._grid.CanPlaceBuilding(this, currentX, currentY))
            {
                UI_Build.instanse.clickConfirmButton.interactable = true;
                if (_baseArea)
                {
                    _baseArea.color = Color.green;
                }
            }
            else
            {
                UI_Build.instanse.clickConfirmButton.interactable = false;
                if (_baseArea)
                {
                    _baseArea.color = Color.red;
                }
            }
        }

        [HideInInspector] public bool waitingReplaceResponse = false;

        public void Selected()
        {
            if (selectedInstanse != null)
            {
                if(selectedInstanse == this)
                {
                    return;
                }
                else
                {
                    selectedInstanse.Deselected();
                }
            }

            if (waitingReplaceResponse)
            {
                return;
            }

            if (_rangeEffects && _data.radius > 0)
            {
                _rangeEffects.SetActive(true);
                if (_blindRangeEffects)
                {
                    _blindRangeEffects.SetActive(true);
                }
            }

            selectedInstanse = this;
            
            UI_SelectedBuilding.instance._elements.SetActive(true);
            UI_SelectedBuilding.instance._buildingNameText.SetText(Language.instanse.GetBuildingName(id, data.level));
            UI_SelectedBuilding.instance._buildingNameText.ForceMeshUpdate(true);
            
            if (!scout)
            {
                if (_selectEffects)
                {
                    _selectEffects.SetActive(true);
                }
                _originalX = currentX;
                _originalY = currentY;
                UI_BuildingOptions.instanse.SetStatus(true);
            }
        }

        public void Deselected()
        {
            if (_rangeEffects)
            {
                _rangeEffects.SetActive(false);
                if (_blindRangeEffects)
                {
                    _blindRangeEffects.SetActive(false);
                }
            }
            UI_SelectedBuilding.instance._elements.SetActive(false);
            if (!scout)
            {
                UI_BuildingOptions.instanse.SetStatus(false);
                CameraController.instanse.isReplacingBuilding = false;
                if (_originalX != currentX || _originalY != currentY)
                {
                    SaveLocation();
                }
                if (_selectEffects)
                {
                    _selectEffects.SetActive(false);
                }
            }
            selectedInstanse = null;
        }

        public void SaveLocation(bool resetIfNot = true)
        {
            if (UI_Main.instanse._grid.CanPlaceBuilding(this, currentX, currentY) && (_X != currentX || _Y != currentY) && !waitingReplaceResponse)
            {
                waitingReplaceResponse = true;
                Packet packet = new Packet();
                packet.Write((int)Player.RequestsID.REPLACE);
                packet.Write(selectedInstanse.databaseID);
                packet.Write(selectedInstanse.currentX);
                packet.Write(selectedInstanse.currentY);
                packet.Write(UI_WarLayout.instanse.isActive ? 2 : 1);
                Sender.TCP_Send(packet);
                if (_baseArea)
                {
                    _baseArea.gameObject.SetActive(false);
                }
            }
            else
            {
                if (resetIfNot)
                {
                    if (waitingReplaceResponse == false)
                    {
                        PlacedOnGrid(_originalX, _originalY);
                    }
                    if (_baseArea)
                    {
                        if (_baseArea)
                        {
                            _baseArea.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (_originalX == currentX && _originalY == currentY)
                    {
                        _baseArea.gameObject.SetActive(false);
                    }
                }
            }
        }

        public void CheckLevel()
        {
            if (_levels != null && _levels.Length > 0)
            {
                levelIndex = _levels.Length - 1;
                for (int i = 0; i < _levels.Length; i++) { if (_levels[i].level == _data.level || (_data.level == 0 && _levels[i].level == 1)) { levelIndex = i; break; } }
                for (int i = 0; i < _levels.Length; i++)
                {
                    if (_levels[i].elements != null)
                    {
                        _levels[i].elements.SetActive(i == levelIndex);
                    }
                }
                if (id == Data.BuildingID.wall)
                {
                    CheckWall();
                }
            }
        }

        private void CheckWall()
        {
            if (levelIndex >= 0 && _levels[levelIndex].elements != null && _levels[levelIndex].wall != null)
            {
                // bool index_r = false;
                bool index_l = false;
                // bool index_f = false;
                bool index_b = false;
                for (int i = 0; i < UI_Main.instanse._grid.buildings.Count; i++)
                {
                    if (UI_Main.instanse._grid.buildings[i].id != Data.BuildingID.wall) { continue; }
                    if (UI_Main.instanse._grid.buildings[i].currentX == (currentX + 1) && UI_Main.instanse._grid.buildings[i].currentY == currentY)
                    {
                        index_l = true;
                    }
                    else if (UI_Main.instanse._grid.buildings[i].currentX == (currentX - 1) && UI_Main.instanse._grid.buildings[i].currentY == currentY)
                    {
                        // index_r = true;
                    }
                    else if (UI_Main.instanse._grid.buildings[i].currentX == currentX && UI_Main.instanse._grid.buildings[i].currentY == (currentY + 1))
                    {
                        index_b = true;
                    }
                    else if (UI_Main.instanse._grid.buildings[i].currentX == currentX && UI_Main.instanse._grid.buildings[i].currentY == (currentY - 1))
                    {
                        // index_f = true;
                    }
                }
                if(_levels[levelIndex].wall.left != null)
                {
                    _levels[levelIndex].wall.left.gameObject.SetActive(index_l);
                }
                if(_levels[levelIndex].wall.back != null)
                {
                    _levels[levelIndex].wall.back.gameObject.SetActive(index_b);
                }
                if(_levels[levelIndex].wall.center != null)
                {
                    _levels[levelIndex].wall.center.gameObject.SetActive(!index_b && !index_l);
                }
                /*
                if (_levels[levelIndex].wall_right != null)
                {
                    _levels[levelIndex].wall_right.SetActive(index_r);
                }
                if (_levels[levelIndex].wall_left != null)
                {
                    _levels[levelIndex].wall_left.SetActive(index_l);
                }
                if (_levels[levelIndex].wall_front != null)
                {
                    _levels[levelIndex].wall_front.SetActive(index_f);
                }
                if (_levels[levelIndex].wall_back != null)
                {
                    _levels[levelIndex].wall_back.SetActive(index_b);
                }
                */
            }
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if(!Application.isPlaying && _center != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_center.position, 0.1f);
            }
        }
        #endif

    }
}
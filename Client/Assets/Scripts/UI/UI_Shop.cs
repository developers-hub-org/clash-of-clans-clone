namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Shop : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] public RectTransform _buildingsGrid = null;
        [SerializeField] public TextMeshProUGUI _goldText = null;
        [SerializeField] public TextMeshProUGUI _elixirText = null;
        [SerializeField] public TextMeshProUGUI _darkText = null;
        [SerializeField] public TextMeshProUGUI _gemsText = null;
        [SerializeField] private UI_Building _buildingPrefab = null;
        [SerializeField] private Data.BuildingID[] _buildingsAvailable = null;

        private bool _active = false; public bool isActive { get { return _active; } }
        private static UI_Shop _instance = null; public static UI_Shop instanse { get { return _instance; } }
        private List<UI_Building> ui_buildings = new List<UI_Building>();

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            if (_buildingsAvailable != null)
            {
                Data.BuildingID[] buildingsAvailable = _buildingsAvailable.Distinct().ToArray();
                for (int i = 0; i < buildingsAvailable.Length; i++)
                {
                    UI_Building building = Instantiate(_buildingPrefab, _buildingsGrid);
                    building.id = buildingsAvailable[i];
                    ui_buildings.Add(building);
                }
            }
            _closeButton.onClick.AddListener(CloseShop);
        }

        public bool IsBuildingInShop(Data.BuildingID id)
        {
            if (_buildingsAvailable != null)
            {
                for (int i = 0; i < _buildingsAvailable.Length; i++)
                {
                    if (_buildingsAvailable[i] == id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void SetStatus(bool status)
        {
            if (status)
            {
                _goldText.text = Player.instanse.gold.ToString();
                _elixirText.text = Player.instanse.elixir.ToString();
                _darkText.text = Player.instanse.darkElixir.ToString();
                _gemsText.text = Player.instanse.data.gems.ToString();
                _buildingsGrid.anchoredPosition = new Vector2(0, _buildingsGrid.anchoredPosition.y);

                int _workers = 0;
                int _busyWorkers = 0;
                if (Player.instanse.data.buildings != null && Player.instanse.data.buildings.Count > 0)
                {
                    for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
                    {
                        if (Player.instanse.data.buildings[i].isConstructing)
                        {
                            _busyWorkers += 1;
                        }
                        if(Player.instanse.data.buildings[i].id != Data.BuildingID.buildershut)
                        {
                            continue;
                        }
                        _workers += 1;
                    }
                }

                if (ui_buildings != null)
                {
                    for (int i = 0; i < ui_buildings.Count; i++)
                    {
                        ui_buildings[i].Initialize(_workers > _busyWorkers);
                    }
                }
            }
            _active = status;
            _elements.SetActive(status);
        }

        public bool PlaceBuilding(Data.BuildingID id, int x = -1, int y = -1)
        {
            var prefab = UI_Main.instanse.GetBuildingPrefab(id);
            if (prefab.Item1 != null)
            {
                if(x < 0 || y < 0)
                {
                    Vector2Int point = UI_Main.instanse._grid.GetBestBuildingPlace(prefab.Item2.rows, prefab.Item2.columns);
                    x = point.x;
                    y = point.y;
                }

                Vector3 position = Vector3.zero;

                Data.Building data = new Data.Building();
                data.id = id;
                data.x = x;
                data.y = y;
                data.level = 1;
                data.databaseID = 0;

                bool havrResources = false;

                int sbi = -1;

                for (int i = 0; i < Player.instanse.initializationData.serverBuildings.Count; i++)
                {
                    if(Player.instanse.initializationData.serverBuildings[i].id != id.ToString() || Player.instanse.initializationData.serverBuildings[i].level != 1) { continue; }
                    data.columns = Player.instanse.initializationData.serverBuildings[i].columns;
                    data.rows = Player.instanse.initializationData.serverBuildings[i].rows;
                    data.buildTime = Player.instanse.initializationData.serverBuildings[i].buildTime;
                    sbi = i;
                    havrResources = Player.instanse.data.gems >= Player.instanse.initializationData.serverBuildings[i].requiredGems && Player.instanse.elixir >= Player.instanse.initializationData.serverBuildings[i].requiredElixir && Player.instanse.gold >= Player.instanse.initializationData.serverBuildings[i].requiredGold && Player.instanse.darkElixir >= Player.instanse.initializationData.serverBuildings[i].requiredDarkElixir;
                    break;
                }

                if(!havrResources)
                {
                    return false;
                }

                UI_Shop.instanse.SetStatus(false);
                UI_Main.instanse.SetStatus(true);

                Building building = Instantiate(prefab.Item1, position, Quaternion.identity);
                building.rows = data.rows;
                building.columns = data.columns;

                building.serverIndex = sbi;

                data.radius = 0;
                building.data = data;
                building.databaseID = 0;
                building.PlacedOnGrid(x, y);
                if (building._baseArea)
                {
                    building._baseArea.gameObject.SetActive(true);
                }

                Building.buildInstanse = building;
                CameraController.instanse.isPlacingBuilding = true;
                UI_Build.instanse.SetStatus(true);
                return true;
            }
            return false;
        }

        private void CloseShop()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            SetStatus(false);
            UI_Main.instanse.SetStatus(true);
        }

    }
}
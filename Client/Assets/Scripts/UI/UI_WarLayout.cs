namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_WarLayout : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _backButton = null;
        [SerializeField] private UI_WarLayoutBuilding _listPrefab = null;
        [SerializeField] private RectTransform _listGrid = null;
        [SerializeField] private RectTransform _lostRoot = null;

        [HideInInspector] public List<UI_WarLayoutBuilding> buildingItems = new List<UI_WarLayoutBuilding>();
        private static UI_WarLayout _instance = null; public static UI_WarLayout instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }
        private long _placingID = 0; public long placingID { get { return _placingID; } set { _placingID = value; } }

        [HideInInspector] public GameObject placingItem = null;
        private float itemHeight = 1;

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _backButton.onClick.AddListener(Back);
            itemHeight = (_lostRoot.anchorMax.y - _lostRoot.anchorMin.y) * Screen.height;
        }

        public void SetStatus(bool status)
        {
            _active = status;
            _placingID = 0;
            if (status)
            {
                UI_Main.instanse.SetStatus(false);
                PlaceBuildings();
                Player.instanse.RushSyncRequest();
            }
            _elements.SetActive(status);
        }

        private void Back()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            ClearItems();
            UI_Main.instanse._grid.Clear();
            UI_Main.instanse.SetStatus(true);
            SetStatus(false);
        }

        private void PlaceBuildings()
        {
            ClearItems();
            UI_Main.instanse._grid.Clear();
            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if(Player.instanse.data.buildings[i].warX >= 0 && Player.instanse.data.buildings[i].warY >= 0)
                {
                    var prefab = UI_Main.instanse.GetBuildingPrefab(Player.instanse.data.buildings[i].id);
                    if (prefab.Item1 != null)
                    {
                        Building building = Instantiate(prefab.Item1, Vector3.zero, Quaternion.identity);
                        building.data = Player.instanse.data.buildings[i];
                        building.rows = prefab.Item2.rows;
                        building.columns = prefab.Item2.columns;
                        building.databaseID = Player.instanse.data.buildings[i].databaseID;
                        building.PlacedOnGrid(Player.instanse.data.buildings[i].warX, Player.instanse.data.buildings[i].warY);
                        if (building._baseArea)
                        {
                            building._baseArea.gameObject.SetActive(false);
                        }
                        UI_Main.instanse._grid.buildings.Add(building);
                    }
                }
                else
                {
                    UI_WarLayoutBuilding building = Instantiate(_listPrefab, _listGrid);
                    building.Initialized(Player.instanse.data.buildings[i]);
                    RectTransform rect = building.GetComponent<RectTransform>();
                    rect.sizeDelta = new Vector2(itemHeight, itemHeight);
                    buildingItems.Add(building);
                }
            }
            UI_Main.instanse._grid.RefreshBuildings();
        }

        public void ClearItems()
        {
            for (int i = 0; i < buildingItems.Count; i++)
            {
                if (buildingItems[i])
                {
                    Destroy(buildingItems[i].gameObject);
                }
            }
            buildingItems.Clear();
        }

        public void DataSynced()
        {
            if (Player.instanse.data.buildings != null && Player.instanse.data.buildings.Count > 0)
            {
                for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
                {
                    if (Player.instanse.data.buildings[i].warX >= 0 && Player.instanse.data.buildings[i].warY >= 0)
                    {
                        Building building = UI_Main.instanse._grid.GetBuilding(Player.instanse.data.buildings[i].databaseID);
                        if (building != null)
                        {
                            building.data = Player.instanse.data.buildings[i];
                        }
                        else
                        {
                            building = UI_Main.instanse._grid.GetBuilding(Player.instanse.data.buildings[i].id, Player.instanse.data.buildings[i].x, Player.instanse.data.buildings[i].y);
                            if (building != null)
                            {
                                UI_Main.instanse._grid.RemoveUnidentifiedBuilding(building);
                                building.databaseID = Player.instanse.data.buildings[i].databaseID;
                                UI_Main.instanse._grid.buildings.Add(building);
                            }
                            else
                            {
                                var prefab = UI_Main.instanse.GetBuildingPrefab(Player.instanse.data.buildings[i].id);
                                if (prefab.Item1 != null)
                                {
                                    building = Instantiate(prefab.Item1, Vector3.zero, Quaternion.identity);
                                    building.data = Player.instanse.data.buildings[i];
                                    building.rows = prefab.Item2.rows;
                                    building.columns = prefab.Item2.columns;
                                    building.databaseID = Player.instanse.data.buildings[i].databaseID;
                                    building.PlacedOnGrid(Player.instanse.data.buildings[i].warX, Player.instanse.data.buildings[i].warY, true);
                                    if (building._baseArea)
                                    {
                                        building._baseArea.gameObject.SetActive(false);
                                    }
                                    UI_Main.instanse._grid.buildings.Add(building);
                                }
                            }
                        }
                        building.data = Player.instanse.data.buildings[i];
                        building.AdjustUI(true);
                    }
                }
            }
        }

    }
}
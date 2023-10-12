namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DevelopersHub.RealtimeNetworking.Client;
    using System;

    public class UI_Build : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        public RectTransform buttonConfirm = null;
        public RectTransform buttonCancel = null;
        [HideInInspector] public Button clickConfirmButton = null;
        [SerializeField] private float height = 0.06f;
        private Vector2 size = Vector2.one;

        private static UI_Build _instance = null; public static UI_Build instanse { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
            clickConfirmButton = buttonConfirm.gameObject.GetComponent<Button>();
        }

        private void Start()
        {
            buttonConfirm.gameObject.GetComponent<Button>().onClick.AddListener(Confirm);
            buttonCancel.gameObject.GetComponent<Button>().onClick.AddListener(Cancel);
            buttonConfirm.anchorMin = Vector3.zero;
            buttonConfirm.anchorMax = Vector3.zero;
            buttonCancel.anchorMin = Vector3.zero;
            buttonCancel.anchorMax = Vector3.zero;
            size = new Vector2(Screen.height * height, Screen.height * height);
            buttonConfirm.sizeDelta = size * CameraController.instanse.zoomScale;
            buttonCancel.sizeDelta = size * CameraController.instanse.zoomScale;
        }

        private void Update()
        {
            if(Building.buildInstanse != null && CameraController.instanse.isPlacingBuilding)
            {
                buttonConfirm.sizeDelta = size / CameraController.instanse.zoomScale;
                buttonCancel.sizeDelta = size / CameraController.instanse.zoomScale;

                Vector3 end = UI_Main.instanse._grid.GetEndPosition(Building.buildInstanse);

                Vector3 planDownLeft = CameraController.instanse.planDownLeft;
                Vector3 planTopRight = CameraController.instanse.planTopRight;

                float w = planTopRight.x - planDownLeft.x;
                float h = planTopRight.y - planDownLeft.y;

                float endW = end.x - planDownLeft.x;
                float endH = end.y - planDownLeft.y;

                Vector2 screenPoint = new Vector2(endW / w * Screen.width, endH / h * Screen.height);

                Vector2 confirmPoint = screenPoint;
                confirmPoint.x += (buttonConfirm.rect.width + 10f);
                buttonConfirm.anchoredPosition = confirmPoint;

                Vector2 cancelPoint = screenPoint;
                cancelPoint.x -= (buttonCancel.rect.width + 10f);
                buttonCancel.anchoredPosition = cancelPoint;
            }
        }

        public void SetStatus(bool status)
        {
            _elements.SetActive(status);
        }

        [HideInInspector] public bool isBuildingWall = false;
        [HideInInspector] public int wallX = 0;
        [HideInInspector] public int wallY = 0;
        [HideInInspector] public List<Vector2Int> wallsBuilt = new List<Vector2Int>();

        private void Confirm()
        {
            //SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            if (Building.buildInstanse != null/* && UI_Main.instanse._grid.CanPlaceBuilding(Building.instanse, Building.instanse.currentX, Building.instanse.currentY)*/)
            {
                if (!UI_WarLayout.instanse.isActive)
                {
                    if (!CheckLimit())
                    {
                        Cancel();
                        return;
                    }

                    if (Building.buildInstanse.serverIndex >= 0)
                    {
                        Player.instanse.data.gems -= Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredGems;
                        Player.instanse.elixir -= Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredElixir;
                        Player.instanse.gold -= Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredGold;
                        Player.instanse.darkElixir -= Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredDarkElixir;

                        Building.buildInstanse.placeGemCost = Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredGems;
                        Building.buildInstanse.placeElixirCost = Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredElixir;
                        Building.buildInstanse.placeGoldCost = Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredGold;
                        Building.buildInstanse.placeDarkElixirCost = Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredDarkElixir;
                    }
                }
                isBuildingWall = (Building.buildInstanse.id == Data.BuildingID.wall);
                if (!isBuildingWall)
                {
                    wallsBuilt.Clear();
                }
                wallX = Building.buildInstanse.currentX;
                wallY = Building.buildInstanse.currentY;
                Packet packet = new Packet();
                packet.Write((int)Player.RequestsID.BUILD);
                packet.Write(SystemInfo.deviceUniqueIdentifier);
                packet.Write(Building.buildInstanse.id.ToString());
                packet.Write(Building.buildInstanse.currentX);
                packet.Write(Building.buildInstanse.currentY);
                packet.Write(UI_WarLayout.instanse.isActive ? 2 : 1);
                packet.Write(UI_WarLayout.instanse.placingID);
                Building.buildInstanse.lastChange = DateTime.Now;
                Sender.TCP_Send(packet);
                if (UI_WarLayout.instanse.isActive && UI_WarLayout.instanse.placingItem != null)
                {
                    Destroy(UI_WarLayout.instanse.placingItem);
                    UI_WarLayout.instanse.placingItem = null;
                }
                BuildConf();
                if (isBuildingWall)
                {
                    CheckeWall();
                }
                else
                {
                    SoundManager.instanse.PlaySound(SoundManager.instanse.buildStart);
                }
            }
        }

        private bool CheckLimit()
        {
            if (Building.buildInstanse.serverIndex >= 0)
            {
                if(Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredGems > Player.instanse.data.gems || Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredElixir > Player.instanse.elixir || Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredDarkElixir > Player.instanse.darkElixir || Player.instanse.initializationData.serverBuildings[Building.buildInstanse.serverIndex].requiredGold > Player.instanse.gold)
                {
                    return false;
                }
            }
            int townHallLevel = 1;
            int count = 0;
            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if (Player.instanse.data.buildings[i].id == Data.BuildingID.townhall)
                {
                    townHallLevel = Player.instanse.data.buildings[i].level;
                }
                if (Player.instanse.data.buildings[i].id == Building.buildInstanse.id)
                {
                    count++;
                }
            }

            Data.BuildingCount limits = Data.GetBuildingLimits(townHallLevel, Building.buildInstanse.id.ToString());
            if (limits != null)
            {
                if (count >= limits.count)
                {
                    return false;
                }
            }
            return true;
        }

        private void CheckeWall()
        {
            int warLayoutIndex = -1;
                    bool haveMoreWall = false;
                    if (UI_WarLayout.instanse.isActive)
                    {
                        if (UI_WarLayout.instanse.buildingItems != null)
                        {
                            for (int i = 0; i < UI_WarLayout.instanse.buildingItems.Count; i++)
                            {
                                if (UI_WarLayout.instanse.buildingItems[i] != null && UI_WarLayout.instanse.buildingItems[i].globalID == Data.BuildingID.wall)
                                {
                                    haveMoreWall = true;
                                    warLayoutIndex = i;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        int townhallLevel = 1;
                        int haveCount = 0;
                        for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
                        {
                            if (Player.instanse.data.buildings[i].id == Data.BuildingID.townhall) { townhallLevel = Player.instanse.data.buildings[i].level; }
                            if (Player.instanse.data.buildings[i].id != Data.BuildingID.wall) { continue; }
                            haveCount++;
                        }
                        Data.BuildingCount limit = Data.GetBuildingLimits(townhallLevel, Data.BuildingID.wall.ToString());
                        if (limit != null && haveCount < limit.count)
                        {
                            haveMoreWall = true;
                        }
                    }
                    if (haveMoreWall)
                    {
                        UI_Build.instanse.wallsBuilt.Add(new Vector2Int(UI_Build.instanse.wallX, UI_Build.instanse.wallY));
                        bool handled = false;
                        if (UI_Build.instanse.wallsBuilt.Count > 1)
                        {
                            int deltaX = UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x - UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 2].x;
                            int deltaY = UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y - UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 2].y;
                            if (Mathf.Abs(deltaX) == 1 && deltaY == 0)
                            {
                                if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x + deltaX, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y, 1, 1))
                                {
                                    handled = true;
                                    PlaceWall(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x + deltaX, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y, warLayoutIndex);
                                }
                                else
                                {
                                    if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y + 1, 1, 1))
                                    {
                                        handled = true;
                                        PlaceWall(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y + 1, warLayoutIndex);
                                    }
                                    else if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y - 1, 1, 1))
                                    {
                                        handled = true;
                                        PlaceWall(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y - 1, warLayoutIndex);
                                    }
                                }
                            }
                            else if (Mathf.Abs(deltaY) == 1 && deltaX == 0)
                            {
                                if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y + deltaY, 1, 1))
                                {
                                    handled = true;
                                    PlaceWall(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y + deltaY, warLayoutIndex);
                                }
                                else
                                {
                                    if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x + 1, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y, 1, 1))
                                    {
                                        handled = true;
                                        PlaceWall(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x + 1, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y, warLayoutIndex);
                                    }
                                    else if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x - 1, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y, 1, 1))
                                    {
                                        handled = true;
                                        PlaceWall(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x - 1, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y, warLayoutIndex);
                                    }
                                }
                            }
                        }
                        if (handled == false)
                        {
                            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
                            {
                                if (Player.instanse.data.buildings[i].id != Data.BuildingID.wall) { continue; }
                                Vector2Int pos = Building.GetBuildingPosition(Player.instanse.data.buildings[i]);
                                int deltaX = pos.x - UI_Build.instanse.wallX;
                                int deltaY = pos.y - UI_Build.instanse.wallY;
                                if (Mathf.Abs(deltaX) == 1 && deltaY == 0)
                                {
                                    if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallX + deltaX, UI_Build.instanse.wallY, Player.instanse.data.buildings[i].rows, Player.instanse.data.buildings[i].columns))
                                    {
                                        handled = true;
                                        PlaceWall(UI_Build.instanse.wallX + deltaX, UI_Build.instanse.wallY, warLayoutIndex);
                                        break;
                                    }
                                    else
                                    {
                                        if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallX, UI_Build.instanse.wallY + 1, Player.instanse.data.buildings[i].rows, Player.instanse.data.buildings[i].columns))
                                        {
                                            handled = true;
                                            PlaceWall(UI_Build.instanse.wallX, UI_Build.instanse.wallY + 1, warLayoutIndex);
                                            break;
                                        }
                                        else if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallX, UI_Build.instanse.wallY - 1, Player.instanse.data.buildings[i].rows, Player.instanse.data.buildings[i].columns))
                                        {
                                            handled = true;
                                            PlaceWall(UI_Build.instanse.wallX, UI_Build.instanse.wallY - 1, warLayoutIndex);
                                            break;
                                        }
                                    }
                                }
                                else if (Mathf.Abs(deltaY) == 1 && deltaX == 0)
                                {
                                    if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallX, UI_Build.instanse.wallY + deltaY, Player.instanse.data.buildings[i].rows, Player.instanse.data.buildings[i].columns))
                                    {
                                        handled = true;
                                        PlaceWall(UI_Build.instanse.wallX, UI_Build.instanse.wallY + deltaY, warLayoutIndex);
                                        break;
                                    }
                                    else
                                    {
                                        if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallX + 1, UI_Build.instanse.wallY, Player.instanse.data.buildings[i].rows, Player.instanse.data.buildings[i].columns))
                                        {
                                            handled = true;
                                            PlaceWall(UI_Build.instanse.wallX + 1, UI_Build.instanse.wallY, warLayoutIndex);
                                            break;
                                        }
                                        else if (UI_Main.instanse._grid.CanPlaceBuilding(UI_Build.instanse.wallX - 1, UI_Build.instanse.wallY, Player.instanse.data.buildings[i].rows, Player.instanse.data.buildings[i].columns))
                                        {
                                            handled = true;
                                            PlaceWall(UI_Build.instanse.wallX - 1, UI_Build.instanse.wallY, warLayoutIndex);
                                            break;
                                        }
                                    }
                                }
                            }
                            if (handled == false)
                            {
                                PlaceWall(UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].x + 1, UI_Build.instanse.wallsBuilt[UI_Build.instanse.wallsBuilt.Count - 1].y, warLayoutIndex);
                            }
                        }
                    }
        }

        private void PlaceWall(int x, int y, int warIndex)
        {
            if (warIndex >= 0)
            {
                UI_WarLayout.instanse.buildingItems[warIndex].PlaceWall(x, y);
            }
            else
            {
                UI_Shop.instanse.PlaceBuilding(Data.BuildingID.wall, x, y);
            }
        }

        public void BuildConf()
        {
            if (Building.buildInstanse != null)
            {
                CameraController.instanse.isPlacingBuilding = false;
                Building.buildInstanse.BuildForFirstTimeStarted();
                if (UI_WarLayout.instanse.isActive && UI_WarLayout.instanse.placingItem != null)
                {
                    UI_WarLayout.instanse.placingItem.SetActive(true);
                    UI_WarLayout.instanse.placingItem = null;
                }
            }
        }

        public void Cancel()
        {
            if (Building.buildInstanse != null)
            {
                CameraController.instanse.isPlacingBuilding = false;
                Building.buildInstanse.RemovedFromGrid();
                if (UI_WarLayout.instanse.isActive && UI_WarLayout.instanse.placingItem != null)
                {
                    UI_WarLayout.instanse.placingItem.SetActive(true);
                    UI_WarLayout.instanse.placingItem = null;
                }
            }
        }

    }
}
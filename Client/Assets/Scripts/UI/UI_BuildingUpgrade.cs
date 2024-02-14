namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DevelopersHub.RealtimeNetworking.Client;
    using System;

    public class UI_BuildingUpgrade : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        private static UI_BuildingUpgrade _instance = null; public static UI_BuildingUpgrade instanse { get { return _instance; } }

        [SerializeField] private Button _closeButton = null;
        [SerializeField] private GameObject _maxLevelPanel = null;
        [SerializeField] private GameObject _detailsPanel = null;
        [SerializeField] private TextMeshProUGUI _titleLevel = null;
        [SerializeField] private TextMeshProUGUI _titleBuilding = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private Sprite _defaultIcon = null;
        [SerializeField] private TextMeshProUGUI reqGold = null;
        [SerializeField] private TextMeshProUGUI reqElixir = null;
        [SerializeField] private TextMeshProUGUI reqDark = null;
        [SerializeField] private TextMeshProUGUI reqGems = null;
        [SerializeField] private TextMeshProUGUI reqTime = null;
        [SerializeField] private Button _upgradeButton = null;
        [SerializeField] private GameObject _requiredBuildingPanel = null;
        [SerializeField] private GameObject _townHallRequiredPanel = null;
        [SerializeField] private TextMeshProUGUI _townHallRequiredText = null;
        [SerializeField] private UI_RequiredBuilding _requiredBuildingPrefab = null;
        [SerializeField] private RectTransform _requiredBuildingGrid = null;
        [SerializeField] private RectTransform _requiredBuildingRoot = null;

        private List<UI_RequiredBuilding> _buildings = new List<UI_RequiredBuilding>();
        private bool _active = false; public bool isActive { get { return _active; } }
        private Data.ServerBuilding serverBuilding = null;
        private Building selectedInstanse = null;

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private long id = 0;

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _upgradeButton.onClick.AddListener(Upgrade);
        }

        public void Open()
        {
            serverBuilding = null;
            ClearBuildings();
            _upgradeButton.interactable = true;
            _requiredBuildingPanel.SetActive(false);
            _townHallRequiredPanel.SetActive(false);
            id = Building.selectedInstanse.data.databaseID;
            selectedInstanse = Building.selectedInstanse;
            int x = -1;
            int townHallLevel = 0;
            int haveCount = 0;
            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if (Player.instanse.data.buildings[i].databaseID == id) { x = i; }
                if (Player.instanse.data.buildings[i].id == Data.BuildingID.townhall) { townHallLevel = Player.instanse.data.buildings[i].level ; }
                if (townHallLevel > 0 && x >= 0) { break; }
            }
            if (x >= 0)
            {
                for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
                {
                    if (Player.instanse.data.buildings[i].id == Player.instanse.data.buildings[x].id)
                    {
                        haveCount++;
                    }
                }
                _titleBuilding.text = Language.instanse.GetBuildingName(Player.instanse.data.buildings[x].id);
                Sprite icon = AssetsBank.GetBuildingIcon(Player.instanse.data.buildings[x].id, Player.instanse.data.buildings[x].level);
                if(icon != null)
                {
                    _icon.sprite = icon;
                }
                else
                {
                    _icon.sprite = _defaultIcon;
                }
                serverBuilding = Player.instanse.GetServerBuilding(Player.instanse.data.buildings[x].id, Player.instanse.data.buildings[x].level + 1);
                if(serverBuilding != null && !(Player.instanse.data.buildings[x].id == Data.BuildingID.townhall && Player.instanse.data.buildings[x].level >= Data.maxTownHallLevel))
                {
                    bool haveResources = true;
                    if (serverBuilding.requiredGold > Player.instanse.gold || serverBuilding.requiredElixir > Player.instanse.elixir || serverBuilding.requiredDarkElixir > Player.instanse.darkElixir || serverBuilding.requiredGems > Player.instanse.data.gems)
                    {
                        haveResources = false;
                    }
                    if (Player.instanse.data.buildings[x].id == Data.BuildingID.townhall)
                    {
                        Data.BuildingAvailability limits = Data.GetTownHallLimits(Player.instanse.data.buildings[x].level);
                        if(limits != null)
                        {
                            List<Data.BuildingCount> buildings = new List<Data.BuildingCount>();
                            for (int i = 0; i < limits.buildings.Length; i++)
                            {
                                if(limits.buildings[i].id == Data.BuildingID.townhall.ToString() || limits.buildings[i].id == Data.BuildingID.clancastle.ToString() || limits.buildings[i].id == Data.BuildingID.buildershut.ToString() || limits.buildings[i].id == Data.BuildingID.decoration.ToString() || limits.buildings[i].id == Data.BuildingID.obstacle.ToString())
                                {
                                    continue;
                                }
                                if (UI_Shop.instanse.IsBuildingInShop((Data.BuildingID)Enum.Parse(typeof(Data.BuildingID), limits.buildings[i].id)))
                                {
                                    for (int j = 0; j < Player.instanse.data.buildings.Count; j++)
                                    {
                                        if (Player.instanse.data.buildings[j].id.ToString() == limits.buildings[i].id)
                                        {
                                            limits.buildings[i].have += 1;
                                        }
                                    }
                                    if (limits.buildings[i].have < limits.buildings[i].count)
                                    {
                                        buildings.Add(limits.buildings[i]);
                                    }
                                }
                            }
                            if(buildings.Count > 0)
                            {
                                float h = (_requiredBuildingRoot.anchorMax.y - _requiredBuildingRoot.anchorMin.y) * Screen.height;
                                for (int i = 0; i < buildings.Count; i++)
                                {
                                    UI_RequiredBuilding requiredBuilding = Instantiate(_requiredBuildingPrefab, _requiredBuildingGrid);
                                    requiredBuilding.Initialize((Data.BuildingID)Enum.Parse(typeof(Data.BuildingID), buildings[i].id), buildings[i].count - buildings[i].have);
                                    RectTransform rect = requiredBuilding.GetComponent<RectTransform>();
                                    rect.sizeDelta = new Vector2(h, h);
                                    _buildings.Add(requiredBuilding);
                                }
                                _requiredBuildingGrid.anchoredPosition = new Vector2(0, _requiredBuildingGrid.anchoredPosition.y);
                                _requiredBuildingPanel.SetActive(true);
                                _upgradeButton.interactable = false;
                            }
                            else
                            {
                                if (haveResources == false)
                                {
                                    _townHallRequiredPanel.SetActive(true);
                                    switch (Language.instanse.language)
                                    {
                                        case Language.LanguageID.persian:
                                            _townHallRequiredText.text = "به اندازه کافی منابع ندارید";
                                            break;
                                        default:
                                            _townHallRequiredText.text = "You do not have enough resources";
                                            break;
                                    }
                                }
                                else if (UI_Main.instanse.haveAvalibaleBuilder == false)
                                {
                                    _upgradeButton.interactable = false;
                                    _townHallRequiredPanel.SetActive(true);
                                    switch (Language.instanse.language)
                                    {
                                        case Language.LanguageID.persian:
                                            _townHallRequiredText.text = "کارگر آزاد برای ساخت ندارید";
                                            break;
                                        default:
                                            _townHallRequiredText.text = "You do not have available worker";
                                            break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            _townHallRequiredPanel.SetActive(true);
                            switch (Language.instanse.language)
                            {
                                case Language.LanguageID.persian:
                                    _townHallRequiredText.text = Language.instanse.GetBuildingName(Data.BuildingID.townhall) + " به حداکثر سطح رسیده است";
                                    break;
                                default:
                                    _townHallRequiredText.text = Language.instanse.GetBuildingName(Data.BuildingID.townhall) + " is at Max Level";
                                    break;
                            }
                            _upgradeButton.interactable = false;
                        }
                    }
                    else
                    {
                        Data.BuildingCount limits = Data.GetBuildingLimits(townHallLevel, Player.instanse.data.buildings[x].id.ToString());
                        if(limits != null && limits.maxLevel > Player.instanse.data.buildings[x].level)
                        {
                            if (haveResources == false)
                            {
                                _townHallRequiredPanel.SetActive(true);
                                switch (Language.instanse.language)
                                {
                                    case Language.LanguageID.persian:
                                        _townHallRequiredText.text = "به اندازه کافی منابع ندارید";
                                        break;
                                    default:
                                        _townHallRequiredText.text = "You do not have enough resources";
                                        break;
                                }
                            }
                            else if (UI_Main.instanse.haveAvalibaleBuilder == false)
                            {
                                _upgradeButton.interactable = false;
                                _townHallRequiredPanel.SetActive(true);
                                switch (Language.instanse.language)
                                {
                                    case Language.LanguageID.persian:
                                        _townHallRequiredText.text = "کارگر آزاد برای ساخت ندارید";
                                        break;
                                    default:
                                        _townHallRequiredText.text = "You do not have available worker";
                                        break;
                                }
                            }
                        }
                        else
                        {
                            _upgradeButton.interactable = false;
                            _townHallRequiredPanel.SetActive(true);
                            switch (Language.instanse.language)
                            {
                                case Language.LanguageID.persian:
                                    _townHallRequiredText.text = "نیاز به ارتقای " + Language.instanse.GetBuildingName(Data.BuildingID.townhall) + " میباشد";
                                    break;
                                default:
                                    _townHallRequiredText.text = Language.instanse.GetBuildingName(Data.BuildingID.townhall) + " Upgrade is Required";
                                    break;
                            }
                        }
                    }
                    reqGold.text = serverBuilding.requiredGold.ToString();
                    reqElixir.text = serverBuilding.requiredElixir.ToString();
                    reqDark.text = serverBuilding.requiredDarkElixir.ToString();
                    reqGems.text = serverBuilding.requiredGems.ToString();
                    if(haveResources == false)
                    {
                        _upgradeButton.interactable = false;
                    }
                    reqTime.text = Tools.SecondsToTimeFormat(serverBuilding.buildTime);
                    switch (Language.instanse.language)
                    {
                        case Language.LanguageID.persian:
                            _titleLevel.text = "ارتقاء به سطح " + (Player.instanse.data.buildings[x].level + 1).ToString();
                            break;
                        default:
                            _titleLevel.text = "Upgrade to Level " + (Player.instanse.data.buildings[x].level + 1).ToString();
                            break;
                    }
                    _maxLevelPanel.SetActive(false);
                    _detailsPanel.SetActive(true);
                }
                else
                {
                    // Building is at max level
                    _maxLevelPanel.SetActive(true);
                    _detailsPanel.SetActive(false);
                }
                _active = true;
                _elements.SetActive(true);
                _titleLevel.ForceMeshUpdate(true);
                _titleLevel.ForceMeshUpdate(true);
                reqTime.ForceMeshUpdate(true);
                reqGold.ForceMeshUpdate(true);
                reqElixir.ForceMeshUpdate(true);
                reqDark.ForceMeshUpdate(true);
                reqGems.ForceMeshUpdate(true);
                _titleBuilding.ForceMeshUpdate(true);
            }
        }

        public void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _active = false;
            ClearBuildings();
            _elements.SetActive(false);
        }

        private void Close2()
        {
            _active = false;
            ClearBuildings();
            _elements.SetActive(false);
        }

        private void Upgrade()
        {
            if(serverBuilding != null)
            {
                if (serverBuilding.buildTime > 0)
                {
                    selectedInstanse.isCons = true;
                    selectedInstanse.data.constructionTime = Player.instanse.data.nowTime.AddSeconds(serverBuilding.buildTime);
                    selectedInstanse.data.buildTime = serverBuilding.buildTime;
                }
                else
                {
                    selectedInstanse.isCons = false;
                    selectedInstanse.level = selectedInstanse.level + 1;
                    selectedInstanse.AdjustUI(true);
                }
                selectedInstanse.lastChange = DateTime.Now;
                Packet packet = new Packet();
                packet.Write((int)Player.RequestsID.UPGRADE);
                packet.Write(id);
                Sender.TCP_Send(packet);
                Close2();
                SoundManager.instanse.PlaySound(SoundManager.instanse.buildStart);
            }
            else
            {
                Close();
            }
        }

        public void ClearBuildings()
        {
            for (int i = 0; i < _buildings.Count; i++)
            {
                if (_buildings[i] != null)
                {
                    Destroy(_buildings[i].gameObject);
                }
            }
            _buildings.Clear();
        }

    }
}
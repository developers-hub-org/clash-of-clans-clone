namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DevelopersHub.RealtimeNetworking.Client;
    using TMPro;
    using System;

    public class UI_ResearchSpell : MonoBehaviour
    {

        [SerializeField] private Data.SpellID _id = Data.SpellID.lightning; public Data.SpellID id { get { return _id; } set { _id = value; } }
        [SerializeField] private Button _button = null;
        [SerializeField] private Button _buttonInfo = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private Image _resourceIcon = null;
        [SerializeField] public TextMeshProUGUI _titleText = null;
        [SerializeField] public TextMeshProUGUI _resourceText = null;
        [SerializeField] public TextMeshProUGUI _levelText = null;
        [SerializeField] public TextMeshProUGUI _maxLevelText = null;
        [SerializeField] public TextMeshProUGUI _reqTimeText = null;
        [SerializeField] public TextMeshProUGUI _timeText = null;
        [SerializeField] public GameObject _normalPanel = null;
        [SerializeField] public GameObject _maxPanel = null;
        [SerializeField] public GameObject _researchingPanel = null;

        private bool researching = false;
        private DateTime _endResearch;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
            _buttonInfo.onClick.AddListener(Info);
        }

        public void Initialize()
        {
            _button.interactable = true;
            researching = false;
            _normalPanel.SetActive(false);
            _maxPanel.SetActive(false);
            _researchingPanel.SetActive(false);
            _titleText.text = Language.instanse.GetSpellName(_id);
            if (Language.instanse.IsRTL && _titleText.horizontalAlignment == HorizontalAlignmentOptions.Left)
            {
                _titleText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            }
            int dataIndex = -1;
            int researchIndex = -1;
            int level = 1;
            for (int i = 0; i < Player.instanse.initializationData.research.Count; i++)
            {
                if (Player.instanse.initializationData.research[i].type == Data.ResearchType.spell && Player.instanse.initializationData.research[i].globalID == _id.ToString())
                {
                    researchIndex = i; level = Player.instanse.initializationData.research[i].level; break;
                }
            }
            Sprite icon = AssetsBank.GetSpellIcon(_id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            for (int i = 0; i < Player.instanse.initializationData.serverSpells.Count; i++)
            {
                if (Player.instanse.initializationData.serverSpells[i].id == _id && Player.instanse.initializationData.serverSpells[i].level == level + 1)
                {
                    dataIndex = i; break;
                }
            }
            if (dataIndex >= 0)
            {
                if (researchIndex >= 0)
                {
                    if (Player.instanse.initializationData.research[researchIndex].end <= Player.instanse.data.nowTime)
                    {
                        SetupItem(level, dataIndex);
                    }
                    else
                    {
                        _timeText.text = "";
                        _button.interactable = false;
                        _endResearch = Player.instanse.initializationData.research[researchIndex].end;
                        _researchingPanel.SetActive(true);
                        researching = true;
                    }
                }
                else
                {
                    SetupItem(level, dataIndex);
                }
            }
            else
            {
                _maxLevelText.text = "+" + level.ToString();
                _button.interactable = false;
                _maxPanel.SetActive(true);
            }
        }

        private void SetupItem(int level, int dataIndex)
        {
            _reqTimeText.text = Tools.SecondsToTimeFormat(Player.instanse.initializationData.serverSpells[dataIndex].researchTime);
            _levelText.text = "+" + level.ToString();
            if (Player.instanse.initializationData.serverSpells[dataIndex].researchGold > 0)
            {
                _resourceIcon.sprite = AssetsBank.instanse.goblinIcon;
                _resourceText.text = Player.instanse.initializationData.serverSpells[dataIndex].researchGold.ToString();
            }
            else if (Player.instanse.initializationData.serverSpells[dataIndex].researchElixir > 0)
            {
                _resourceIcon.sprite = AssetsBank.instanse.elixirIcon;
                _resourceText.text = Player.instanse.initializationData.serverSpells[dataIndex].researchElixir.ToString();
            }
            else if (Player.instanse.initializationData.serverSpells[dataIndex].researchDarkElixir > 0)
            {
                _resourceIcon.sprite = AssetsBank.instanse.darkIcon;
                _resourceText.text = Player.instanse.initializationData.serverSpells[dataIndex].researchDarkElixir.ToString();
            }
            else
            {
                _resourceIcon.sprite = AssetsBank.instanse.gemsIcon;
                _resourceText.text = Player.instanse.initializationData.serverSpells[dataIndex].researchGems.ToString();
            }
            if (!Data.IsSpellUnlocked(_id, Player.instanse.spellFactoryLevel, Player.instanse.darkBarracksLevel))
            {
                _button.interactable = false;
            }
            if (_button.interactable && (Player.instanse.initializationData.serverSpells[dataIndex].researchGems > Player.instanse.data.gems || Player.instanse.initializationData.serverSpells[dataIndex].researchDarkElixir > Player.instanse.darkElixir || Player.instanse.initializationData.serverSpells[dataIndex].researchGold > Player.instanse.gold || Player.instanse.initializationData.serverSpells[dataIndex].researchElixir > Player.instanse.elixir))
            {
                _button.interactable = false;
            }
            _normalPanel.SetActive(true);
        }

        private void Clicked()
        {
            _button.interactable = false;
            Packet paket = new Packet();
            paket.Write((int)Player.RequestsID.RESEARCH);
            paket.Write((int)Data.ResearchType.spell);
            paket.Write(_id.ToString());
            Sender.TCP_Send(paket);
        }

        private void Update()
        {
            if (researching)
            {
                if (_endResearch > Player.instanse.data.nowTime)
                {
                    _timeText.text = Tools.SecondsToTimeFormat(_endResearch - Player.instanse.data.nowTime);
                }
                else
                {
                    researching = false;
                    Initialize();
                }
            }
        }

        private void Info()
        {
            UI_Info.instanse.OpenSpellInfo(_id);
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using UnityEngine.UI;
    using DevelopersHub.RealtimeNetworking.Client;

    public class UI_UnitsTraining : MonoBehaviour
    {

        [SerializeField] private Image _bar = null;
        [SerializeField] private Button _buttonRemove = null;
        [SerializeField] private Image _icon = null;

        private Data.Unit _unit = null; public long databaseID { get { return _unit != null ? _unit.databaseID : 0; } }
        [HideInInspector] public Data.UnitID id = Data.UnitID.barbarian;

        [HideInInspector] public int index = -1;
        private bool _remove = false; public bool remove { get { return _remove; } }

        private void Start()
        {
            _buttonRemove.onClick.AddListener(Remove);
        }

        public void Initialize(Data.Unit unit)
        {
            _bar.fillAmount = 0;
            _unit = unit;
            id = _unit.id;
            Sprite icon = AssetsBank.GetUnitIcon(unit.id);
            if(icon != null)
            {
                _icon.sprite = icon;
            }
            if(_remove)
            {
                Remove();
            }
        }

        public bool Initialize(Data.UnitID id)
        {
            bool havrResources = false;
            this.id = id;
            _bar.fillAmount = 0;
            _unit = null;
            int level = 1;
            for (int i = 0; i < Player.instanse.initializationData.research.Count; i++)
            {
                if (Player.instanse.initializationData.research[i].type == Data.ResearchType.unit && Player.instanse.initializationData.research[i].globalID == id.ToString())
                {
                    level = Player.instanse.initializationData.research[i].level;
                    break;
                }
            }
            for (int i = 0; i < Player.instanse.initializationData.serverUnits.Count; i++)
            {
                if (Player.instanse.initializationData.serverUnits[i].id == id && Player.instanse.initializationData.serverUnits[i].level == level)
                {
                    _unit = new Data.Unit();
                    _unit.id = id;
                    _unit.trained = false;
                    _unit.ready = false;
                    _unit.level = level;
                    _unit.databaseID = 0;
                    _unit.trainedTime = 0;
                    _unit.trainTime = Player.instanse.initializationData.serverUnits[i].trainTime;
                    if(Player.instanse.data.gems >= Player.instanse.initializationData.serverUnits[i].requiredGems && Player.instanse.elixir >= Player.instanse.initializationData.serverUnits[i].requiredElixir && Player.instanse.gold >= Player.instanse.initializationData.serverUnits[i].requiredGold && Player.instanse.darkElixir >= Player.instanse.initializationData.serverUnits[i].requiredDarkElixir)
                    {
                        havrResources = true;
                    }
                }
            }
            Sprite icon = AssetsBank.GetUnitIcon(id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
            return havrResources;
        }

        public void Remove()
        {
            if (_remove)
            {
                return;
            }
            if (databaseID <= 0)
            {
                //_remove = true;
                //transform.GetChild(0).gameObject.SetActive(false);
                //transform.SetParent(null);
                return;
            }
            _remove = true;
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            Packet paket = new Packet();
            paket.Write((int)Player.RequestsID.CANCELTRAIN);
            paket.Write(_unit.databaseID);
            Sender.TCP_Send(paket);
            UI_Train.instanse.RemoveTrainingItem(index);
        }

        private void Update()
        {
            if (_unit != null)
            {
                if (index == 0)
                {
                    _unit.trainedTime += Time.deltaTime;
                    if (_unit.trainTime > 0)
                    {
                        float fill = _unit.trainedTime / _unit.trainTime;
                        if(fill > 1f)
                        {
                            fill = 1f;
                        }
                        _bar.fillAmount = fill;
                    }
                }
                if (_unit.trainTime <= 0 || _unit.trainedTime >= _unit.trainTime)
                {
                    _bar.fillAmount = 1f;
                    for (int i = Player.instanse.data.units.Count - 1; i >= 0; i--)
                    {
                        if (Player.instanse.data.units[i].databaseID == databaseID)
                        {
                            Player.instanse.data.units[i].ready = true;
                            break;
                        }
                    }
                    UI_Train.instanse.RemoveTrainingItem(index);
                }
            }
        }

    }
}
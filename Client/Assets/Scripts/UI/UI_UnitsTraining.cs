namespace DevelopersHub.ClashOfWhatecer
{
    using UnityEngine;
    using UnityEngine.UI;
    using DevelopersHub.RealtimeNetworking.Client;

    public class UI_UnitsTraining : MonoBehaviour
    {

        [SerializeField] private Image _bar = null;
        [SerializeField] private Button _buttonRemove = null;
        [SerializeField] private Image _icon = null;

        private Data.Unit _unit = null; public long databaseID { get { return _unit != null ? _unit.databaseID : -1; } }
        [HideInInspector] public Data.UnitID id = Data.UnitID.barbarian;

        [HideInInspector] public int index = -1;
        public bool done { get { return _unit.ready || _unit.trainedTime >= _unit.trainTime; } }
        public bool isTrained { get { return _unit.trained; } }
        
        private void Start()
        {
            _buttonRemove.onClick.AddListener(Remove);
        }

        public void Initialize(Data.Unit unit)
        {
            _unit = unit;
            float fill = _unit.trainedTime / _unit.trainTime;
            if(fill > 1f)
            {
                fill = 1f;
            }
            _bar.fillAmount = fill;
            id = _unit.id;
            Sprite icon = AssetsBank.GetUnitIcon(unit.id);
            if(icon != null)
            {
                _icon.sprite = icon;
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
            if (databaseID <= 0)
            {
                //_remove = true;
                //transform.GetChild(0).gameObject.SetActive(false);
                //transform.SetParent(null);
                return;
            }
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            Packet paket = new Packet();
            paket.Write((int)Player.RequestsID.CANCELTRAIN);
            paket.Write(_unit.databaseID);
            Sender.TCP_Send(paket);
            gameObject.SetActive(false);
        }

        public void UpdateStatus(float deltaTime)
        {
            float trainedTime = _unit.trainedTime;
            trainedTime += deltaTime;
            if (trainedTime > _unit.trainTime)
            {
                trainedTime = _unit.trainTime;
            }
            _unit.trainedTime = trainedTime;
            float fill = _unit.trainedTime / _unit.trainTime;
            _bar.fillAmount = fill;
            if (fill >= 1f && _unit.trained)
            {
                Player.instanse.RushSyncRequest();
                gameObject.SetActive(false);
            }
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using DevelopersHub.RealtimeNetworking.Client;
    using TMPro;

    public class UI_Unit : MonoBehaviour
    {

        [SerializeField] private Data.UnitID _id = Data.UnitID.barbarian; public Data.UnitID id { get { return _id; } set { _id = value; } }
        [SerializeField] private Button _button = null;
        [SerializeField] private Button _buttonInfo = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private Image _resourceIcon = null;
        [SerializeField] public TextMeshProUGUI _titleText = null;
        [SerializeField] public TextMeshProUGUI _resourceText = null;
        [SerializeField] public TextMeshProUGUI _countText = null;
        [SerializeField] public TextMeshProUGUI _housingText = null;

        private int count = 0; public int haveCount { get { return count; } set { count = value; _countText.text = "x" + count.ToString(); _countText.ForceMeshUpdate(true); } }
        private bool canTrain = false;
        private int _housing = 0; public int housing { get { return _housing; } }
        private int _housingUnit = 1;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
            _buttonInfo.onClick.AddListener(Info);
        }

        public void Initialize(Data.ServerUnit unit)
        {
            _housingUnit = unit.housing;
            _housingText.text = unit.housing.ToString();
            _titleText.text = Language.instanse.GetUnitName(_id);
            if (Language.instanse.IsRTL && _titleText.horizontalAlignment == HorizontalAlignmentOptions.Left)
            {
                _titleText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            }
            Sprite icon = AssetsBank.GetUnitIcon(_id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }
         
            int barrackLevel = 0;
            int darkBarracksLevel = 0;

            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if(Player.instanse.data.buildings[i].id == Data.BuildingID.barracks)
                {
                    barrackLevel = Player.instanse.data.buildings[i].level;
                }
                else if (Player.instanse.data.buildings[i].id == Data.BuildingID.darkbarracks)
                {
                    darkBarracksLevel = Player.instanse.data.buildings[i].level;
                }
                if(barrackLevel > 0 && darkBarracksLevel > 0)
                {
                    break;
                }
            }

            canTrain = Data.IsUnitUnlocked(_id, barrackLevel, darkBarracksLevel);

            bool haveResources = false;
            if (unit.requiredGold > 0)
            {
                _resourceText.text = unit.requiredGold.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.goldIcon;
                haveResources = (unit.requiredGold <= Player.instanse.gold);
            }
            else if (unit.requiredElixir > 0)
            {
                _resourceText.text = unit.requiredElixir.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.elixirIcon;
                haveResources = (unit.requiredElixir <= Player.instanse.elixir);
            }
            else if (unit.requiredDarkElixir > 0)
            {
                _resourceText.text = unit.requiredDarkElixir.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.darkIcon;
                haveResources = (unit.requiredDarkElixir <= Player.instanse.darkElixir);
            }
            else
            {
                _resourceText.text = unit.requiredGems.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.gemsIcon;
                haveResources = (unit.requiredGems <= Player.instanse.data.gems);
            }
            _resourceText.color = haveResources ? Color.white : Color.red;
            _button.interactable = canTrain && haveResources;
            
            _titleText.ForceMeshUpdate(true);
            _resourceText.ForceMeshUpdate(true);
            _housingText.ForceMeshUpdate(true);
        }

        private void Clicked()
        {
            UI_Train.instanse.StartTrainUnit(id);
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            Packet paket = new Packet();
            paket.Write((int)Player.RequestsID.TRAIN);
            paket.Write(_id.ToString());
            Sender.TCP_Send(paket);
        }

        public void Sync()
        {
            count = 0;
            for (int i = 0; i < Player.instanse.data.units.Count; i++)
            {
                if (Player.instanse.data.units[i].id == _id && Player.instanse.data.units[i].ready)
                {
                    count++;
                }
            }
            haveCount = count;
            _housing = _housingUnit * count;
        }

        private void Info()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            UI_Info.instanse.OpenUnitInfo(_id);
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_SpellItem : MonoBehaviour
    {

        [SerializeField] private Data.SpellID _id = Data.SpellID.healing; public Data.SpellID id { get { return _id; }set { _id = value; } }
        [SerializeField] private Button _button = null;
        [SerializeField] private Button _buttonInfo = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private Image _resourceIcon = null;
        [SerializeField] public TextMeshProUGUI _titleText = null;
        [SerializeField] public TextMeshProUGUI _resourceText = null;
        [SerializeField] public TextMeshProUGUI _countText = null;
        [SerializeField] public TextMeshProUGUI _housingText = null;

        private int count = 0; public int haveCount { get { return count; } set { count = value; _countText.text = "x" + count.ToString(); } }
        private bool canBrew = false;
        private int _housing = 0; public int housing { get { return _housing; } }
        private int _housingUnit = 1;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
            _buttonInfo.onClick.AddListener(Info);
        }

        public void Initialize(Data.ServerSpell spell)
        {
            _housingUnit = spell.housing;
            _housingText.text = spell.housing.ToString();
            _titleText.text = Language.instanse.GetSpellName(_id);
            if (Language.instanse.IsRTL && _titleText.horizontalAlignment == HorizontalAlignmentOptions.Left)
            {
                _titleText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            }
            Sprite icon = AssetsBank.GetSpellIcon(_id);
            if (icon != null)
            {
                _icon.sprite = icon;
            }

            int spellFactoryLevel = 0;
            int darkSpellFactoryLevel = 0;

            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if (Player.instanse.data.buildings[i].id == Data.BuildingID.spellfactory)
                {
                    spellFactoryLevel = Player.instanse.data.buildings[i].level;
                }
                else if (Player.instanse.data.buildings[i].id == Data.BuildingID.darkspellfactory)
                {
                    darkSpellFactoryLevel = Player.instanse.data.buildings[i].level;
                }
                if (spellFactoryLevel > 0 && darkSpellFactoryLevel > 0)
                {
                    break;
                }
            }

            canBrew = Data.IsSpellUnlocked(_id, spellFactoryLevel, darkSpellFactoryLevel);

            bool haveResources = false;
            if (spell.requiredGold > 0)
            {
                _resourceText.text = spell.requiredGold.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.goldIcon;
                haveResources = (spell.requiredGold <= Player.instanse.gold);
            }
            else if (spell.requiredElixir > 0)
            {
                _resourceText.text = spell.requiredElixir.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.elixirIcon;
                haveResources = (spell.requiredElixir <= Player.instanse.elixir);
            }
            else if (spell.requiredDarkElixir > 0)
            {
                _resourceText.text = spell.requiredDarkElixir.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.darkIcon;
                haveResources = (spell.requiredDarkElixir <= Player.instanse.darkElixir);
            }
            else
            {
                _resourceText.text = spell.requiredGems.ToString();
                _resourceIcon.sprite = AssetsBank.instanse.gemsIcon;
                haveResources = (spell.requiredGems <= Player.instanse.data.gems);
            }
            _resourceText.color = haveResources ? Color.white : Color.red;
            _button.interactable = canBrew && haveResources;
        }

        private void Clicked()
        {
            UI_Spell.instanse.StartBrewingSpell(id);
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            Packet paket = new Packet();
            paket.Write((int)Player.RequestsID.BREW);
            paket.Write(_id.ToString());
            Sender.TCP_Send(paket);
        }

        public void Sync()
        {
            count = 0;
            for (int i = 0; i < Player.instanse.data.spells.Count; i++)
            {
                if (Player.instanse.data.spells[i].id == _id && Player.instanse.data.spells[i].ready)
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
            UI_Info.instanse.OpenSpellInfo(_id);
        }

    }
}
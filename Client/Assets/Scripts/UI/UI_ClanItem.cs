namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_ClanItem : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TextMeshProUGUI _trophiesText = null;
        [SerializeField] private TextMeshProUGUI _rankText = null;
        [SerializeField] private Image _background = null;
        [SerializeField] private Image _icon = null;
        [SerializeField] private Button _button = null;

        private Data.Clan _clan = null;

        private void Start()
        {
            _button.onClick.AddListener(Select);
        }

        public void Initialize(Data.Clan clan)
        {
            _clan = clan;
            _trophiesText.text = clan.trophies.ToString();
            _rankText.text = clan.rank.ToString();
            _nameText.text = Data.DecodeString(clan.name);
            _icon.sprite = UI_Clan.instanse.patterns[clan.pattern];
            _background.color = Tools.HexToColor(clan.backgroundColor);
            _icon.color = Tools.HexToColor(clan.patternColor);
            _trophiesText.ForceMeshUpdate(true);
            _rankText.ForceMeshUpdate(true);
            _nameText.ForceMeshUpdate(true);
        }

        private void Select()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.OPENCLAN);
            long id = 0;
            packet.Write(id);
            packet.Write(_clan.id);
            Sender.TCP_Send(packet);
        }

    }
}
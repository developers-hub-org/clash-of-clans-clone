namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_WarReportItem : MonoBehaviour
    {

        [SerializeField] private Button _detailsButton = null;
        [SerializeField] private TextMeshProUGUI _result = null;
        [SerializeField] private TextMeshProUGUI _stars = null;
        [SerializeField] private Image _background = null;
        [SerializeField] private GameObject _victory = null;
        [SerializeField] private GameObject _defeate = null;
        [SerializeField] private GameObject _draw = null;

        Data.ClanWarData _data = null;

        private void Start()
        {
            _detailsButton.onClick.AddListener(Clicked);
        }

        public void Initialize(Data.ClanWarData data)
        {
            _data = data;
            _victory.SetActive(data.winnerID == Player.instanse.data.clanID);
            _defeate.SetActive(data.winnerID != Player.instanse.data.clanID);
            _draw.SetActive(data.winnerID <= 0);
            if (data.winnerID <= 0)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian: _result.text = "مساوی"; break;
                    default: _result.text = "Draw"; break;
                }
                _background.color = Color.gray;
            }
            else if (data.winnerID == Player.instanse.data.clanID)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian: _result.text = "پیروزی"; break;
                    default: _result.text = "Victory"; break;
                }
                _background.color = Color.green;
            }
            else
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian: _result.text = "شکست"; break;
                    default: _result.text = "Defeat"; break;
                }
                _background.color = Color.red;
            }
            _stars.text = data.clan1Stars.ToString() + " - " + data.clan2Stars.ToString();
            _detailsButton.gameObject.SetActive(data.hasReport);
        }

        private void Clicked()
        {
            _detailsButton.interactable = false;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.WARREPORT);
            packet.Write(_data.id);
            Sender.TCP_Send(packet);
        }

    }
}
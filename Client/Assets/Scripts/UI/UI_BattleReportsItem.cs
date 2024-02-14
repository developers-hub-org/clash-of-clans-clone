namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_BattleReportsItem : MonoBehaviour
    {

        [SerializeField] private Button _playButton = null;
        private Data.BattleReportItem _data = null;
        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TextMeshProUGUI _trophiesText = null;
        [SerializeField] private GameObject _attackIcon = null;
        [SerializeField] private GameObject _defendIcon = null;
        [SerializeField] private GameObject _backgroundVictory = null;
        [SerializeField] private GameObject _backgroundDefeate = null;

        private void Start()
        {
            _playButton.onClick.AddListener(Reply);
        }

        public void Initialize(Data.BattleReportItem data)
        {
            _data = data;
            _nameText.text = Data.DecodeString(_data.username);
            _nameText.ForceMeshUpdate(true);
            _trophiesText.text = _data.trophies.ToString();
            _trophiesText.ForceMeshUpdate(true);
            _attackIcon.SetActive(_data.attacker == Player.instanse.data.id);
            _defendIcon.SetActive(_data.defender == Player.instanse.data.id);
            if ((_data.attacker == Player.instanse.data.id && _data.stars > 0) || (_data.defender == Player.instanse.data.id && _data.stars <= 0))
            {
                _backgroundVictory.SetActive(true);
                _backgroundDefeate.SetActive(false);
            }
            else
            {
                _backgroundVictory.SetActive(false);
                _backgroundDefeate.SetActive(true);
            }
            _playButton.gameObject.SetActive(data.hasReply);
        }

        private void Reply()
        {
            _playButton.interactable = false;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.BATTLEREPORT);
            packet.Write(_data.id);
            Sender.TCP_Send(packet);
        }

    }
}
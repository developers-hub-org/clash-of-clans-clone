namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_ClanJoinRequest : MonoBehaviour
    {

        [SerializeField] private Button _acceptButton = null;
        [SerializeField] private Button _rejectButton = null;
        [SerializeField] private TextMeshProUGUI _name = null;
        [SerializeField] private TextMeshProUGUI _trophies = null;
        [SerializeField] private TextMeshProUGUI _level = null;

        private Data.JoinRequest _data = null; public long id { get { return _data.id; } }
        public static UI_ClanJoinRequest active = null;

        private void Start()
        {
            _acceptButton.onClick.AddListener(Accept);
            _rejectButton.onClick.AddListener(Reject);
        }

        public void Initialize(Data.JoinRequest data, bool havePermission)
        {
            _acceptButton.interactable = havePermission;
            _rejectButton.interactable = havePermission;
            _data = data;
            _name.text = Data.DecodeString(data.name);
            _trophies.text = data.trophies.ToString();
            _level.text = data.level.ToString();
            _name.ForceMeshUpdate(true);
            _trophies.ForceMeshUpdate(true);
            _name.ForceMeshUpdate(true);
        }

        private void Accept()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            active = this;
            _acceptButton.interactable = false;
            _rejectButton.interactable = false;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.JOINRESPONSE);
            packet.Write(_data.id);
            packet.Write(true);
            Sender.TCP_Send(packet);
        }

        private void Reject()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            active = this;
            _acceptButton.interactable = false;
            _rejectButton.interactable = false;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.JOINRESPONSE);
            packet.Write(_data.id);
            packet.Write(false);
            Sender.TCP_Send(packet);
        }

        public void Response(int response)
        {
            if(response == 1)
            {
                Destroy(gameObject);
            }
            else
            {
                _acceptButton.interactable = true;
                _rejectButton.interactable = true;
            }
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_ClanMember : MonoBehaviour
    {

        [SerializeField] private Button _kickButton = null;
        [SerializeField] private Button _PromoteButton = null;
        [SerializeField] private Button _demoteButton = null;
        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TextMeshProUGUI _trophiesText = null;
        [SerializeField] private TextMeshProUGUI _levelText = null;
        [SerializeField] private TextMeshProUGUI _rankText = null;

        private Data.ClanMember _data = null; public long id { get { return _data.id; } }

        private void Start()
        {
            _kickButton.onClick.AddListener(KickClicked);
            _PromoteButton.onClick.AddListener(PromoteClicked);
            _demoteButton.onClick.AddListener(DemoteClicked);
            GetComponent<RectTransform>().sizeDelta = new Vector2(0, Screen.height * 0.15f);
        }

        public void Initialize(Data.ClanMember member, bool haveKickPermission, bool havePromotePermossion)
        {
            _data = member;
            _nameText.text = Data.DecodeString(_data.name);
            _trophiesText.text = _data.trophies.ToString();
            _levelText.text = _data.level.ToString();
            switch ((Data.ClanRank)_data.rank)
            {
                case Data.ClanRank.member:
                    _rankText.text = "Member";
                    break;
                case Data.ClanRank.leader:
                    _rankText.text = "Leader";
                    break;
                case Data.ClanRank.coleader:
                    _rankText.text = "Co-Leader";
                    break;
                case Data.ClanRank.elder:
                    _rankText.text = "Elder";
                    break;
            }
            _nameText.ForceMeshUpdate(true);
            _trophiesText.ForceMeshUpdate(true);
            _levelText.ForceMeshUpdate(true);
            _rankText.ForceMeshUpdate(true);
            bool isInWar = false;
            bool canBeKicked = false;
            bool canPromote = false;
            bool canDemote = false;
            if (_data.rank != Player.instanse.data.clanRank && Player.instanse.data.clanRank > 0 && (_data.rank <= 0 || (_data.rank > 0 && Player.instanse.data.clanRank < _data.rank)))
            {
                canBeKicked = haveKickPermission;
                canPromote = havePromotePermossion;
                canDemote = (havePromotePermossion && _data.rank > 1 && _data.rank <= Enum.GetValues(typeof(Data.ClanRank)).Cast<int>().Max());
            }
            if (canBeKicked && member.warID >= 0)
            {
                isInWar = true;
            }
            _kickButton.gameObject.SetActive(member.id != Player.instanse.data.id && member.clanID == Player.instanse.data.clanID);
            _PromoteButton.gameObject.SetActive(member.id != Player.instanse.data.id && member.clanID == Player.instanse.data.clanID);
            _demoteButton.gameObject.SetActive(member.id != Player.instanse.data.id && member.clanID == Player.instanse.data.clanID);
            _kickButton.interactable = (!isInWar && canBeKicked);
            _PromoteButton.interactable = canPromote;
            _demoteButton.interactable = canDemote;
        }

        private void KickClicked()
        {
            MessageBox.Open(3, 0.8f, true, _KickClicked, new string[] { "Are you sure that you want to kick out " + Data.DecodeString(_data.name) + " from the clan." }, new string[] { "Yes", "No" });
        }

        private void _KickClicked(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 3)
            {
                if (buttonIndex == 0)
                {
                    _kickButton.interactable = false;
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.KICKMEMBER);
                    packet.Write(_data.id);
                    Sender.TCP_Send(packet);
                }
                MessageBox.Close();
            }
        }

        private void PromoteClicked()
        {
            string message = "";
            if (Player.instanse.data.clanRank == (int)Data.ClanRank.leader && _data.rank == (int)Data.ClanRank.leader + 1)
            {
                message = "Are you sure that you want to transfer leadership to " + Data.DecodeString(_data.name) + ".";
            }
            else
            {
                message = "Are you sure that you want to promote " + Data.DecodeString(_data.name) + "`s clan rank.";
            }
            MessageBox.Open(3, 0.8f, true, _PromoteClicked, new string[] {  message }, new string[] { "Yes", "No" });
        }

        private void _PromoteClicked(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 3)
            {
                if (buttonIndex == 0)
                {
                    _kickButton.interactable = false;
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.PROMOTEMEMBER);
                    packet.Write(_data.id);
                    Sender.TCP_Send(packet);
                }
                MessageBox.Close();
            }
        }

        private void DemoteClicked()
        {
            MessageBox.Open(3, 0.8f, true, _DemoteClicked, new string[] { "Are you sure that you want to demote " + Data.DecodeString(_data.name) + "`s clan rank." }, new string[] { "Yes", "No" });
        }

        private void _DemoteClicked(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 3)
            {
                if (buttonIndex == 0)
                {
                    _kickButton.interactable = false;
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.DEMOTEMEMBER);
                    packet.Write(_data.id);
                    Sender.TCP_Send(packet);
                }
                MessageBox.Close();
            }
        }

        public void kickResponse(int response)
        {
            if(response == 1)
            {
                Destroy(gameObject);
            }
            else
            {
                _kickButton.interactable = true;
            }
        }

        public void PromoteResponse(int response)
        {
           
        }

        public void DemoteResponse(int response)
        {

        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using System;
    using DevelopersHub.RealtimeNetworking.Client;

    public class UI_ChatItem : MonoBehaviour
    {

        [SerializeField] private GameObject _options = null;
        [SerializeField] private TextMeshProUGUI _nameText = null;
        [SerializeField] private TextMeshProUGUI _messageText = null;
        [SerializeField] private TextMeshProUGUI _timeText = null;
        [SerializeField] private Button _buttonOptions = null;
        [SerializeField] private Button _buttonReport = null;
        [SerializeField] private Button _buttonCancel = null;
        [SerializeField] private Image _background = null;
        [SerializeField] private Color _yourColor;
        [SerializeField] private Color _othersColor;
        [SerializeField] private Color _allyColor;

        private Data.CharMessage _data = null; public long id { get { return _data.id; } }
        private bool options = false;

        private void Awake()
        {
            options = false;
            _options.SetActive(false);
        }

        private void Start()
        {
            _buttonOptions.onClick.AddListener(OptionsClicked);
            _buttonReport.onClick.AddListener(ReportClicked);
            _buttonCancel.onClick.AddListener(CancelClicked);
        }

        private void OptionsClicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            options = !options;
            _options.SetActive(options);
            UI_Chat.instanse.RefreshList();
        }

        private void ReportClicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "گزارش سوء استفاده یا فحاشی؟" }, new string[] { "بله", "خیر" });
                    break;
                default:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "Report abuse or profanity?" }, new string[] { "Yes", "No" });
                    break;
            }
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 3)
            {
                if (buttonIndex == 0)
                {
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.REPORTCHAT);
                    packet.Write(_data.id);
                    Sender.TCP_Send(packet);
                    CancelClicked();
                }
                MessageBox.Close();
            }
        }

        private void CancelClicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            options = false;
            _options.SetActive(false);
            UI_Chat.instanse.RefreshList();
        }

        public void Inirialize(Data.CharMessage data)
        {
            _data = data;
            if(data.accountID == Player.instanse.data.id)
            {
                _background.color = _yourColor;
            }
            else
            {
                _background.color = _othersColor;
            }
            _nameText.text = Data.DecodeString(_data.name);
            _nameText.ForceMeshUpdate(true);
            _messageText.text = Data.DecodeString(_data.message);
            _messageText.ForceMeshUpdate(true);
            DateTime time = Player.instanse.data.nowTime;
            DateTime.TryParse(_data.time, out time);
            _timeText.text = time.ToString();
            _timeText.ForceMeshUpdate(true);
        }

        public void UpdareColor()
        {
            if (_data.accountID == Player.instanse.data.id)
            {
                _background.color = _yourColor;
            }
            else if (_data.clanID > 0 && _data.clanID == Player.instanse.data.clanID)
            {
                _background.color = _allyColor;
            }
            else
            {
                _background.color = _othersColor;
            }
        }

    }
}
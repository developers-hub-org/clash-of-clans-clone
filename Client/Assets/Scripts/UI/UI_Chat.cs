namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UI_Chat : MonoBehaviour
    {

        [SerializeField] private bool _globalLock = false;
        [SerializeField] private GameObject _elements = null;
        [SerializeField] private RectTransform _panel = null;
        // [SerializeField] private Button _buttonOpen = null; in ui main
        [SerializeField] private Button _buttonClose = null;
        [SerializeField] private Button _buttonSend = null;
        [SerializeField] private Button _buttonClan = null;
        [SerializeField] private Button _buttonGlobal = null;
        [SerializeField] private TMP_InputField _inputMessage = null;
        [SerializeField] private UI_ChatItem _chatPrefab = null;
        [SerializeField] private RectTransform _chatGridClan = null;
        [SerializeField] private RectTransform _chatGridGlobal = null;
        [SerializeField] public TextMeshProUGUI _globalLockText = null;
        //[SerializeField] private GameObject _loadingPrefab = null;

        private static UI_Chat _instance = null; public static UI_Chat instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }

        private List<UI_ChatItem> clanChats = new List<UI_ChatItem>();
        private List<UI_ChatItem> globalChats = new List<UI_ChatItem>();

        private bool updating = false;
        private float timer = 0;
        private Data.ChatType type = Data.ChatType.global;
        private Vector2 closePosition = Vector2.zero;
        private Vector2 openPosition = Vector2.zero;
        private float transitionDuration = 0.5f;
        private float transitionTimer = 0.5f;
        // private GameObject loading = null;
        private bool sending = false;

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _panel.anchorMax = Vector2.zero;
            _panel.anchorMin = Vector2.zero;
            _panel.pivot = Vector2.zero;
            _panel.sizeDelta = new Vector2(Screen.width * 0.25f, Screen.height);
            closePosition = new Vector2(-_panel.sizeDelta.x, 0);
            openPosition = Vector2.zero;
            _panel.anchoredPosition = closePosition;
            _buttonSend.onClick.AddListener(Send);
            _buttonClose.onClick.AddListener(Close);
            _buttonClan.onClick.AddListener(Clan);
            _buttonGlobal.onClick.AddListener(Global);
        }

        private void Update()
        {
            if (_active)
            {
                if(timer < Data.chatSyncPeriod)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    if (!updating)
                    {
                        Sync();
                    }
                }
            }
        }

        public void ChatSynced(List<Data.CharMessage> messages, Data.ChatType chatType)
        {
            if (!_active) { return; }
            for (int i = 0; i < messages.Count; i++)
            {
                UI_ChatItem chat = Instantiate(_chatPrefab, messages[i].type == Data.ChatType.clan ? _chatGridClan : _chatGridGlobal);
                LayoutRebuilder.ForceRebuildLayoutImmediate(messages[i].type == Data.ChatType.clan ? _chatGridClan : _chatGridGlobal);
                chat.Inirialize(messages[i]);
                if (messages[i].type == Data.ChatType.clan)
                {
                    clanChats.Add(chat);
                }
                else
                {
                    globalChats.Add(chat);
                }
            }
            if(clanChats.Count > Data.clanChatArchiveMaxMessages)
            {
                for (int i = 0; i < clanChats.Count - Data.clanChatArchiveMaxMessages; i++)
                {
                    Destroy(clanChats[i].gameObject);
                }
            }
            if (globalChats.Count > Data.globalChatArchiveMaxMessages)
            {
                for (int i = 0; i < globalChats.Count - Data.globalChatArchiveMaxMessages; i++)
                {
                    Destroy(globalChats[i].gameObject);
                }
            }
            /*
            if(!sending && loading != null)
            {
                Destroy(loading);
                loading = null;
            }
            */
            updating = false;
        }

        private void Send()
        {
            string message = _inputMessage.text.Trim().Replace("'", "''");
            if (!string.IsNullOrEmpty(message))
            {
                SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
                message = Data.EncodeString(message);
                sending = true;
                _inputMessage.interactable = false;
                _buttonSend.interactable = false;
                Packet packet = new Packet();
                packet.Write((int)Player.RequestsID.SENDCHAT);
                packet.Write(message);
                packet.Write((int)type);
                long target = 0;
                if (type == Data.ChatType.clan)
                {
                    target = Player.instanse.data.clanID;
                }
                else
                {
                    target = 0;
                }
                packet.Write(target);
                Sender.TCP_Send(packet);
            }
        }

        public void ChatSendResponse(int response)
        {
            if(response == 2)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "دسترسی شما برای ارسال پیام مسدود شده است." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "You are banned from sending message." }, new string[] { "OK" });
                        break;
                }
            }
            sending = false;
            _inputMessage.text = "";
            _inputMessage.interactable = true;
            if (type == Data.ChatType.global && _globalLock)
            {
                _buttonSend.interactable = false;
            }
            else
            {
                _buttonSend.interactable = true;
            }
        }

        private void Clan()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            type = Data.ChatType.clan;
            _chatGridGlobal.gameObject.SetActive(false);
            _chatGridClan.gameObject.SetActive(true);
            _buttonClan.interactable = false;
            _buttonGlobal.interactable = true;
            _globalLockText.gameObject.SetActive(false);
            _buttonSend.interactable = true;
            AddLoading();
            Sync();
        }

        private void Global()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            type = Data.ChatType.global;
            _chatGridGlobal.gameObject.SetActive(true);
            _chatGridClan.gameObject.SetActive(false);
            _buttonClan.interactable = true;
            _buttonGlobal.interactable = false;
            _globalLockText.gameObject.SetActive(_globalLock);
            _buttonSend.interactable = !_globalLock;
            AddLoading();
            Sync();
        }

        private void AddLoading()
        {/*
            if(loading != null)
            {
                Destroy(loading);
                loading = null;
            }
            loading = Instantiate(_loadingPrefab, type == Data.ChatType.clan ? _chatGridClan : _chatGridGlobal);
            */
        }

        private void Sync()
        {
            timer = 0;
            updating = true;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.GETCHATS);
            packet.Write((int)type);
            long lastMessage = 0;
            if(type == Data.ChatType.clan)
            {
                if(clanChats.Count > 0)
                {
                    lastMessage = clanChats[clanChats.Count - 1].id;
                }
            }
            else
            {
                if (globalChats.Count > 0)
                {
                    lastMessage = globalChats[globalChats.Count - 1].id;
                }
            }
            packet.Write(lastMessage);
            Sender.TCP_Send(packet);
        }

        public void Open()
        {
            sending = false;
            _chatGridGlobal.gameObject.SetActive(false);
            _chatGridClan.gameObject.SetActive(false);
            _buttonClan.interactable = false;
            _buttonGlobal.interactable = false;
            _elements.SetActive(true);
            if (Player.instanse.data.clanID > 0)
            {
                _globalLockText.gameObject.SetActive(false);
                type = Data.ChatType.clan;
            }
            else
            {
                _globalLockText.gameObject.SetActive(_globalLock);
                type = Data.ChatType.global;
            }
            StartCoroutine(_Open());
        }

        private IEnumerator _Open()
        {
            transitionTimer = 0;
            while (_panel.anchoredPosition != openPosition)
            {
                transitionTimer += Time.deltaTime; if (transitionTimer > transitionDuration) { transitionTimer = transitionDuration; }
                _panel.anchoredPosition = Vector2.Lerp(closePosition, openPosition, transitionTimer / transitionDuration);
                yield return null;
            }
            if (type == Data.ChatType.clan)
            {
                _buttonGlobal.interactable = true;
                _chatGridClan.gameObject.SetActive(true);
            }
            else
            {
                _buttonClan.interactable = (Player.instanse.data.clanID > 0);
                _chatGridGlobal.gameObject.SetActive(true);
            }
            _inputMessage.interactable = true;
            if (type == Data.ChatType.global && _globalLock)
            {
                _buttonSend.interactable = false;
            }
            else
            {
                _buttonSend.interactable = true;
            }
            _active = true;
            Sync();
        }

        private bool _closing = false;

        public void Close()
        {
            if (_closing)
            {
                return;
            }
            _closing = true;
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _active = false;
            _chatGridGlobal.gameObject.SetActive(false);
            _chatGridClan.gameObject.SetActive(false);
            _inputMessage.interactable = false;
            _buttonSend.interactable = false;
            StartCoroutine(_Close());
        }

        private IEnumerator _Close()
        {
            transitionTimer -= transitionDuration;
            while (_panel.anchoredPosition != closePosition)
            {
                transitionTimer += Time.deltaTime; if (transitionTimer > transitionDuration) { transitionTimer = transitionDuration; }
                _panel.anchoredPosition = Vector2.Lerp(openPosition, closePosition, transitionTimer / transitionDuration);
                yield return null;
            }
            _elements.SetActive(false);
            _closing = false;
        }

        public void ReportResult(int response)
        {
            if(response == 1)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "گزارش شما ثبت شد. این مورد توسط کارشناس ما بررسی میشود و در صورت مشاهده تخلف، با کاربر مورد نظر برخورد میگردد." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Report submited successfully. This case will be checked by us and if any violation is observed, the intended user will be dealt with." }, new string[] { "OK" });
                        break;
                }
            }
            else if (response == 2)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "تعداد گزارش های شما در 24 ساعت گذشته به حد مجاز رسیده است." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Your number of reports in the last 24 hours has reached it's limit." }, new string[] { "OK" });
                        break;
                }
            }
        }

        public void RefreshList()
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(type == Data.ChatType.clan ? _chatGridClan : _chatGridGlobal);
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 1)
            {
                MessageBox.Close();
            }
        }

    }
}
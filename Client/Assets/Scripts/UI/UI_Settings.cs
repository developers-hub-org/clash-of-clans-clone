namespace DevelopersHub.ClashOfWhatecer 
{ 
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DevelopersHub.RealtimeNetworking.Client;

    public class UI_Settings : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private Button _editButton = null;
        [SerializeField] private Button _privacyButton = null;
        [SerializeField] private Button _cancelButton = null;
        [SerializeField] private Button _saveButton = null;
        [SerializeField] private Button _logoutButton = null;
        [SerializeField] private Button _languageButton = null;
         [SerializeField] private Button _renameButton = null;
        [SerializeField] private Image _languageIcon = null;
        [SerializeField] private TextMeshProUGUI _languageText = null;
        [SerializeField] private TMP_InputField _emailInput = null;
        [SerializeField] private Button _musicButton = null;
        [SerializeField] private Button _soundButton = null;
        [SerializeField] private Image _musicMute = null;
        [SerializeField] private Image _musicUnmute = null;
        [SerializeField] private Image _soundMute = null;
        [SerializeField] private Image _soundUnmute = null;

        private static UI_Settings _instance = null; public static UI_Settings instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }
        private string email = "";

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _editButton.onClick.AddListener(EditEmail);
            _cancelButton.onClick.AddListener(CancelEmail);
            _saveButton.onClick.AddListener(SaveEmail);
            _logoutButton.onClick.AddListener(LogOut);
            _languageButton.onClick.AddListener(SwitchLanguage);
            _musicButton.onClick.AddListener(MusicMute);
            _soundButton.onClick.AddListener(SoundMute);
            _privacyButton.onClick.AddListener(PrivacyPolicyClicked);
            _renameButton.onClick.AddListener(RenameClicked);
        }

        private void RenameClicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    MessageBox.Open(4, 0.8f, true, MessageResponded, new string[] { "نام را وارد کنید." }, new string[] { "تأیید", "لغو" }, null, new string[] { "" });
                    break;
                default:
                    MessageBox.Open(4, 0.8f, true, MessageResponded, new string[] { "Enter the name." }, new string[] { "Confirm", "Cancel" }, null, new string[] { "" });
                    break;
            }
        }

        private void PrivacyPolicyClicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            UI_PrivacyPolicy.instanse.Open();
        }

        private void EditEmail()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _emailInput.interactable = true;
            _saveButton.gameObject.SetActive(true);
            _editButton.gameObject.SetActive(false);
            _cancelButton.gameObject.SetActive(true);
        }

        public void Open()
        {
            _languageIcon.sprite = AssetsBank.GetLanguageIcon(Language.instanse.language);
            _languageText.text = Language.instanse.GetLanguageName(Language.instanse.language);
            if (Language.instanse.IsRTL)
            {
                _languageText.horizontalAlignment = HorizontalAlignmentOptions.Right;
            }
            else
            {
                _languageText.horizontalAlignment = HorizontalAlignmentOptions.Left;
            }
            _saveButton.interactable = true;
            _cancelButton.interactable = true;
            _saveButton.gameObject.SetActive(false);
            _editButton.gameObject.SetActive(true);
            _cancelButton.gameObject.SetActive(false);
            _emailInput.text = Player.instanse.data.email;
            _emailInput.interactable = false;

            UpdateSoundButtons();

            _active = true;
            _elements.SetActive(true);
            _languageText.ForceMeshUpdate(true);
        }

        private void UpdateSoundButtons()
        {
            try
            {
                if (PlayerPrefs.HasKey("music_mute"))
                {
                    _musicMute.gameObject.SetActive(PlayerPrefs.GetInt("music_mute") == 1);
                    _musicUnmute.gameObject.SetActive(PlayerPrefs.GetInt("music_mute") != 1);
                }
                else
                {
                    _musicMute.gameObject.SetActive(false);
                    _musicUnmute.gameObject.SetActive(true);
                }
                if (PlayerPrefs.HasKey("sound_mute"))
                {
                    _soundMute.gameObject.SetActive(PlayerPrefs.GetInt("sound_mute") == 1);
                    _soundUnmute.gameObject.SetActive(PlayerPrefs.GetInt("sound_mute") != 1);
                }
                else
                {
                    _soundMute.gameObject.SetActive(false);
                    _soundUnmute.gameObject.SetActive(true);
                }
            }
            catch (System.Exception)
            {

            }
        }

        private void SoundMute()
        {
            try
            {
                int status = 0;
                if (PlayerPrefs.HasKey("sound_mute"))
                {
                    status = PlayerPrefs.GetInt("sound_mute");
                }
                if(status == 1)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                PlayerPrefs.SetInt("sound_mute", status);
                SoundManager.instanse.soundMute = (status == 1);
            }
            catch (System.Exception)
            {

            }
            UpdateSoundButtons();
        }

        private void MusicMute()
        {
            try
            {
                int status = 0;
                if (PlayerPrefs.HasKey("music_mute"))
                {
                    status = PlayerPrefs.GetInt("music_mute");
                }
                if (status == 1)
                {
                    status = 0;
                }
                else
                {
                    status = 1;
                }
                PlayerPrefs.SetInt("music_mute", status);
                SoundManager.instanse.musicMute = (status == 1);
            }
            catch (System.Exception)
            {

            }
            UpdateSoundButtons();
        }

        public void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _active = false;
            _elements.SetActive(false);
        }

        private void SwitchLanguage()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            LanguageSwitch.instanse.Open();
        }

        private void CancelEmail()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _saveButton.gameObject.SetActive(false);
            _editButton.gameObject.SetActive(true);
            _cancelButton.gameObject.SetActive(false);
            _emailInput.text = Player.instanse.data.email;
            _emailInput.interactable = false;
        }

        private void SaveEmail()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            email = _emailInput.text.Trim();
            if (!string.IsNullOrEmpty(email) && email != Player.instanse.data.email)
            {
                Loading.Open();
                _saveButton.interactable = false;
                _cancelButton.interactable = false;
                Packet packet = new Packet();
                packet.Write((int)Player.RequestsID.EMAILCODE);
                string device = SystemInfo.deviceUniqueIdentifier;
                packet.Write(device);
                packet.Write(email);
                Sender.TCP_Send(packet);
            }
        }

        public void EmailSendResponse(int response, int expiration)
        {
            Loading.Close();
            if (response == 1)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(2, 0.8f, true, MessageResponded, new string[] { "لطفاً کد تأیید را وارد کنید. این کد در صندوق دریافت و یا اسپم ایمیل شما موجود است." }, new string[] { "تأیید", "لغو" }, null, new string[] { "" });
                        break;
                    default:
                        MessageBox.Open(2, 0.8f, true, MessageResponded, new string[] { "Please enter the confirmation code. You can find it in your email address inbox or spams." }, new string[] { "Confirm", "Cancel" }, null, new string[] { "" });
                        break;
                }
            }
            else if(response == 3)
            {
                _cancelButton.interactable = true;
                _saveButton.interactable = true;
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "ایمیل به یک حساب دیگر متصل است." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Email is in sync with another account." }, new string[] { "OK" });
                        break;
                }
            }
            else
            {
                _cancelButton.interactable = true;
                _saveButton.interactable = true;
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "کد معتبر نمی باشد." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Code is not valid." }, new string[] { "OK" });
                        break;
                }
            }
        }

        public void EmailConfirmResponse(int response, string password)
        {
            if (response == 1)
            {
                Player.instanse.data.email = email;
                Open();
            }
            else if (response == 3)
            {
                _cancelButton.interactable = true;
                _saveButton.interactable = true;
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "ایمیل به یک حساب دیگر متصل است." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Email is in sync with another account." }, new string[] { "OK" });
                        break;
                }
            }
            else
            {
                _cancelButton.interactable = true;
                _saveButton.interactable = true;
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "کد معتبر نمی باشد." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Code is not valid." }, new string[] { "OK" });
                        break;
                }
            }
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 1)
            {
                MessageBox.Close();
            }
            else if(layoutIndex == 2)
            {
                if (buttonIndex == 0)
                {
                    string code = MessageBox.GetInputValue(2, 0).Trim();
                    if (!string.IsNullOrEmpty(code))
                    {
                        MessageBox.Close();
                        Packet packet = new Packet();
                        packet.Write((int)Player.RequestsID.EMAILCONFIRM);
                        string device = SystemInfo.deviceUniqueIdentifier;
                        packet.Write(device);
                        packet.Write(email);
                        packet.Write(code);
                        Sender.TCP_Send(packet);
                    }
                }
                else
                {
                    _cancelButton.interactable = true;
                    _saveButton.interactable = true;
                    MessageBox.Close();
                }
            }
            else if (layoutIndex == 3)
            {
                if (buttonIndex == 0)
                {
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.LOGOUT);
                    string device = SystemInfo.deviceUniqueIdentifier;
                    packet.Write(device);
                    Sender.TCP_Send(packet);
                    PlayerPrefs.DeleteAll();
                    Player.RestartGame();
                }
                MessageBox.Close();
            }
            else if (layoutIndex == 4)
            {
                if (buttonIndex == 0)
                {
                    string str = MessageBox.GetInputValue(layoutIndex, 0).Trim();
                    if(str.Length > 20)
                    {
                        switch (Language.instanse.language)
                        {
                            case Language.LanguageID.persian:
                                MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "نام نباید از 20 کاراکتر بیشتر باشد." }, new string[] { "باشه" });
                                break;
                            default:
                                MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Name length is longer that 20 characters." }, new string[] { "OK" });
                                break;
                        }
                    }
                    else if (!string.IsNullOrEmpty(str) && Data.IsMessageGoodToSend(str))
                    {
                        MessageBox.Close();
                        Packet packet = new Packet();
                        packet.Write((int)Player.RequestsID.RENAME);
                        packet.Write(Data.EncodeString(str));
                        Sender.TCP_Send(packet);
                    }
                    else
                    {
                        switch (Language.instanse.language)
                        {
                            case Language.LanguageID.persian:
                                MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "نام مورد نظر قابل قبول نیست." }, new string[] { "باشه" });
                                break;
                            default:
                                MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Name is not valid." }, new string[] { "OK" });
                                break;
                        }
                    }
                }
                else
                {
                    MessageBox.Close();
                }
            }
        }

        private void LogOut()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "از حساب کاربری خارج میشوید؟ در صورتی که حساب را به ایمیل متصل نکرده باشید، تمام اطلاعات خود را از دست میدهید." }, new string[] { "خروج", "لغو" });
                    break;
                default:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "Log out of your account? If you haven't binded your account to an email then all your progress will be lost." }, new string[] { "Log Out", "Cancel" });
                    break;
            }
        }

    } 
}
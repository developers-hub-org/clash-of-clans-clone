namespace DevelopersHub.ClashOfWhatecer 
{ 
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;
    using DevelopersHub.RealtimeNetworking.Client;
    using UnityEngine.SceneManagement;
    using System;

    public class StartLoading : MonoBehaviour
    {

        [Header("Loading")]
        [SerializeField] private int gameSceneIndex = 1;
        [SerializeField] private Image progressBar = null;
        // [SerializeField] private TextMeshProUGUI progressText = null;

        [Header("Auth")]
        [SerializeField] private GameObject _authPanel = null;
        [SerializeField] private GameObject _termsPanel = null;
        [SerializeField] private GameObject _loadingPanel = null;
        [SerializeField] private Button _buttonCreate = null;
        [SerializeField] private Button _buttonRecover = null;
        [SerializeField] private Button _buttonConfirm = null;
        [SerializeField] private Button _buttonCancel = null;
        [SerializeField] private Button _buttonAccept = null;
        [SerializeField] private Button _buttonReject = null;
        [SerializeField] private TMP_InputField _inputName = null;
        [SerializeField] private TMP_InputField _inputEmail = null;
        [SerializeField] private TMP_InputField _inputCode = null;
        [SerializeField] private TextMeshProUGUI _textLog = null;

        private float minLoadTime = 2f;
        private float realLoadPortion = 0.8f;
        //private float dotsTimer = 0;
        //private float dotsSpeed = 0.5f;
        private bool waitingForCode = false;
        private bool creatingNew = false;
        private DateTime _baseTime;
        private DateTime _expireTime;
        private string email = "";
        private static StartLoading _instance = null; public static StartLoading instanse { get { return _instance; } }

        private void Awake()
        {
            _instance = this;
        }

        private void Start()
        {
            _loadingPanel.gameObject.SetActive(false);
            _authPanel.gameObject.SetActive(false);
            _termsPanel.gameObject.SetActive(false);
            progressBar.fillAmount = 0;
            _buttonCreate.onClick.AddListener(CreateNew);
            _buttonRecover.onClick.AddListener(RecoverAccount);
            _buttonConfirm.onClick.AddListener(RecoverConfirm);
            _buttonCancel.onClick.AddListener(RecoverCancel);
            _buttonAccept.onClick.AddListener(AcceptTerms);
            _buttonReject.onClick.AddListener(RejectTerms);
            RealtimeNetworking.OnPacketReceived += ReceivedPaket;
            RealtimeNetworking.OnConnectingToServerResult += ConnectionResponse;
            RealtimeNetworking.Connect();
        }

        private string createName = "";

        private void AcceptTerms()
        {
            _inputName.interactable = false;
            _buttonConfirm.interactable = false;
            _buttonCancel.interactable = false;
            PlayerPrefs.SetString(Player.username_key, Data.EncodeString(createName));
            Authenticate();
            _termsPanel.gameObject.SetActive(false);
        }

        private void RejectTerms()
        {
            _termsPanel.gameObject.SetActive(false);
        }

        private void ReceivedPaket(Packet packet)
        {
            try
            {
                int id = packet.ReadInt();
                if (id == (int)Player.RequestsID.SENDCODE)
                {
                    int response = packet.ReadInt();
                    int exp = packet.ReadInt();
                    RecoverSendResponse(response, exp);
                }
                else if (id == (int)Player.RequestsID.CONFIRMCODE)
                {
                    int response = packet.ReadInt();
                    string pass = packet.ReadString();
                    RecoverConfirmResponse(response, pass);
                }
            }
            catch (Exception)
            {

            }
        }

        private void OnDestroy()
        {
            RealtimeNetworking.OnPacketReceived -= ReceivedPaket;
        }

        private void CreateNew()
        {
            creatingNew = true;
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    _textLog.text = "یک نام برای حساب جدید خود وارد کنید:";
                    break;
                default:
                    _textLog.text = "Enter a name for your new account:";
                    break;
            }
            _inputName.interactable = true;
            _buttonConfirm.interactable = true;
            _buttonCancel.interactable = true;
            HideAll();
            _inputName.gameObject.SetActive(true);
            _buttonConfirm.gameObject.SetActive(true);
            _buttonCancel.gameObject.SetActive(true);
        }

        private void HideAll()
        {
            _buttonCreate.gameObject.SetActive(false);
            _buttonRecover.gameObject.SetActive(false);
            _buttonConfirm.gameObject.SetActive(false);
            _buttonCancel.gameObject.SetActive(false);
            _inputEmail.gameObject.SetActive(false);
            _inputCode.gameObject.SetActive(false);
            _inputName.gameObject.SetActive(false);
        }

        private void Authenticate()
        {
            _loadingPanel.gameObject.SetActive(true);
            _authPanel.gameObject.SetActive(false);
            StartCoroutine(LoadGame());
        }

        private void RecoveryPanel()
        {
            waitingForCode = false;
            creatingNew = false;
            _textLog.text = "";
            HideAll();
            _buttonCreate.gameObject.SetActive(true);
            _buttonRecover.gameObject.SetActive(true);
            _authPanel.gameObject.SetActive(true);
            _loadingPanel.gameObject.SetActive(false);
        }

        private void ConfirmCreate()
        {
            createName = _inputName.text.Trim();
            if (!string.IsNullOrEmpty(createName))
            {
                if(createName.Length > 20)
                {
                    switch (Language.instanse.language)
                    {
                        case Language.LanguageID.persian:
                            _textLog.text = "نام نباید از 20 کاراکتر بیشتر باشد.";
                            break;
                        default:
                            _textLog.text = "Name length is longer that 20 characters.";
                            break;
                    }
                }
                else if (Data.IsMessageGoodToSend(createName))
                {
                    _termsPanel.gameObject.SetActive(true);
                }
                else
                {
                    switch (Language.instanse.language)
                    {
                        case Language.LanguageID.persian:
                            _textLog.text = "این نام قابل قبول نیست.";
                            break;
                        default:
                            _textLog.text = "Name is not acceptable.";
                            break;
                    }
                }
            }
            else
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        _textLog.text = "این نام معتبر نیست.";
                        break;
                    default:
                        _textLog.text = "Name is not valid.";
                        break;
                }
            }
        }

        private void RecoverAccount()
        {
            _inputEmail.interactable = true;
            _buttonConfirm.interactable = true;
            _buttonCancel.interactable = true;
            waitingForCode = false;
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    _textLog.text = "لطفاً ایمیل متصل به حساب خود را وارد کنید:";
                    break;
                default:
                    _textLog.text = "Please enter your account recovery email.";
                    break;
            }
            HideAll();
            _buttonConfirm.gameObject.SetActive(true);
            _buttonCancel.gameObject.SetActive(true);
            _inputEmail.gameObject.SetActive(true);
            _inputEmail.text = "";
        }

        private void RecoverConfirm()
        {
            if (creatingNew)
            {
                ConfirmCreate();
            }
            else if (waitingForCode)
            {
                string code = _inputCode.text.Trim();
                if (!string.IsNullOrEmpty(code))
                {
                    _inputCode.interactable = false;
                    _buttonConfirm.interactable = false;
                    _buttonCancel.interactable = false;
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.CONFIRMCODE);
                    string device = SystemInfo.deviceUniqueIdentifier;
                    packet.Write(device);
                    packet.Write(email);
                    packet.Write(code);
                    Sender.TCP_Send(packet);
                }
            }
            else
            {
                email = _inputEmail.text.Trim();
                if (!string.IsNullOrEmpty(email) && Data.IsEmailValid(email))
                {
                    _inputEmail.interactable = false;
                    _buttonConfirm.interactable = false;
                    _buttonCancel.interactable = false;
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.SENDCODE);
                    string device = SystemInfo.deviceUniqueIdentifier;
                    packet.Write(device);
                    packet.Write(email);
                    Debug.Log(email);
                    Sender.TCP_Send(packet);
                }
                else
                {
                    switch (Language.instanse.language)
                    {
                        case Language.LanguageID.persian:
                            _textLog.text = "این ایمیل معتبر نیست.";
                            break;
                        default:
                            _textLog.text = "Email is not valid.";
                            break;
                    }
                }
            }
        }

        private void RecoverSendResponse(int response, int expiration)
        {
            if (response == 1)
            {
                _baseTime = DateTime.Now;
                _expireTime = DateTime.Now.AddSeconds(expiration);
                waitingForCode = true;
                _inputEmail.gameObject.SetActive(false);
                _inputCode.gameObject.SetActive(true);
                _inputCode.text = "";
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        _textLog.text = "کد ارسال شده به ایمیل خود را وارد کنید:";
                        break;
                    default:
                        _textLog.text = "Please enter the code that has been sent to your email:";
                        break;
                }
            }
            else
            {
                _inputEmail.interactable = true;
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        _textLog.text = "ارسال کد با خطا مواجه شد.";
                        break;
                    default:
                        _textLog.text = "Failed to send the code.";
                        break;
                }
            }
            _buttonConfirm.interactable = true;
            _buttonCancel.interactable = true;
        }

        private void RecoverConfirmResponse(int response, string password)
        {
            if (response == 1)
            {
                PlayerPrefs.SetString(Player.password_key, password);
                Authenticate();
            }
            else
            {
                _inputCode.interactable = true;
                _buttonConfirm.interactable = true;
                _buttonCancel.interactable = true;
            }
        }

        private void RecoverCancel()
        {
            if (creatingNew)
            {
                RecoveryPanel();
            }
            else if (waitingForCode)
            {
                RecoverAccount();
            }
            else
            {
                RecoveryPanel();
            }
        }

        private void Update()
        {
            if (!waitingForCode) { return; }
            int time = Mathf.FloorToInt((float)(_expireTime - _baseTime).TotalSeconds);
            if (time <= 0)
            {
                _inputCode.interactable = false;
                _buttonConfirm.interactable = false;
                _buttonCancel.interactable = true;
                waitingForCode = false;
                _textLog.text = "Time expired.";
            }
        }

        private IEnumerator LoadGame()
        {
            float loadingTimer = Time.realtimeSinceStartup;
            yield return new WaitForEndOfFrame();
            bool done = false;
            AsyncOperation async = SceneManager.LoadSceneAsync(gameSceneIndex);
            async.allowSceneActivation = false;
            while (!async.isDone && !done)
            {
                float progress = Mathf.Clamp01(async.progress / 0.9f) * realLoadPortion;
                progressBar.fillAmount = progress;
                // progressText.text = progress * 100f + "%";
                if (async.progress >= 0.9f)
                {
                    done = true;
                }
                yield return null;
            }
            float remained = minLoadTime - (Time.realtimeSinceStartup - loadingTimer);
            while (remained > 0)
            {
                float progress = realLoadPortion + ((1f - realLoadPortion) * (1f - (remained / minLoadTime)));
                progressBar.fillAmount = progress;
                remained -= Time.deltaTime;
                yield return null;
            }
            progressBar.fillAmount = 1;
            async.allowSceneActivation = true;
        }

        private void ConnectionResponse(bool successful)
        {
            RealtimeNetworking.OnConnectingToServerResult -= ConnectionResponse;
            if (successful)
            {
                if (PlayerPrefs.HasKey(Player.password_key))
                {
                    Authenticate();
                }
                else
                {
                    RecoveryPanel();
                }
            }
            else
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(0, 0.8f, false, MessageResponded, new string[] { "اتصال به سرور برقرار نشد. لطفاً اینترنت خود را چک و مجدداً تلاش کنید." }, new string[] { "تلاش مجدد" });
                        break;
                    default:
                        MessageBox.Open(0, 0.8f, false, MessageResponded, new string[] { "Failed to connect to server. Please check you internet connection and try again." }, new string[] { "Try Again" });
                        break;
                }
            }
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 0)
            {
                SceneManager.LoadScene(0);
            }
        }

    }
}
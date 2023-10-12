namespace DevelopersHub.ClashOfWhatecer
{
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Store : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] public RectTransform _itemsGrid = null;
        [SerializeField] public TextMeshProUGUI _goldText = null;
        [SerializeField] public TextMeshProUGUI _elixirText = null;
        [SerializeField] public TextMeshProUGUI _darkText = null;
        [SerializeField] public TextMeshProUGUI _gemsText = null;
        [SerializeField] private UI_GemPack[] gemPacks = null;
        [SerializeField] private UI_ShieldPack[] shieldPacks = null;
        [SerializeField] private UI_ResourcePack[] resourcePacks = null;
        private bool _active = false; public bool isActive { get { return _active; } }
        private static UI_Store _instance = null; public static UI_Store instanse { get { return _instance; } }
        private bool waitingSync = false;

        private void Awake()
        {
            for (int i = 0; i < gemPacks.Length; i++)
            {
                gemPacks[i].SetStatus(false);
            }
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
        }

        public void Open(int pack)
        {
            waitingSync = false;
            for (int i = 0; i < gemPacks.Length; i++)
            {
                gemPacks[i].gameObject.SetActive(pack == 1);
            }
            for (int i = 0; i < shieldPacks.Length; i++)
            {
                shieldPacks[i].gameObject.SetActive(pack == 2);
            }
            for (int i = 0; i < resourcePacks.Length; i++)
            {
                resourcePacks[i].Initialize();
                resourcePacks[i].gameObject.SetActive(pack == 3);
            }
            Sync();
            _itemsGrid.anchoredPosition = new Vector2(0, _itemsGrid.anchoredPosition.y);
            transform.SetAsLastSibling();
            _active = true;
            _elements.SetActive(true);
            if (pack == 1)
            {
                // ToDo: Initialize IAP Service
            }
        }

        public void Sync()
        {
            _goldText.text = Player.instanse.gold.ToString();
            _elixirText.text = Player.instanse.elixir.ToString();
            _darkText.text = Player.instanse.darkElixir.ToString();
            _gemsText.text = Player.instanse.data.gems.ToString();
            if (waitingSync)
            {
                waitingSync = false;
                for (int i = 0; i < resourcePacks.Length; i++)
                {
                    resourcePacks[i].Initialize();
                }
            }
        }

        public void ServiceStarted()
        {
            for (int i = 0; i < gemPacks.Length; i++)
            {
                gemPacks[i].SetStatus(true);
            }
        }

        public int GetGemPackNumber(string product)
        {
            for (int i = 0; i < gemPacks.Length; i++)
            {
                if (gemPacks[i].id == product)
                {
                    return gemPacks[i].pack;
                }
            }
            return 0;
        }

        public void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _active = false;
            _elements.SetActive(false);
        }

        public void GemPurchased()
        {
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "بسته مورد نظر با موفقیت خریداری شد." }, new string[] { "باشه" });
                    break;
                default:
                    MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Target pack purchased successfully." }, new string[] { "OK" });
                    break;
            }
        }

        public void ShieldPurchased(bool success, int pack)
        {
            for (int i = 0; i < shieldPacks.Length; i++)
            {
                if (shieldPacks[i].pack == pack)
                {
                    shieldPacks[i].SetStatus(true);
                    break;
                }
            }
            if (success)
            {
                switch (Language.instanse.language)
                {
                    case Language.LanguageID.persian:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "سپر دفاعی مورد نظر با موفقیت خریداری و فعال شد." }, new string[] { "باشه" });
                        break;
                    default:
                        MessageBox.Open(1, 0.8f, true, MessageResponded, new string[] { "Target shield purchased and activated successfully." }, new string[] { "OK" });
                        break;
                }
            }
        }

        public void GoingToBuyResource(Data.BuyResourcePack pack)
        {
            for (int i = 0; i < resourcePacks.Length; i++)
            {
                resourcePacks[i].SetStatus(false);
            }
        }

        public void ResourcePurchased(bool success, int pack)
        {
            if (success)
            {
                waitingSync = true;
                Player.instanse.RushSyncRequest();
            }
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            Debug.Log(layoutIndex);
            if (layoutIndex == 1)
            {
                MessageBox.Close();
            }
        }

    }
}
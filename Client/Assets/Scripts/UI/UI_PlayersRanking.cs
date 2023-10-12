namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_PlayersRanking : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private UI_PlayerRank _playersPrefab = null;
        [SerializeField] private RectTransform _playersParent = null;
        [SerializeField] private Button _nextButton = null;
        [SerializeField] private Button _lastButton = null;
        [SerializeField] private Button _prevButton = null;
        [SerializeField] private Button _firstButton = null;
        [SerializeField] private TextMeshProUGUI _pageText = null;

        private static UI_PlayersRanking _instance = null; public static UI_PlayersRanking instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }

        private List<UI_PlayerRank> items = new List<UI_PlayerRank>();

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _nextButton.onClick.AddListener(OpenClansListNext);
            _prevButton.onClick.AddListener(OpenClansListPrev);
            _firstButton.onClick.AddListener(OpenClansListFirst);
            _lastButton.onClick.AddListener(OpenClansListLast);
            _closeButton.onClick.AddListener(Close);
        }

        public void Open()
        {
            ClearItems();
            OpenClansList(0);
            _active = true;
            _elements.SetActive(true);
        }

        private int clansPage = 1;
        private int clansMaxPage = 1;

        public void OpenResponse(Data.PlayersRanking players)
        {
            clansPage = players.page;
            clansMaxPage = players.pagesCount;
            ClearItems();
            if (players.players.Count > 0)
            {
                for (int i = 0; i < players.players.Count; i++)
                {
                    UI_PlayerRank item = Instantiate(_playersPrefab, _playersParent);
                    item.Initialize(players.players[i]);
                    items.Add(item);
                }
            }
            _nextButton.interactable = (players.page != players.pagesCount && players.players.Count > 0);
            _lastButton.interactable = (players.page != players.pagesCount && players.players.Count > 0);
            _prevButton.interactable = (players.page != 1 && players.players.Count > 0);
            _firstButton.interactable = (players.page != 1 && players.players.Count > 0);
            _pageText.text = players.page.ToString() + "/" + players.pagesCount.ToString();
        }

        private void OpenClansListNext()
        {
            if (clansPage + 1 <= clansMaxPage)
            {
                OpenClansList(clansPage + 1);
            }
        }

        private void OpenClansListPrev()
        {
            if (clansPage - 1 > 0)
            {
                OpenClansList(clansPage - 1);
            }
        }

        private void OpenClansListLast()
        {
            OpenClansList(clansMaxPage);
        }

        private void OpenClansListFirst()
        {
            OpenClansList(1);
        }

        private void OpenClansList(int page)
        {
            _nextButton.interactable = false;
            _lastButton.interactable = false;
            _prevButton.interactable = false;
            _firstButton.interactable = false;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.PLAYERSRANK);
            packet.Write(page);
            Sender.TCP_Send(packet);
        }

        public void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _active = false;
            _elements.SetActive(false);
        }

        private void ClearItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i])
                {
                    Destroy(items[i].gameObject);
                }
            }
            items.Clear();
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_BattleReports : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private UI_BattleReportsItem _reportPrefab = null;
        [SerializeField] private RectTransform _reportsParent = null;

        private static UI_BattleReports _instance = null; public static UI_BattleReports instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }
        private List<UI_BattleReportsItem> items = new List<UI_BattleReportsItem>();

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
        }

        public void Open()
        {
            ClearItems();
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.BATTLEREPORTS);
            Sender.TCP_Send(packet);
            _active = true;
            _elements.SetActive(true);
        }

        public void OpenResponse(List<Data.BattleReportItem> reports)
        {
            ClearItems();
            if (reports.Count > 0)
            {
                for (int i = 0; i < reports.Count; i++)
                {
                    UI_BattleReportsItem item = Instantiate(_reportPrefab, _reportsParent);
                    item.Initialize(reports[i]);
                    items.Add(item);
                }
            }
        }

        public void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _Close();
        }

        public void _Close()
        {
            if(Building.selectedInstanse != null)
            {
                Building.selectedInstanse.Deselected();
            }
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

        public void PlayReply(Data.BattleReport report, Data.Player player)
        {
            if (isActive)
            {
                _Close();
            }
            UI_Scout.instanse.Open(player, report.type, report);
        }

    }
}
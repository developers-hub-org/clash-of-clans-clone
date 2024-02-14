namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Search : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private Button _findButton = null;
        [SerializeField] private TextMeshProUGUI _costText = null;

        private static UI_Search _instance = null; public static UI_Search instanse { get { return _instance; } }
        private bool _active = true; public bool isActive { get { return _active; } }
        private long lastTarget = 0;

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _findButton.onClick.AddListener(Find);
        }

        public void SetStatus(bool status)
        {
            if (status)
            {
                lastTarget = 0;
                Check();
            }
            _active = status;
            _elements.SetActive(status);
            _costText.ForceMeshUpdate(true);
        }

        private void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            SetStatus(false);
            UI_Main.instanse.SetStatus(true);
        }

        public void Find()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _findButton.interactable = false;
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.BATTLEFIND);
            Sender.TCP_Send(packet);
        }

        private void Check()
        {
            int townHallLevel = 1;
            for (int i = 0; i < Player.instanse.data.buildings.Count; i++)
            {
                if (Player.instanse.data.buildings[i].id == Data.BuildingID.townhall) { townHallLevel = Player.instanse.data.buildings[i].level; break; }
            }
            int cost = Data.GetBattleSearchCost(townHallLevel);
            _costText.text = cost.ToString();
            if (cost > Player.instanse.gold)
            {
                _findButton.interactable = false;
                _costText.color = Color.red;
            }
            else
            {
                _findButton.interactable = true;
                _costText.color = Color.white;
            }
        }

        public void FindResponded(long target, Data.OpponentData opponent)
        {
            if(target > 0 && opponent != null && target != lastTarget)
            {
                SetStatus(false);
                bool attack = UI_Battle.instanse.Display(opponent.data, opponent.buildings, target, Data.BattleType.normal);
                if (attack)
                {
                    lastTarget = target;
                }
                else
                {
                    UI_Main.instanse.SetStatus(true);
                }
            }
            else
            {
                UI_Battle.instanse.NoTarget();
                Debug.Log("No target found.");
            }
        }

    }
}
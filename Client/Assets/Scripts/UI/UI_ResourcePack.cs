namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_ResourcePack : MonoBehaviour
    {

        [SerializeField] private Data.BuyResourcePack _pack = Data.BuyResourcePack.gold_10; public Data.BuyResourcePack pack { get { return _pack; } }
        [SerializeField] private Button _button = null;
        [SerializeField] public TextMeshProUGUI _priceText = null;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
        }

        public void Initialize()
        {
            int tatgetGold = Player.instanse.maxGold - Player.instanse.gold;
            int tatgetElixir = Player.instanse.maxElixir - Player.instanse.elixir;
            int tatgetDark = Player.instanse.maxDarkElixir - Player.instanse.darkElixir;
            switch (_pack)
            {
                case Data.BuyResourcePack.gold_10:
                    tatgetGold = (int)Math.Floor(tatgetGold * 0.1d);
                    tatgetElixir = 0;
                    tatgetDark = 0;
                    break;
                case Data.BuyResourcePack.gold_50:
                    tatgetGold = (int)Math.Floor(tatgetGold * 0.5d);
                    tatgetElixir = 0;
                    tatgetDark = 0;
                    break;
                case Data.BuyResourcePack.gold_100:
                    tatgetElixir = 0;
                    tatgetDark = 0;
                    break;
                case Data.BuyResourcePack.elixir_10:
                    tatgetElixir = (int)Math.Floor(tatgetElixir * 0.1d);
                    tatgetGold = 0;
                    tatgetDark = 0;
                    break;
                case Data.BuyResourcePack.elixir_50:
                    tatgetElixir = (int)Math.Floor(tatgetElixir * 0.5d);
                    tatgetGold = 0;
                    tatgetDark = 0;
                    break;
                case Data.BuyResourcePack.elixir_100:
                    tatgetGold = 0;
                    tatgetDark = 0;
                    break;
                case Data.BuyResourcePack.dark_10:
                    tatgetDark = (int)Math.Floor(tatgetDark * 0.1d);
                    tatgetGold = 0;
                    tatgetElixir = 0;
                    break;
                case Data.BuyResourcePack.dark_50:
                    tatgetDark = (int)Math.Floor(tatgetDark * 0.5d);
                    tatgetGold = 0;
                    tatgetElixir = 0;
                    break;
                case Data.BuyResourcePack.dark_100:
                    tatgetGold = 0;
                    tatgetElixir = 0;
                    break;
            }

            if (tatgetGold < 0) { tatgetGold = 0; }
            if (tatgetElixir < 0) { tatgetElixir = 0; }
            if (tatgetDark < 0) { tatgetDark = 0; }

            int cost = Data.GetResourceGemCost(tatgetGold, tatgetElixir, tatgetDark);
            _priceText.text = cost.ToString();
            if (Player.instanse.data.gems >= cost)
            {
                SetStatus(cost > 0);
                _priceText.color = Color.white;
            }
            else
            {
                SetStatus(false);
                _priceText.color = Color.red;
            }
        }

        private void Clicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            UI_Store.instanse.GoingToBuyResource(_pack);
            Packet packet = new Packet();
            packet.Write((int)Player.RequestsID.BUYRESOURCE);
            packet.Write((int)_pack);
            Sender.TCP_Send(packet);
        }

        public void SetStatus(bool enabled)
        {
            _button.interactable = enabled;
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using DevelopersHub.RealtimeNetworking.Client;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_ShieldPack : MonoBehaviour
    {

        [SerializeField] private int _pack = 1; public int pack { get { return _pack; } }
        [SerializeField] private Button _button = null;
        [SerializeField] private TextMeshProUGUI _cooldownText = null;
        [SerializeField] private TextMeshProUGUI _priceText = null;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "خرید بسته سپر دفاعی؟" }, new string[] { "بله", "خیر" });
                    break;
                default:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "Buy the shield pack?" }, new string[] { "Yes", "No" });
                    break;
            }
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 3)
            {
                if(buttonIndex == 0)
                {
                    SetStatus(false);
                    Packet packet = new Packet();
                    packet.Write((int)Player.RequestsID.BUYSHIELD);
                    packet.Write(_pack);
                    Sender.TCP_Send(packet);
                }
                MessageBox.Close();
            }
        }

        public void SetStatus(bool enabled)
        {
            if (_button.interactable == enabled)
            {
                return;
            }
            _button.interactable = enabled;
        }

        private void Update()
        {
            int price = 0;
            string cooldown = "0H";
            TimeSpan span = new TimeSpan();
            if (pack == 1)
            {
                span = Player.instanse.data.shield1 - Player.instanse.data.nowTime;
                price = 10;
                cooldown = "23H";
            }
            else if (pack == 2)
            {
                span = Player.instanse.data.shield2 - Player.instanse.data.nowTime;
                price = 100;
                cooldown = "5d";
            }
            else if (pack == 3)
            {
                span = Player.instanse.data.shield3 - Player.instanse.data.nowTime;
                price = 250;
                cooldown = "35d";
            }
            if (Player.instanse.data.gems >= price)
            {
                _priceText.color = Color.white;
                SetStatus(true);
            }
            else
            {
                _priceText.color = Color.red;
                SetStatus(false);
            }
            if (span.TotalSeconds > 0)
            {
                _cooldownText.text = Tools.SecondsToTimeFormat(span);
                _cooldownText.color = Color.red;
                SetStatus(false);
            }
            else
            {
                _cooldownText.color = Color.white;
                _cooldownText.text = cooldown;
            }
        }

    }
}
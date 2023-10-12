namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class LanguageSwitch : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;

        private static LanguageSwitch _instance = null; public static LanguageSwitch instanse { get { return _instance; } }
        private bool _active = false; public bool isActive { get { return _active; } }

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
            _active = true;
            transform.SetAsLastSibling();
            _elements.SetActive(true);
        }

        public void Close()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            _active = false;
            _elements.SetActive(false);
        }

        private void MessageResponded(int layoutIndex, int buttonIndex)
        {
            if (layoutIndex == 3)
            {
                if (buttonIndex == 0)
                {
                    PlayerPrefs.SetInt("language", (int)language);
                    Player.RestartGame();
                }
                MessageBox.Close();
            }
        }

        private Language.LanguageID language = Language.LanguageID.english;

        public void Switch(Language.LanguageID id)
        {
            language = id;
            switch (Language.instanse.language)
            {
                case Language.LanguageID.persian:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "تغییر زبان به " + Language.instanse.GetLanguageName(id) + "؟" }, new string[] { "بله", "خیر" });
                    break;
                default:
                    MessageBox.Open(3, 0.8f, true, MessageResponded, new string[] { "Change language to " + Language.instanse.GetLanguageName(id) + "?" }, new string[] { "Yes", "No" });
                    break;
            }
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class LanguageItem : MonoBehaviour
    {

        [SerializeField] private Language.LanguageID _id = Language.LanguageID.english; public Language.LanguageID id { get { return _id; } set { _id = value; } }

        [SerializeField] private Button _button = null;

        private void Awake()
        {
            _button.interactable = Language.instanse.language != id;
        }

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            LanguageSwitch.instanse.Switch(id);
        }

    }
}
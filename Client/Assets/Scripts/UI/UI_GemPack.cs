namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_GemPack : MonoBehaviour
    {

        [SerializeField] private int _pack = 1; public int pack { get { return _pack; } }
        [SerializeField] private string _id = "id"; public string id { get { return _id; } }
        [SerializeField] private Button _button = null;

        private void Start()
        {
            _button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            SoundManager.instanse.PlaySound(SoundManager.instanse.buttonClickSound);
            // ToDo: Purchase Product
        }

        public void SetStatus(bool enabled)
        {
            _button.interactable = enabled;
        }

    }
}
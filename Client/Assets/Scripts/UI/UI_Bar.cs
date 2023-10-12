namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class UI_Bar : MonoBehaviour
    {

        public Image bar = null;
        public RectTransform rect = null;
        public TextMeshProUGUI[] texts = null;
        [SerializeField] private float height = 0.04f;
        [SerializeField] private float aspect = 2.5f;

        private Vector2 size = Vector2.one;

        private void Awake()
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.zero;
        }

        private void Start()
        {
            if (rect != null)
            {
                size = new Vector2(Screen.height * height * aspect, Screen.height * height);
                rect.sizeDelta = size * CameraController.instanse.zoomScale;
            }
        }

        private void Update()
        {
            if (rect != null)
            {
                rect.sizeDelta = size / CameraController.instanse.zoomScale;
            }
        }

    }
}
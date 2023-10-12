namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_Button : MonoBehaviour
    {

        public Button button = null;
        public RectTransform rect = null;
        [SerializeField] private float height = 0.08f;
        [SerializeField] private float aspect = 1f;
        private Vector2 size = Vector2.one;

        private void Awake()
        {
            if(rect == null)
            {
                rect = GetComponent<RectTransform>();
            }
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

        private void OnDestroy()
        {
            button.onClick.RemoveAllListeners();
        }

    }
}
namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UI_ColorPicker : MonoBehaviour
    {

        [SerializeField] public GameObject _elements = null;
        [SerializeField] private Button _closeButton = null;
        [SerializeField] private Button _confirmButton = null;
        [SerializeField] private CUIColorPicker _colorPicker = null;

        private bool _active = false; public bool isActive { get { return _active; } }
        private static UI_ColorPicker _instance = null; public static UI_ColorPicker instanse { get { return _instance; } }

        public delegate void ColorPickCallback(bool result, Color color);
        private ColorPickCallback _callback = null;
        private Color _color;

        private void Awake()
        {
            _instance = this;
            _elements.SetActive(false);
        }

        private void Start()
        {
            _confirmButton.onClick.AddListener(Confirm);
            _closeButton.onClick.AddListener(Close);
        }

        public void Open(Color color, ColorPickCallback callback)
        {
            transform.SetAsLastSibling();
            _color = color;
            _colorPicker.Color = color;
            _callback = callback;
            _active = true;
            _elements.SetActive(true);
        }

        private void Confirm()
        {
            if (_callback != null)
            {
                _callback.Invoke(true, _colorPicker.Color);
                _callback = null;
            }
            Close();
        }

        public void Close()
        {
            if(_callback != null)
            {
                _callback.Invoke(false, _color);
                _callback = null;
            }
            _active = false;
            _elements.SetActive(false);
        }

    }
}
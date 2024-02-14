namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using TMPro;
    using System;
    using UnityEngine.UI;

    public class MessageBox : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Image _backgroundImage = null;
        [SerializeField] private Button _backgroundButton = null;
        [SerializeField] private Layout[] _layouts = null;

        public delegate void MessageButtonCallback(int layoutIndex, int buttonIndex);
        private MessageButtonCallback _buttonCallback = null;
        private bool _cabBeClosedByBackgroundClick = true;
        private static MessageBox _instance = null; public static MessageBox instance { get { return _instance; } }
        public static bool isActive { get { return instance._layout >= 0; } }
        private int _layout = -1; public static int activeLayout { get { return _instance._layout; } }

        [Serializable] public class Layout
        {
            public string name = "";
            public GameObject panel = null;
            public TextMeshProUGUI[] texts = null;
            public TMP_InputField[] inputs = null;
            public Image[] images = null;
            public Button[] buttons = null;
        }

        private void Awake()
        {
            _instance = this;
            _layout = -1;
            _elements.SetActive(false);
        }

        private void Start()
        {
            if(_layouts != null && _layouts.Length > 0)
            {
                for (int i = 0; i < _layouts.Length; i++)
                {
                    if(_layouts[i].buttons != null && _layouts[i].buttons.Length > 0)
                    {
                        for (int j = 0; j < _layouts[i].buttons.Length; j++)
                        {
                            if(_layouts[i].buttons[j] != null)
                            {
                                AddButtonListener(_layouts[i].buttons[j], i, j);
                            }
                        }
                    }
                }
            }

            if(_backgroundButton != null)
            {
                _backgroundButton.onClick.AddListener(BackgroundClicked);
            }
        }

        private void AddButtonListener(Button button, int layoutIndex, int buttonIndex)
        {
            button.onClick.AddListener(delegate { ButtonClicked(layoutIndex, buttonIndex); });
        }

        public static void Open(int index, float backgroundAlpha = 0.8f, bool cabBeClosedByBackgroundClick = true, MessageButtonCallback callback = null, string[] texts = null, string[] buttons = null, Sprite[] images = null, string[] inputs = null)
        {
            _instance.OpenLayout(index, backgroundAlpha, cabBeClosedByBackgroundClick, callback, texts, buttons, images, inputs);
        }

        private void OpenLayout(int index, float backgroundAlpha = 0.8f, bool cabBeClosedByBackgroundClick = true, MessageButtonCallback callback = null, string[] texts = null, string[] buttons = null, Sprite[] images = null, string[] inputs = null)
        {
            if (_elements == null || index < 0 || index >= _layouts.Length)
            {
                return;
            }

            if (_backgroundImage != null)
            {
                backgroundAlpha = Mathf.Clamp(backgroundAlpha, 0f, 1f);
                var color = _backgroundImage.color;
                color.a = backgroundAlpha;
                _backgroundImage.color = color;
            }

            _cabBeClosedByBackgroundClick = cabBeClosedByBackgroundClick;
            _buttonCallback = callback;

            if (_layouts != null && _layouts.Length > 0)
            {
                for (int i = 0; i < _layouts.Length; i++)
                {
                    if (index == i)
                    {
                        _layout = i;
                        if(_layouts[i].panel != null)
                        {
                            _layouts[i].panel.SetActive(true);
                        }
                       
                        if (_layouts[i].texts != null && _layouts[i].texts.Length > 0 && texts != null && texts.Length > 0)
                        {
                            for (int j = 0; j < Mathf.Min(_layouts[i].texts.Length, texts.Length); j++)
                            {
                                if (_layouts[i].texts[j] != null)
                                {
                                    _layouts[i].texts[j].text = texts[j];
                                    _layouts[i].texts[j].ForceMeshUpdate(true);
                                }
                            }
                        }

                        if (_layouts[i].buttons != null && _layouts[i].buttons.Length > 0 && buttons != null && buttons.Length > 0)
                        {
                            for (int j = 0; j < Mathf.Min(_layouts[i].buttons.Length, buttons.Length); j++)
                            {
                                if (_layouts[i].buttons[j] != null)
                                {
                                    TextMeshProUGUI text = _layouts[i].buttons[j].GetComponentInChildren<TextMeshProUGUI>();
                                    if (text != null)
                                    {
                                        text.text = buttons[j];
                                        text.ForceMeshUpdate(true);
                                    }
                                }
                            }
                        }

                        if (_layouts[i].images != null && _layouts[i].images.Length > 0 && images != null && images.Length > 0)
                        {
                            for (int j = 0; j < Mathf.Min(_layouts[i].images.Length, images.Length); j++)
                            {
                                if (_layouts[i].images[j] != null)
                                {
                                    _layouts[i].images[j].sprite = images[j];
                                }
                            }
                        }

                        if (_layouts[i].inputs != null && _layouts[i].inputs.Length > 0 && inputs != null && inputs.Length > 0)
                        {
                            for (int j = 0; j < Mathf.Min(_layouts[i].inputs.Length, inputs.Length); j++)
                            {
                                if (_layouts[i].inputs[j] != null)
                                {
                                    TextMeshProUGUI text = _layouts[i].inputs[j].placeholder.GetComponentInChildren<TextMeshProUGUI>();
                                    if (text != null)
                                    {
                                        text.text = inputs[j];
                                        text.ForceMeshUpdate(true);
                                    }
                                }
                            }
                        }

                        transform.SetAsLastSibling();
                        _elements.SetActive(true);
                    }
                    else
                    {
                        if (_layouts[i].panel != null)
                        {
                            _layouts[i].panel.SetActive(false);
                        }
                    }
                }
            }
        }

        public static void Close()
        {
            _instance.CloseLayout();
        }

        private void BackgroundClicked()
        {
            if (_cabBeClosedByBackgroundClick)
            {
                CloseLayout();
            }
        }

        private void CloseLayout()
        {
            if(_layout >= 0)
            {
                if (_buttonCallback != null)
                {
                    _buttonCallback.Invoke(-1, -1);
                    _buttonCallback = null;
                }
                _elements.SetActive(false);
                _layout = -1;
            }
        }

        private void ButtonClicked(int layoutIndex, int buttonIndex)
        {
            if(_buttonCallback != null)
            {
                _buttonCallback.Invoke(layoutIndex, buttonIndex);
            }
        }

        public static string GetInputValue(int layoutIndex, int inputIndex)
        {
            return _instance.InputValue(layoutIndex, inputIndex);
        }

        private string InputValue(int layoutIndex, int inputIndex)
        {
            if (_layouts != null && _layouts.Length > 0)
            {
                if(layoutIndex >= 0 && layoutIndex < _layouts.Length && _layouts[layoutIndex] != null)
                {
                    if (_layouts[layoutIndex].inputs != null && inputIndex >= 0 && inputIndex < _layouts[layoutIndex].inputs.Length && _layouts[layoutIndex].inputs[inputIndex] != null)
                    {
                        return _layouts[layoutIndex].inputs[inputIndex].text;
                    }
                }
            }
            return "";
        }

    }
}
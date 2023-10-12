namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class Loading : MonoBehaviour
    {

        [SerializeField] private GameObject _elements = null;
        [SerializeField] private Image _image = null;
        [SerializeField] [Range(0f, 0.1f)] private float _speed = 0.01f;
        [SerializeField] private Sprite[] _sprites = null;

        private static Loading _instance = null; public static Loading instance { get { return _instance; } }
        private bool active = false; public static bool isActive { get { return instance.active; } }
        private float _timer = 0;
        private int _index = 0;

        private void Awake()
        {
            _instance = this;
        }

        private void Update()
        {
            if (!active || _image == null)
            {
                return;
            }

            if (_timer >= _speed)
            {
                _timer = 0;
                _index++;
                if (_index >= _sprites.Length)
                {
                    _index = 0;
                }
                _image.sprite = _sprites[_index];
            }
            else
            {
                _timer += Time.deltaTime;
            }
        }

        public static void Open()
        {
            instance._index = 0;
            instance.active = true;
            instance.transform.SetAsLastSibling();
            instance._elements.SetActive(true);
        }

        public static void Close()
        {
            instance.active = false;
            instance._elements.SetActive(false);
        }

    }
}
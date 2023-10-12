using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimator : MonoBehaviour
{

    public enum Type
    {
        UIImage = 0, SpriteRenderer = 1
    }

    private bool animate = true;
    [SerializeField] public Type type = Type.UIImage;
    [SerializeField] [Range(0f, 0.1f)] private float delay = 0.01f;
    [SerializeField] private Sprite[] sprites = null;
    private float timer = 0;
    private int index = 0;
    [SerializeField] private Image image = null;
    [SerializeField] private SpriteRenderer _renderer = null;

    private void Awake()
    {
        timer = 0;
        index = 0;
        if (sprites != null && sprites.Length > 0 && sprites[index] != null)
        {
            if (type == Type.SpriteRenderer && _renderer != null)
            {
                _renderer.sprite = sprites[index];
            }
            if (type == Type.UIImage && image != null)
            {
                image.sprite = sprites[index];
            }
        }
    }

    private void Update()
    {
        if (!animate || sprites == null)
        {
            return;
        }
        if (timer >= delay)
        {
            timer = 0;
            index++;
            if (index >= sprites.Length)
            {
                index = 0;
            }
            if (sprites[index] != null)
            {
                if (type == Type.SpriteRenderer && _renderer != null)
                {
                    _renderer.sprite = sprites[index];
                }
                if (type == Type.UIImage && image != null)
                {
                    image.sprite = sprites[index];
                }
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    public void EditorUpdate()
    {
        Update();
    }

}
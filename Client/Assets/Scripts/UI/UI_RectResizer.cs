using System.Collections;
using System.Collections.Generic;
using DevelopersHub.ClashOfWhatecer;
using UnityEngine;

public class UI_RectResizer : MonoBehaviour
{

    public enum Type
    {
        sizeOverride = 0, matchWidthWithHeight = 1, matchHeightWithWidth = 2
    }

    public Type action = Type.sizeOverride;
    [Range(0f, 1f)] public float width = 0.1f;
    [Range(0f, 1f)] public float height = 0.1f;
    public bool offset = true;
    [Range(-1f, 1f)] public float offsetX = 0;
    [Range(-1f, 1f)] public float offsetY = 0;
    private RectTransform rect = null;

    private void Start()
    {
       Apply();
       this.enabled = false;
    }

    public void Apply()
    {
        if(rect == null)
        {
            rect = GetComponent<RectTransform>();
        }
        Vector2 size = rect.sizeDelta;
        Resolution res = Tools.GetCurrentResolutionEditor();
        switch(action)
        {
            case Type.sizeOverride:
                if(width > 0)
                {
                    size.x = res.height * width;
                }
                if(height > 0)
                {
                    size.y = res.height * height;
                }
                if(offset)
                {
                    rect.anchoredPosition = new Vector2(res.height * offsetX, res.height * offsetY);
                }
                break;
            case Type.matchWidthWithHeight:
                size.x = size.y;
                break;
            case Type.matchHeightWithWidth:
                size.y = size.x;
                break;
        }
        rect.sizeDelta = size;
    }

}
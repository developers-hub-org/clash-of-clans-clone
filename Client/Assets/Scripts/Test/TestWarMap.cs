using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWarMap : MonoBehaviour
{

    #if UNITY_EDITOR
    public int columns = 4;
    public int count = 50;
    public bool reverse = false;
    public RectTransform root = null;
    public DevelopersHub.ClashOfWhatecer.UI_WarMember prefab = null;
    private float sizeY, spaceY, sizeX, spaceX = 0;

    private List<RectTransform> GetRects()
    {
        List<RectTransform> rects = new List<RectTransform>();
        if (root.childCount > 0)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                rects.Add(root.GetChild(i).GetComponent<RectTransform>());
            }
        }
        if (rects.Count > count)
        {
            for (int i = rects.Count - 1; i >= count; i--)
            {
                if(rects[i] != null)
                {
                    DestroyImmediate(rects[i].gameObject);
                }
                rects.RemoveAt(i);
            }
        }
        else if (rects.Count < count)
        {
            int x = rects.Count;
            for (int i = x; i < count; i++)
            {
                GameObject go = new GameObject("Pos_" + i.ToString());
                go.transform.SetParent(root);
                RectTransform rect = go.GetComponent<RectTransform>();
                if(rect == null)
                {
                    rect = go.AddComponent<RectTransform>();
                }
                rects.Add(rect);
            }
        }
        return rects;
    }

    public void Arrange()
    {
        List<RectTransform> rects = GetRects();
        if (rects != null && count > 0 && root != null)
        {
            int rows = Mathf.CeilToInt((float)rects.Count / (float)columns);
            if ((float)rows % 2f == 0f)
            {
                rows++;
            }

            sizeY = 4096f;
            spaceY = (sizeY / (rows + 1f)) / sizeY;

            sizeX = 1920f;
            spaceX = (sizeX / (columns + 1f)) / sizeX;

            for (int i = 0; i < rects.Count; i++)
            {
                int c = Mathf.CeilToInt((float)(i + 1) / (float)rows);
                int r = rows - ((c * rows) - (i + 1));
                DevelopersHub.ClashOfWhatecer.UI_WarMember warMember = rects[i].GetComponentInChildren<DevelopersHub.ClashOfWhatecer.UI_WarMember>(true);
                if (warMember != null)
                {
                    warMember.Initialize(null, 0, null, reverse);
                }

                Vector2 position = Vector2.zero;
                position.x = reverse ? (spaceX * c) : 1f - (spaceX * c);
                if (r == 1)
                {
                    position.y = 0.5f;
                }
                else
                {
                    int k = Mathf.CeilToInt(((float)r - 1f) / 2f);
                    if ((float)r % 2f == 0f)
                    {
                        position.y = 0.5f - (k * spaceY);
                    }
                    else
                    {
                        position.y = 0.5f + (k * spaceY);
                    }
                }
                rects[i].anchorMin = position;
                rects[i].anchorMax = position;
                rects[i].anchoredPosition = Vector2.zero;
            }
        }
    }

    public void Create()
    {
        List<RectTransform> rects = GetRects();
        if (rects != null && prefab != null)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                if (rects[i] != null)
                {
                    DevelopersHub.ClashOfWhatecer.UI_WarMember warMember = rects[i].GetComponentInChildren<DevelopersHub.ClashOfWhatecer.UI_WarMember>(true);
                    if (warMember == null)
                    {
                        Instantiate(prefab, rects[i]);
                    }
                }
            }
        }
    }

    public void Remove()
    {
        List<RectTransform> rects = GetRects();
        if (rects != null)
        {
            for (int i = 0; i < rects.Count; i++)
            {
                if (rects[i] != null)
                {
                    DevelopersHub.ClashOfWhatecer.UI_WarMember warMember = rects[i].GetComponentInChildren<DevelopersHub.ClashOfWhatecer.UI_WarMember>(true);
                    if (warMember != null)
                    {
                        DestroyImmediate(warMember.gameObject);
                    }
                }
            }
        }
    }
    
    #endif
}
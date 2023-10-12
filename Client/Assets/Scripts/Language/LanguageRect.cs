namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LanguageRect : MonoBehaviour
    {

        private void Awake()
        {
            if (Language.instanse.IsRTL)
            {
                RectTransform rect = GetComponent<RectTransform>();
                if(rect != null)
                {
                    /*
                    if(transform.parent != null)
                    {
                        RectTransform parent = transform.parent.GetComponent<RectTransform>();
                        if(parent != null)
                        {
                            
                        }
                    }
                    */
                    float size = rect.anchorMax.x - rect.anchorMin.x;
                    Vector2 min = rect.anchorMin;
                    Vector2 max = rect.anchorMax;
                    if(rect.anchorMin.x >= 1f - rect.anchorMax.x)
                    {
                        min.x = 1f - rect.anchorMax.x;
                        max.x = min.x + size;

                    }
                    else
                    {
                        max.x = 1f - rect.anchorMin.x;
                        min.x = max.x - size;
                    }
                    rect.anchorMin = min;
                    rect.anchorMax = max;
                    rect.anchoredPosition = Vector2.zero;
                }
            }
        }

    }
}
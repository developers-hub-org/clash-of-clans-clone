namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using TMPro;
    using UnityEngine;

    [RequireComponent(typeof(TextMeshProUGUI))] public class LanguageText : MonoBehaviour
    {

        [SerializeField] private Language.Translation[] translations = null;
        [SerializeField] private bool changeAlignment = true;

        private void Awake()
        {
            TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
            if(text != null)
            {
                if(translations != null)
                {
                    for (int i = 0; i < translations.Length; i++)
                    {
                        if (translations[i].language == Language.instanse.language)
                        {
                            if (!string.IsNullOrEmpty(translations[i].text))
                            {
                                text.text = translations[i].text;
                            }
                            break;
                        }
                    }
                }
                if (changeAlignment && text.horizontalAlignment == HorizontalAlignmentOptions.Left || text.horizontalAlignment == HorizontalAlignmentOptions.Right)
                {
                    text.horizontalAlignment = Language.instanse.IsRTL ? HorizontalAlignmentOptions.Right : HorizontalAlignmentOptions.Left;
                }
            }
        }

    }
}
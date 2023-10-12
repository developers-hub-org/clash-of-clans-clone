namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class LanguageObject : MonoBehaviour
    {

        [SerializeField] private Item[] items = null;

        [System.Serializable] public class Item
        {
            public Language.LanguageID language;
            public GameObject[] objects = null;
        }

        private void Awake()
        {
            if (items != null && items.Length > 0)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if(items[i].objects != null && items[i].objects.Length > 0)
                    {
                        for (int j = 0; j < items[i].objects.Length; j++)
                        {
                            items[i].objects[j].SetActive(Language.instanse.language == items[i].language);
                        }
                    }
                }
            }
        }

    }
}
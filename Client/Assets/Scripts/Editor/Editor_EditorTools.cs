using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[CustomEditor(typeof(EditorTools))] public class Editor_EditorTools : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        EditorTools tools = target as EditorTools;
        if(tools.font != null)
        {
            if(GUILayout.Button("Change All Fonts"))
            {
                ChangeAllFonts(tools.font);
            }
        }
    }

    private void ChangeAllFonts(TMP_FontAsset font)
    {
        TextMeshProUGUI[] texts = FindObjectsByType<TextMeshProUGUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        if(texts != null)
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].font = font;
                if(PrefabUtility.IsPartOfPrefabInstance(texts[i].gameObject))
                {
                    
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UI_RectResizer))] public class Editor_RectResizer : Editor
{
    public override void OnInspectorGUI()
    {
        UI_RectResizer resizer = target as UI_RectResizer;
        resizer.action = (UI_RectResizer.Type)EditorGUILayout.EnumPopup("Action", resizer.action);
        if(resizer.action == UI_RectResizer.Type.sizeOverride)
        {
            resizer.width = EditorGUILayout.Slider("Width", resizer.width, 0f, 1f);
            resizer.height = EditorGUILayout.Slider("Height", resizer.height, 0f, 1f);
            resizer.offset = EditorGUILayout.Toggle("Offset", resizer.offset);
            if(resizer.offset)
            {
                resizer.offsetX = EditorGUILayout.Slider("offset X", resizer.offsetX, -1f, 1f);
                resizer.offsetY = EditorGUILayout.Slider("offset Y", resizer.offsetY, -1f, 1f);
            }
        }
        resizer.Apply();
    }
}
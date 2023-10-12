using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestWarMap))] public class TestWarMapEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TestWarMap instance = (TestWarMap)target;
        if (GUILayout.Button("Refresh"))
        {
            instance.Create();
            instance.Arrange();
        }
        if (GUILayout.Button("Remove"))
        {
            instance.Remove();
        }
    }

}
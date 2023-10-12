using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpriteAnimator))] public class Editor_SpriteAnimator : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (!Application.isPlaying)
        {
            SpriteAnimator animator = target as SpriteAnimator;
            animator.EditorUpdate();
        }
    }

}
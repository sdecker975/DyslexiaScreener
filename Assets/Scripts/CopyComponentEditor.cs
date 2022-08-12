using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CopyComponent), true)]
[CanEditMultipleObjects]
public class CopyComponentEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CopyComponent c = (CopyComponent)target;

        if (GUILayout.Button("Copy"))
        {
            c.Copy();
        }
    }
}

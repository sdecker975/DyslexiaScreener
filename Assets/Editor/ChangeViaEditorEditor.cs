using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChangeViaEditor), true)]
[CanEditMultipleObjects]
public class ChangeViaEditorEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        ChangeViaEditor c = (ChangeViaEditor)target;

        if (GUILayout.Button("Change"))
        {
            c.Change();
        }
    }
}

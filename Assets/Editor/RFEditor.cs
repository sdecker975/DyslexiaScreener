using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RFTestHandler), true)]
[CanEditMultipleObjects]
public class RFEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RFTestHandler t = (RFTestHandler)target;

        if (GUILayout.Button("Sort"))
        {
            t.SortItems();
        }
        if (GUILayout.Button("Export"))
        {
            t.ExportItems();
        }
        if (GUILayout.Button("Reset"))
        {
            t.ResetItemPositions();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TestHandler), true)]
[CanEditMultipleObjects]
public class LWEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        //WITestHandler t = (WITestHandler)target;

      //  if (GUILayout.Button("Add events after mouse"))
        //{
          // t.SortItems();
           // t.AddStopTimers();
        //}
        //if (GUILayout.Button("Export"))
        //{
          //t.ExportItems();
        //}
      //  if (GUILayout.Button("Reset"))
       // {
         //  t.ResetItemPositions();
        //}
       // if (GUILayout.Button("Sort"))
        //{
         //  t.SortItems();
       //}
    }
}

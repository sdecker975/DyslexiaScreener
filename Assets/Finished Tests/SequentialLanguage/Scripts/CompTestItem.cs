using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CompTestItem
{
   // public int pos;
    public bool skipPrint;
    public bool isExplanation;
    public bool isExample;
    public bool useOrder;
    public string id;
    public GameObject[] TriggerPos;
    public int[] OrderOfShapes;
    public int[] OrderOfInput;
    public GameObject[] SortShape;
}
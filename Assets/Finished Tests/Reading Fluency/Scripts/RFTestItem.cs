using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RFTestItem
{
    public int pos;
    public string[] id;

    public bool isExample;
    public bool isRetention;
    public bool isNotSure;
    public bool isButton;
    public bool startTimer;
    public string[] testSentences;
    public bool[] isCorrect;
    public GameObject[] buttonSet;
    public int activeItemNumber = 0;
    public GameObject sentenceBox;
}


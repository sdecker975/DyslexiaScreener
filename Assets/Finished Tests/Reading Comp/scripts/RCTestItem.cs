using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RCTestItem
{
    public int pos;
    public string id;

    public bool isExample;
    public bool skipPrint;
    public string[] testSounds;
    public bool[] isCorrect;
    public string displayedText;
    public GameObject[] LetterButtons;
    public GameObject sentenceBox;
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WISTestItem
{
    public int pos;
    public string id;

    public bool isExample;
    public bool skipPrint;
    public Sprite[] wordPictures;
    public bool[] isCorrect;
    public string displayedText;
    public GameObject[] LetterButtons;
    public GameObject sentenceBox;
}


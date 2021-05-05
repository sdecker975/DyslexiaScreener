using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GNGTestItem
{
    public int pos;
    public string id;

    public bool isExample;
    public int isi;
    public bool skipPrint;
    public Sprite[] wordPictures;
    public bool[] isCorrect;
    public string displayedText;
    public GameObject[] LetterButtons;
    public GameObject sentenceBox;
}


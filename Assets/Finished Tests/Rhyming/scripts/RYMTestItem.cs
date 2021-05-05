﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RYMTestItem
{
    public int pos;
    public string id;

    public bool isExample;
    public bool skipPrint;
    public Sprite[] wordPictures;
    public bool[] isCorrect;
    public GameObject[] LetterButtons;
    public int audioLoopBack;
}

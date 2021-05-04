using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CardSortTestItem
{
    public int pos;
    public string id;

    public bool isExample;
    public bool skipPrint;
    public GameObject[] Cards;
    public GameObject CardToSort;
    public bool[] isCorrect;
    public bool hasGreenButton;
    public bool hasLetterBox;
    public GameObject LetterBox;
    public bool nextGame;
    public string nextGameName;
}

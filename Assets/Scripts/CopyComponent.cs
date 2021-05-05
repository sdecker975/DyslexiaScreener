using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyComponent : MonoBehaviour
{

    public WITestHandler primary;
    public GNGTestHandler secondary;

    public GameObject box;

    public void Copy()
    {
        secondary.currentTestNumber = primary.currentTestNumber;
        secondary.testAbbrev = primary.testAbbrev;
        secondary.ceiling = primary.ceiling;
        secondary.wrongCount = primary.wrongCount;
        secondary.startingEvent = primary.startingEvent;

        print(primary.testItemBackEnd.Count);
        secondary.testItemBackEnd = primary.testItemBackEnd;

        secondary.testItemFrontEnd = new List<GNGTestItem>();

        foreach (WITestItem lw in primary.testItemFrontEnd)
        {
            GNGTestItem ls = new GNGTestItem();
            ls.pos = lw.pos;
            ls.id = lw.id;
            ls.isExample = lw.isExample;
            ls.skipPrint = lw.skipPrint;
            ls.isCorrect = lw.isCorrect;
            ls.wordPictures = lw.wordPictures;
            GameObject[] buttons = lw.LetterButtons;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (buttons[i].name == "plaincard-WI")
                {
                    buttons[i] = box;
                }
            }
            ls.LetterButtons = buttons;
            ls.sentenceBox = lw.sentenceBox;
            secondary.testItemFrontEnd.Add(ls);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonRYM : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardRYM clickedCard;

    public void Continue(int val)
    {
        RYMTestHandler rym = Camera.main.GetComponent<RYMTestHandler>();

        e = rym.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardRYM c in FindObjectsOfType(typeof(ClickCardRYM)) as ClickCardRYM[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        RYMOutputHandler.correct = isCorrect;
        RYMOutputHandler.responsePosition = clickedCard.responsePosition;
        RYMOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            rym.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !rym.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = rym.backEndItem.eventNumber + 1; i < rym.backEndItem.events.Length; i++)
            {
                if (rym.backEndItem.currentEvent.jumpLabel.Equals(rym.backEndItem.events[i].jumpLabel))
                {
                    rym.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && rym.frontEndItem.isExample)
            {
                foreach (ClickCardRYM c in FindObjectsOfType(typeof(ClickCardRYM)) as ClickCardRYM[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                rym.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardRYM c in FindObjectsOfType(typeof(ClickCardRYM)) as ClickCardRYM[])
                {
                    c.isClicked = false;
                }
                rym.backEndItem.eventNumber++;
            }
        }
    }
}

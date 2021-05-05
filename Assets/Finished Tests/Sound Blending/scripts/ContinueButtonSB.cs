using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonSB : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardSB clickedCard;

    public void Continue(int val)
    {
        SBTestHandler sb = Camera.main.GetComponent<SBTestHandler>();

        e = sb.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardSB c in FindObjectsOfType(typeof(ClickCardSB)) as ClickCardSB[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        SBOutputHandler.correct = isCorrect;
        SBOutputHandler.responsePosition = clickedCard.responsePosition;
        SBOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            sb.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !sb.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = sb.backEndItem.eventNumber + 1; i < sb.backEndItem.events.Length; i++)
            {
                if (sb.backEndItem.currentEvent.jumpLabel.Equals(sb.backEndItem.events[i].jumpLabel))
                {
                    sb.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && sb.frontEndItem.isExample)
            {
                foreach (ClickCardSB c in FindObjectsOfType(typeof(ClickCardSB)) as ClickCardSB[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                sb.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardSB c in FindObjectsOfType(typeof(ClickCardSB)) as ClickCardSB[])
                {
                    c.isClicked = false;
                }
                sb.backEndItem.eventNumber++;
            }
        }
    }
}

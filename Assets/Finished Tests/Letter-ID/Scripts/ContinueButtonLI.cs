using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonLI : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardLI clickedCard;

    public void Continue(int val)
    {
        LITestHandler li = Camera.main.GetComponent<LITestHandler>();

        e = li.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardLI c in FindObjectsOfType(typeof(ClickCardLI)) as ClickCardLI[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        LIOutputHandler.correct = isCorrect;
        LIOutputHandler.responsePosition = clickedCard.responsePosition;
        LIOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            li.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !li.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = li.backEndItem.eventNumber + 1; i < li.backEndItem.events.Length; i++)
            {
                if (li.backEndItem.currentEvent.jumpLabel.Equals(li.backEndItem.events[i].jumpLabel))
                {
                    li.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && li.frontEndItem.isExample)
            {
                foreach (ClickCardLI c in FindObjectsOfType(typeof(ClickCardLI)) as ClickCardLI[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                li.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardLI c in FindObjectsOfType(typeof(ClickCardLI)) as ClickCardLI[])
                {
                    c.isClicked = false;
                }
                li.backEndItem.eventNumber++;
            }
        }
    }
}

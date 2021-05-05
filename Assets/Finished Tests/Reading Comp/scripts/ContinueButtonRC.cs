using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonRC : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardRC clickedCard;

    public void Continue(int val)
    {
        RCTestHandler rc = Camera.main.GetComponent<RCTestHandler>();

        e = rc.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardRC c in FindObjectsOfType(typeof(ClickCardRC)) as ClickCardRC[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }

        RCOutputHandler.correct = isCorrect;
        RCOutputHandler.responsePosition = clickedCard.responsePosition;
        RCOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            rc.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !rc.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = rc.backEndItem.eventNumber + 1; i < rc.backEndItem.events.Length; i++)
            {
                if (rc.backEndItem.currentEvent.jumpLabel.Equals(rc.backEndItem.events[i].jumpLabel))
                {
                    rc.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && rc.frontEndItem.isExample)
            {
                foreach (ClickCardRC c in FindObjectsOfType(typeof(ClickCardRC)) as ClickCardRC[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                rc.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardRC c in FindObjectsOfType(typeof(ClickCardRC)) as ClickCardRC[])
                {
                    c.isClicked = false;
                }
                rc.backEndItem.eventNumber++;
            }
        }
    }
}

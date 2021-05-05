using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonWI : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardWI clickedCard;

    public void Continue(int val)
    {
        WITestHandler wi = Camera.main.GetComponent<WITestHandler>();

        e = wi.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardWI c in FindObjectsOfType(typeof(ClickCardWI)) as ClickCardWI[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        WIOutputHandler.correct = isCorrect;
        WIOutputHandler.responsePosition = clickedCard.responsePosition;
        WIOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            wi.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !wi.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = wi.backEndItem.eventNumber + 1; i < wi.backEndItem.events.Length; i++)
            {
                if (wi.backEndItem.currentEvent.jumpLabel.Equals(wi.backEndItem.events[i].jumpLabel))
                {
                    wi.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && wi.frontEndItem.isExample)
            {
                foreach (ClickCardWI c in FindObjectsOfType(typeof(ClickCardWI)) as ClickCardWI[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                wi.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardWI c in FindObjectsOfType(typeof(ClickCardWI)) as ClickCardWI[])
                {
                    c.isClicked = false;
                }
                wi.backEndItem.eventNumber++;
            }
        }
    }
}

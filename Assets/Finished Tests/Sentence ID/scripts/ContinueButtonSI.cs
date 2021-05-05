using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonSI : MonoBehaviour {

    EventSystem.typeOfEvent e;
    ClickCardSI clickedCard;

    public void Continue(int val)
    {
        SITestHandler si = Camera.main.GetComponent<SITestHandler>();

        e = si.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardSI c in FindObjectsOfType(typeof(ClickCardSI)) as ClickCardSI[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        SIOutputHandler.correct = isCorrect;
        SIOutputHandler.responsePosition = clickedCard.responsePosition;
        SIOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            si.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !si.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = si.backEndItem.eventNumber + 1; i < si.backEndItem.events.Length; i++)
            {
                if (si.backEndItem.currentEvent.jumpLabel.Equals(si.backEndItem.events[i].jumpLabel))
                {
                    si.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && si.frontEndItem.isExample)
            {
                foreach (ClickCardSI c in FindObjectsOfType(typeof(ClickCardSI)) as ClickCardSI[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                si.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardSI c in FindObjectsOfType(typeof(ClickCardSI)) as ClickCardSI[])
                {
                    c.isClicked = false;
                }
                si.backEndItem.eventNumber++;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonPM : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardPM clickedCard;

    public void Continue(int val)
    {
        PMTestHandler pm = Camera.main.GetComponent<PMTestHandler>();

        e = pm.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardPM c in FindObjectsOfType(typeof(ClickCardPM)) as ClickCardPM[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        PMOutputHandler.correct = isCorrect;
        PMOutputHandler.responsePosition = clickedCard.responsePosition;
        PMOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            pm.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !pm.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = pm.backEndItem.eventNumber + 1; i < pm.backEndItem.events.Length; i++)
            {
                if (pm.backEndItem.currentEvent.jumpLabel.Equals(pm.backEndItem.events[i].jumpLabel))
                {
                    pm.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && pm.frontEndItem.isExample)
            {
                foreach (ClickCardPM c in FindObjectsOfType(typeof(ClickCardPM)) as ClickCardPM[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                pm.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardPM c in FindObjectsOfType(typeof(ClickCardPM)) as ClickCardPM[])
                {
                    c.isClicked = false;
                }
                pm.backEndItem.eventNumber++;
            }
        }
    }
}

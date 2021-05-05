using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonOC : MonoBehaviour {

    EventSystem.typeOfEvent e;
    ClickCardOC clickedCard;

    public void Continue(int val)
    {
        OCTestHandler oc = Camera.main.GetComponent<OCTestHandler>();

        e = oc.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardOC c in FindObjectsOfType(typeof(ClickCardOC)) as ClickCardOC[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        OCOutputHandler.correct = isCorrect;
        OCOutputHandler.responsePosition = clickedCard.responsePosition;
        OCOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            oc.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !oc.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = oc.backEndItem.eventNumber + 1; i < oc.backEndItem.events.Length; i++)
            {
                if (oc.backEndItem.currentEvent.jumpLabel.Equals(oc.backEndItem.events[i].jumpLabel))
                {
                    oc.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && oc.frontEndItem.isExample)
            {
                foreach (ClickCardOC c in FindObjectsOfType(typeof(ClickCardOC)) as ClickCardOC[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                oc.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardOC c in FindObjectsOfType(typeof(ClickCardOC)) as ClickCardOC[])
                {
                    c.isClicked = false;
                }
                oc.backEndItem.eventNumber++;
            }
        }
    }
}

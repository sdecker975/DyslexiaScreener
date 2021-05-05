using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonLSSI : MonoBehaviour {

    EventSystem.typeOfEvent e;
    ClickCardLSSI clickedCard;

    public void Continue(int val)
    {
        LSSITestHandler lssi = Camera.main.GetComponent<LSSITestHandler>();

        e = lssi.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardLSSI c in FindObjectsOfType(typeof(ClickCardLSSI)) as ClickCardLSSI[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        LSSIOutputHandler.correct = isCorrect;
        LSSIOutputHandler.responsePosition = clickedCard.responsePosition;
        LSSIOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            lssi.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !lssi.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = lssi.backEndItem.eventNumber + 1; i < lssi.backEndItem.events.Length; i++)
            {
                if (lssi.backEndItem.currentEvent.jumpLabel.Equals(lssi.backEndItem.events[i].jumpLabel))
                {
                    lssi.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if(!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && lssi.frontEndItem.isExample)
            {
                foreach (ClickCardLSSI c in FindObjectsOfType(typeof(ClickCardLSSI)) as ClickCardLSSI[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                lssi.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardLSSI c in FindObjectsOfType(typeof(ClickCardLSSI)) as ClickCardLSSI[])
                {
                    c.isClicked = false;
                }
                lssi.backEndItem.eventNumber++;
            }
        }
    }
}

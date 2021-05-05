using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonOV : MonoBehaviour {

    EventSystem.typeOfEvent e;
    ClickCardOV clickedCard;

    public void Continue(int val)
    {
        OVTestHandler ov = Camera.main.GetComponent<OVTestHandler>();

        e = ov.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardOV c in FindObjectsOfType(typeof(ClickCardOV)) as ClickCardOV[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        OVOutputHandler.correct = isCorrect;
        OVOutputHandler.responsePosition = clickedCard.responsePosition;
        OVOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            ov.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !ov.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = ov.backEndItem.eventNumber + 1; i < ov.backEndItem.events.Length; i++)
            {
                if (ov.backEndItem.currentEvent.jumpLabel.Equals(ov.backEndItem.events[i].jumpLabel))
                {
                    ov.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && ov.frontEndItem.isExample)
            {
                foreach (ClickCardOV c in FindObjectsOfType(typeof(ClickCardOV)) as ClickCardOV[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                ov.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardOV c in FindObjectsOfType(typeof(ClickCardOV)) as ClickCardOV[])
                {
                    c.isClicked = false;
                }
                ov.backEndItem.eventNumber++;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonWIS : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardWIS clickedCard;

    public void Continue(int val)
    {
        WISTestHandler wis = Camera.main.GetComponent<WISTestHandler>();

        e = wis.backEndItem.currentEvent.type;
        bool isCorrect = false;
        foreach (ClickCardWIS c in FindObjectsOfType(typeof(ClickCardWIS)) as ClickCardWIS[])
        {
            if (c.isClicked)
            {
                clickedCard = c;
                isCorrect = c.isCorrect;
                break;
            }
        }
        print("hit");
        WISOutputHandler.correct = isCorrect;
        WISOutputHandler.responsePosition = clickedCard.responsePosition;
        WISOutputHandler.responseName = clickedCard.responseName;

        if (GameObject.Find("ArrowButton") && val == 1)
        {
            GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
            wis.backEndItem.eventNumber++;
            return;
        }

        if (isCorrect && !wis.backEndItem.currentEvent.jumpLabel.Equals(""))
        {

            for (int i = wis.backEndItem.eventNumber + 1; i < wis.backEndItem.events.Length; i++)
            {
                if (wis.backEndItem.currentEvent.jumpLabel.Equals(wis.backEndItem.events[i].jumpLabel))
                {
                    wis.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!GameObject.Find("ArrowButton"))
        {
            if (!isCorrect && wis.frontEndItem.isExample)
            {
                foreach (ClickCardWIS c in FindObjectsOfType(typeof(ClickCardWIS)) as ClickCardWIS[])
                {
                    if (c.isCorrect)
                        c.isAnim = true;
                    c.isClicked = false;
                }
                wis.backEndItem.eventNumber++;
            }
            else
            {
                foreach (ClickCardWIS c in FindObjectsOfType(typeof(ClickCardWIS)) as ClickCardWIS[])
                {
                    c.isClicked = false;
                }
                wis.backEndItem.eventNumber++;
            }
        }
    }
}

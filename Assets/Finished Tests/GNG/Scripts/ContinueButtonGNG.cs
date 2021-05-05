using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonGNG : MonoBehaviour
{

    EventSystem.typeOfEvent e;
    ClickCardGNG clickedCard;

    public void Continue(int val)
    {
        GNGTestHandler gng = Camera.main.GetComponent<GNGTestHandler>();
        e = gng.backEndItem.currentEvent.type;
        if (GameObject.Find("go"))
        {
            gng.backEndItem.eventNumber++;
            return;
        }
        bool isCorrect;
        ClickCardGNG[] c = FindObjectsOfType(typeof(ClickCardGNG)) as ClickCardGNG[];
        print(c[0].responseName);
        if (!GameObject.Find("go") && ((c[0].isClicked && gng.frontEndItem.isCorrect[0]) || (!c[0].isClicked && !gng.frontEndItem.isCorrect[0])))
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }

        GNGOutputHandler.correct = isCorrect;
        GNGOutputHandler.responsePosition = c[0].responsePosition;
        GNGOutputHandler.responseName = c[0].responseName;
        
        if (isCorrect && !gng.backEndItem.currentEvent.jumpLabel.Equals("")) //This jumps from teaching
        {

            for (int i = gng.backEndItem.eventNumber + 1; i < gng.backEndItem.events.Length; i++)
            {
                if (gng.backEndItem.currentEvent.jumpLabel.Equals(gng.backEndItem.events[i].jumpLabel))
                {
                    gng.backEndItem.eventNumber = i;
                    break;
                }
            }
        }
        else if (!isCorrect && gng.frontEndItem.isExample && !gng.frontEndItem.id.Contains("d"))
        {
            gng.backEndItem.eventNumber++;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextChecker : MonoBehaviour {

    public bool isCorrect;
    bool isMouseDown = false;
    public int itemNumber;
    EventSystem.typeOfEvent e;

    void Update()
    {
        LetterWordTestHandler lw = Camera.main.GetComponent<LetterWordTestHandler>();
        e = lw.backEndItem.currentEvent.type;

        if (isMouseDown)
        {
            if (isCorrect && !lw.backEndItem.currentEvent.jumpLabel.Equals(""))
            {
                for (int i = lw.backEndItem.eventNumber + 1; i < lw.backEndItem.events.Length; i++)
                {
                    if (lw.backEndItem.currentEvent.jumpLabel.Equals(lw.backEndItem.events[i].jumpLabel))
                    {
                        lw.backEndItem.eventNumber = i;
                        break;
                    }
                }
            }
            else
            {
                lw.backEndItem.eventNumber++;
            }
            isMouseDown = false;
        }
    }

    void OnMouseDown()
    {
        isMouseDown = e == EventSystem.typeOfEvent.Mouse;
    }

    void OnMouseUp()
    {
        isMouseDown = false;
    }
}

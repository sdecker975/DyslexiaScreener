using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinueButtonCS : MonoBehaviour
{
    EventSystem.typeOfEvent e;
    ClickCardCS clickedCard;

    public void Continue()
    {
        CardSortTestHandler cs = Camera.main.GetComponent<CardSortTestHandler>();
       
        cs.backEndItem.eventNumber++;

        Destroy(GameObject.Find("startbutton"));
    }
}

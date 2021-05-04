using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerCS : MonoBehaviour {

    CardSortTestHandler cs;

	// Use this for initialization
	void Start () {
        cs = Camera.main.GetComponent<CardSortTestHandler>();
	}
	
	// Update is called once per frame
	void Update () {
        EventSystem.typeOfEvent e = cs.backEndItem.currentEvent.type;

        if (e == EventSystem.typeOfEvent.Mouse)
        {
            GetComponent<Button>().interactable = true;
        }
    }
}

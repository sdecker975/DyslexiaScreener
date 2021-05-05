using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VWMButtonHandler : MonoBehaviour {

    NumberPadHandler nph;
    VerbalWorkingTestHandler vwm;

	// Use this for initialization
	void Start () {
        nph = Camera.main.GetComponent<NumberPadHandler>();
        vwm = Camera.main.GetComponent<VerbalWorkingTestHandler>();
	}
	
	// Update is called once per frame
	void Update () {
        EventSystem.typeOfEvent e = vwm.backEndItem.currentEvent.type;
        if (name.Equals("ArrowButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && vwm.frontEndItem.isExample)
            {
                GetComponent<Button>().interactable = true;
            }
            //remove if it fucks up audio
            else if (e != EventSystem.typeOfEvent.Mouse)
            {
                GetComponent<Button>().interactable = false;
            }
            else if (e == EventSystem.typeOfEvent.Mouse)
            {
                if(!nph.numbers.Equals(""))
                    GetComponent<Button>().interactable = true;
            }
        }
	}
}

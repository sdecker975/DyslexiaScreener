using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerOV : MonoBehaviour {

    OVTestHandler ov;

    // Use this for initialization
    void Start()
    {
        ov = Camera.main.GetComponent<OVTestHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.typeOfEvent e = ov.backEndItem.currentEvent.type;
        if (name.Equals("ArrowButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && ov.frontEndItem.isExample)
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
                bool anyClicked = false;
                foreach (ClickCardOV c in FindObjectsOfType(typeof(ClickCardOV)) as ClickCardOV[])
                {
                    if (c.isClicked)
                    {
                        anyClicked = true;
                        break;
                    }
                }
                GetComponent<Button>().interactable = anyClicked;
            }
        }
        else if (name.Equals("AudioButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && !ov.frontEndItem.isExample)
            {
                GetComponent<Button>().interactable = true;
            }
            else
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}

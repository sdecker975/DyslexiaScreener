using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerRC : MonoBehaviour {

    RCTestHandler rc;

    // Use this for initialization
    void Start () {
        rc = Camera.main.GetComponent<RCTestHandler>();
    }

    // Update is called once per frame
    void Update () {
        EventSystem.typeOfEvent e = rc.backEndItem.currentEvent.type;
        if (name.Equals("ArrowButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && rc.frontEndItem.isExample)
            {
                GetComponent<Button>().interactable = true;
            }
            //remove if it fucks up audio
            else if(e != EventSystem.typeOfEvent.Mouse)
            {
                GetComponent<Button>().interactable = false;
            }
            else if(e == EventSystem.typeOfEvent.Mouse)
            {
                bool anyClicked = false;
                foreach (ClickCardRC c in FindObjectsOfType(typeof(ClickCardRC)) as ClickCardRC[])
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
            if (e == EventSystem.typeOfEvent.Mouse && !rc.frontEndItem.isExample)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerLI : MonoBehaviour
{
    LITestHandler li;

    // Use this for initialization
    void Start()
    {
        li = Camera.main.GetComponent<LITestHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.typeOfEvent e = li.backEndItem.currentEvent.type;
        if (name.Equals("ArrowButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && li.frontEndItem.isExample)
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
                foreach (ClickCardLI c in FindObjectsOfType(typeof(ClickCardLI)) as ClickCardLI[])
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
            if (e == EventSystem.typeOfEvent.Mouse && !li.frontEndItem.isExample)
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

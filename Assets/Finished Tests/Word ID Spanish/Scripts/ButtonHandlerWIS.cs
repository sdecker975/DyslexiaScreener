using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerWIS : MonoBehaviour
{
    WISTestHandler wis;

    // Use this for initialization
    void Start()
    {
        wis = Camera.main.GetComponent<WISTestHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.typeOfEvent e = wis.backEndItem.currentEvent.type;
        if (name.Equals("ArrowButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && wis.frontEndItem.isExample)
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
                foreach (ClickCardWIS c in FindObjectsOfType(typeof(ClickCardWIS)) as ClickCardWIS[])
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
            if (e == EventSystem.typeOfEvent.Mouse && !wis.frontEndItem.isExample)
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

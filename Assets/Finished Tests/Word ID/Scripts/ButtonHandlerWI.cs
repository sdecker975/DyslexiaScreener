using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerWI : MonoBehaviour
{
    WITestHandler wi;

    // Use this for initialization
    void Start()
    {
        wi = Camera.main.GetComponent<WITestHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.typeOfEvent e = wi.backEndItem.currentEvent.type;
        if (name.Equals("ArrowButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && wi.frontEndItem.isExample)
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
                foreach (ClickCardWI c in FindObjectsOfType(typeof(ClickCardWI)) as ClickCardWI[])
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
            if (e == EventSystem.typeOfEvent.Mouse && !wi.frontEndItem.isExample)
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

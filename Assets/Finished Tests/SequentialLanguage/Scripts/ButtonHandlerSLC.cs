using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerSLC : MonoBehaviour
{
    SeqLangTestHandler slc;

    // Use this for initialization
    void Start()
    {
        slc = Camera.main.GetComponent<SeqLangTestHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.typeOfEvent e = slc.backEndItem.currentEvent.type;
        if (name.Equals("ArrowButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && slc.frontEndItem.isExample)
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
                if (Camera.main.GetComponent<TestChecker>().AllFilledArrow())
                {
                    GetComponent<Button>().interactable = true;
                }
                else
                {
                    GetComponent<Button>().interactable = false;
                }
            }
        }
        else if (name.Equals("XButton"))
        {
            if (e == EventSystem.typeOfEvent.Mouse && !slc.frontEndItem.isExample)
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

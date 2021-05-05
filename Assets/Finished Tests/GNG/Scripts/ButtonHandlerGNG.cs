using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandlerGNG : MonoBehaviour
{
    GNGTestHandler gng;
    ButtonAssignerGNG ba;
    SpriteRenderer buttonImage;
    Color c;

    // Use this for initialization
    void Start()
    {
        gng = Camera.main.GetComponent<GNGTestHandler>();
        ba = GetComponent<ButtonAssignerGNG>();
        if (name.Equals("ArrowButton"))
        {
            buttonImage = GetComponent<SpriteRenderer>();
            c = buttonImage.color;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.typeOfEvent e = gng.backEndItem.currentEvent.type;
        if (name.Equals("go") && e == EventSystem.typeOfEvent.Mouse)
        {
            if (GetComponent<Button>().interactable == false)
                GetComponent<Button>().interactable = true;
        }
 /*       else
        {
           // GetComponent<Button>().interactable = false;
        }*/
        if (name.Equals("ArrowButton"))
        {
            //print("Down is " + ba.down.ToString());
            if (ba.down)
            {
              c.a = .60f;
              buttonImage.color = c;
            }

            if (e == EventSystem.typeOfEvent.Mouse && ba.down == false)
            {
                //if (//GetComponent<Button>().interactable == false)
                c.a = 1;
                buttonImage.color = c;
                GetComponent<Collider2D>().enabled = true;
            }
            //remove if it fucks up audio
            else if (e != EventSystem.typeOfEvent.Mouse && gng.frontEndItem.isExample)
            {
                c.a = .60f;
                buttonImage.color = c;
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }
}

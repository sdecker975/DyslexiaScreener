using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCardSB : MonoBehaviour
{

    public bool isClicked = false;
    public bool isCorrect = false;
    public bool isAnim = false;
    public int responsePosition;
    public string responseName;

    EventSystem.typeOfEvent e;

    // Update is called once per frame
    void Update()
    {
        SBTestHandler sb = Camera.main.GetComponent<SBTestHandler>();
        e = sb.backEndItem.currentEvent.type;

        if (e == EventSystem.typeOfEvent.Destroy)
            isAnim = false;

        //get name of test
        //if its rhyming then do white

        //if(!LI.testAbbrev.Equals("RYM"))
        //{
        GameObject border = transform.Find("GameObject").gameObject;
        if (isClicked)
            border.GetComponent<SpriteRenderer>().color = Color.yellow;
        else if (isAnim)
            border.GetComponent<SpriteRenderer>().color = Color.green;
        else
            border.GetComponent<SpriteRenderer>().color = new Color(41f / 255f, 171f / 255f, 226f / 255f);
        //}
        //else
        //{
        //GameObject button = transform.gameObject;
        //if(button.GetComponent<SpriteRenderer>())
        //{
        //    if (isClicked)
        //        button.GetComponent<SpriteRenderer>().color = Color.gray;
        //    else if (isAnim)
        //        button.GetComponent<SpriteRenderer>().color = Color.green;
        //    else
        //        button.GetComponent<SpriteRenderer>().color = Color.white;
        //}

        //}
    }

    void OnMouseDown()
    {
        SBTestHandler sb = Camera.main.GetComponent<SBTestHandler>();
        if (sb.frontEndItem.isExample && GameObject.Find("ArrowButton"))
            return;
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            isClicked = !isClicked;
            foreach (ClickCardSB c in FindObjectsOfType(typeof(ClickCardSB)) as ClickCardSB[])
            {
                if (!c.Equals(this))
                    c.isClicked = false;
            }
            if (!sb.frontEndItem.isExample)
            {
                GameObject.Find("ArrowButton").GetComponent<Button>().interactable = isClicked;
            }
            else
            {
                Camera.main.GetComponent<ContinueButtonSB>().Continue(0);
            }
        }
    }
}

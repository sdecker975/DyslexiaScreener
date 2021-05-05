using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCardPM : MonoBehaviour
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
        PMTestHandler pm = Camera.main.GetComponent<PMTestHandler>();
        e = pm.backEndItem.currentEvent.type;

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
        PMTestHandler pm = Camera.main.GetComponent<PMTestHandler>();
        if (pm.frontEndItem.isExample && GameObject.Find("ArrowButton"))
            return;
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            isClicked = !isClicked;
            foreach (ClickCardPM c in FindObjectsOfType(typeof(ClickCardPM)) as ClickCardPM[])
            {
                if (!c.Equals(this))
                    c.isClicked = false;
            }
            if (!pm.frontEndItem.isExample)
            {
                GameObject.Find("ArrowButton").GetComponent<Button>().interactable = isClicked;
            }
            else
            {
                Camera.main.GetComponent<ContinueButtonPM>().Continue(0);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCardRYM : MonoBehaviour
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
        RYMTestHandler rym = Camera.main.GetComponent<RYMTestHandler>();
        e = rym.backEndItem.currentEvent.type;

        if (e == EventSystem.typeOfEvent.Destroy)
            isAnim = false;

        GameObject button = transform.gameObject;
        if (button.GetComponent<SpriteRenderer>())
        {
            if (isClicked)
                button.GetComponent<SpriteRenderer>().color = Color.gray;
            else if (isAnim)
                button.GetComponent<SpriteRenderer>().color = Color.green;
            else
                button.GetComponent<SpriteRenderer>().color = Color.white;
        }

    }

    void OnMouseDown()
    {
        RYMTestHandler rym = Camera.main.GetComponent<RYMTestHandler>();
        if (rym.frontEndItem.isExample && GameObject.Find("ArrowButton"))
            return;
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            isClicked = !isClicked;
            foreach (ClickCardRYM c in FindObjectsOfType(typeof(ClickCardRYM)) as ClickCardRYM[])
            {
                if (!c.Equals(this))
                    c.isClicked = false;
            }
            if (!rym.frontEndItem.isExample)
            {
                GameObject.Find("ArrowButton").GetComponent<Button>().interactable = isClicked;
            }
            else
            {
                Camera.main.GetComponent<ContinueButtonRYM>().Continue(0);
            }
        }
    }
}

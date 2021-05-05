using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCardOV : MonoBehaviour {

    public bool isClicked = false;
    public bool isCorrect = false;
    public bool isAnim = false;
    public int responsePosition;
    public string responseName;

    EventSystem.typeOfEvent e;

    // Update is called once per frame
    void Update()
    {
        OVTestHandler ov = Camera.main.GetComponent<OVTestHandler>();
        e = ov.backEndItem.currentEvent.type;

        if (e == EventSystem.typeOfEvent.Destroy)
            isAnim = false;

        GameObject border = transform.Find("GameObject").gameObject;
        if (isClicked)
            border.GetComponent<SpriteRenderer>().color = Color.yellow;
        else if (isAnim)
            border.GetComponent<SpriteRenderer>().color = Color.green;
        else
            border.GetComponent<SpriteRenderer>().color = new Color(41f / 255f, 171f / 255f, 226f / 255f);
    }

    void OnMouseDown()
    {
        OVTestHandler ov = Camera.main.GetComponent<OVTestHandler>();
        if (ov.frontEndItem.isExample && GameObject.Find("ArrowButton"))
            return;
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            isClicked = !isClicked;
            foreach (ClickCardOV c in FindObjectsOfType(typeof(ClickCardOV)) as ClickCardOV[])
            {
                if (!c.Equals(this))
                    c.isClicked = false;
            }
            if (!ov.frontEndItem.isExample)
            {
                GameObject.Find("ArrowButton").GetComponent<Button>().interactable = isClicked;
            }
            else
            {
                Camera.main.GetComponent<ContinueButtonOV>().Continue(0);
            }
        }
    }
}

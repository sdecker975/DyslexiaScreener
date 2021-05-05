using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPadDigit : MonoBehaviour {

    public int digit;
    Color originalColor = new Color(204f/255f, 204f/255f, 204f/255f);
    Color downColor = new Color(150f/255f, 150f/255f, 150f/255f);

    bool inCard;
    bool isDown;

    float posX, posY;

    EventSystem.typeOfEvent e;
    VerbalWorkingTestHandler lw;

    private void Start()
    {
        lw = Camera.main.GetComponent<VerbalWorkingTestHandler>();
    }

    private void Update()
    {
        posX = Input.mousePosition.x;
        posY = Input.mousePosition.y;

        e = lw.backEndItem.currentEvent.type;
    }


    private void OnMouseDown()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 10.0f));
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            if((lw.frontEndItem.isExample && !GameObject.Find("ArrowButton")) || !lw.frontEndItem.isExample)
            {
                GetComponent<SpriteRenderer>().color = downColor;
                isDown = true;
            }
        }
    }
    private void OnMouseUp()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 10.0f));
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            if ((lw.frontEndItem.isExample && !GameObject.Find("ArrowButton")) || !lw.frontEndItem.isExample)
            {
                NumberPadHandler nph = Camera.main.GetComponent<NumberPadHandler>();
                nph.appendToNumbers(digit);
                GetComponent<SpriteRenderer>().color = originalColor;
            }
        }
        isDown = false;
    }
    private void OnMouseEnter()
    {
        if(e == EventSystem.typeOfEvent.Mouse)
        {
            inCard = true;
            if (isDown)
                GetComponent<SpriteRenderer>().color = downColor;
        }
    }
    private void OnMouseExit()
    {
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            GetComponent<SpriteRenderer>().color = originalColor;
            inCard = false;
        }
    }

    public void MakeClicked()
    {
        GetComponent<SpriteRenderer>().color = downColor;
    }

    public void MakeUnclicked()
    {
        GetComponent<SpriteRenderer>().color = originalColor;
    }
}

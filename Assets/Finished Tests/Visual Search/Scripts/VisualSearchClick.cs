using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualSearchClick : MonoBehaviour {

    VisualSearchTestHandler vis;
    EventSystem.typeOfEvent e;

    bool inCard;
    bool isDown;

    public bool isCorrect;
    public int x, y;

    float posX, posY;

    public static int totalIncorrect = 0;

    public Sprite orig, clicked;

    // Use this for initialization
    void Start () {
        vis = Camera.main.GetComponent<VisualSearchTestHandler>();
	}

    private void Update()
    {
        posX = Input.mousePosition.x;
        posY = Input.mousePosition.y;
        e = vis.backEndItem.currentEvent.type;
    }


    private void OnMouseDown()
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(posX, posY, 10.0f));
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            VISOutputHandler.correctness = isCorrect;
            VISOutputHandler.position = x + "-" + y;
            VISOutputHandler.content = gameObject.name;

            if (vis.frontEndItem.isExample)
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = clicked;
                isDown = true;

                if (isCorrect)
                {
                    if (!vis.backEndItem.currentEvent.jumpLabel.Equals(""))
                    {
                        for (int i = vis.backEndItem.eventNumber + 1; i < vis.backEndItem.events.Length; i++)
                        {
                            if (vis.backEndItem.currentEvent.jumpLabel.Equals(vis.backEndItem.events[i].jumpLabel))
                            {
                                vis.backEndItem.eventNumber = i;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    vis.backEndItem.eventNumber++;
                }
            }
            else
            {
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = clicked;
                if (isCorrect)
                {
                    print("in correct");
                    vis.backEndItem.eventNumber++;
                    totalIncorrect = 0;
                    vis.wrongCount = 0;
                }
                else
                {
                    print(totalIncorrect);
                    totalIncorrect++;
                    if (totalIncorrect == 1)
                    {
                        totalIncorrect = 0;
                        //vis.wrongCount++;
                        vis.backEndItem.eventNumber++;
                    }
                }
            }
        }
    }
    private void OnMouseUp()
    {
        if (inCard && e == EventSystem.typeOfEvent.Mouse)
        {
            if (vis.frontEndItem.isExample)
            {
                //transform.GetChild(0).GetComponent<SpriteRenderer>().color = originalColor;
            }
        }
        isDown = false;
    }
    //private void OnMouseEnter()
    //{
    //    if (e == EventSystem.typeOfEvent.Mouse)
    //    {
    //        inCard = true;
    //        if (isDown)
    //        {
    //            transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = clicked;
    //        }
    //    }
    //}
    private void OnMouseExit()
    {
        if (e == EventSystem.typeOfEvent.Mouse)
        {
            //transform.GetChild(0).GetComponent<SpriteRenderer>().color = originalColor;
            inCard = false;
        }
    }

    public void MakeClicked()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = clicked;
    }

    public void MakeUnclicked()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = orig;
    }
}

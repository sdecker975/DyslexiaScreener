using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingCardChecker : MonoBehaviour {

    public bool isChecker = false;
    public int orderNumber;

    public bool isColliding = false;
    public bool isMouseDown = false;

    public GameObject collidingGO;

	// Use this for initialization
	void Start () {
		
	}
	
    //TODO: Bug in dropping card after a card is already there, locks previous card
	// Update is called once per frame
	void Update () {
		CardSortTestHandler cs = Camera.main.GetComponent<CardSortTestHandler>();
        EventSystem.typeOfEvent e = cs.backEndItem.currentEvent.type;

        isMouseDown = Input.GetMouseButton(0);

        if (!isMouseDown && isColliding && collidingGO && e == EventSystem.typeOfEvent.Mouse)
        {
            //collidingGO.transform.position = new Vector2(transform.position.x, transform.position.y);
            print(gameObject.name);
            if (gameObject.name.Contains("true"))
            {
                CardSortOutputHandler.correct = true;

                if (!cs.backEndItem.currentEvent.jumpLabel.Equals(""))
                {
                    for (int i = cs.backEndItem.eventNumber + 1; i < cs.backEndItem.events.Length; i++)
                    {
                        if (cs.backEndItem.currentEvent.jumpLabel.Equals(cs.backEndItem.events[i].jumpLabel))
                        {
                            cs.backEndItem.eventNumber = i;
                            break;
                        }
                    }
                }
                else
                {
                    cs.backEndItem.eventNumber++;
                }
            }
            else
            {
                CardSortOutputHandler.correct = false;
                cs.backEndItem.eventNumber++;
            }
        }
    }

    public bool isCorrect()
    {
        if (orderNumber == -1 && !collidingGO)
            return true;
        else if (!collidingGO)
            return false;
        return orderNumber == collidingGO.GetComponent<CardObject>().orderNumber;
    }

    //TODO: Bug when moving cards around:
    //      If you place a card on a spot, add a card to another spot then move that card to an already used spot it messes up the cards and locks one down

    void OnTriggerEnter2D(Collider2D coll)
    {
        print("Hit");
        if(!coll.gameObject.Equals(collidingGO))
        {
            isColliding = true;
            collidingGO = coll.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if(coll.gameObject.Equals(collidingGO))
        {
            isColliding = false;
            collidingGO = null;
        }
    }
}

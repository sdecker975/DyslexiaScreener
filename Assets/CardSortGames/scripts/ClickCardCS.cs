using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCardCS : MonoBehaviour {

	float x;
	float y;

	CardSortTestHandler cs;
	EventSystem.typeOfEvent e;

    float deltaX, deltaY;
    public bool isClicked = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		x = Input.mousePosition.x;
		y = Input.mousePosition.y;

		cs = Camera.main.GetComponent<CardSortTestHandler>();
		e = cs.backEndItem.currentEvent.type;
	}

	void OnMouseDrag()
	{
		if (e == EventSystem.typeOfEvent.Mouse && GameObject.Find("GreenButton") == false) {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 10.0f));
            if (GetComponent<BoxCollider2D>().OverlapPoint (worldPoint) || isClicked) {
                if(!isClicked)
                {
                    deltaX = transform.position.x - worldPoint.x;
                    deltaY = transform.position.y - worldPoint.y;
                    isClicked = true;
                }
				transform.position = new Vector3 (worldPoint.x + deltaX, worldPoint.y + deltaY, 10.0f);
			}
		}
	}

    private void OnMouseUp()
    {
        isClicked = false;
        deltaX = deltaY = 0;
    }
}

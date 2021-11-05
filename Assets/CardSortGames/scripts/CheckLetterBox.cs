using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckLetterBox : MonoBehaviour {

	float x;
	float y;

	CardSortTestHandler cs;
	EventSystem.typeOfEvent e;

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

	void OnMouseDown(){
		if (e == EventSystem.typeOfEvent.Mouse) {
			if (GetComponent<BoxCollider2D> ().OverlapPoint (Camera.main.ScreenToWorldPoint (new Vector3 (x, y, 10.0f)))) {
				if (gameObject.name.Contains ("true")) {
					if (!cs.frontEndItem.isExample) {
						Debug.Log ("correct!");
					}

					if (!cs.backEndItem.currentEvent.jumpLabel.Equals ("")) {
						for (int i = cs.backEndItem.eventNumber + 1; i < cs.backEndItem.events.Length; i++) {
							if (cs.backEndItem.currentEvent.jumpLabel.Equals (cs.backEndItem.events [i].jumpLabel)) {
								cs.backEndItem.eventNumber = i;
								break;
							}
						}
					} else {
						cs.backEndItem.eventNumber++;
					}
				} else {
					if (!cs.frontEndItem.isExample) {
						Debug.Log ("incorrect :(");
					}
					cs.backEndItem.eventNumber++;
				}
			}
		}
	}
}

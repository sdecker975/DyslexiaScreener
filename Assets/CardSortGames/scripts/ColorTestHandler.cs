using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * 
 * TestHandler for Color Sort card test
 * 
 */

[System.Serializable]
public class CSTestItem
{
	public bool isDemo;
	public GameObject[] Cards;
	public GameObject CardToSort;
	public bool[] CorrectShape;
	public bool hasGreenButton;
	public bool hasLetterBox;
	public GameObject LetterBox;
	public bool nextGame;
	public string nextGameName;
    public bool skipPrint;
}

public class ColorTestHandler : TestHandler {

	public List<CSTestItem> testItemFrontEnd;
	public CSTestItem frontEndItem;
	public GameObject greenButton;

	private ColorTestHandler cs;
	private EventSystem.typeOfEvent e;

	// Use this for initialization
	void Start()
	{
		setNextTestItem();
		base.mouseIsDone = false;
		CreateTest(frontEndItem);
		cs = Camera.main.GetComponent<ColorTestHandler>();
	}

	// Update is called once per frame
	protected override void Update()
	{
		e = cs.backEndItem.currentEvent.type;

		if (base.nextTest)
		{
			LoadNextTest();
		}
	}
		
	public void HideGreenButton(){
		if (e == EventSystem.typeOfEvent.Mouse) {
            OutputHandler.ResetTimer();
			greenButton.SetActive (false);
            //play audio "Go"
		}
	}

	public void GoToNextTest(){
		LoadNextTest();
	}

	/////this might need to be changed for specific tests!!!
	public void CreateTest(CSTestItem t)
	{
        //ToDo: this is where I need to link the card sort stuff with the rest of the test
		if (t.nextGame) {
			SceneManager.LoadScene (t.nextGameName);
		}

		//this will give us the world unit width of the screen (doubled it to account for the negative side)
		float fullScreenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
		//divide the screen width by the amount of objects in the array to get offset from left side
		float offsetX = fullScreenWidth / t.Cards.Length;

		float fullScreenHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;
		float offsetY = fullScreenHeight / 2;

		float leftScreenPos = -fullScreenWidth / 2 + offsetX / 2;
		float topScreenPos = fullScreenHeight / 2 - offsetY / 2;

		GameObject o;

		if (!GameObject.Find("origin"))
		{
			o = new GameObject("origin");
			o.transform.position = new Vector3(0, -topScreenPos, 0);
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;
        }

		if (!GameObject.Find("letterBox"))
		{
			o = new GameObject("letterBox");
			o.transform.position = new Vector3(0, topScreenPos, 0);
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;
        }

		for (int i = 0; i < t.Cards.Length; i++)
		{
			o = Instantiate(t.Cards[i], new Vector3(leftScreenPos + offsetX * i, topScreenPos, 0), new Quaternion());
			o.name = i.ToString() + "clone";
			//o.GetComponent<SpriteRenderer> ().sortingLayerName = "bottom";
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;

            if (t.CorrectShape [i]) {
				o.name = o.name + "true";
			}
		}
			
		o = Instantiate (t.CardToSort, new Vector3 (0, -topScreenPos, 0), new Quaternion ());
		o.name = "sortCardclone";
        o.transform.localScale = Vector3.one * fullScreenHeight / 4f;

        if (t.hasGreenButton) {
			greenButton.SetActive (true);
			greenButton.GetComponent<Button>().interactable = false;
		}

		if (t.hasLetterBox) {
			o = Instantiate (t.LetterBox, new Vector3(0, topScreenPos, 0), new Quaternion ());
			o.name = "letterBoxclone";
		}

        if (!t.skipPrint)
            OutputHandler.StartTimer(0);
    }

	public void DeletePrevTest()
	{
		GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
		foreach (GameObject go in allObjects)
		{
			if (go.name.Contains("clone"))
			{
				Destroy(go);
			}
		}
	}

	public void LoadNextTest()
	{
        if (!testItemFrontEnd[currentTestNumber].skipPrint)
            OutputHandler.StopTimer(currentTestNumber, testAbbrev);
        DeletePrevTest();
		currentTestNumber++;
		//setNextTestItem();
		//currEvent = 0;
		if (currentTestNumber < testItemFrontEnd.Count)
		{
            print("hit");
            setNextTestItem();
			CreateTest(frontEndItem);
		}
		else
		{
            print("hit2");
            SceneHandler.GoToNextScene();
        }
    }

	void setNextTestItem()
	{
		base.backEndItem = base.testItemBackEnd[base.currentTestNumber];
		frontEndItem = testItemFrontEnd[base.currentTestNumber];
		base.nextTest = false;
	}
}

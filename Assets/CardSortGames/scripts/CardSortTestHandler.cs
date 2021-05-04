using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSortTestHandler : TestHandler {

    public List<CardSortTestItem> testItemFrontEnd;
    public CardSortTestItem frontEndItem;
    public int startingEvent;

    // Use this for initialization
    void Start()
    {
        base.currentTestNumber = startingEvent;
        setNextTestItem();
        base.mouseIsDone = false;
        SQLHandler.UpdateTest(8);
        CreateTest(frontEndItem);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (base.nextTest)
        {
            LoadNextTest();
        }
    }

    /////this might need to be changed for specific tests!!!
    public void CreateTest(CardSortTestItem t)
    {
        //ToDo: this is where I need to link the card sort stuff with the rest of the test
        //if (t.nextGame)
        //{
        //    SceneManager.LoadScene(t.nextGameName);
        //}

        float fullScreenHeight = Camera.main.orthographicSize * 2;
        float fullScreenWidth = fullScreenHeight * 16 / 9; // basically height * screen aspect ratio

        //this will give us the world unit width of the screen (doubled it to account for the negative side)
        //divide the screen width by the amount of objects in the array to get offset from left side
        //changed this to fix something, make sure it didn't break everything else
        float offsetX = fullScreenWidth / t.Cards.Length;

        float offsetY = fullScreenHeight / 2;

        float leftScreenPos = -fullScreenWidth / 2 + offsetX / 2;
        float topScreenPos = fullScreenHeight / 2 - offsetY / 2;

        GameObject o;

        if (!GameObject.Find("origin"))
        {
            o = new GameObject("origin");
            o.transform.position = new Vector3(0, -offsetY / 2, 0);
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;

            o = new GameObject("midOrigin");
            o.transform.position = new Vector3(0, 0, 0);
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;
        }

        if (!GameObject.Find("letterBox"))
        {
            o = new GameObject("letterBox");
            o.transform.position = new Vector3(0, offsetY / 2, 0);
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;
        }

        for (int i = 0; i < t.Cards.Length; i++)
        {
            o = Instantiate(t.Cards[i], new Vector3(leftScreenPos + offsetX * i, topScreenPos, 0), new Quaternion());
            o.name = i.ToString() + "clone";
            //o.GetComponent<SpriteRenderer> ().sortingLayerName = "bottom";
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;

            if (t.isCorrect[i])
            {
                o.name = o.name + "true";
            }
        }

        if(t.CardToSort)
        {
            o = Instantiate(t.CardToSort, new Vector3(0, -topScreenPos, 0), new Quaternion());
            o.name = "sortCardclone";
            o.transform.localScale = Vector3.one * fullScreenHeight / 4f;
        }

        //if (t.hasGreenButton)
        //{
        //    greenButton.SetActive(true);
        //    greenButton.GetComponent<Button>().interactable = false;
        //}

        if (t.hasLetterBox)
        {
            o = Instantiate(t.LetterBox, new Vector3(0, topScreenPos, 0), new Quaternion());
            o.name = "letterBoxclone";
        }
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
            CardSortOutputHandler.StopTimer(testAbbrev, testItemFrontEnd[currentTestNumber].id);
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
            SQLHandler.UpdateTest(1);
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

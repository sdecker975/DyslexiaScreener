using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

/*
 * 
 * TestHandler for Sequential Language
 * 
 */

public class WITestHandler : TestHandler
{
    public List<WITestItem> testItemFrontEnd;
    public WITestItem frontEndItem;
    public int startingEvent;

    // Use this for initialization
    void Start()
    {
        base.currentTestNumber = startingEvent;
        setNextTestItem();
        base.mouseIsDone = false;
        SQLHandler.InsertTest();
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

    public void CreateTest(WITestItem t)
    {
        float fullScreenHeight = Camera.main.orthographicSize * 2;
        float fullScreenWidth = fullScreenHeight * 16 / 9; // basically height * screen aspect ratio

        //this will give us the world unit width of the screen (doubled it to account for the negative side)
        //divide the screen width by the amount of objects in the array to get offset from left side
        //changed this to fix something, make sure it didn't break everything else
        float offsetX = fullScreenWidth / (t.LetterButtons.Length + 2);

        float offsetY = fullScreenHeight / 2;

        float leftScreenPos = -fullScreenWidth / 2 + offsetX / 2;
        float topScreenPos = fullScreenHeight / 2 - offsetY / 2;

        GameObject o;
        if (!GameObject.Find("origin"))
        {
            o = new GameObject("origin");
            o.transform.position = new Vector3(0, -offsetY / 2, 0);
            TextMesh tm = o.AddComponent<TextMesh>();
            tm.alignment = TextAlignment.Center;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.fontSize = 100;
            tm.color = Color.black;
            o.transform.localScale = Vector3.one * fullScreenHeight / 6f;

            o = new GameObject("topOrigin");
            o.transform.position = new Vector3(0, offsetY / 2, 0);
            tm = o.AddComponent<TextMesh>();
            tm.alignment = TextAlignment.Center;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.fontSize = 100;
            tm.color = Color.black;
            o.transform.localScale = Vector3.one * fullScreenHeight / 6f;
            //o.layer = LayerMask.NameToLayer("fade");

            o = new GameObject("midOrigin");
            o.transform.position = new Vector3(0, 0, 0);
            tm = o.AddComponent<TextMesh>();
            tm.alignment = TextAlignment.Center;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.fontSize = 100;
            tm.color = Color.black;
            o.transform.localScale = Vector3.one * fullScreenHeight / 6f;
            //o.layer = LayerMask.NameToLayer("fade");

            o = new GameObject("audio");
            o.transform.position = new Vector3((-fullScreenWidth / 16) * 5, -offsetY / 2, 0);

            o = new GameObject("arrow");
            o.transform.position = new Vector3((fullScreenWidth / 16) * 5, -offsetY / 2, 0);
        }
        for (int i = 0; i < t.LetterButtons.Length; i++)
        {
            print("words");
            offsetX = fullScreenWidth / 10;
            float dist = fullScreenHeight / 3f;
            o = Instantiate(t.LetterButtons[i], new Vector3((i - 2) * dist + dist / 2, offsetY / 3, 0), new Quaternion());
            o.layer = LayerMask.NameToLayer("fadeOut");
            if (t.isCorrect[i])
                o.name = "correct clone";
            else
                o.name = "incorrect clone";
            GameObject child = new GameObject("image");
            SpriteRenderer s = child.AddComponent<SpriteRenderer>();
            s.sortingOrder = 0;
            s.sprite = t.wordPictures[i];
            child.transform.parent = o.transform;
            child.transform.localPosition = new Vector3(0, 0, 0);
            o.GetComponent<ClickCardWI>().isCorrect = t.isCorrect[i];
            o.GetComponent<ClickCardWI>().responsePosition = i + 1;
            o.GetComponent<ClickCardWI>().responseName = t.wordPictures[i].name;
            o.transform.localScale = Vector3.one * fullScreenHeight / 10f * 3f;
            //GameObject.Destroy(o.transform.GetChild(0).gameObject);
        }

        print(GameObject.Find("origin").transform.position);
        o = Instantiate(t.sentenceBox, GameObject.Find("origin").transform.position, new Quaternion());
        o.transform.position = GameObject.Find("origin").transform.position;
        o.GetComponent<RectTransform>().sizeDelta = new Vector2(fullScreenWidth / 4 * 2, fullScreenHeight / 5);
        o.name = "sentence clone";
        o.GetComponent<TMP_Text>().text = t.displayedText;
        o.transform.localScale = Vector3.one * fullScreenHeight / 10f;
        //GameObject.Find("origin").GetComponent<TextMesh>().text = t.displayedText;

        //if (!t.skipPrint)
        //    //may need to be 0, check if resetTimer
        //    WIOutputHandler.StartTimer(0);
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
            WIOutputHandler.StopTimer(testAbbrev, testItemFrontEnd[currentTestNumber].id);
        DeletePrevTest();
        currentTestNumber++;
        //currEvent = 0;
        if (currentTestNumber < testItemFrontEnd.Count)
        {
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

    public void SortItems()
    {
        List<int> posValues = new List<int>();
        foreach (WITestItem l in testItemFrontEnd)
        {
            posValues.Add(l.pos);
            print(l.pos);
        }
        IEnumerator position = posValues.GetEnumerator();
        testItemBackEnd = testItemBackEnd.OrderBy(p =>
        {
            position.MoveNext();
            return position.Current;
        }
        ).ToList();
        testItemFrontEnd = testItemFrontEnd.OrderBy(o => o.pos).ToList();
    }



    public void ResetItemPositions()
    {
        int counter = 0;
        foreach (WITestItem l in testItemFrontEnd)
        {
            l.pos = counter;
            counter++;
        }
    }


}

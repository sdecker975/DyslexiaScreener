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

public class RCTestHandler : TestHandler
{

    public List<RCTestItem> testItemFrontEnd;
    public RCTestItem frontEndItem;
    bool replayButtonDown = false;
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
        if (replayButtonDown && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = null;
            replayButtonDown = false;
        }
    }

    public void CreateTest(RCTestItem t)
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
            offsetY = fullScreenHeight / 18;
            o = Instantiate(t.LetterButtons[i], new Vector3(0, fullScreenHeight / 2 - offsetY * ((i * 2) + 7), 0), new Quaternion());
            if (t.isCorrect[i])
                o.name = "correct clone";
            else {
                o.name = "incorrect clone";
               
            }
            
            string text = t.testSounds[i].Replace("N_L", "\n");
            o.transform.Find("TextMeshObject").GetComponent<TMP_Text>().text = text;
            o.GetComponent<ClickCardRC>().isCorrect = t.isCorrect[i];
            o.GetComponent<ClickCardRC>().responsePosition = i + 1;
            o.GetComponent<ClickCardRC>().responseName = text;
            o.transform.localScale = Vector3.one * fullScreenHeight / 10f;
        }
        
        o = Instantiate(t.sentenceBox, new Vector3(0, fullScreenHeight / 2 - offsetY * 4, 0), new Quaternion());
        o.transform.position = new Vector3(0, fullScreenHeight / 2 - offsetY * 4, 0);
        o.name = "sentenceBox clone";
        o.GetComponent<RectTransform>().sizeDelta = new Vector2(fullScreenWidth / 3 * 2, fullScreenHeight / 6);
        o.GetComponent<TMP_Text>().text = t.displayedText;
        o.transform.localScale = Vector3.one * fullScreenHeight / 10f;
        
        //if (!t.skipPrint)
        //    //may need to be 0, check if resetTimer
        //    RCOutputHandler.StartTimer(0);
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
            RCOutputHandler.StopTimer(testAbbrev, testItemFrontEnd[currentTestNumber].id);
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
        foreach (RCTestItem l in testItemFrontEnd)
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

}

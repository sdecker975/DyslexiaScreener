using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

/*
 * 
 * TestHandler for Letter Identification
 * 
 */

public class LITestHandler : TestHandler
{
    public List<LITestItem> testItemFrontEnd;
    public LITestItem frontEndItem;
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

    public void ReplayButton()
    {
        print(base.backEndItem.currentEvent.type);

        if (!replayButtonDown && base.backEndItem.currentEvent.type == EventSystem.typeOfEvent.Mouse)
        {
            GetComponent<AudioSource>().clip = frontEndItem.replay;
            GetComponent<AudioSource>().Play();
            replayButtonDown = true;
        }
    }

    public void CreateTest(LITestItem t)
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
            if (t.LetterButtons.Length == 4)
            {
                float sideValue = (i % 2 == 0) ? -2f : 2f;
                float upValue = (i / 2 == 0) ? 2f : -2f;
                o = Instantiate(t.LetterButtons[i], new Vector3(sideValue, upValue, 0), new Quaternion());
                o.transform.localScale = Vector3.one * fullScreenHeight / 3f;
            }
            else
            {
                o = Instantiate(t.LetterButtons[i], new Vector3(leftScreenPos + offsetX * (i + 1), i % 2 * 2f, 0), new Quaternion());
                o.transform.localScale = Vector3.one * fullScreenHeight / 6f;
            }
            o.layer = LayerMask.NameToLayer("fadeOut");
            if (t.isCorrect[i])
                o.name = "correct clone";
            else
                o.name = "incorrect clone";
            GameObject text = new GameObject();
            text.transform.parent = o.transform;
            float charCountRatio = t.testSounds[i].Length == 0 ? 1 : t.testSounds[i].Length;
            text.transform.localScale = new Vector3(.125f / charCountRatio, .125f / charCountRatio, 1);
            text.transform.localPosition = new Vector3(0, 0, -1);
            TextMesh tm = text.AddComponent<TextMesh>();
            tm.alignment = TextAlignment.Center;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.fontSize = 60;
            tm.color = Color.black;
            tm.text = t.testSounds[i] + "";
            o.GetComponent<ClickCardLI>().isCorrect = t.isCorrect[i];
            o.GetComponent<ClickCardLI>().responsePosition = i + 1;
            o.GetComponent<ClickCardLI>().responseName = t.testSounds[i];
        }
        //This is to make sure we are only printing what we need
        //Will be used for all examples so event system will handle printing
        //if (!t.skipPrint)
        //    //may need to be 0, check if resetTimer
        //    LIOutputHandler.StartTimer(0);
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
            LIOutputHandler.StopTimer(testAbbrev, testItemFrontEnd[currentTestNumber].id);
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
}
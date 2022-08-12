using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;
using System.Linq;

/*
 * 
 * TestHandler for Reading Fluency
 * 
 */

public class RFTestHandler : TestHandler
{
    public List<RFTestItem> testItemFrontEnd;
    public RFTestItem frontEndItem;
    bool replayButtonDown = false;
    public int startingEvent;
    public Font tnr;
    public float timeInSeconds;
    public Stopwatch timer = new Stopwatch();

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
        if (base.nextTest)
        {
            LoadNextTest();
        }
        if (replayButtonDown && !GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<AudioSource>().clip = null;
            replayButtonDown = false;
        }

        if(timer.ElapsedMilliseconds/1000f >= timeInSeconds)
        {
            //if(currentTestNumber != testItemFrontEnd.Count)
            //    OutputHandler.PrintScores(testAbbrev);
            //0 not taken
            //1 valid all items
            //2 valid ceiling 1
            //3 valid ceiling 2
            //4 valid timeout
            //5 valid failed teaching
            //6 invalid manual skipped test
            //7 exit care
            //8 started/invalid unknown
            //9 intended to take
            SQLHandler.UpdateTest(4);
            SceneHandler.GoToNextScene();
        }
    }

    public void SortItems()
    {
        testItemFrontEnd = testItemFrontEnd.OrderBy(o => o.pos).ToList();
    }

    public void ExportItems()
    {

    }

    public void ResetItemPositions()
    {
        int counter = 0;
        foreach (RFTestItem l in testItemFrontEnd)
        {
            l.pos = counter;
            counter++;
        }
    }

    public void CreateTest(RFTestItem t)
    {
        float fullScreenHeight = Camera.main.orthographicSize * 2;
        float fullScreenWidth = fullScreenHeight * 16 / 9; // basically height * screen aspect ratio
        //this will give us the world unit width of the screen (doubled it to account for the negative side)
        //divide the screen width by the amount of objects in the array to get offset from left side
        //changed this to fix something, make sure it didn't break everything else
        float offsetX = fullScreenWidth / 11;
        float sizeX = fullScreenWidth / 2;

        float offsetY = fullScreenHeight / 7;
        float sizeY = fullScreenHeight / 11;

        float leftScreenPos = -fullScreenWidth / 2 + offsetX / 2;
        float topScreenPos = fullScreenHeight / 2 - offsetY / 2;

        GameObject o;
        if (!GameObject.Find("origin"))
        {
            o = new GameObject("origin");
            o.transform.position = new Vector3(0, -offsetY * 2, 0);
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
        }
        if(t.isRetention)
        {
            o = Instantiate(t.buttonSet[0], new Vector3(0, 0, 0), new Quaternion());
            o.transform.localScale = Vector3.one * fullScreenHeight / 3;
            if (!t.isNotSure)
            {
                o.transform.Find("yes").GetComponent<RFButton>().isCorrect = t.isCorrect[0];
                o.transform.Find("yes").GetComponent<RFButton>().posNumber = 0;
                o.transform.Find("no").GetComponent<RFButton>().isCorrect = !t.isCorrect[0];
                o.transform.Find("no").GetComponent<RFButton>().posNumber = 1;
                o.transform.Find("notsure").GetComponent<RFButton>().isCorrect = false;
                o.transform.Find("notsure").GetComponent<RFButton>().posNumber = 2;
            }
            else
            {
                o.transform.Find("yes").GetComponent<RFButton>().isCorrect = false;
                o.transform.Find("yes").GetComponent<RFButton>().posNumber = 0;
                o.transform.Find("no").GetComponent<RFButton>().isCorrect = false;
                o.transform.Find("no").GetComponent<RFButton>().posNumber = 1;
                o.transform.Find("notsure").GetComponent<RFButton>().isCorrect = true;
                o.transform.Find("notsure").GetComponent<RFButton>().posNumber = 2;
            }
            o.name = "buttonSet 0 clone " + currentTestNumber;
        }
        else if(t.isButton)
        {
            o = Instantiate(t.buttonSet[0], new Vector3(0, 0, 0), new Quaternion());
            o.transform.localScale = Vector3.one * fullScreenHeight / 4;
            o.name = "start clone";
        }
        else
        {
            for (int i = 0; i < t.testSentences.Length; i++)
            {
                o = Instantiate(t.sentenceBox, new Vector3(-fullScreenWidth / 2 + sizeX / 2 + offsetX, fullScreenHeight / 2 - offsetY * (i + 1), 0), new Quaternion());
                o.transform.position = new Vector3(-fullScreenWidth / 2 + sizeX / 2 + offsetX, fullScreenHeight / 2 - offsetY * (i + 1), 0);
                o.layer = LayerMask.NameToLayer("fadeOut");
                o.name = "sentenceBox " + i + " clone " + currentTestNumber;
                o.GetComponent<TMP_Text>().text = t.testSentences[i];
                o.GetComponent<TMP_Text>().alignment = TextAlignmentOptions.Right;
                if(i != 0)
                {
                    Color c = o.GetComponent<TMP_Text>().color;
                    c.a = .3f;
                    o.GetComponent<TMP_Text>().color = c;
                }
                //o.GetComponent<TextMeshPro>().color = new Color(o.GetComponent<TextMeshPro>().color.r, o.GetComponent<TextMeshPro>().color.g, o.GetComponent<TextMeshPro>().color.b, .3f);
                o.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, sizeY);

                o.transform.localScale = Vector3.one * fullScreenHeight / 10f;

                o = Instantiate(t.buttonSet[i], new Vector3(-fullScreenWidth / 2 + (sizeX / 2 * 3), fullScreenHeight / 2 - offsetY * (i + 1), 0), new Quaternion());
                o.transform.position = new Vector3(-fullScreenWidth / 2 + (sizeX / 2 * 3), fullScreenHeight / 2 - offsetY * (i + 1), 0);
                foreach (Transform child in o.transform)
                    child.gameObject.GetComponent<RFButton>().buttonItemNumber = i;

                o.transform.Find("yes").GetComponent<RFButton>().isCorrect = t.isCorrect[i];
                o.transform.Find("yes").gameObject.layer = LayerMask.NameToLayer("fadeOut");
                o.transform.Find("yes").GetComponent<RFButton>().posNumber = 0;
                o.transform.Find("no").GetComponent<RFButton>().isCorrect = !t.isCorrect[i];
                o.transform.Find("no").gameObject.layer = LayerMask.NameToLayer("fadeOut");
                o.transform.Find("no").GetComponent<RFButton>().posNumber = 1;
                o.transform.Find("notsure").GetComponent<RFButton>().isCorrect = false;
                o.transform.Find("notsure").gameObject.layer = LayerMask.NameToLayer("fadeOut");
                o.transform.Find("notsure").GetComponent<RFButton>().posNumber = 2;

                if (i != 0)
                {
                    Color y = o.transform.Find("yes").GetComponent<SpriteRenderer>().color;
                    y.a = .3f;
                    o.transform.Find("yes").GetComponent<SpriteRenderer>().color = y;
                    Color n = o.transform.Find("no").GetComponent<SpriteRenderer>().color;
                    n.a = .3f;
                    o.transform.Find("no").GetComponent<SpriteRenderer>().color = n;
                    Color s = o.transform.Find("notsure").GetComponent<SpriteRenderer>().color;
                    s.a = .3f;
                    o.transform.Find("notsure").GetComponent<SpriteRenderer>().color = s;
                }

                o.layer = LayerMask.NameToLayer("fadeOut");
                o.name = "buttonSet " + i + " clone " + currentTestNumber;
            }
            GameObject.Find("buttonSet 0 clone " + currentTestNumber + "/yes").GetComponent<RFButton>().changeLayerToFadeIn();
        }


        if (t.startTimer)
        {
            timer.Start();
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

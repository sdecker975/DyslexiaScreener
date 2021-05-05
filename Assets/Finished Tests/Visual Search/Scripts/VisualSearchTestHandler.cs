using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using TMPro;
using System.Linq;

/*
 * 
 * TestHandler for Sequential Language
 * 
 */

public class VisualSearchTestHandler : TestHandler
{

    public List<VISTestItem> testItemFrontEnd;
    public VISTestItem frontEndItem;
    bool replayButtonDown = false;
    public int startingEvent;

    public GameObject c, g, ug;

    public float timeInSeconds;
    public Stopwatch timer = new Stopwatch();
    //public Font tnr;

    // Use this for initialization
    void Start()
    {
        base.currentTestNumber = startingEvent;
        setNextTestItem();
        base.mouseIsDone = false;
        SQLHandler.InsertTest();
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

        if (timer.ElapsedMilliseconds / 1000f >= timeInSeconds)
        {
            //if (currentTestNumber != testItemFrontEnd.Count)
            //    OutputHandler.PrintScores(testAbbrev);
            SQLHandler.UpdateTest(4);
            SceneHandler.GoToNextScene();
        }

    }

    public void SortItems()
    {
        testItemFrontEnd = testItemFrontEnd.OrderBy(o => o.pos).ToList();
    }

    public void ResetItemPositions()
    {
        int counter = 0;
        foreach (VISTestItem l in testItemFrontEnd)
        {
            l.pos = counter;
            counter++;
        }
    }

    public void CreateTest(VISTestItem t)
    {
        float fullScreenHeight = Camera.main.orthographicSize * 2;
        float fullScreenWidth = fullScreenHeight * 16 / 9; // basically height * screen aspect ratio

        //this will give us the world unit width of the screen (doubled it to account for the negative side)
        //divide the screen width by the amount of objects in the array to get offset from left side
        //changed this to fix something, make sure it didn't break everything else
        //float offsetX = fullScreenWidth / (t.LetterButtons.Length + 2);

        float offsetY = fullScreenHeight / 2;

        //float leftScreenPos = -fullScreenWidth / 2 + offsetX / 2;
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
            o.transform.localScale = Vector3.one * fullScreenHeight / 3f;

            o = new GameObject("topOrigin");
            o.transform.position = new Vector3(0, offsetY / 2, 0);
            tm = o.AddComponent<TextMesh>();
            tm.alignment = TextAlignment.Center;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.fontSize = 100;
            tm.color = Color.black;
            o.transform.localScale = Vector3.one * fullScreenHeight / 3f;
            //o.layer = LayerMask.NameToLayer("fade");

            o = new GameObject("midOrigin");
            o.transform.position = new Vector3(0, 0, 0);
            tm = o.AddComponent<TextMesh>();
            tm.alignment = TextAlignment.Center;
            tm.anchor = TextAnchor.MiddleCenter;
            tm.fontSize = 100;
            tm.color = Color.black;
            o.transform.localScale = Vector3.one * fullScreenHeight / 3f;
            //o.layer = LayerMask.NameToLayer("fade");

            o = new GameObject("audio");
            o.transform.position = new Vector3((-fullScreenWidth / 16) * 5, -offsetY / 2, 0);

            o = new GameObject("arrow");
            o.transform.position = new Vector3((fullScreenWidth / 16) * 5, -offsetY / 2, 0);
        }

        if(t.hasItem)
        {
            string fs = t.item.text;
            //whoever wrote this code is dumb, i found it somewhere, needs fixing
            string[] fLines = Regex.Split(fs, "\r\n|\n|\r");

            int x = 0, y = 0;

            string valueLine = fLines[0];
            string[] values = Regex.Split(valueLine, ","); // your splitter here

            Int32.TryParse(values[0], out x);
            Int32.TryParse(values[1], out y);

            print(x + " " + y);

            string[,] gridSearchChars = new string[y, x];
            string[,] gridSearchAngles = new string[y, x];

            print(fLines.Length);

            for (int i = 1; i < fLines.Length; i++)
            {
                valueLine = fLines[i];
                if(!valueLine.Equals(""))
                {
                    print(valueLine);
                    values = Regex.Split(valueLine, ",");
                    print(i + " " + gridSearchChars.Length);
                    if (i - 1 < y)
                    {
                        for (int j = 0; j < values.Length; j++)
                        {
                            print(values[j]);
                            gridSearchChars[i - 1, j] = values[j];
                        }
                    }
                    else if (i - 1 >= y)
                    {
                        for (int j = 0; j < values.Length; j++)
                        {
                            print(values[j] + " " + j + " " + (i - 1 - y));
                            gridSearchAngles[i - 1 - y, j] = values[j];
                        }
                    }
                }
            }

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    if(!t.hasExampleScaling)
                    {
                        if (gridSearchChars[i, j].Equals("C"))
                        {
                            o = Instantiate(c, new Vector3(j - ((float)x / 2f) + .5f, -(i - ((float)y / 2f) + .5f), 0), new Quaternion());
                            o.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Int32.Parse(gridSearchAngles[i, j]));
                            o.GetComponent<VisualSearchClick>().x = j;
                            o.GetComponent<VisualSearchClick>().y = i;
                            o.name = "C clone";
                        }
                        if (gridSearchChars[i, j].Equals("G"))
                        {
                            o = Instantiate(g, new Vector3(j - ((float)x / 2f) + .5f, -(i - ((float)y / 2f) + .5f), 0), new Quaternion());
                            o.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Int32.Parse(gridSearchAngles[i, j]));
                            o.GetComponent<VisualSearchClick>().x = j;
                            o.GetComponent<VisualSearchClick>().y = i;
                            o.name = "G clone";
                        }
                        if (gridSearchChars[i, j].Equals("A"))
                        {
                            o = Instantiate(ug, new Vector3(j - ((float)x / 2f) + .5f, -(i - ((float)y / 2f) + .5f), 0), new Quaternion());
                            o.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Int32.Parse(gridSearchAngles[i, j]));
                            o.GetComponent<VisualSearchClick>().x = j;
                            o.GetComponent<VisualSearchClick>().y = i;
                            o.name = "UG clone";
                        }
                    }
                    else
                    {
                        float scale = fullScreenHeight / 3f;
                        if (gridSearchChars[i, j].Equals("C"))
                        {
                            o = Instantiate(c, new Vector3(j - ((float)x / 2f) + .5f, -(i - ((float)y / 2f) + .5f), 0) * scale, new Quaternion());
                            o.transform.localScale = Vector3.one * scale;
                            o.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Int32.Parse(gridSearchAngles[i, j]));
                            o.GetComponent<VisualSearchClick>().x = j;
                            o.GetComponent<VisualSearchClick>().y = i;
                            o.name = "C clone";
                        }
                        if (gridSearchChars[i, j].Equals("G"))
                        {
                            o = Instantiate(g, new Vector3(j - ((float)x / 2f) + .5f, -(i - ((float)y / 2f) + .5f), 0) * scale, new Quaternion());
                            o.transform.localScale = Vector3.one * scale;
                            o.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Int32.Parse(gridSearchAngles[i, j]));
                            o.GetComponent<VisualSearchClick>().x = j;
                            o.GetComponent<VisualSearchClick>().y = i;
                            o.name = "G clone";
                        }
                        if (gridSearchChars[i, j].Equals("A"))
                        {
                            o = Instantiate(ug, new Vector3(j - ((float)x / 2f) + .5f, -(i - ((float)y / 2f) + .5f), 0) * scale, new Quaternion());
                            o.transform.localScale = Vector3.one * scale;
                            o.transform.GetChild(0).localEulerAngles = new Vector3(0, 0, Int32.Parse(gridSearchAngles[i, j]));
                            o.GetComponent<VisualSearchClick>().x = j;
                            o.GetComponent<VisualSearchClick>().y = i;
                            o.name = "UG clone";
                        }
                    }
                }
            }
        }

        if (t.startTimer)
            timer.Start();

        //if (!t.skipPrint)
        //    VISOutputHandler.StartTimer(0);
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
            VISOutputHandler.StopTimer(testItemFrontEnd[currentTestNumber].id);
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

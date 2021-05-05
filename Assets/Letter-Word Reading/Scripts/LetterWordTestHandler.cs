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

[System.Serializable]
public class LWTestItem
{
    public int pos;
    public string id;

    public bool isSounds;
    public bool isWords;
    public bool isSentences;
    public bool isButtons;
    public bool isExample;
    public bool skipPrint;
    public char[] testLetters;
    public string[] testSounds;
    public Sprite[] wordPictures;
    public bool[] isCorrect;
    public string displayedText;
    public GameObject[] LetterButtons;
    public GameObject sentenceBox;
    public AudioClip replay;
    public bool useText = true;
    public int audioLoopBack;
}

public class LetterWordTestHandler : TestHandler {

    public List<LWTestItem> testItemFrontEnd;
    public LWTestItem frontEndItem;
    bool replayButtonDown = false;
    public int startingEvent;
    public Font tnr;
    public bool multiReplay;

    // Use this for initialization
    void Start()
    {
        base.currentTestNumber = startingEvent;
        setNextTestItem();
        base.mouseIsDone = false;
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

    static List<TestItem> quicksort(List<int> posList, List<TestItem> testItems)
    {
        if (testItems.Count <= 1) return testItems;
        int pivotPosition = posList.Count / 2;
        int pivotValue = posList[pivotPosition];
        TestItem pivotTestItem = testItems[pivotPosition];
        posList.RemoveAt(pivotPosition);
        testItems.RemoveAt(pivotPosition);

        List<int> smaller = new List<int>();
        List<TestItem> smallerTestItems = new List<TestItem>();
        List<int> greater = new List<int>();
        List<TestItem> greaterTestItems = new List<TestItem>();

        int index = 0;
        foreach (int item in posList)
        {
            if (item < pivotValue)
            {
                smaller.Add(item);
                smallerTestItems.Add(testItems[index]);
            }
            else
            {
                greater.Add(item);
                greaterTestItems.Add(testItems[index]);
            }
            index++;
        }
        List<TestItem> sorted = quicksort(smaller, testItems);
        sorted.Add(pivotTestItem);
        sorted.AddRange(quicksort(greater, testItems));
        return sorted;
    }

    public void SortItems()
    {
        List<int> posValues = new List<int>();
        foreach (LWTestItem l in testItemFrontEnd)
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

    public void ExportItems()
    {
        int counter = 1;
        string filePath = "testItems/" + testAbbrev + "_items.csv";
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
        fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
        {
            file.Write("Item Number, Event Number, Correct Number, Display Text, Correct Item, Option 1, Option 2, Option 3, Option 4, Option 5, Option 6, Option 7,\n");
            
        }
        int eventSystemCounter = 0;
        foreach (LWTestItem l in testItemFrontEnd)
        {
            if(!l.isExample)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    file.Write(counter + ",");
                    file.Write(eventSystemCounter + ",");
                    counter++;
                    for (int i = 0; i < l.isCorrect.Length; i++)
                    {
                        if (l.isCorrect[i])
                        {
                            file.Write((i+1) + ",");
                            file.Write(l.displayedText);
                            if (l.isSounds)
                                file.Write(l.testSounds[i] + ",");
                            else if (l.isWords)
                                file.Write(l.wordPictures[i].name);
                        }
                    }
                    if(l.isSounds)
                    {
                        for (int i = 0; i < l.testSounds.Length; i++)
                        {
                            file.Write(l.testSounds[i] + ",");
                        }
                    }
                    if(l.isWords)
                    {
                        for (int i = 0; i < l.testSounds.Length; i++)
                        {
                            file.Write(l.wordPictures[i].name + ",");
                        }
                    }
                    file.Write("\n");
                }
            }
            eventSystemCounter++;
        }
    }

    public void ResetItemPositions()
    {
        int counter = 0;
        foreach (LWTestItem l in testItemFrontEnd)
        {
            l.pos = counter;
            counter++;
        }
    }

    public void ReplayButton()
    {
        print(base.backEndItem.currentEvent.type);

        if (!replayButtonDown && base.backEndItem.currentEvent.type == EventSystem.typeOfEvent.Mouse)
        {
            if(multiReplay)
            {
                base.backEndItem.eventNumber = frontEndItem.audioLoopBack;
            }
            else
            {
                GetComponent<AudioSource>().clip = frontEndItem.replay;
                GetComponent<AudioSource>().Play();
                replayButtonDown = true;
            }
        }
    }

    public void CreateTest(LWTestItem t)
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
            o.transform.position = new Vector3((-fullScreenWidth/16) * 5, -offsetY / 2, 0);

            o = new GameObject("arrow");
            o.transform.position = new Vector3((fullScreenWidth/16) * 5, -offsetY / 2, 0);
        }
        for (int i = 0; i < t.LetterButtons.Length; i++)
        {
            if (t.isSounds)
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
                if(t.isCorrect[i])
                    o.name = "correct clone";
                else
                    o.name = "incorrect clone";
                GameObject text = new GameObject();
                text.transform.parent = o.transform;
                float charCountRatio = t.testSounds[i].Length == 0 ? 1 : t.testSounds[i].Length;
                text.transform.localScale = new Vector3(.125f/charCountRatio, .125f/charCountRatio, 1);
                text.transform.localPosition = new Vector3(0, 0, -1);
                TextMesh tm = text.AddComponent<TextMesh>();
                tm.alignment = TextAlignment.Center;
                tm.anchor = TextAnchor.MiddleCenter;
                tm.fontSize = 60;
                //tm.font = (Font)Resources.Load<Font>("Fonts/TimesNewRoman");
                //tm.font = tnr;
                tm.color = Color.black;
                tm.text = t.testSounds[i] + "";
                o.GetComponent<ClickCardLSSI>().isCorrect = t.isCorrect[i];
                o.GetComponent<ClickCardLSSI>().responsePosition = i + 1;
                o.GetComponent<ClickCardLSSI>().responseName = t.testSounds[i];
            }
            else if(t.isWords)
            {
                print("words");
                offsetX = fullScreenWidth / 10;
                float dist = fullScreenHeight / 3f;
                o = Instantiate(t.LetterButtons[i], new Vector3((i - 2) * dist + dist / 2, offsetY/3, 0), new Quaternion());
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
                o.GetComponent<ClickCardLSSI>().isCorrect = t.isCorrect[i];
                o.GetComponent<ClickCardLSSI>().responsePosition = i + 1;
                o.GetComponent<ClickCardLSSI>().responseName = t.wordPictures[i].name;
                o.transform.localScale = Vector3.one * fullScreenHeight / 10f * 3f;
                //GameObject.Destroy(o.transform.GetChild(0).gameObject);
            }
            else if(t.isSentences)
            {
                offsetY = fullScreenHeight / 18;
                o = Instantiate(t.LetterButtons[i], new Vector3(0, fullScreenHeight/2 - offsetY * ((i*2)+7), 0), new Quaternion());
                if (t.isCorrect[i])
                    o.name = "correct clone";
                else
                    o.name = "incorrect clone"; string text = t.testSounds[i].Replace("N_L", "\n");
                o.transform.Find("TextMeshObject").GetComponent<TMP_Text>().text = text;
                o.GetComponent<ClickCardLSSI>().isCorrect = t.isCorrect[i];
                o.GetComponent<ClickCardLSSI>().responsePosition = i + 1;
                o.GetComponent<ClickCardLSSI>().responseName = text;
                o.transform.localScale = Vector3.one * fullScreenHeight / 10f;
            }
            else if(t.isButtons)
            {
                print("words");
                offsetX = fullScreenWidth / 5;
                float scale = fullScreenHeight / 10f * 3f;
                o = Instantiate(t.LetterButtons[i], new Vector3(leftScreenPos + offsetX * ((i * 2) + 1), 0, 0), new Quaternion());
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
                o.GetComponent<ClickCardLSSI>().isCorrect = t.isCorrect[i];
                o.GetComponent<ClickCardLSSI>().responsePosition = i + 1;
                o.GetComponent<ClickCardLSSI>().responseName = t.wordPictures[i].name;
                o.transform.localScale = Vector3.one * scale;
                //GameObject.Destroy(o.transform.GetChild(0).gameObject);
            }
            //else
            //{
            //    print("In else statement");
            //    o = Instantiate(t.LetterButtons[i], new Vector3(leftScreenPos + offsetX * i, 0, 0), new Quaternion());
            //    o.layer = LayerMask.NameToLayer("fadeOut");
            //    if (t.isCorrect[i])
            //        o.name = "correct clone";
            //    else
            //        o.name = "incorrect clone";
            //    TextMesh tm = o.AddComponent<TextMesh>();
            //    tm.text = t.testLetters[i] + "";
            //    tm.alignment = TextAlignment.Center;
            //    tm.anchor = TextAnchor.MiddleCenter;
            //    tm.fontSize = 100;
            //    o.GetComponent<TextChecker>().isCorrect = t.isCorrect[i];
            //    o.GetComponent<TextChecker>().itemNumber = i + 1;
            //    o.transform.localScale = Vector3.one * fullScreenHeight / 6f;
            //}
        }
        if (t.isWords && t.useText)
        {
			print (GameObject.Find ("origin").transform.position);
            o = Instantiate(t.sentenceBox, GameObject.Find("origin").transform.position, new Quaternion());
			o.transform.position = GameObject.Find ("origin").transform.position;
            o.GetComponent<RectTransform>().sizeDelta = new Vector2(fullScreenWidth / 4 * 2, fullScreenHeight / 5);
            o.name = "sentence clone";
            o.GetComponent<TMP_Text>().text = t.displayedText;
            o.transform.localScale = Vector3.one * fullScreenHeight / 10f;
            //GameObject.Find("origin").GetComponent<TextMesh>().text = t.displayedText;
        }
        else if(t.isSentences)
        {
            o = Instantiate(t.sentenceBox, new Vector3(0, fullScreenHeight / 2 - offsetY * 4, 0), new Quaternion());
            o.transform.position = new Vector3(0, fullScreenHeight / 2 - offsetY * 4, 0);
            o.name = "sentenceBox clone";
            o.GetComponent<RectTransform>().sizeDelta = new Vector2(fullScreenWidth / 3 * 2, fullScreenHeight / 6);
            o.GetComponent<TMP_Text>().text = t.displayedText;
            o.transform.localScale = Vector3.one * fullScreenHeight / 10f;
        }
        //This is to make sure we are only printing what we need
        //Will be used for all examples so event system will handle printing
        if(!t.skipPrint)
            //may need to be 0, check if resetTimer
            OutputHandler.StartTimer(t.replay?t.replay.length:0);
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
        if(!testItemFrontEnd[currentTestNumber].skipPrint)
            OutputHandler.StopTimer(currentTestNumber, testAbbrev);
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

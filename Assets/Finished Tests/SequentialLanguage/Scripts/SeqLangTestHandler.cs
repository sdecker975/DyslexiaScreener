using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

/*
 * 
 * TestHandler for Sequential Language
 * 
 */

public class SeqLangTestHandler : TestHandler
{
    public List<CompTestItem> testItemFrontEnd;
    public CompTestItem frontEndItem { get; set; }

    // Use this for initialization
    void Start()
    {
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
        base.Update();
        if (frontEndItem.isExample)
        {
            if(Camera.main.GetComponent<TestChecker>().AllFilled() && !frontEndItem.isExplanation)
                CheckCorrectness();
        }
    }
    public void CheckCorrectness()
    {
        string userShapes = "";
        string userInput = "";
        string actualShapes = "";
        string actualInput = "";
        int points = 0;

        if (Camera.main.GetComponent<TestChecker>().CheckTestCorrectness() && backEndItem.currentEvent.jumpLabel == "correct")
        {
            for (int i = backEndItem.eventNumber + 1; i < backEndItem.events.Length; i++)
            {
                if (backEndItem.currentEvent.jumpLabel.Equals(backEndItem.events[i].jumpLabel))
                {
                    backEndItem.eventNumber = i;
                    frontEndItem.isExplanation = true;

                    foreach (GameObject t in Camera.main.GetComponent<TestChecker>().checkers)
                    {
                        if (t.GetComponent<SeqCardChecker>().shapeNumber == t.GetComponent<SeqCardChecker>().userShapeNumber)
                        {
                            points = 1;
                        }
                        else
                        {
                            points = 0;
                            break;
                        }
                    }
                    if (points != 0 && frontEndItem.useOrder)
                    {
                        foreach (GameObject t in Camera.main.GetComponent<TestChecker>().checkers)
                        {
                            if (t.GetComponent<SeqCardChecker>().inputNumber == t.GetComponent<SeqCardChecker>().userInputNumber)
                            {
                                points = 1;
                                //points = 2; //Michael commented this out and changed to 1 to match non-partial credit Rasch Model for test
                            }
                            else
                            {
                                points = 0;
                                //points = 1; //Michael commented this out and changed to 0 to match non-partial credit Rasch Model for test
                                break;
                            }
                        }
                    }

                    foreach (GameObject t in Camera.main.GetComponent<TestChecker>().checkers)
                    {
                        actualShapes += t.GetComponent<SeqCardChecker>().shapeNumber + "";
                        actualInput += t.GetComponent<SeqCardChecker>().inputNumber + "";
                        userShapes += t.GetComponent<SeqCardChecker>().userShapeNumber + "";
                        userInput += t.GetComponent<SeqCardChecker>().userInputNumber + "";
                    }
                    SeqLangOutputHandler.points = points;
                    SeqLangOutputHandler.userOrder = userInput;
                    SeqLangOutputHandler.userSeq = userShapes;
                    SeqLangOutputHandler.correctOrder = actualInput;
                    SeqLangOutputHandler.correctSeq = actualShapes;

                    break;
                }
            }
        }
        else if (!Camera.main.GetComponent<TestChecker>().CheckTestCorrectness() || backEndItem.currentEvent.jumpLabel == "")
        {
            if (frontEndItem.isExample)
            {
                Camera.main.GetComponent<TestChecker>().ClearCheckers();
            }
            else
            {
                foreach(GameObject t in Camera.main.GetComponent<TestChecker>().checkers)
                {
                    if (t.GetComponent<SeqCardChecker>().shapeNumber == t.GetComponent<SeqCardChecker>().userShapeNumber)
                    {
                        points = 1;
                    }
                    else
                    {
                        points = 0;
                        break;
                    }
                }
                if(points != 0 && frontEndItem.useOrder)
                {
                    foreach (GameObject t in Camera.main.GetComponent<TestChecker>().checkers)
                    {
                        if (t.GetComponent<SeqCardChecker>().inputNumber == t.GetComponent<SeqCardChecker>().userInputNumber)
                        {
                            points = 2;
                        }
                        else
                        {
                            points = 1;
                            break;
                        }
                    }
                }
            }

            foreach (GameObject t in Camera.main.GetComponent<TestChecker>().checkers)
            {
                actualShapes += t.GetComponent<SeqCardChecker>().shapeNumber + "";
                actualInput += t.GetComponent<SeqCardChecker>().inputNumber + "";
                userShapes += t.GetComponent<SeqCardChecker>().userShapeNumber + "";
                userInput += t.GetComponent<SeqCardChecker>().userInputNumber + "";
            }
            SeqLangOutputHandler.points = points;
            SeqLangOutputHandler.userOrder = userInput;
            SeqLangOutputHandler.userSeq = userShapes;
            SeqLangOutputHandler.correctOrder = actualInput;
            SeqLangOutputHandler.correctSeq = actualShapes;

            backEndItem.eventNumber++;
        }
    }

    public void ResetShapes()
    {
        print("reset");
        CardObject[] cards = Object.FindObjectsOfType<CardObject>();
        foreach(CardObject card in cards)
        {
            card.ToggleCollider();
            card.inBox = false;
            int posIndex = card.gameObject.name.ToCharArray()[4] - '0';
            card.gameObject.transform.position = GameObject.Find("Origin" + posIndex + " clone").transform.position;
            card.ToggleCollider();
        }
        SeqCardChecker[] triggers = Object.FindObjectsOfType<SeqCardChecker>();
        foreach (SeqCardChecker trigger in triggers)
        {
            trigger.userInputNumber = 0;
            trigger.userShapeNumber = 0;
            SeqCardChecker.currentActionNumber = 0;
        }
    }

    public void CreateTest(CompTestItem t)
    {
        //this will give us the world unit width of the screen (doubled it to account for the negative side)
        float fullScreenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x * 2;
        //divide the screen width by the amount of objects in the array to get offset from left side
        float fullScreenHeight = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y * 2;

        float offsetX = fullScreenWidth / t.TriggerPos.Length;
        float offsetY = fullScreenHeight / 2;

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

            o = new GameObject("reset");
            o.transform.position = new Vector3((-fullScreenWidth / 16) * 5, -offsetY / 2, 0);

            o = new GameObject("arrow");
            o.transform.position = new Vector3((fullScreenWidth / 16) * 5, -offsetY / 2, 0);
        }

        float leftScreenPos = -fullScreenWidth / 2 + offsetX / 2;
        float topScreenPos = fullScreenHeight / 2 - offsetY / 2;

        GameObject[] checkers = new GameObject[t.TriggerPos.Length];
        for (int i = 0; i < t.TriggerPos.Length; i++)
        {
            offsetX = fullScreenWidth / 10;
            //from center, pick distance, i-(arrayLength/2) * distance between cards + arrayLength%2 == 1?0:distance/2
            //ToDo: Change this math everywhere (all other handlers)
            float dist = fullScreenHeight / 5f;
            o = Instantiate(t.TriggerPos[i], new Vector3((i-t.TriggerPos.Length/2) * dist + (t.TriggerPos.Length%2 != 0?0:dist /2), topScreenPos, 0), new Quaternion());
            o.GetComponent<SeqCardChecker>().shapeNumber = t.OrderOfShapes[i];
            o.GetComponent<SeqCardChecker>().inputNumber = t.OrderOfInput[i];
            o.GetComponent<SeqCardChecker>().usesOrder = t.useOrder;
            checkers[i] = o;
            o.name = "Trigger" + t.OrderOfShapes[i] + " clone";
            o.transform.localScale = Vector3.one * fullScreenHeight / 10f * 1.5f;
        }

        Camera.main.GetComponent<TestChecker>().SetCheckers(checkers);

        offsetX = fullScreenWidth / t.SortShape.Length;
        offsetY = fullScreenHeight / 2;

        leftScreenPos = -fullScreenWidth / 2 + offsetX / 2;
        topScreenPos = fullScreenHeight / 2 - offsetY / 2;

        for (int i = 0; i < t.SortShape.Length; i++)
        {
            float dist = fullScreenWidth / 8f;
            float posX;
            if(i < 4)
            {
                int width = (t.SortShape.Length > 4 ? 4 : t.SortShape.Length);
                posX = (i - width / 2) * dist + (width % 2 != 0 ? 0 : dist / 2);
                o = Instantiate(t.SortShape[i], new Vector3(posX, -topScreenPos, 0), new Quaternion());
                o.GetComponent<CardObject>().orderNumber = i;
                o.name = "Sort" + i + " clone";
                o.transform.localScale = Vector3.one * fullScreenHeight / 10f * 1.5f;
                o = new GameObject();
                o.transform.position = new Vector3(posX, -topScreenPos, 0);
                o.name = "Origin" + i + " clone";
                o.transform.localScale = Vector3.one * fullScreenHeight / 10f * 1.5f;
            }
            else
            {
                int width = (t.SortShape.Length-4 > 4 ? 4 : t.SortShape.Length-4);
                posX = ((i-4) - width / 2) * dist + (width % 2 != 0 ? 0 : dist / 2);
                o = Instantiate(t.SortShape[i], new Vector3(posX, 0, 0), new Quaternion());
                o.GetComponent<CardObject>().orderNumber = i;
                o.name = "Sort" + i + " clone";
                o.transform.localScale = Vector3.one * fullScreenHeight / 10f * 1.5f;
                o = new GameObject();
                o.transform.position = new Vector3(posX, 0, 0);
                o.name = "Origin" + i + " clone";
                o.transform.localScale = Vector3.one * fullScreenHeight / 10f * 1.5f;
            }
        }

        //SeqLangOutputHandler.StartTimer(0);
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
            SeqLangOutputHandler.StopTimer(testItemFrontEnd[currentTestNumber].id, testAbbrev, testItemFrontEnd[currentTestNumber].useOrder);
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

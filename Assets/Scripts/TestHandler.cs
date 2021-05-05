using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class TestHandler : MonoBehaviour
{
    public List<TestItem> testItemBackEnd;
    public TestItem backEndItem { get; set; }
    public int currentTestNumber = 0;
    public bool mouseIsDone { get; protected set; }
    public bool nextTest { get; set; }
    public bool isFading { get; set; }
    public string testAbbrev;
    public int testNumber;

    public int ceiling;
    public int checkThreshold, correctThreshold;

    //consecutive wrong answers
    public int wrongCount;

    public int currentCorrect = 0;
    public int currentWrong = 0;

    public bool reminded = false;
    public float reminderTime = 10f;
    public float pauseTime = 60f;

    //variable for starting item for counting
    //when currentTestNumber >= startingItem do this stuff
    //variables for amount right out of total (3 right out of 5)


    // Use this for initialization
    protected void setNextTestBackEnd()
    {
        backEndItem = testItemBackEnd[currentTestNumber];
    }

    public void AddStopTimers()
    {
        int counter = 0;
        foreach(TestItem t in testItemBackEnd)
        {
            foreach(EventSystem.EventObject e in t.events)
            {
                counter++;
                if(e.type == EventSystem.typeOfEvent.Mouse)
                {
                    print("hit mouse");
                    EventSystem.EventObject test = new EventSystem.EventObject();
                    test.type = EventSystem.typeOfEvent.stopTimer;

                    List<EventSystem.EventObject> holder = t.events.ToList();

                    holder.Insert(counter, test);
                    t.events = holder.ToArray();
                }
            }
            counter = 0;
        }
    }

    protected virtual void Update()
    {
        if (mouseIsDone)
            OutputHandler.timer.Stop();

        //if (!reminded && OutputHandler.timer.ElapsedMilliseconds >= reminderTime * 1000f && !mouseIsDone)
        //{
        //    GetComponent<EventSystem>().ReminderAudio();
        //    reminded = true;
        //}
        ////I need to figure out if this works, pausing game will reset the variables
        ////need to print output and skip to next item so when unpaused it starts a new item
        //else if (OutputHandler.timer.ElapsedMilliseconds >= pauseTime * 1000f)
        //{
        //    GetComponent<Quit>().Pause();
        //    //GetComponent<OutputHandler>().PrintOutput("PAUSED");
        //}

        if (wrongCount >= ceiling)
        {
            //ScoreReports.PrintScores(testAbbrev);
            SQLHandler.UpdateTest(2);
            SceneHandler.GoToNextScene();
            return;
        }

        if (currentCorrect + currentWrong == checkThreshold)
        {
            if (correctThreshold > currentCorrect)
            {
                //ScoreReports.PrintScores(testAbbrev);
                SQLHandler.UpdateTest(3);
                SceneHandler.GoToNextScene();
            }
            else
            {
                currentWrong = 0;
                currentCorrect = 0;
            }
        }
    }
}

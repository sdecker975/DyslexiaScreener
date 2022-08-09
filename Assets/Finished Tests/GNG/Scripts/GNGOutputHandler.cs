using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GNGOutputHandler : OutputHandler
{
    public static int itemNumber;

    //ToDo: This is never called but should be oops
    public static void StopTimer(string testName, string itemID)
    {
        timer.Stop();
        //print(delayTime);
        if (correct)
        {
            totalCorrect++;
            Camera.main.GetComponent<TestHandler>().wrongCount = 0;
            Camera.main.GetComponent<TestHandler>().currentCorrect++;
        }
        else
        {
            totalIncorrect++;
            Camera.main.GetComponent<TestHandler>().wrongCount++;
            Camera.main.GetComponent<TestHandler>().currentWrong++;
        }
        reactionTimes.Add((timer.ElapsedMilliseconds / 1000f - delayTime));
        print(itemID);
        PrintOutput(itemID);
        itemNumber++;
        //timer.Reset();
        timer.Start();
    }

    public static void PrintOutput(string itemID)
    {
        if (InternetAvailable.internetAvailableStatic)
        {
            print(responseName);

            string values = string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')",
           Settings.studentID, Settings.testID, itemID, correct ? 1 : 0, (timer.ElapsedMilliseconds / 1000f - delayTime), FindObjectOfType<ClickCardGNG>().hitTimes);

            string command = "insert into university.GoNoGo (student_id, test_id, item_id, correctness, reaction_time, hit_num) values " + values;

            ScoreReports.SaveToCSVLocalData("GNG", values);

            SQLHandler.RunCommand(command);
        }
    }
}

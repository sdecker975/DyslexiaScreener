using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSortOutputHandler : OutputHandler {

    public static int itemNumber = 0;
    public static string correctSeq;
    public static bool correctness;
    public static float reactionTime;
    public static string inputSeq;

    //ToDo: This is never called but should be oops
    public static void StopTimer(string testAbbrev, string itemID)
    {
        timer.Stop();
        print(delayTime);
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
        PrintOutput(testAbbrev, itemID);
        itemNumber++;
        timer.Reset();
    }

    public static void PrintOutput(string testAbbrev, string itemID)
    {
        if (InternetAvailable.internetAvailableStatic)
        {
            string values = string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                                            Camera.main.GetComponent<TestHandler>().testNumber, Settings.studentID, Settings.testID, itemID, correct ? 1 : 0, (timer.ElapsedMilliseconds / 1000f - delayTime));
            print(values);
            string command = "insert into CollectedData.CardSort1 (exam_type, student_id, test_id, item_id, correctness, reaction_time) values " + values; ;
            print(command);
            ScoreReports.SaveToCSVLocalData(testAbbrev, values);

            SQLHandler.RunCommand(command);
        }
    }
}

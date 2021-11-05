using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class SeqLangOutputHandler : OutputHandler {

    public static int itemNumber;
    public static int points;
    public static float reactionTime;
    public static string correctSeq;
    public static string userSeq;
    public static string correctOrder;
    public static string userOrder;

    public static void StopTimer(string itemID, string testName, bool usesOrder)
    {
        timer.Stop();
        if (points == 2 || (points == 1 && !usesOrder))
            totalCorrect++;
        else if (points == 1)
            totalPartial++;
        else
            totalIncorrect++;

        if (points == 0 || (points == 1 && usesOrder))
        {
            Camera.main.GetComponent<TestHandler>().wrongCount++;
            Camera.main.GetComponent<TestHandler>().currentWrong++;
        }
        else
        {
            Camera.main.GetComponent<TestHandler>().wrongCount = 0;
            Camera.main.GetComponent<TestHandler>().currentCorrect++;
        }

        reactionTimes.Add((timer.ElapsedMilliseconds / 1000f - delayTime));
        reactionTime = (timer.ElapsedMilliseconds / 1000f - delayTime);
        PrintOutput(itemID);
        ClearData();
        timer.Reset();
    }

    public static void PrintOutput(string itemID)
    {
        if (InternetAvailable.internetAvailableStatic)
        {
            string[] data = dataToArray();

            string values = string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}')",
                                             Camera.main.GetComponent<TestHandler>().testNumber, Settings.studentID, Settings.testID, itemID, points, reactionTime, correctSeq, userSeq, correctOrder, userOrder, System.DateTime.Parse(Settings.DateTimeM).ToString("yyyy-MM-dd"));
            string command = "insert into university.exam_results (exam_type, student_id, test_id, item_id, correctness, reaction_time, correct_seq, user_seq, correct_order, user_order, dot) values " + values;

            ScoreReports.SaveToCSVLocalData("SLC", values);

            SQLHandler.RunCommand(command);
        }
    }

    public static string[] dataToArray()
    {
        string[] output = new string[7];
        output[0] = itemNumber + "";
        output[1] = points + "";
        output[2] = reactionTime + "";
        output[3] = correctSeq;
        output[4] = userSeq;
        output[5] = correctOrder;
        output[6] = userOrder;

        return output;
    }

    public static void ClearData()
    {
        itemNumber++;
        points = 0;
        reactionTime = 0;
        correctSeq = "";
        userSeq = "";
        correctOrder = "";
        userOrder = "";
        SeqCardChecker.currentActionNumber = 0;
    }
}

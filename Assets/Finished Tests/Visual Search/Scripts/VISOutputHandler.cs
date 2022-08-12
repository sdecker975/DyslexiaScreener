using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class VISOutputHandler : OutputHandler {

    public static int itemNumber;
    public static bool correctness;
    public static float reactionTime;
    public static string position;
    public static string content;

    public static void StopTimer(string itemID)
    {
        timer.Stop();
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
        PrintOutput(itemID);
        itemNumber++;
        timer.Reset();
    }

    public static void PrintOutput(string itemID)
    {
        if (InternetAvailable.internetAvailableStatic)
        {
            string values = string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}')",
                                            Camera.main.GetComponent<TestHandler>().testNumber, Settings.studentID, Settings.testID, itemID, correctness ? 1 : 0, (timer.ElapsedMilliseconds / 1000f - delayTime), position, content, System.DateTime.Parse(Settings.DateTimeM).ToString("yyyy-MM-dd"));
            string command = "insert into university.exam_results (exam_type, student_id, test_id, item_id, correctness, reaction_time, select_pos, select_name, dot) values " + values;

            ScoreReports.SaveToCSVLocalData("VIS", values);

            SQLHandler.RunCommand(command);
        }
    }

    public void clearData()
    {
        itemNumber++;
        correctness = false;
        reactionTime = 0;
        position = "";
        content = "";
    }
}

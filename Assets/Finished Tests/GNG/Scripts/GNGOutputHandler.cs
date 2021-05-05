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
        PrintOutput(itemID);
        itemNumber++;
        //timer.Reset();
        timer.Start();
    }

    public static void PrintOutput(string itemID)
    {

        //print(responseName);

        //string values = string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
        //                            Camera.main.GetComponent<TestHandler>().testNumber, Settings.studentID, Settings.testID, itemID, correct ? 1 : 0, (timer.ElapsedMilliseconds / 1000f - delayTime), responsePosition, responseName, System.DateTime.Parse(Settings.DateTimeM).ToString("yyyy-MM-dd"), FindObjectOfType<ClickCardGNG>().hitTimes);

        //string command = "insert into CARE1.CPT (exam_type, id, test_id, item_id, correctness, reaction_time, select_pos, select_name, dot, hit_num) values " + values;
        
        //ScoreReports.PrintLocalData("CPT", values);
        

        //SQLHandler.RunCommand(command);
    }
}

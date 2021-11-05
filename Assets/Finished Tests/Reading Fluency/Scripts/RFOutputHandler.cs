using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class RFOutputHandler : OutputHandler {
    public static int itemNumber;

    public static List<string> commandList;

    public static void PrintOutput(string itemID)
    {
        if (InternetAvailable.internetAvailableStatic)
        {
            print(responseName);

            string values = string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}','{6}', '{7}')",
                                            Camera.main.GetComponent<TestHandler>().testNumber, Settings.studentID, Settings.testID, itemID, correct ? 1 : 0, (timer.ElapsedMilliseconds / 1000f - delayTime), responsePosition, System.DateTime.Parse(Settings.DateTimeM).ToString("yyyy-MM-dd"));
            string command = "insert into university.exam_results (exam_type, student_id, test_id, item_id, correctness, reaction_time, select_pos, dot) values " + values;

            ScoreReports.SaveToCSVLocalData("RF", values);
            //if (!SQLHandler.IsBusy())
            SQLHandler.RunCommand(command);
            //else
            //    commandList.Add(command);
        }
    }

    //void Update()
    //{
    //    if(commandList.Count != 0 && !SQLHandler.IsBusy())
    //    {
    //        SQLHandler.RunCommand(commandList[0]);
    //        commandList.RemoveAt(0);
    //    }
    //}
}

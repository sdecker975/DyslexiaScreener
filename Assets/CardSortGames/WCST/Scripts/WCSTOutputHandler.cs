using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WCSTOutputHandler : OutputHandler {

    public static void PrintOutput(string testAbbrev, int itemNum, bool correct, bool perseveration, float timer, string currRule, string prevRule, string sortRule)
    {
        string values = string.Format("('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}','{10}')",
                                       17, Settings.studentID, Settings.testID, "WCST" + System.Convert.ToString(itemNum), correct ? 1 : 0, perseveration ? "P" : "NP", timer, currRule, prevRule, sortRule, System.DateTime.Parse(Settings.DateTimeM).ToString("yyyy/M/d HH:mm:ss"));

        string command = "insert into CARE1.UNIVERSAL (exam_type, id, test_id, item_id, correctness, perseveration, reaction_time, current_rule, previous_rule, sort_rule, dot) values " + values;

        //ScoreReports.PrintLocalData(testAbbrev, values);

        SQLHandler.RunCommand(command);
    }
}

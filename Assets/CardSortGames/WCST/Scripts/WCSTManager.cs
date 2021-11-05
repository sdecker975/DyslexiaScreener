using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class WCSTManager : MonoBehaviour {

    public static int streak { get; set; }
    public static int target = 6;
    private static int ruleCount = 0;
    private static int targetRuleCount = 2;
    public static int totalRun = 0;
    public enum Rule { Amount, Color, Shape, None }

    public static Rule[] ruleList = { Rule.Shape, Rule.Amount, Rule.Color, Rule.Amount, Rule.Shape, Rule.Color, Rule.Amount }; //Rule.Color, Rule.Shape, Rule.Color, Rule.Amount, Rule.Color };
    public static Rule[] ruleList2 = { Rule.Color, Rule.Amount, Rule.Shape, Rule.Amount, Rule.Color, Rule.Shape, Rule.Amount }; //Rule.Shape, Rule.Color, Rule.Shape, Rule.Amount, Rule.Shape };

    public static bool setUsedRuleList = false;
    public static Rule[] usedRuleList;

    private int ruleCounter = 0;
    public static Rule currRule = Rule.Color;
    public static Rule prevRule = Rule.None;

    public static int shapeCount = 2;
    public static int colorCount = 2;
    public static int amountCount = 1;

    public static int attemptNumber = 0;

    static float timer = 0.0f;
    public static bool runTimer = false;

    void Start()
    {
        SQLHandler.UpdateTest(8);
        if (SceneManager.GetActiveScene().name.Equals("wcst"))
            ResetVars();
    }

    void ResetVars()
    {
        ruleCount = 0;
        shapeCount = 2;
        colorCount = 2;
        amountCount = 1;
        attemptNumber = 0;
        targetRuleCount = 2;
        usedRuleList = null;
        streak = 0;
        totalRun = 0;
        currRule = Rule.Color;
        prevRule = Rule.None;
        setUsedRuleList = false;
    }

    void FixedUpdate()
    {
        if(runTimer)
            timer += Time.deltaTime;
        if (totalRun > 12 && streak == 0)
        {
            SQLHandler.UpdateTest(2);
            SceneHandler.GoToNextScene();
        }
    }

    public static bool HandleScore(bool correct, bool prevCorrect, Rule sortRule)
    {
        if(!setUsedRuleList)
        {
            setUsedRuleList = true;
            if(correct)
            {
                correct = false;
                usedRuleList = ruleList2;
                sortRule = Rule.Color;
                currRule = Rule.Shape;
            }
            else
            {
                usedRuleList = ruleList;
            }
        }

        if (correct)
            streak++;
        else
            streak = 0;

        WCSTOutputHandler.PrintOutput("wcst", attemptNumber, correct, prevCorrect, timer, currRule.ToString(), prevRule.ToString(), sortRule.ToString());
        Debug.Log((correct ? "1" : "0") + "," + (prevCorrect ? "P" : "NP") + "," + timer + "," + currRule + "," + prevRule + "," + sortRule + ",");
        attemptNumber++;

        timer = 0.0f;
        runTimer = false;

        if (streak == target)
        {
            totalRun = 0;
            if (ruleCount >= usedRuleList.Length)
            {
                SQLHandler.UpdateTest(1);
                SceneHandler.GoToNextScene();
            }
            prevRule = currRule;
            currRule = usedRuleList[ruleCount++];
            totalRun = 0;
            if (ruleCount == 1)
            {
                amountCount = 1;
                colorCount = 3;
                shapeCount = 3;
                SceneManager.LoadScene("wcst3Card");
            }
            else if (ruleCount == 2)
            {
                amountCount = 4;
                colorCount = 4;
                shapeCount = 4;
                SceneManager.LoadScene("wcst4Card");
            }
            streak = 0;
        }
        return correct;
    }
}

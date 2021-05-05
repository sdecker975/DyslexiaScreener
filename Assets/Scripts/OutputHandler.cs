using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;

public class OutputHandler : MonoBehaviour {

    public static bool correct;
    public static Stopwatch timer = new Stopwatch();
    public static int responsePosition;
    public static string responseName;
    public static float delayTime;

    public static int totalCorrect;
    public static int totalPartial;
    public static int totalIncorrect;
    public static int totalNoResponse;
    public static List<float> reactionTimes = new List<float>();

    public static void StartTimer(float dt)
    {
        if (timer.IsRunning)
            timer.Reset();
        timer.Start();
        delayTime = dt;
    }

    public static void StopTimer(int itemNumber, string testID)
    {
        //timer.Stop();
        //print(delayTime);
        //if (correct)
        //{
        //    totalCorrect++;
        //    Camera.main.GetComponent<TestHandler>().wrongCount = 0;
        //}
        //else
        //{
        //    totalIncorrect++;
        //    Camera.main.GetComponent<TestHandler>().wrongCount++;
        //}
        //reactionTimes.Add((timer.ElapsedMilliseconds / 1000f - delayTime));
        //string[] output = { correct.ToString(), (timer.ElapsedMilliseconds / 1000f - delayTime).ToString(), responsePosition.ToString(), responseName };
        //PrintOutput(output, testName);

        //timer.Reset();
    }

    public static void ResetTimer()
    {
        print("Hit timer");
        timer.Reset();
        timer.Start();
    }

    public static void ResetValues()
    {
        correct = false;
        responsePosition = -1;
        responseName = "NA";
    }

    public static float Average(IEnumerable<float> values)
    {
        float avg = 0f;
        int count = 0;
        foreach (float i in values)
        {
            avg += i;
            count++;
        }
        avg = avg / count;
        return avg;
    }

    public static float CalculateStdDev(IEnumerable<float> values)
    {
        float ret = 0;
        if (values.Count() > 0)
        {
            //Compute the Average      
            float avg = values.Average();
            //Perform the Sum of (value-avg)_2_2      
            float sum = values.Sum(d => Mathf.Pow(d - avg, 2));
            //Put it all together      
            ret = Mathf.Sqrt((sum) / (values.Count() - 1));
        }
        return ret;
    }
}

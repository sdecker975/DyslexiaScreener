using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreReports : OutputHandler {

    string[] TestOrder = { "LI", "LSSI", "LWR", "PM", "RF", "SB", "SI" };
    // GradeNorms Rows 1:3 == Means Ages 5:7; 4:6 == STD Ages 5:7
    static double[,] AgeNorms = new double[,]
                {
                    { 23.43478261, 22.79166667, 27.84, 8.043478261, 000, 15.8, 000 },
                    { 23.53225806, 25.68181818, 38.70238095, 11.68292683, 26.35294118, 19.52380952, 16.33333333 },
                    { 22.84615385, 33.7, 58.74242424, 14.44615385, 24.04545455, 22.59375, 15.86666667 },
                    { 1.440520328, 10.36289017, 23.77617855, 4.204740408, 000, 7.291547618, 000 },
                    { 2.288372189, 7.52873284, 22.52216031, 4.948819659, 8.177264033, 4.63479816, 3.694192581},
                    { 5.698627815, 3.785011993, 9.389536904, 4.130794299, 9.078352851, 4.590254515, 4.911026554}
                };

    public static void PrintScores(string testName)
    {
        string filePath = "data/" + Settings.last + "_" + Settings.first + "/scores.csv";
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
        fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.

        if (!System.IO.File.Exists(filePath))
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                string[] demographics = { Settings.first, Settings.last, Settings.dob, Settings.dot, Settings.id, Settings.sex };
                file.Write(string.Join(",", demographics) + ",\n");
            }
        }
        float avg = Average(reactionTimes);

        //    string[] TestOrder = { "LI", "LSSI", "LWR", "PM", "RF", "SB", "SI" };

        string output = testName + "," + totalCorrect + ",";
        DateTime birthday = Convert.ToDateTime(Settings.dob);
        int age = new DateTime(DateTime.Now.Subtract(birthday).Ticks).Year - 1;


        if (testName.Equals("LI"))
        {
            double mean = age == 5 ? AgeNorms[0, 0] : (age == 6 ? AgeNorms[1, 0] : AgeNorms[2, 0]);
            double std = age == 5 ? AgeNorms[3, 0] : (age == 6 ? AgeNorms[4, 0] : AgeNorms[5, 0]);
            output += (((float)totalCorrect - mean) / std) + ",\n";
        }
        else if (testName.Equals("LSSI"))
        {
            double mean = age == 5 ? AgeNorms[0, 1] : (age == 6 ? AgeNorms[1, 1] : AgeNorms[2, 1]);
            double std = age == 5 ? AgeNorms[3, 1] : (age == 6 ? AgeNorms[4, 1] : AgeNorms[5, 1]);
            output += (((float)totalCorrect - mean) / std) + ",\n";
        }
        else if (testName.Equals("LWR"))
        {
            double mean = age == 5 ? AgeNorms[0, 2] : (age == 6 ? AgeNorms[1, 2] : AgeNorms[2, 2]);
            double std = age == 5 ? AgeNorms[3, 2] : (age == 6 ? AgeNorms[4, 2] : AgeNorms[5, 2]);
            output += (((float)totalCorrect - mean) / std) + ",\n";
        }
        else if (testName.Equals("PM"))
        {
            double mean = age == 5 ? AgeNorms[0, 3] : (age == 6 ? AgeNorms[1, 3] : AgeNorms[2, 3]);
            double std = age == 5 ? AgeNorms[3, 3] : (age == 6 ? AgeNorms[4, 3] : AgeNorms[5, 3]);
            output += (((float)totalCorrect - mean) / std) + ",\n";
        }
        else if (testName.Equals("RF"))
        {
            double mean = age == 5 ? AgeNorms[0, 4] : (age == 6 ? AgeNorms[1, 4] : AgeNorms[2, 4]);
            double std = age == 5 ? AgeNorms[3, 4] : (age == 6 ? AgeNorms[4, 4] : AgeNorms[5, 4]);
            output += (((float)totalCorrect - mean) / std) + ",\n";
        }
        else if (testName.Equals("SB"))
        {
            double mean = age == 5 ? AgeNorms[0, 5] : (age == 6 ? AgeNorms[1, 5] : AgeNorms[2, 5]);
            double std = age == 5 ? AgeNorms[3, 5] : (age == 6 ? AgeNorms[4, 5] : AgeNorms[5, 5]);
            output += (((float)totalCorrect - mean) / std) + ",\n";
        }
        else if (testName.Equals("SI"))
        {
            double mean = age == 5 ? AgeNorms[0, 6] : (age == 6 ? AgeNorms[1, 6] : AgeNorms[2, 6]);
            double std = age == 5 ? AgeNorms[3, 6] : (age == 6 ? AgeNorms[4, 6] : AgeNorms[5, 6]);
            output += (((float)totalCorrect - mean) / std) + ",\n";
        }
        else
        {
            output += ",\n";
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
        {
            file.Write(output);
        }

        totalCorrect = 0;
        totalIncorrect = 0;
        totalNoResponse = 0;
        totalPartial = 0;
        reactionTimes.Clear();
    }

    public static void PrintLocalData(string testName, string output)
    {
        string filePath = "data/" + Settings.last + "_" + Settings.first + "/" + testName + "-localdata-"+ Settings.dateTime + ".csv";
        System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
        fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.

        if (!System.IO.File.Exists(filePath))
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath))
            {
                string[] demographics = { Settings.first, Settings.last, Settings.dob, Settings.dot, Settings.id, Settings.sex };
                file.Write(string.Join(",", demographics) + ",\n");
            }
        }

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
        {
            file.Write(output + "\n");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;

public class SceneHandler : MonoBehaviour
{

    public static string[] scenes =     { "LetterID", "LetterSymbol", "LetterWord2", "SentenceID", "ReadingComp", "ReadingFluency2", "OralVocab2", "OralComp2", "SequentialLanguage", "Rhyming", "SoundBlending", "Phoneme", "LexicalFluency", "RapidAutomaticNaming", "RapidLetterNumber-Number", "RapidLetterNumber-Letter", "RapidLetterNumber-Mixed", "VerbalMemory", "VerbalMemoryBackwards", "VisualSearch", "ColorSort", "ShapeSort3", "AdvancedGame", "wcst","GNG", "WIS", "ending", "drawRetention" };
    public static string[] abbrevs = { "LI", "LSSI", "WI", "SI", "RC", "RF", "OV", "OC", "SLC", "RYM", "SB", "PM", "LF", "RAN", "RLNN", "RLNL", "RLNM", "VWM", "VWMB", "VIS", "CSC", "CSS", "CSA", "WCST", "WIS", "end" };
    public static bool[] sceneActive =  { true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true };
    public static int currScene = 0;

    public static void GoToNextScene()
    {
        //for (int i = 0; i < sceneActive.Length; i++)
        //    print(sceneActive[i]);
        //print(sceneActive.ToString());
        for (int i = currScene; i < sceneActive.Length; i++)
        {
            if (sceneActive[i])
            {
                print(scenes[i]);
                SceneManager.LoadScene(scenes[i]);
                currScene = i + 1;
                return;
            }
        }
        //OutputHandler.MergeCSV();

        SceneManager.LoadScene("Menu");
    }

}

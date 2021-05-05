using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolutionHandler : MonoBehaviour {
    static Resolution currentResolution;
    static Resolution maxResolution;
    static float currentRatio;
    static float maxRatio;
    static float screenWidth;
    static float screenHeight;

    // Use this for initialization
    void Start () {
        SetMaxResolution();
	}

    private void SetMaxResolution()
    {
        string[] output = { "set max res" };
        //OutputHandler.PrintLog(output);
        maxResolution = Screen.resolutions[Screen.resolutions.Length - 1];
        maxRatio = maxResolution.width / ((float)maxResolution.height);
    }
    public static void SetCurrentResolution()
    {
        currentResolution = Screen.currentResolution;
        currentRatio = currentResolution.width / ((float)currentResolution.height);
        screenWidth = Screen.width;
        screenHeight = Screen.height;

        //ToDo: Set resolution back to whatever it was if it changes
        if (Mathf.Abs(currentRatio - maxRatio) > 0.01f)
        {
            //string[] output = { "set curr res", currentRatio.ToString(), maxRatio.ToString(), maxResolution.ToString(), currentResolution.ToString() };
            //OutputHandler.PrintLog(output);
            if (screenHeight > screenWidth)
                Screen.SetResolution((int) screenWidth, Mathf.RoundToInt(screenWidth / maxRatio), true);
            else
                Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

    public float versionNumberVisible = 0;
    public static float versionNumber = 0;

    public void Update()
    {
        versionNumber = versionNumberVisible;
    }

    public void StartMenu()
    {
        SceneHandler.GoToNextScene();
    }
    public void SettingsButton()
    {
        SceneManager.LoadScene("Settings");
    }
}

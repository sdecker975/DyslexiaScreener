using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.IO;

public class Quit : MonoBehaviour {

    public GameObject pauseScreen;
    List<GameObject> pausedObjects = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.DownArrow))
        {
            print("escape");
            if (SceneManager.GetActiveScene().name.Equals("Menu") || SceneManager.GetActiveScene().name.Equals("Login") || SceneManager.GetActiveScene().name.Equals("Settings"))
            {
                //CheckQuit.canQuit = true;
                //Application.Quit();
                SceneManager.LoadScene("quit");
            }

            //this shouldn't be a permanent fix
            OutputHandler.timer.Reset();
            //else
            //{
            //    // Path to directory of files to compress and decompress.
            //    string filePath = Settings.last + "_" + Settings.first;
            //    FileInfo fileInfo = new FileInfo(filePath);
            //    DirectoryInfo di = new DirectoryInfo(fileInfo.FullName);

            //    print(di.FullName);
            //    string date = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss");
            //    print(date);
            //    lzip.compressDir(di.FullName, 9, filePath + " " + date + ".zip", false, null);
            if (!SceneManager.GetActiveScene().name.Equals("ending"))
                SQLHandler.UpdateTest(7);
            SceneManager.LoadScene("Menu");
            //}
        }

        if(Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.DownArrow))
        {
            Pause();
        }

        if (Input.GetKey(KeyCode.I) && Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<TestHandler>().nextTest = true;
            OutputHandler.ResetTimer();
        }

        if(Input.GetKey(KeyCode.T) && Input.GetKey(KeyCode.DownArrow))
        {
            OutputHandler.timer.Reset();
            if (!SceneManager.GetActiveScene().name.Equals("ending"))
                SQLHandler.UpdateTest(6);
            SceneHandler.GoToNextScene();
        }

        //ScreenResolutionHandler.SetCurrentResolution();
        //// set the desired aspect ratio (the values in this example are
        //// hard-coded for 16:9, but you could make them into public
        //// variables instead so you can set them at design time)
        //float targetaspect = 16.0f / 9.0f;

        //// determine the game window's current aspect ratio
        //float windowaspect = (float)Screen.width / (float)Screen.height;

        //// current viewport height should be scaled by this amount
        //float scaleheight = windowaspect / targetaspect;

        //// obtain camera component so we can modify its viewport
        //Camera camera = GetComponent<Camera>();

        //// if scaled height is less than current height, add letterbox
        //if (scaleheight < 1.0f)
        //{
        //    Rect rect = camera.rect;

        //    rect.width = 1.0f;
        //    rect.height = scaleheight;
        //    rect.x = 0;
        //    rect.y = (1.0f - scaleheight) / 2.0f;

        //    camera.rect = rect;
        //}
        //else // add pillarbox
        //{
        //    float scalewidth = 1.0f / scaleheight;

        //    Rect rect = camera.rect;

        //    rect.width = scalewidth;
        //    rect.height = 1.0f;
        //    rect.x = (1.0f - scalewidth) / 2.0f;
        //    rect.y = 0;

        //    camera.rect = rect;
        //}
    }

    //what was this for??
    public void OutputPause()
    {
        GetComponent<TestHandler>();
    }

    public void LogoutButton()
    {
        Settings.userID = "";
        Settings.hasPermissions = false;
        SceneManager.LoadScene("Login");
    }

    public void Pause()
    {
        if(pauseScreen)
        {
            pauseScreen.SetActive(!pauseScreen.activeSelf);

            if (pauseScreen.activeSelf)
            {
                GameObject[] allObjects = FindObjectsOfType<GameObject>();
                foreach (GameObject go in allObjects)
                {
                    if (!go.name.Equals("Pause") && !go.name.Equals("Image") && !go.name.Equals("Canvas") && !go.name.Contains("Main Camera"))
                        pausedObjects.Add(go);
                }

                foreach (GameObject go in pausedObjects)
                    go.SetActive(false);
            }

            else
            {
                foreach (GameObject go in pausedObjects)
                    go.SetActive(true);

                pausedObjects.Clear();
            }

            GetComponent<EventSystem>().Pausing();
            if (EventSystem.paused)
            {
                OutputHandler.timer.Reset();
                //this is where I need to reset the values I think
                OutputHandler.ResetValues();
            }
        }
    }
}

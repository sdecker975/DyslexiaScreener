using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ReadingButtonAssigner : MonoBehaviour
{

    public bool wait3 = false;
    bool start = false;
    Stopwatch watch = new Stopwatch();

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void Update()
    {
        print(watch.ElapsedMilliseconds);

        if (Camera.main.GetComponent<VisualSearchTestHandler>().backEndItem.currentEvent.type == EventSystem.typeOfEvent.Mouse)
        {
            if (wait3 && !watch.IsRunning && watch.ElapsedMilliseconds / 1000f <= 3f)
            {
                watch.Start();
            }

            if (wait3 && watch.ElapsedMilliseconds / 1000f >= 3)
            {
                GetComponent<Button>().interactable = true;
                watch.Stop();
            }
            else if (!wait3 || !start)
                GetComponent<Button>().interactable = true;
        }
    }

    void TaskOnClick()
    {
        print("hit");
        Camera.main.GetComponent<VisualSearchTestHandler>().backEndItem.eventNumber++;
        GetComponent<Button>().interactable = false;
        start = true;
        watch.Reset();
    }
}

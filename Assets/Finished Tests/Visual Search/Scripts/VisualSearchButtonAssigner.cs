using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualSearchButtonAssigner : MonoBehaviour {

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void Update()
    {
        if(Camera.main.GetComponent<VisualSearchTestHandler>().backEndItem.currentEvent.type == EventSystem.typeOfEvent.Mouse)
        {
            GetComponent<Button>().interactable = true;
        }
    }

    void TaskOnClick()
    {
        Camera.main.GetComponent<VisualSearchTestHandler>().backEndItem.eventNumber++;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAssignerWIS : MonoBehaviour
{

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Camera.main.GetComponent<ContinueButtonWIS>().Continue(1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAssignerSB : MonoBehaviour
{

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (name.Equals("AudioButton"))
            Camera.main.GetComponent<SBTestHandler>().ReplayButton();
        else if (name.Equals("ArrowButton"))
            Camera.main.GetComponent<ContinueButtonSB>().Continue(1);
    }
}

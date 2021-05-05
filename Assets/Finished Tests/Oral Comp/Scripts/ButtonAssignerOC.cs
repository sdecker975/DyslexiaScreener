using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAssignerOC : MonoBehaviour {

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (name.Equals("AudioButton"))
            Camera.main.GetComponent<OCTestHandler>().ReplayButton();
        else if (name.Equals("ArrowButton"))
            Camera.main.GetComponent<ContinueButtonOC>().Continue(1);
    }
}

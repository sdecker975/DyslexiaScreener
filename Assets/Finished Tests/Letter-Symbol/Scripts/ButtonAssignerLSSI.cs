using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonAssignerLSSI : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if(name.Equals("AudioButton"))
            Camera.main.GetComponent<LSSITestHandler>().ReplayButton();
        else if(name.Equals("ArrowButton"))
            Camera.main.GetComponent<ContinueButtonLSSI>().Continue(1);
    }
}
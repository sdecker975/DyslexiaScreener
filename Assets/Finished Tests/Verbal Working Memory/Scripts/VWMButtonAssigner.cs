using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VWMButtonAssigner : MonoBehaviour {

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (name.Equals("AudioButton"))
            Camera.main.GetComponent<LetterWordTestHandler>().ReplayButton();
        else if (name.Equals("ArrowButton"))
            Continue();
    }

    void Continue()
    {
        VerbalWorkingTestHandler vwm = Camera.main.GetComponent<VerbalWorkingTestHandler>();
        EventSystem.typeOfEvent e = vwm.backEndItem.currentEvent.type;
        NumberPadHandler nph = Camera.main.GetComponent<NumberPadHandler>();

        GameObject.Find("ArrowButton").GetComponent<Button>().interactable = false;
        nph.checkCorrectness(vwm.frontEndItem.correctNumber);
        vwm.backEndItem.eventNumber++;
    }
}

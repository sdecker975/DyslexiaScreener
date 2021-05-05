using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAssignerSLC : MonoBehaviour {

    void Start()
    {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (name.Equals("ArrowButton"))
            Camera.main.GetComponent<SeqLangTestHandler>().CheckCorrectness();
        else if (name.Equals("XButton"))
            Camera.main.GetComponent<SeqLangTestHandler>().ResetShapes();
    }
}

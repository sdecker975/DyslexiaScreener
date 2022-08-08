using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAssignerGNG : MonoBehaviour
{
    public bool down = false;

        void Start()
        {
            if (GameObject.Find("go"))
            {
            Button btn = GetComponent<Button>();
            btn.onClick.AddListener(TaskOnClick);
            }
        }


        void TaskOnClick()
        {
           Camera.main.GetComponent<ContinueButtonGNG>().Continue(0);
        }
    

        //"This may work, but it might not" - Will P.S. "It might do something"
    private void OnMouseDown()
    {
        print(GNGOutputHandler.timer.ElapsedMilliseconds);
        GNGTestHandler gng = Camera.main.GetComponent<GNGTestHandler>();
        if (GameObject.Find("ArrowButton"))
        {
            down = true;
            ClickCardGNG[] c = FindObjectsOfType(typeof(ClickCardGNG)) as ClickCardGNG[];
            c[0].isClicked = true;
            Camera.main.GetComponent<ContinueButtonGNG>().Continue(0);
            if (!gng.frontEndItem.isExample)
            {
                c[0].hitTimes++;
                GNGOutputHandler.StopTimer("GNG", gng.frontEndItem.id);
            }
        }
        else
        {
            Camera.main.GetComponent<ContinueButtonGNG>().Continue(0);
        }
    }
    private void OnMouseUp()
    {
        down = false;
    }

   
}

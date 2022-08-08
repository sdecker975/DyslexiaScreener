using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClickCardGNG : MonoBehaviour
{
    public bool corrected = false;
    public bool isClicked = false;
    public bool isCorrect = false;
    public bool isAnim = false;
    public int hitTimes = 0;
    public int responsePosition;
    public string responseName;

    EventSystem.typeOfEvent e;

    // Update is called once per frame
    void Update()
    {
        GNGTestHandler gng = Camera.main.GetComponent<GNGTestHandler>();
        e = gng.backEndItem.currentEvent.type;

        if (gng.frontEndItem.id.Contains("GNG240B"))
        {
            SQLHandler.UpdateTest(1);
        }

        if (!gng.frontEndItem.id.Contains("B") && !gng.frontEndItem.id.Contains("b"))
        {
            gng.frontEndItem.isi = 800;
        }
        else if (gng.currentTestNumber < 66 || (gng.currentTestNumber > 138 && gng.currentTestNumber < 198) || (gng.currentTestNumber > 265 && gng.currentTestNumber < 325) || (gng.currentTestNumber > 396 && gng.currentTestNumber < 457))
        {
            gng.frontEndItem.isi = 5000;
        }
        else if (gng.currentTestNumber < 127 || (gng.currentTestNumber > 199 & gng.currentTestNumber < 259) || (gng.currentTestNumber > 327 && gng.currentTestNumber < 387) || (gng.currentTestNumber > 457 && gng.currentTestNumber < 518))
        {
            gng.frontEndItem.isi = 500;
        }


        if (e == EventSystem.typeOfEvent.Destroy)
            isAnim = false;
        print(OutputHandler.timer.ElapsedMilliseconds);
        if (OutputHandler.timer.ElapsedMilliseconds >= gng.frontEndItem.isi && e == EventSystem.typeOfEvent.Mouse && !gng.frontEndItem.isExample)
        {
            if (hitTimes == 0)
            {
                if (gng.frontEndItem.isCorrect[0])
                {
                    GNGOutputHandler.correct = false;
                    GNGOutputHandler.responseName = responseName;
                    Camera.main.GetComponent<ContinueButtonGNG>().Continue(0);
                    GNGOutputHandler.StopTimer("GNG", gng.frontEndItem.id);
                }
                else
                {
                    GNGOutputHandler.correct = true;
                    GNGOutputHandler.responseName = responseName;
                    Camera.main.GetComponent<ContinueButtonGNG>().Continue(0);
                    GNGOutputHandler.StopTimer("GNG", gng.frontEndItem.id);
                }
            }
            gng.LoadNextTest();
        }

        if (e == EventSystem.typeOfEvent.Mouse && gng.frontEndItem.isExample && gng.frontEndItem.wordPictures.Length > 0 )
        {
            //This keeps the Continue function in ContinueButton script from running again
            corrected = false;
        }
        if (e == EventSystem.typeOfEvent.loop)
        {
            ClickCardGNG[] c = FindObjectsOfType(typeof(ClickCardGNG)) as ClickCardGNG[];
            c[0].isClicked = false;
        }
        if (OutputHandler.timer.ElapsedMilliseconds >= 5000 && gng.frontEndItem.isExample && !corrected)
        {
            print(gng.frontEndItem.isCorrect.Length);
            if (gng.frontEndItem.isCorrect[0])
            {
                //Missed hit
                GNGOutputHandler.correct = false;
                corrected = true;
                Camera.main.GetComponent<ContinueButtonGNG>().Continue(0);
            }
            else if (!gng.frontEndItem.isCorrect[0])
            {
                //Appropriate no hit
                GNGOutputHandler.correct = true;
                Camera.main.GetComponent<ContinueButtonGNG>().Continue(0);
            }
        }
    }
}

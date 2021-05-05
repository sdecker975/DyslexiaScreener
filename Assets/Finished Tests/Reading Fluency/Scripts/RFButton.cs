using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RFButton : MonoBehaviour {

    public int posNumber;
    bool isMouseDown = false;
    public bool isCorrect = false;
    EventSystem.typeOfEvent e;
    public int buttonItemNumber;
    public bool isStart;
    RFTestHandler rf;

    void Start()
    {
        rf = Camera.main.GetComponent<RFTestHandler>();
    }

    void Update()
    {
        e = Camera.main.GetComponent<RFTestHandler>().backEndItem.currentEvent.type;
        RFTestItem item = rf.frontEndItem;
        if (isMouseDown && buttonItemNumber == item.activeItemNumber)
        {
            if(isStart)
            {
                rf.backEndItem.eventNumber++;
                return;
            }
            if (isCorrect && item.isExample && !rf.backEndItem.currentEvent.jumpLabel.Equals(""))
            {
                for (int i = rf.backEndItem.eventNumber + 1; i < rf.backEndItem.events.Length; i++)
                {
                    if (rf.backEndItem.currentEvent.jumpLabel.Equals(rf.backEndItem.events[i].jumpLabel))
                    {
                        buttonPushed();
                        rf.backEndItem.eventNumber = i;
                        break;
                    }
                }
            }
            else
            {
                //go forward an event if it is incorrect, only do button pushed if it isn't an example
                if (!item.isExample)
                {
                    buttonPushed();
                }
                rf.backEndItem.eventNumber++;
            }
            if(!item.isExample)
            {
                RFOutputHandler.correct = isCorrect;
                if (posNumber == 2)
                    RFOutputHandler.totalNoResponse++;
                else if (isCorrect)
                    RFOutputHandler.totalCorrect++;
                else if (!isCorrect)
                    RFOutputHandler.totalIncorrect++;

                RFOutputHandler.responsePosition = posNumber;
                RFOutputHandler.reactionTimes.Add((RFOutputHandler.timer.ElapsedMilliseconds / 1000f));
                //string[] output = { isCorrect ? "TRUE" : "FALSE", (RFOutputHandler.timer.ElapsedMilliseconds / 1000f).ToString(), posNumber.ToString() };
                RFOutputHandler.PrintOutput(rf.frontEndItem.id[RFOutputHandler.itemNumber % 5]);
                print(rf.frontEndItem.id[RFOutputHandler.itemNumber % 5]);
                RFOutputHandler.itemNumber++;
                RFOutputHandler.ResetTimer();
            }

            isMouseDown = false;
        }
    }

    public void buttonPushed()
    {
        RFTestHandler rf = Camera.main.GetComponent<RFTestHandler>();
        RFTestItem item = rf.frontEndItem;
        GetComponent<SpriteRenderer>().color = new Color(.3f,.3f,.3f, 1);
        changeLayerToFadeOut();
        item.activeItemNumber++;
        if (item.activeItemNumber < item.testSentences.Length)
            changeLayerToFadeIn();
    }

    public void changeLayerToFadeOut()
    {
        RFTestHandler rf = Camera.main.GetComponent<RFTestHandler>();
        RFTestItem item = rf.frontEndItem;
        int num = item.activeItemNumber;
        GameObject sentence = GameObject.Find("sentenceBox " + num + " clone " + rf.currentTestNumber);
        GameObject buttons = GameObject.Find("buttonSet " + num + " clone " + rf.currentTestNumber);
        if(sentence)
            sentence.layer = LayerMask.NameToLayer("fadeOut");
        if(buttons)
            buttons.layer = LayerMask.NameToLayer("fadeOut");
        foreach (Transform child in buttons.transform)
            child.gameObject.layer = LayerMask.NameToLayer("fadeOut");
    }

    public void changeLayerToFadeIn()
    {
        print("changed layer");
        RFTestHandler rf = Camera.main.GetComponent<RFTestHandler>();
        RFTestItem item = rf.frontEndItem;
        int num = item.activeItemNumber;
        GameObject sentence = GameObject.Find("sentenceBox " + num + " clone " + rf.currentTestNumber);
        GameObject buttons = GameObject.Find("buttonSet " + num + " clone " + rf.currentTestNumber);
        sentence.layer = LayerMask.NameToLayer("fadeIn");
        buttons.layer = LayerMask.NameToLayer("fadeIn");
        foreach (Transform child in buttons.transform)
        {
            print("hit");
            child.gameObject.layer = LayerMask.NameToLayer("fadeIn");
        }
    }

    public void unclickButton()
    {
        RFTestHandler rf = Camera.main.GetComponent<RFTestHandler>();
        RFTestItem item = rf.frontEndItem;
        GetComponent<SpriteRenderer>().color = Color.white;
        item.activeItemNumber--;
    }

    void OnMouseDown()
    {
        isMouseDown = e == EventSystem.typeOfEvent.Mouse;
    }
    private void OnMouseUp()
    {
        isMouseDown = false;
    }
}

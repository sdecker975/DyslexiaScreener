using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPadHandler : MonoBehaviour {

    public string numbers = "";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void appendToNumbers(int digit)
    {
        numbers += digit;
    }

    public void checkCorrectness(string input)
    {
        VerbalWorkingTestHandler vm = Camera.main.GetComponent<VerbalWorkingTestHandler>();
        EventSystem.typeOfEvent e = vm.backEndItem.currentEvent.type;
        
        if (input.Equals(numbers))
        {
            VWMOutputHandler.correct = true;
            VWMOutputHandler.correctSeq = input;
            VWMOutputHandler.inputSeq = numbers;
            //vm.wrongCount = 0;
            numbers = "";
        }
        else
        {
            VWMOutputHandler.correct = false;
            VWMOutputHandler.correctSeq = input;
            VWMOutputHandler.inputSeq = numbers;
            //if(!vm.frontEndItem.isExample)
            //    vm.wrongCount++;
            numbers = "";
        }
    }

    public void checkExampleCorrectness(string input)
    {
        VerbalWorkingTestHandler vm = Camera.main.GetComponent<VerbalWorkingTestHandler>();
        EventSystem.typeOfEvent e = vm.backEndItem.currentEvent.type;
        print(input + " " + numbers);
        if (input.Equals(numbers))
        {
            VWMOutputHandler.correct = true;
            VWMOutputHandler.correctSeq = input;
            VWMOutputHandler.inputSeq = numbers;

            if (!vm.backEndItem.currentEvent.jumpLabel.Equals(""))
            {
                for (int i = vm.backEndItem.eventNumber + 1; i < vm.backEndItem.events.Length; i++)
                {
                    if (vm.backEndItem.currentEvent.jumpLabel.Equals(vm.backEndItem.events[i].jumpLabel))
                    {
                        vm.backEndItem.eventNumber = i;
                        break;
                    }
                }
            }
            else
            {
                vm.backEndItem.eventNumber++;
            }
            numbers = "";
        }
        else
        {
            print("in incorrect");
            VWMOutputHandler.correct = false;
            VWMOutputHandler.correctSeq = input;
            VWMOutputHandler.inputSeq = numbers;
            vm.backEndItem.eventNumber++;
            numbers = "";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevMode : MonoBehaviour {

    public TestHandler t;
    public Text textBlock;

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (Settings.DemoMode)
            textBlock.text = " Test " + SceneHandler.currScene + ": " + t.testAbbrev + " Item: " + t.currentTestNumber + " Student ID: " + Settings.studentID + " Queued: " + SQLHandler.pushingStack.Count;
        else
            Destroy(this.gameObject);
	}
}

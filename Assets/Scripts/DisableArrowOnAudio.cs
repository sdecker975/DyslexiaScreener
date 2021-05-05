using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableArrowOnAudio : MonoBehaviour {

    bool changedByMe = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Camera.main.GetComponent<AudioSource>().isPlaying)
        {
            if(GetComponent<Button>().interactable)
            {
                GetComponent<Button>().interactable = false;
                changedByMe = true;
            }
        }
        else if(changedByMe)
        {
            GetComponent<Button>().interactable = true;
            changedByMe = false;
        }
    }
}

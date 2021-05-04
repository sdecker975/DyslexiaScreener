using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButton : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(!Camera.main.GetComponent<AudioSource>().isPlaying)
        {
            GetComponent<Button>().interactable = true;
        }
	}
}

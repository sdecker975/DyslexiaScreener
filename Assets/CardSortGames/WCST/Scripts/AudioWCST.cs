using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioWCST : MonoBehaviour {

    public AudioClip instructions;
    public float waitTime = 2f;
    float counter = 0f;

    void Start()
    {
        GetComponent<AudioSource>().clip = instructions;
        GetComponent<AudioSource>().PlayDelayed(waitTime);
    }
	
}

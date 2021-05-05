using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFadeIn : MonoBehaviour {

    Image buttonImage;
    Color c;


	// Use this for initialization
	void Start () {
        buttonImage = GetComponent<Image>();
        c = buttonImage.color;
        c.a = 0;
        buttonImage.color = c;
	}

    // Update is called once per frame
    void Update() {
        if (c.a <= 1f)
        {
            c.a += .01f;
            buttonImage.color = c;
        }
    }
}

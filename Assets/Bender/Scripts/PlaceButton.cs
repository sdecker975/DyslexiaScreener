using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceButton : MonoBehaviour {

    public GameObject pos;
    
	// Use this for initialization
	void Start () {
        this.transform.position = pos.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsManager : MonoBehaviour {

    float time = 0.0f;
    WCSTGenerator gen;

    void Start()
    {
        gen = Camera.main.GetComponent<WCSTGenerator>();
    }

    void FixedUpdate () {
        time += Time.deltaTime;
        if(time >= 1)
        {
            gen.GenerateCard();
            Destroy(gameObject);
        }
	}
}

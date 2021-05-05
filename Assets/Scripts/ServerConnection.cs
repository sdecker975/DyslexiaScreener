using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ServerConnection : MonoBehaviour {

    SpriteRenderer wifi;
    Thread connThread = new Thread(SQLHandler.CheckConnection);

	// Use this for initialization
	void Start () {
        wifi = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if(!connThread.IsAlive)
        {
            connThread = new Thread(SQLHandler.CheckConnection);
            // connThread.Start();
        }

		if(SQLHandler.state)
        {
            Color c = new Color();
            c.a = 0f;
            wifi.color = c;
        }
        else
        {
            wifi.color = Color.red;
        }

        if(SQLHandler.pushingStack.Count != 0)
        {
            SQLHandler.RunCommand(SQLHandler.pushingStack.Pop());
        }
	}
}

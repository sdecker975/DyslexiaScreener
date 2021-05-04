using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Images()
	{
		SceneManager.LoadScene ("images");
	}

    public void DrawImages()
    {
        SceneManager.LoadScene("drawImages");
    }

    public void DrawRecall()
    {
        SceneManager.LoadScene("drawRetention");
    }

    public void BenderMenu1()
    {
        SceneManager.LoadScene("BenderMenu1");
    }

    public void BenderMenu2()
    {
        SceneManager.LoadScene("BenderMenu2");
        DrawingScript.isFirst = false;
    }
}

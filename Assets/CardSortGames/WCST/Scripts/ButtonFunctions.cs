using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class ButtonFunctions : MonoBehaviour {

    public GameObject canvas;
    public AudioClip go;

    public void TurnOffCanvas2()
    {
        canvas.SetActive(false);
        if(Camera.main.GetComponent<AudioSource>())
        {
            Camera.main.GetComponent<AudioSource>().clip = go;
            Camera.main.GetComponent<AudioSource>().Play();
        }
    }
}

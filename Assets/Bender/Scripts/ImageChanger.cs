using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ImageChanger : MonoBehaviour {

	public Sprite[] benderImages;
	int benderIndex = 0;
	public SpriteRenderer currentImage;

    public bool useButton = false;
    public bool isRetention = false;
    public DrawingScript d;

	// Use this for initialization
	void Start () {
		currentImage.sprite = benderImages [benderIndex];
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) && !useButton) 
		{
            ChangeImage();
		}
	}

    public void ChangeImage()
    {
        if(!isRetention)
        {
            if (benderIndex + 1 < benderImages.Length)
            {
                currentImage.sprite = benderImages[++benderIndex];
            }
            else
            {
                SceneManager.LoadScene("Menu");
            }
        }

        if(useButton)
        {
            d.ClearLines();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ServerConnection : MonoBehaviour {

    //SpriteRenderer wifi;
    public Image wifiImage;
    private float timerToCheckInternet = 2f;

    Thread connThread = new Thread(SQLHandler.CheckConnection);

	// Use this for initialization
	void Start () {
        wifiImage.gameObject.SetActive(false);
        //wifi = GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update () {
        timerToCheckInternet -= Time.deltaTime;

        if(timerToCheckInternet <= 0)
        {
            StartCoroutine(CheckInternetConnection());
            SQLHandler.CheckConnection();
            timerToCheckInternet = 2f;
        }
        if(!connThread.IsAlive)
        {
            //print("Conn thread is:" + connThread.IsAlive.ToString());
            connThread = new Thread(SQLHandler.CheckConnection);
            connThread.Start();
        }
        
		if(SQLHandler.state)
        {
            //Color c = new Color();
            //c.a = 0f;
            //wifi.color = c;
            wifiImage.gameObject.SetActive(false);
        }
        else
        {
            //wifi.color = Color.red;
            wifiImage.gameObject.SetActive(true);
        }
    }
    IEnumerator CheckInternetConnection()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            string urlString = "http://google.com";
            UnityWebRequest request = UnityWebRequest.Get(urlString);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Internet is not available");
                // internet connection is not available
                InternetAvailable.internetAvailableStatic = false;
                if (!wifiImage.gameObject.activeInHierarchy)
                {
                    wifiImage.gameObject.SetActive(true);

                }
            }
            else
            {
                Debug.Log("Internet is available");
                // internet connection is available
                InternetAvailable.internetAvailableStatic = true;
                if (wifiImage.gameObject.activeInHierarchy)
                {
                    wifiImage.gameObject.SetActive(false);

                }
            }
        }
        else
        {
            Debug.Log("Internet is not reachable");
            // internet connection is not available
            InternetAvailable.internetAvailableStatic = false;
            if (!wifiImage.gameObject.activeInHierarchy)
            {
                wifiImage.gameObject.SetActive(true);

            }
        }
    }
}

using System.Text;
using System.Collections;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Net.NetworkInformation;
using UnityEngine.EventSystems;

public class LoginScript : MonoBehaviour {

    private const string pingAddress = "8.8.8.8"; // Google Public DNS server
    private const float waitingTime = 1.0f;
    private UnityEngine.Ping ping;
    private float pingStartTime;
    bool internetReachability;
    bool internetPingCheck;

    public InputField user, password;
    public string stringUser = "Email";
    public string stringPassword = "Password";
    // used by incorrectLoginText if the login information is incorrect
    public string stringIncorrectLogin = "Incorrect login";
    string salt = "qbM9rDvqmCFWRUEt";
    private TouchScreenKeyboard keyboard;
    // displays message if login information is incorrect, otherwise displays an empty string
    public Text incorrectLoginText;
    // checks if the PHP request returned true in GetPHPRequest coroutine
    private bool responseBool;
    // checks if the PHPRequest is running, used in the Update method to rotate the loading image
    private bool runningPHPBool;
    public Button loginButton;

    float timerToCheckInternet;

    public GameObject loadingImage;
    public GameObject loadingText;
    public OfflineUserDatabase offlineUserDatabase;


    UnityEngine.EventSystems.EventSystem system;
    public GameObject emailInputField;
    public GameObject passwordInputField;

    // Use this for initialization
    void Start() {
        Ping();
        pingStartTime = Time.time;
        loadingImage.SetActive(false);
        loadingText.SetActive(false);
        system = UnityEngine.EventSystems.EventSystem.current;
        incorrectLoginText.text = "";
        
        StartCoroutine(CheckInternetConnection());
        SQLHandler.MakeConnection();
        //GetPHPRequest("ayanmitra", "$2y$10$qGDDKK2AVplLeX5Iixy90.ZVpC186MLw3mq7gO8PteaULnUZSHYnG");
    }

    private void Ping() {
        try {
            ping = new UnityEngine.Ping(pingAddress);
        } catch (Exception e) {
            Debug.LogError("Exception pinging");
            InternetAvailable.internetAvailableStatic = false;
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
                if (InternetAvailable.internetAvailableStatic)
                {
                    InternetAvailable.internetAvailableStatic = false;
                }
            }
            else
            {
                Debug.Log("Internet is available");
                // internet connection is available
                if (!InternetAvailable.internetAvailableStatic)
                {
                    InternetAvailable.internetAvailableStatic = true;
                }
            }
        }
        else
        {
            Debug.Log("Internet is not reachable");
            // internet connection is not available
            if (InternetAvailable.internetAvailableStatic)
            {
                InternetAvailable.internetAvailableStatic = false;
            }
        }
    }

    

    public void LoginButton() {
        Debug.Log($"internetReachability= {Application.internetReachability}");

        if ((Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork ||
            Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork) &&
            InternetAvailable.internetAvailableStatic
            ) {
            if (SQLHandler.pushingStack.Count != 0) {
                SQLHandler.CheckConnection();
                SQLHandler.RunCommand(SQLHandler.pushingStack.Pop());
            }

            StartCoroutine(PHPRequestRecievedCoroutine());
            
        } else {

            // offline mode
            var offlineUsername = user.text.Trim();
            var offlinePassword = password.text.Trim();

            var loginSuccess = offlineUserDatabase.Matches(offlineUsername, offlinePassword);

            if (loginSuccess) {
                SceneManager.LoadScene("Menu");
            } else {
                Debug.Log("Unsuccessful offline login");
                incorrectLoginText.text = stringIncorrectLogin;
            }
        }
        //exists = rows[0][0].Equals("1");
        //print(exists);
        //if(exists)
        //{
        //    //get permissions
        //    //command = string.Format("select canUse from CARE1.users_old where user = '{0}'", userText);
        //    //rows = SQLHandler.RunCommand(command);

        //    //SQLHandler.CloseConnection();

        //    command = string.Format("select user_id from CARE1.users where email = '{0}' and password = '{1}' limit 0, 1",
        //                                userText, passText);

        //    rows = SQLHandler.RunCommand(command);

        //    Settings.userID = Convert.ToInt32(rows[0][0]);

        //    Settings.hasPermissions = exists;

        //    print(Settings.hasPermissions + " has permission");

        //    SceneManager.LoadScene("Menu");
        //}
    }

    public IEnumerator PHPRequestRecievedCoroutine() {
        loginButton.interactable = false;
        string userText = user.text;
        string passText = password.text;

        bool exists = false;
        //using (MD5 md5Hash = MD5.Create())
        //{
        //    passText = GetMd5Hash(md5Hash, salt + password.text);
        //}
        SQLHandler.CheckConnection();
        string command = string.Format("SELECT password FROM university.users where email = '{0}'", userText);
        List<List<string>> rows = SQLHandler.RunCommand(command);
        string commandStatus = string.Format("SELECT status FROM university.users where email = '{0}'", userText);
        List<List<string>> statusReturned = SQLHandler.RunCommand(commandStatus);


        string passhash = "";

        if (rows.Count != 0)
            passhash = rows[0][0];

        yield return StartCoroutine(GetPHPRequest(passText, passhash));

        if (!passhash.Equals("") && responseBool && statusReturned[0][0].Equals("active")) {
            command = string.Format(
                "select uuid from university.users where email = '{0}' and password = '{1}' limit 0,1", userText,
                passhash);
            rows = SQLHandler.RunCommand(command);

            Settings.userID = rows[0][0];
            Settings.hasPermissions = true;
            // sets incorrecLoginText to an empty string in case the text displayed message upon returning to login screen
            incorrectLoginText.text = "";
            responseBool = false;
            runningPHPBool = false;
            loadingImage.SetActive(false);
            loadingText.SetActive(false);
            loginButton.interactable = true;
            SceneManager.LoadScene("Menu");
        } else if (!passhash.Equals("") && responseBool && statusReturned[0][0].Equals("deactivate")) {
            user.text = "account inactive; Please contact support";
            runningPHPBool = false;
            loadingImage.SetActive(false);
            loadingText.SetActive(false);
            loginButton.interactable = true;
        } else { /*
            user.text = "Incorrect login";
            password.text = "";
           */
            // displays message if the login information is incorrect
            runningPHPBool = false;
            incorrectLoginText.text = stringIncorrectLogin;
            responseBool = false;
            loadingImage.SetActive(false);
            loadingText.SetActive(false);
            loginButton.interactable = true;
        }
    }

    public IEnumerator GetPHPRequest(string pass, string hash) {
        runningPHPBool = true;
        loadingImage.SetActive(true);
        loadingText.SetActive(true);
        string URL = "http://ec2-3-128-150-117.us-east-2.compute.amazonaws.com/unity_test.php";
        WWWForm form = new WWWForm();
        string r_text = "";

        form.AddField("pass", pass);
        form.AddField("hash", hash);

        UnityWebRequest
            w = UnityWebRequest.Post(URL, form); //here we create a var called 'w' and we sync with our URL and the form

        //w.downloadHandler = new DownloadHandlerBuffer();
        yield return w.SendWebRequest(); //we wait for the form to check the PHP file, so our game dont just hang
        //while (!w.isDone) ;

        if (w.error != null) {
            print(w.error); //if there is an error, tell us
        } else {
            print("Test ok");
            r_text = w.downloadHandler.text;
            w.Dispose(); //clear our form in game
        }

        responseBool = r_text.Equals("true");
        ;
    }

    public string GetMd5Hash(MD5 md5Hash, string input) {
        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++) {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }

    private void Update() {

        float wait = 1.0f;
        wait -= Time.deltaTime;
        if(wait <= 0)
        {
            StartCoroutine(CheckInternetConnection());
            wait = 2f;
        }


        if (runningPHPBool) {
            loadingImage.transform.Rotate(new Vector3(0, 0, -1) * Time.deltaTime * 50f);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null) {
                InputField inputfield = next.GetComponent<InputField>();

                if (inputfield != null)
                    inputfield.OnPointerClick(
                        new PointerEventData(system)); //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            } else if (next == null) {
                system.SetSelectedGameObject(emailInputField, new BaseEventData(system));
            }
        }

        if (emailInputField == system.currentSelectedGameObject ||
            passwordInputField == system.currentSelectedGameObject) {
            if (Input.GetKeyDown(KeyCode.Return)) {
                loginButton.onClick.Invoke();
            }
        }
    }

}
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

public class LoginScript : MonoBehaviour {

    public InputField user, password;
    public string stringUser = "Email";
    public string stringPassword = "Password";
    string salt = "qbM9rDvqmCFWRUEt";
    private TouchScreenKeyboard keyboard;

    // Use this for initialization
    void Start () {
        SQLHandler.MakeConnection();
        //GetPHPRequest("ayanmitra", "$2y$10$qGDDKK2AVplLeX5Iixy90.ZVpC186MLw3mq7gO8PteaULnUZSHYnG");
    }

	
    public void LoginButton()
    {
        string userText = user.text;
        string passText = password.text;

        bool exists = false;
        //using (MD5 md5Hash = MD5.Create())
        //{
        //    passText = GetMd5Hash(md5Hash, salt + password.text);
        //}
        string command = string.Format("SELECT password FROM university.users where email = '{0}'", userText);
        List<List<string>> rows = SQLHandler.RunCommand(command);
        string commandStatus = string.Format("SELECT status FROM university.users where email = '{0}'", userText);
        List<List<string>> statusReturned = SQLHandler.RunCommand(commandStatus);



        string passhash = "";
        if(rows.Count != 0)
            passhash = rows[0][0];

        if(!passhash.Equals("") && GetPHPRequest(passText, passhash) && statusReturned[0][0].Equals("active"))
        {
            command = string.Format("select uuid from university.users where email = '{0}' and password = '{1}' limit 0,1", userText, passhash);
            rows = SQLHandler.RunCommand(command);

            Settings.userID = rows[0][0];
            Settings.hasPermissions = true;

            SceneManager.LoadScene("Menu");
        }
        else if (!passhash.Equals("") && GetPHPRequest(passText, passhash) && statusReturned[0][0].Equals("deactivate"))
        {
            user.text = "account inactive; Please contact support";
        }
        else
        {
            user.text = "incorrect login";
            password.text = "";
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

    static bool GetPHPRequest(string pass, string hash)
    {
        string URL = "http://ec2-3-128-150-117.us-east-2.compute.amazonaws.com/unity_test.php";
        WWWForm form = new WWWForm();
        string r_text = "";
        form.AddField("pass", pass);
        form.AddField("hash", hash);

        WWW w = new WWW(URL, form); //here we create a var called 'w' and we sync with our URL and the form
        //yield w; //we wait for the form to check the PHP file, so our game dont just hang
        while (!w.isDone) ;

        if (w.error != null)
        {
            print(w.error); //if there is an error, tell us
        }
        else
        {
            print("Test ok");
            r_text = w.text;
            w.Dispose(); //clear our form in game
        }
        return r_text.Equals("true");
    }

    static string GetMd5Hash(MD5 md5Hash, string input)
    {

        // Convert the input string to a byte array and compute the hash.
        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        // Create a new Stringbuilder to collect the bytes
        // and create a string.
        StringBuilder sBuilder = new StringBuilder();

        // Loop through each byte of the hashed data 
        // and format each one as a hexadecimal string.
        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        // Return the hexadecimal string.
        return sBuilder.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using MySql.Data;
using MySql.Data.MySqlClient;
using UnityEngine;
using System.IO;

public class SQLHandler : MonoBehaviour {

    //static string SERVER_NAME = "162.241.218.214";
    ////this user can read and write, will probably want to make a different user for this and for pushing data, one can read, other can write
    //public static string SERVER_USER = "acnlabu1_mjeason";
    //static string SERVER_PASS = "AA48854bb!";
    //static string DATABASE = "acnlabu1_care";
    //static string DATABASE2 = "university";

    static string SERVER_NAME = "care3.cfssp092qqmp.us-east-2.rds.amazonaws.com";
    //this user can read and write, will probably want to make a different user for this and for pushing data, one can read, other can write
    public static string SERVER_USER = "careuser";
    static string SERVER_PASS = "Neurocog1!";
    static string DATABASE = "CARE1";
    static string DATABASE2 = "university";

    public static int counter = 0;
    public static int max_counter = 150;

    static MySqlConnection conn;
    static bool currDB = false;
    static bool isBusy = false;

    public static bool state = false;

    public static Stack<string> pushingStack = new Stack<string>();

    public static void MakeConnection()
    {
        conn = new MySqlConnection(string.Format("Server={0};User ID={1};Password={2}", 
                                    SERVER_NAME, SERVER_USER, SERVER_PASS));
        try
        {
            conn.Open();
            Debug.Log("Connected");
            state = true;
        }
        catch (System.Exception ex)
        {
            //probably should have a pop up come up if this fails so they know
            Debug.Log("failed to connect to AWS server.");
            state = false;
            InternetAvailable.internetAvailableStatic = false;
        }
    }

    public static void CheckConnection()
    {
        //wait until this is called max_counter times (150)
        //when that happens check to see if we have a connection (maybe find a simple command)
        //
        if(counter >= max_counter)
        {
            //probably change
            RunCommand("select id from CARE1.students limit 1");
            counter = 0;
        }
        counter++;
        if (conn.State != System.Data.ConnectionState.Open)
        {
            MakeConnection();
        }
        state = conn.State == System.Data.ConnectionState.Open;
        //print("Thread Finished");
    }

    public static List<List<string>> RunCommand(string command)
    {
        //ToDo: in catch need to notify user somehow, mark them in output files or something
        try
        {
            print(command);
            print("In sql handler try");
            isBusy = true;
            MySqlCommand cmd = new MySqlCommand(command, conn);
            MySqlDataReader rdr = cmd.ExecuteReader();

            List<List<string>> rows = new List<List<string>>();

            while (rdr.Read())
            {
                List<string> row = new List<string>();
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    row.Add(rdr[i].ToString());
                }
                rows.Add(row);
                //exists = rdr[0].ToString().Equals("1");
            }
            rdr.Close();
            isBusy = false;
            return rows;
        }
        catch (System.Exception e)
        {
            print(e);
            if(!command.Equals("select id from CARE1.students limit 1"))
            {
                pushingStack.Push(command);
                //print(pushingStack.Peek());
                //print(pushingStack.Count);
            }
            List<List<string>> holder = new List<List<string>>();
            List<string> inner = new List<string>();
            inner.Add("-1");
            holder.Add(inner);
            isBusy = false;
            return holder;
        }
    }

    public static void SwitchDB()
    {
        string DB = DATABASE;
        if(currDB)
        {
            currDB = false;
            DB = DATABASE2;
        }
        else
        {
            currDB = true;
        }
        conn = new MySqlConnection(string.Format("Server={0};Database={1};User ID={2};Password={3}",
                                    SERVER_NAME, DB, SERVER_USER, SERVER_PASS));
        try
        {
            conn.Open();
            print("Connected");
            state = true;
        }
        catch (System.Exception ex)
        {
            //probably should have a pop up come up if this fails so they know
            print("failed");
            state = false;
        }
    }

    public static void InsertTest()
    {
        if (InternetAvailable.internetAvailableStatic)
        {
            string command = "insert into university.student_test_info (test_num, student_id, date_of_testing, test_type, result) values (" + Settings.testID + ", '" + Settings.studentID + "', '" + System.DateTime.Parse(Settings.DateTimeM).ToString("yyyy-MM-dd HH:mm:ss") + "', '" + Camera.main.GetComponent<TestHandler>().testAbbrev + "'," + "9)";
            print(command);
            RunCommand(command);
        }
        
    }

    public static void UpdateTest(int code)
    {
        if (InternetAvailable.internetAvailableStatic)
        {

            // TODO: add if statement to check if camera has the component TestHandler, if it does execute otherwise do nothing
            //string command = "update CARE1.student_test_info set " + Camera.main.GetComponent<TestHandler>().testAbbrev + " = " + code + " where test_num = " + Settings.testID;
            if (Camera.main.GetComponent<TestHandler>() != null)
            {
                string command = "update university.student_test_info set result = " + code + " where test_num = " + Settings.testID + " and test_type = '" + Camera.main.GetComponent<TestHandler>().testAbbrev + "'";
                print(command);
                RunCommand(command);
            }
            else
            {
                return;
            }
        }
    }

    public static void SaveReports()
    {
        string[] tableNames = { "LetterID", "LetterSound", "OralComprehension", "OralVocabulary", "PhonemeManipulation", "ReadingComprehension",
                                "ReadingFluency", "Rhyming", "SentenceID", "SequentialLanguage", "SoundBlending", "VerbalWorkingMemory", "VerbalWorkingMemoryBackwards",
                                "VisualSearch", "WordID" };

        foreach(string testName in tableNames)
        {
            string command = "";
            if(!testName.Equals("SequentialLanguage"))
                command = "select studentID, correctness from CARE1." + testName;
            else
                command = "select studentID, points from CARE1." + testName;
            List<List<string>> output = RunCommand(command);

            int prevIndex = int.Parse(output[0][0]);
            string row = output[0][0];

            Dictionary<int, string> dataHolder = new Dictionary<int, string>();

            foreach (List<string> l in output)
            {
                if (dataHolder.ContainsKey(int.Parse(l[0])))
                {
                    dataHolder[int.Parse(l[0])] += l[1] + ",";
                }
                else
                {
                    dataHolder.Add(int.Parse(l[0]), l[0] + "," + l[1] + ",");
                }
            }

            string filePath = "data/" + testName + "-irt.csv";
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            System.IO.FileInfo fileInfo = new System.IO.FileInfo(filePath);
            fileInfo.Directory.Create(); // If the directory already exists, this method does nothing.

            foreach (string s in dataHolder.Values)
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(filePath, true))
                {
                    file.Write(s + "\n");
                }
        }
    }

    public static void CloseConnection()
    {
        conn.Close();
    }

    public static bool IsBusy()
    {
        return isBusy;
    }
}
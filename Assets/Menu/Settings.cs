using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

public class Settings : MonoBehaviour
{

    public static bool hasPermissions = true;

    public static string dateTime = "";
    public static string DateTimeM = ""; //Michael Added code 7.22.2019 to try and push SQL dot to UNIVERSAL
    List<Toggle> testToggles = new List<Toggle>();
    public Button saveAndStartButton;

    public GameObject dropDown;
    public GameObject dropDownStudents;
    
    // NOTE: this turns on/off the demo mode e.g. shows test and item id in top left corner
    public static bool DemoMode = false;

    public static string studentID = "";
    public static string userID = "";
    public static int testID = -1;

    public Dropdown students;
    public Dropdown teachers;
    public List<string> studentIDs;
    public List<int> teacherIDs;
    int currentTeacher = -1;

    //Reading tests
    public static bool LI, LSSI, LW, SI, RF, RC, WIS;
    public Toggle LI_T, LSSI_T, LW_T, SI_T, RF_T, RC_T, WIS_T;

    //Diagnostic tests
    public static bool OV, OC, PM, SB, RYM, RAN, RLNNL, RLNNN, RLNNM, LF;
    public Toggle OV_T, OC_T, PM_T, SB_T, RYM_T, RAN_T, RLNNL_T, RLNNN_T, RLNNM_T, LF_T;

    //Cognitive tests
    public static bool SO1, SO2, VWM, VWMB, VIS, SLC, GNG;
    public Toggle SO1_T, SO2_T, VWM_T, VWMB_T, VIS_T, SLC_T, GNG_T;

    //Bender Drawing
    public static bool BG = true;

    public static bool research;
    public Toggle research_T;
    public static bool report, raw, email, harddrive, xlsx;
    public Toggle report_T, raw_T, email_T, harddrive_T, xlsx_T;

    public static string first = "First", last = "Last", dob = "01/01/2000", dot = DateTime.Today.ToString().Split(' ')[0], id = "CareUser", sex = "M", grade = "1";
    public InputField first_I, last_I, dob_I, dot_I, id_I, sex_I, grade_I;

    private static bool settings1, settings2, settings3, settings4, settings5;
    private static bool[] settings1tests, settings2tests, settings3tests, settings4tests, settings5tests;

    #region "These are the check box functions"
    public void LetterID(bool input)
    {
        LI = input;
        GameObject.Find("Canvas/Highlights/LI").SetActive(input);
    }
    public void LetterSymbol(bool input)
    {
        LSSI = input;
        GameObject.Find("Canvas/Highlights/LS").SetActive(input);
    }
    public void LetterWord(bool input)
    {
        LW = input;
        GameObject.Find("Canvas/Highlights/WI").SetActive(input);
    }
    public void SentenceID(bool input)
    {
        SI = input;
        GameObject.Find("Canvas/Highlights/SI").SetActive(input);
    }
    public void ReadingFluency(bool input)
    {
        RF = input;
        GameObject.Find("Canvas/Highlights/RF").SetActive(input);
    }
    public void ReadingComp(bool input)
    {
        RC = input;
        GameObject.Find("Canvas/Highlights/RC").SetActive(input);
    }
    public void OralVocab(bool input)
    {
        OV = input;
        GameObject.Find("Canvas/Highlights/OV").SetActive(input);
    }
    public void OralComp(bool input)
    {
        OC = input;
        GameObject.Find("Canvas/Highlights/OC").SetActive(input);
    }
    public void Phoneme(bool input)
    {
        PM = input;
        GameObject.Find("Canvas/Highlights/PM").SetActive(input);
    }
    public void SoundBlending(bool input)
    {
        SB = input;
        GameObject.Find("Canvas/Highlights/SB").SetActive(input);
    }
    public void Rhyming(bool input)
    {
        RYM = input;
        GameObject.Find("Canvas/Highlights/RYM").SetActive(input);
    }
    public void RapidAutoName(bool input)
    {
        RAN = input;
        GameObject.Find("Canvas/Highlights/RAN").SetActive(input);
    }
    public void RapidLetterNumber_N(bool input)
    {
        RLNNL = input;
        RLNNN = input;
        RLNNM = input;
        GameObject.Find("Canvas/Highlights/RLNN").SetActive(input);
    }
    public void LexicalFluency(bool input)
    {
        LF = input;
        GameObject.Find("Canvas/Highlights/LF").SetActive(input);
    }
    public void SortingTask1(bool input)
    {
        SO1 = input;
        GameObject.Find("Canvas/Highlights/CS1").SetActive(input);
    }
    public void SortingTask2(bool input)
    {
        SO2 = input;
        GameObject.Find("Canvas/Highlights/CS2").SetActive(input);
    }
    public void SeqLang(bool input)
    {
        SLC = input;
        GameObject.Find("Canvas/Highlights/SL").SetActive(input);
    }
    public void VerbalMemory(bool input)
    {
        VWM = input;
        GameObject.Find("Canvas/Highlights/VWM").SetActive(input);
    }
    public void VerbalMemoryBackwards(bool input)
    {
        VWMB = input;
        GameObject.Find("Canvas/Highlights/VWMB").SetActive(input);
    }
    public void VisualSearch(bool input)
    {
        VIS = input;
        GameObject.Find("Canvas/Highlights/VS").SetActive(input);
    }
    public void GoNoGo(bool input)
    {
        GNG = input;
        GameObject.Find("Canvas/Highlights/GNG").SetActive(input);
    }
    public void WordIDSpanish(bool input)
    {
        WIS = input;
        GameObject.Find("Canvas/Highlights/WIS").SetActive(input);
    }
    #endregion
    #region "These are the button functions"
    public void ClearAll()
    {
        LI_T.isOn = LI = false;
        LSSI_T.isOn = LSSI = false;
        LW_T.isOn = LW = false;
        SI_T.isOn = SI = false;
        RF_T.isOn = RF = false;
        RC_T.isOn = RC = false;
        OV_T.isOn = OV = false;
        OC_T.isOn = OC = false;
        PM_T.isOn = PM = false;
        SB_T.isOn = SB = false;
        RYM_T.isOn = RYM = false;
        RAN_T.isOn = RAN = false;
        RLNNL_T.isOn = RLNNL = false;
        RLNNN_T.isOn = RLNNN = false;
        RLNNM_T.isOn = RLNNM = false;
        LF_T.isOn = LF = false;
        SO1_T.isOn = SO1 = false;
        SO2_T.isOn = SO2 = false;
        VWM_T.isOn = VWM = false;
        VWMB_T.isOn = VWMB = false;
        VIS_T.isOn = VIS = false;
        SLC_T.isOn = SLC = false;
        GNG_T.isOn = GNG = false;
        WIS_T.isOn = WIS = false;
    }

    public void SetSettings1()
    {
        LI_T.isOn = LI = true;
        LSSI_T.isOn = LSSI = true;
        OV_T.isOn = OV = true;
        SB_T.isOn = SB = true;
        RYM_T.isOn = RYM = true;
     }

    public void SetSettings2()
    {
        LW_T.isOn = LW = true;
        SI_T.isOn = SI = true;
        OC_T.isOn = OC = true;
        SB_T.isOn = SB = true;
        PM_T.isOn = PM = true;
        VWM_T.isOn = VWM = true;
    }

    public void SetSettings3()
    {
        RF_T.isOn = RF = true;
        RC_T.isOn = RC = true;
        VWMB_T.isOn = VWMB = true;
        SLC_T.isOn = SLC = true;
    }

    public void SetSettings4()
    {
        SO1_T.isOn = SO1 = true;
        SO2_T.isOn = SO2 = true;
        VIS_T.isOn = VIS = true;
        GNG_T.isOn = GNG = true;
    }
    public void SetSettings5()
    {
        LI_T.isOn = LI = true;
        LSSI_T.isOn = LSSI = true;
        LW_T.isOn = LW = true;
        SI_T.isOn = SI = true;
        RF_T.isOn = RF = true;
        RC_T.isOn = RC = true;
        OV_T.isOn = OV = true;
        OC_T.isOn = OC = true;
        PM_T.isOn = PM = true;
        SB_T.isOn = SB = true;
        RYM_T.isOn = RYM = true;
        RAN_T.isOn = RAN = true;
        RLNNL_T.isOn = RLNNL = true;
        RLNNN_T.isOn = RLNNN = true;
        RLNNM_T.isOn = RLNNM = true;
        LF_T.isOn = LF = true;
        SO1_T.isOn = SO1 = true;
        SO2_T.isOn = SO2 = true;
        VWM_T.isOn = VWM = true;
        VWMB_T.isOn = VWMB = true;
        VIS_T.isOn = VIS = true;
        SLC_T.isOn = SLC = true;
        GNG_T.isOn = GNG = true;
    }

    public void ResetSettings()
    {
        LI_T.isOn = LI = false;
        LSSI_T.isOn = LSSI = false;
        LW_T.isOn = LW = false;
        SI_T.isOn = SI = false;
        RF_T.isOn = RF = false;
        RC_T.isOn = RC = false;
        OV_T.isOn = OV = false;
        OC_T.isOn = OC = false;
        PM_T.isOn = PM = false;
        SB_T.isOn = SB = false;
        RYM_T.isOn = RYM = false;
        RAN_T.isOn = RAN = false;
        RLNNL_T.isOn = RLNNL = false;
        RLNNN_T.isOn = RLNNN = false;
        RLNNM_T.isOn = RLNNM = false;
        LF_T.isOn = LF = false;
        SO1_T.isOn = SO1 = false;
        SO2_T.isOn = SO2 = false;
        VWM_T.isOn = VWM = false;
        VWMB_T.isOn = VWMB = false;
        VIS_T.isOn = VIS = false;
        SLC_T.isOn = SLC = false;
        GNG_T.isOn = GNG = false;
    }

    public void NoPermissions()
    {
        LI_T.enabled = false;
        LSSI_T.enabled = false;
        LW_T.enabled = false;
        SI_T.enabled = false;
        RF_T.enabled = false;
        RC_T.enabled = false;
        OV_T.enabled = false;
        OC_T.enabled = false;
        PM_T.enabled = false;
        SB_T.enabled = false;
        RYM_T.enabled = false;
        RAN_T.enabled = false;
        RLNNL_T.enabled = false;
        RLNNN_T.enabled = false;
        RLNNM_T.enabled = false;
        LF_T.enabled = false;
        SO1_T.enabled = false;
        SO2_T.enabled = false;
        VWM_T.enabled = false;
        VWMB_T.enabled = false;
        VIS_T.enabled = false;
        SLC_T.enabled = false;
        GNG_T.enabled = false;
        WIS_T.enabled = false;
        BG = false;
        report_T.enabled = false;
        raw_T.enabled = false;
        email_T.enabled = false;
        harddrive_T.enabled = false;
        xlsx_T.enabled = false;
        research_T.enabled = false;

        first_I.enabled = false;
        last_I.enabled = false;
        dob_I.enabled = false;
        dot_I.enabled = false;
        id_I.enabled = false;
        sex_I.enabled = false;
    }

    public void SaveAndReturn()
    {
        Debug.Log("Save and Return");
        SceneHandler.sceneActive[0] = LI;
        SceneHandler.sceneActive[1] = LSSI;
        SceneHandler.sceneActive[2] = LW;
        SceneHandler.sceneActive[3] = SI;
        SceneHandler.sceneActive[4] = RC;
        SceneHandler.sceneActive[5] = RF;
        SceneHandler.sceneActive[6] = OV;
        SceneHandler.sceneActive[7] = OC;
        SceneHandler.sceneActive[8] = SLC;
        SceneHandler.sceneActive[9] = RYM;
        SceneHandler.sceneActive[10] = SB;
        SceneHandler.sceneActive[11] = PM;
        SceneHandler.sceneActive[12] = LF;
        SceneHandler.sceneActive[13] = RAN;
        SceneHandler.sceneActive[14] = RLNNN;
        SceneHandler.sceneActive[15] = RLNNL;
        SceneHandler.sceneActive[16] = RLNNM;
        SceneHandler.sceneActive[17] = VWM;
        SceneHandler.sceneActive[18] = VWMB;
        SceneHandler.sceneActive[19] = VIS;
        SceneHandler.sceneActive[20] = SO1;
        SceneHandler.sceneActive[21] = SO1;
        SceneHandler.sceneActive[22] = SO1;
        SceneHandler.sceneActive[23] = SO2;
        SceneHandler.sceneActive[24] = GNG;
        SceneHandler.sceneActive[25] = WIS;


        //Always keep this last and update the index/number so that Bender is given at end as an activity
        SceneHandler.sceneActive[27] = BG;

        // TODO: start inside if to check internet
        if (InternetAvailable.internetAvailableStatic)
        {

            Debug.Log($"studentIDs count= {studentIDs.Count}");
            dateTime = DateTime.Now.ToString("MM-dd-yyyy HH.mm.ss");
            studentID = studentIDs[students.value];
            PushTestSession();
            //Michael added new dateTime variable below -- matches the datetime of the one generated in SQL when test session pushed
            DateTimeM = SQLHandler.RunCommand("select date_of_testing from CARE1.student_test_info where test_num = " + Settings.testID.ToString())[0][0];
            DateTimeM = System.DateTime.Parse(DateTimeM).ToString("yyyy/MM/dd");
            print(studentID);
            UpdateTestInfo();
        }
        // TODO: end inside if to check internet
        SceneHandler.GoToNextScene();
    }

    public void PushTestSession()
    {
        string values = string.Format("values ('{0}'); ", studentID);
        string command = "insert into CARE1.student_test_info (student_id) " + values;

        command += "select last_insert_id();";

        List<List<string>> output = SQLHandler.RunCommand(command);
        Int32.TryParse(output[0][0], out testID);

        print(testID + " " + command);

        //not sure how often to close the connection
        //SQLHandler.CloseConnection();
    }
    #endregion
    #region "These are the data output functions"
    public void SetReport(bool input)
    {
        report = input;
    }
    public void setRaw(bool input)
    {
        raw = input;
    }
    public void setEmail(bool input)
    {
        email = input;
    }
    public void setHarddrive(bool input)
    {
        harddrive = input;
    }
    public void setXlsx(bool input)
    {
        xlsx = input;
    }
    public void setResearch(bool input)
    {
        research = input;
    }
    #endregion
    #region "These are the demographic inputs"
    public void firstname(string input)
    {
        first = input;
    }
    public void lastname(string input)
    {
        last = input;
    }
    public void dateofbirth(string input)
    {
        dob = input;
    }
    public void dateoftest(string input)
    {
        dot = input;
    }
    public void identifier(string input)
    {
        id = input;
    }
    public void setsex(string input)
    {
        sex = input;
    }
    public void setgrade(string input)
    {
        grade = input;
    }
    #endregion

    void FillDropdown()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        //Grab organization identifier
        List<List<string>> valsOrganID = SQLHandler.RunCommand("Select Distinct organization_id FROM university.users where uuid = '" + userID + "'");
        //Grab type of user -- Michael
        List<List<string>> valsAccountType = SQLHandler.RunCommand("Select Distinct type_id FROM university.users where uuid = '" + userID + "'");

        List<List<string>> vals = null;
        //List<List<string>> vals = SQLHandler.RunCommand("SELECT id, first_name, last_name FROM CARE1.students WHERE user_id = " + userID);
        print(valsAccountType[0][0]); // -- Michael
        if (valsAccountType[0][0] == "2")
        {
            //Grab admin's list of teachers 
            //It looks like Goldfish automatically pushes the created_by id as the staff account, so this won't work even if logged in through admin -- Michael
            vals = SQLHandler.RunCommand("Select Distinct name FROM university.users where organization_id = '" + valsOrganID[0][0].ToString() + "'" + " and type_id = 3");
        }
        else if (valsAccountType[0][0] == "3")
        {
            //Grab only this teacher
            vals = SQLHandler.RunCommand("Select Distinct name FROM university.users where uuid = '" + userID + "'");
        }

        //Order the list??? -- Michael
        vals = vals.OrderBy(s => new string(Regex.Replace(s[0], "[^a-zA-Z]", "").Reverse().ToArray())).ToList();

        foreach (List<string> teacherInfo in vals)
        {
            Dropdown.OptionData o = new Dropdown.OptionData();
            o.text = teacherInfo[0];
            options.Add(o);
        }

        teachers.options = options;
        print("Value");
        print(teachers.options[teachers.value].text);
        currentTeacher = 0;
    }

    void FillDropDownStudents()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        List<List<string>> valsOrganID = SQLHandler.RunCommand("Select Distinct organization_id FROM university.users where uuid = '" + userID + "'");
        print(teachers.options[teachers.value].text);
        List<List<string>> valsCreatedBy = SQLHandler.RunCommand("Select Distinct uuid FROM university.users where name = '" + MySqlHelper.EscapeString(teachers.options[teachers.value].text) + "'" + "and organization_id = '" + valsOrganID[0][0] + "'");

        //List<List<string>> vals = null;
        //if (teachers.options[teachers.value].text.Contains("staff"))
        //{
        //    vals = SQLHandler.RunCommand("SELECT id, first_name, last_name FROM university.students WHERE organization_id = " + valsOrganID[0][0].ToString() + " and class_id is NULL");
        //}
        //else
        //{
        List<List<string>> vals = SQLHandler.RunCommand("SELECT student_id, first_name, last_name FROM university.students WHERE organization_id = '" + valsOrganID[0][0].ToString() + "' and created_by = '" + valsCreatedBy[0][0].ToString() + "'");
        //}
        //vals = vals.OrderBy(s => new string(Regex.Replace(s[0], "[^a-zA-Z]", "").Reverse().ToArray())).ToList();

        studentIDs.Clear();

        foreach (List<string> studentInfo in vals)
        {
            Dropdown.OptionData o = new Dropdown.OptionData();
            print(studentInfo[0]);
            o.text = studentInfo[1] + " " + studentInfo[2];
            //subset of list is making ids not synced
            studentIDs.Add(studentInfo[0]);
            options.Add(o);

        }


        students.options = options;
    }

    void UpdateTestInfo()
    {
        string values = string.Format("update CARE1.student_test_info set LI = {0}, LSSI = {1}, WI = {2}, SI = {3}, RC = {4}, RF = {5}, OV = {6}, OC = {7}, SLC = {8}, RYM = {9}, SB = {10}, PM = {11}, VWM = {12}, VWMB = {13}, VIS = {14}, CSC = {15}, CSS = {16}, CSA = {17}, WCST = {18}, WIS = {19}, Research_Data = {20}",
            LI ? 9 : 0, LSSI ? 9 : 0, LW ? 9 : 0, SI ? 9 : 0, RC ? 9 : 0, RF ? 9 : 0, OV ? 9 : 0, OC ? 9 : 0, SLC ? 9 : 0, RYM ? 9 : 0, SB ? 9 : 0, PM ? 9 : 0, VWM ? 9 : 0, VWMB ? 9 : 0, VIS ? 9 : 0, SO1 ? 9 : 0, SO1 ? 9 : 0, SO1 ? 9 : 0, SO2 ? 9 : 0, WIS ? 9 : 0, research);
        string command = values + " where test_num = " + testID;
        print(command);
        SQLHandler.RunCommand(command);

        for (int i = 0; i < SceneHandler.abbrevs.Length; i++)
        {
            if (SceneHandler.sceneActive[i])
            {
                string values2 = string.Format("values ('{0}', '{1}', '{2}', '{3}'); ", studentID, testID, SceneHandler.abbrevs[i], 9);
                string command2 = "insert into university.student_test_info (student_id, test_num, test_type, result) " + values;
                SQLHandler.RunCommand(command2);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        SQLHandler.CheckConnection();
        testToggles.AddRange(new Toggle[] { LI_T, LSSI_T, LW_T, SI_T, RF_T, RC_T, OV_T, OC_T, PM_T, SB_T, RYM_T, RAN_T, RLNNL_T, RLNNN_T, RLNNM_T, LF_T,
        SO1_T, SO2_T, VWM_T, VWMB_T, VIS_T, SLC_T, GNG_T, WIS_T});

        ClearAll();
        if (!hasPermissions)
        {
            NoPermissions();
        }
        //Michael changed this to SQLHandler.State instead of internet reachability since the database is what matters?
        if (Application.internetReachability != NetworkReachability.NotReachable && SQLHandler.state)
        {
            FillDropdown();
            FillDropDownStudents();
        }
        else
        {
            studentIDs.Add("Database Not Connected");
            dropDown.SetActive(false);
            dropDownStudents.SetActive(false);
        }
        SceneHandler.currScene = 0;

        if (dateTime.Equals(""))
            dateTime = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss");
    }

    void Update()
    {
        if (teachers.value != currentTeacher)
        {
            if (SQLHandler.state)
            {
                FillDropDownStudents();
            }

            currentTeacher = teachers.value;
        }
        //if(Input.GetKeyDown(KeyCode.M))
        //{
        //    SQLHandler.SaveReports();
        //}
        bool allTogglesOff = true;


        for (int i = 0; i < testToggles.Count; i++)
        {
            if (testToggles[i].isOn)
            {
                allTogglesOff = false;
            }
            if (!allTogglesOff)
            {
                saveAndStartButton.interactable = true;
            }else if (allTogglesOff)
            {
                saveAndStartButton.interactable = false;
            }
        }
    }
}

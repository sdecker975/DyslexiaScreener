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
    public static bool LI, LSSI, LW, SI, RF, RC;
    public Toggle LI_T, LSSI_T, LW_T, SI_T, RF_T, RC_T;

    //Diagnostic tests
    public static bool OV, OC, PM, SB, RYM, RAN, RLNNL, RLNNN, RLNNM, LF;
    public Toggle OV_T, OC_T, PM_T, SB_T, RYM_T, RAN_T, RLNNL_T, RLNNN_T, RLNNM_T, LF_T;

    //Cognitive tests
    public static bool SO1, SO2, VWM, VWMB, VIS, SLC, GNG;
    public Toggle SO1_T, SO2_T, VWM_T, VWMB_T, VIS_T, SLC_T, GNG_T;

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
        GameObject.Find("Canvas/Highlights/NF").SetActive(input);
    }
    public void VerbalMemoryBackwards(bool input)
    {
        VWMB = input;
        GameObject.Find("Canvas/Highlights/NB").SetActive(input);
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
    }

    public void SetSettings1()
    {
        if (!settings1)
        {
            settings1 = true;
            if (settings1tests == null) settings1tests = new bool[23];
            GameObject.Find("Canvas/Dots/Button 1/LI").SetActive(settings1tests[0] = LI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/LS").SetActive(settings1tests[1] = LSSI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/WI").SetActive(settings1tests[2] = LW_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/SI").SetActive(settings1tests[3] = SI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/RF").SetActive(settings1tests[4] = RF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/RC").SetActive(settings1tests[5] = RC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/OV").SetActive(settings1tests[6] = OV_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/OC").SetActive(settings1tests[7] = OC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/PM").SetActive(settings1tests[8] = PM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/SB").SetActive(settings1tests[9] = SB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/RYM").SetActive(settings1tests[10] = RYM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/RAN").SetActive(settings1tests[11] = RAN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/RLNN").SetActive(settings1tests[12] = RLNNL_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/RLNN").SetActive(settings1tests[13] = RLNNN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/RLNN").SetActive(settings1tests[14] = RLNNM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/LF").SetActive(settings1tests[15] = LF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/CS1").SetActive(settings1tests[16] = SO1_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/CS2").SetActive(settings1tests[17] = SO2_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/NF").SetActive(settings1tests[18] = VWM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/NB").SetActive(settings1tests[19] = VWMB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/VS").SetActive(settings1tests[20] = VIS_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/SL").SetActive(settings1tests[21] = SLC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 1/GNG").SetActive(settings1tests[22] = GNG_T.isOn);
        }
        else
        {
            LI_T.isOn = LI = settings1tests[0];
            LSSI_T.isOn = LSSI = settings1tests[1];
            LW_T.isOn = LW = settings1tests[2];
            SI_T.isOn = SI = settings1tests[3];
            RF_T.isOn = RF = settings1tests[4];
            RC_T.isOn = RC = settings1tests[5];
            OV_T.isOn = OV = settings1tests[6];
            OC_T.isOn = OC = settings1tests[7];
            PM_T.isOn = PM = settings1tests[8];
            SB_T.isOn = SB = settings1tests[9];
            RYM_T.isOn = RYM = settings1tests[10];
            RAN_T.isOn = RAN = settings1tests[11];
            RLNNL_T.isOn = RLNNL = settings1tests[12];
            RLNNN_T.isOn = RLNNN = settings1tests[13];
            RLNNM_T.isOn = RLNNM = settings1tests[14];
            LF_T.isOn = LF = settings1tests[15];
            SO1_T.isOn = SO1 = settings1tests[16];
            SO2_T.isOn = SO2 = settings1tests[17];
            VWM_T.isOn = VWM = settings1tests[18];
            VWMB_T.isOn = VWMB = settings1tests[19];
            VIS_T.isOn = VIS = settings1tests[20];
            SLC_T.isOn = SLC = settings1tests[21];
            GNG_T.isOn = GNG = settings1tests[22];
        }
    }

    public void ResetSettings1()
    {
        settings1tests = new bool[23];
        settings1 = false;
        foreach (Transform child in GameObject.Find("Canvas/Dots/Button 1").transform)
            child.gameObject.SetActive(false);
    }

    public void SetSettings2()
    {
        if (!settings2)
        {
            settings2 = true;
            if (settings2tests == null) settings2tests = new bool[23];
            GameObject.Find("Canvas/Dots/Button 2/LI").SetActive(settings2tests[0] = LI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/LS").SetActive(settings2tests[1] = LSSI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/WI").SetActive(settings2tests[2] = LW_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/SI").SetActive(settings2tests[3] = SI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/RF").SetActive(settings2tests[4] = RF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/RC").SetActive(settings2tests[5] = RC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/OV").SetActive(settings2tests[6] = OV_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/OC").SetActive(settings2tests[7] = OC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/PM").SetActive(settings2tests[8] = PM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/SB").SetActive(settings2tests[9] = SB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/RYM").SetActive(settings2tests[10] = RYM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/RAN").SetActive(settings2tests[11] = RAN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/RLNN").SetActive(settings2tests[12] = RLNNL_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/RLNN").SetActive(settings2tests[13] = RLNNN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/RLNN").SetActive(settings2tests[14] = RLNNM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/LF").SetActive(settings2tests[15] = LF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/CS1").SetActive(settings2tests[16] = SO1_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/CS2").SetActive(settings2tests[17] = SO2_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/NF").SetActive(settings2tests[18] = VWM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/NB").SetActive(settings2tests[19] = VWMB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/VS").SetActive(settings2tests[20] = VIS_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/SL").SetActive(settings2tests[21] = SLC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 2/GNG").SetActive(settings2tests[22] = GNG_T.isOn);
        }
        else
        {
            LI_T.isOn = LI = settings2tests[0];
            LSSI_T.isOn = LSSI = settings2tests[1];
            LW_T.isOn = LW = settings2tests[2];
            SI_T.isOn = SI = settings2tests[3];
            RF_T.isOn = RF = settings2tests[4];
            RC_T.isOn = RC = settings2tests[5];
            OV_T.isOn = OV = settings2tests[6];
            OC_T.isOn = OC = settings2tests[7];
            PM_T.isOn = PM = settings2tests[8];
            SB_T.isOn = SB = settings2tests[9];
            RYM_T.isOn = RYM = settings2tests[10];
            RAN_T.isOn = RAN = settings2tests[11];
            RLNNL_T.isOn = RLNNL = settings2tests[12];
            RLNNN_T.isOn = RLNNN = settings2tests[13];
            RLNNM_T.isOn = RLNNM = settings2tests[14];
            LF_T.isOn = LF = settings2tests[15];
            SO1_T.isOn = SO1 = settings2tests[16];
            SO2_T.isOn = SO2 = settings2tests[17];
            VWM_T.isOn = VWM = settings2tests[18];
            VWMB_T.isOn = VWMB = settings2tests[19];
            VIS_T.isOn = VIS = settings2tests[20];
            SLC_T.isOn = SLC = settings2tests[21];
            GNG_T.isOn = GNG = settings2tests[22];
        }
    }

    public void ResetSettings2()
    {
        settings2tests = new bool[23];
        settings2 = false;
        foreach (Transform child in GameObject.Find("Canvas/Dots/Button 2").transform)
            child.gameObject.SetActive(false);
    }

    public void SetSettings3()
    {
        if (!settings3)
        {
            settings3 = true;
            if (settings3tests == null) settings3tests = new bool[23];
            GameObject.Find("Canvas/Dots/Button 3/LI").SetActive(settings3tests[0] = LI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/LS").SetActive(settings3tests[1] = LSSI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/WI").SetActive(settings3tests[2] = LW_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/SI").SetActive(settings3tests[3] = SI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/RF").SetActive(settings3tests[4] = RF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/RC").SetActive(settings3tests[5] = RC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/OV").SetActive(settings3tests[6] = OV_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/OC").SetActive(settings3tests[7] = OC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/PM").SetActive(settings3tests[8] = PM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/SB").SetActive(settings3tests[9] = SB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/RYM").SetActive(settings3tests[10] = RYM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/RAN").SetActive(settings3tests[11] = RAN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/RLNN").SetActive(settings3tests[12] = RLNNL_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/RLNN").SetActive(settings3tests[13] = RLNNN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/RLNN").SetActive(settings3tests[14] = RLNNM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/LF").SetActive(settings3tests[15] = LF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/CS1").SetActive(settings3tests[16] = SO1_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/CS2").SetActive(settings3tests[17] = SO2_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/NF").SetActive(settings3tests[18] = VWM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/NB").SetActive(settings3tests[19] = VWMB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/VS").SetActive(settings3tests[20] = VIS_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/SL").SetActive(settings3tests[21] = SLC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 3/GNG").SetActive(settings3tests[22] = GNG_T.isOn);
        }
        else
        {
            LI_T.isOn = LI = settings3tests[0];
            LSSI_T.isOn = LSSI = settings3tests[1];
            LW_T.isOn = LW = settings3tests[2];
            SI_T.isOn = SI = settings3tests[3];
            RF_T.isOn = RF = settings3tests[4];
            RC_T.isOn = RC = settings3tests[5];
            OV_T.isOn = OV = settings3tests[6];
            OC_T.isOn = OC = settings3tests[7];
            PM_T.isOn = PM = settings3tests[8];
            SB_T.isOn = SB = settings3tests[9];
            RYM_T.isOn = RYM = settings3tests[10];
            RAN_T.isOn = RAN = settings3tests[11];
            RLNNL_T.isOn = RLNNL = settings3tests[12];
            RLNNN_T.isOn = RLNNN = settings3tests[13];
            RLNNM_T.isOn = RLNNM = settings3tests[14];
            LF_T.isOn = LF = settings3tests[15];
            SO1_T.isOn = SO1 = settings3tests[16];
            SO2_T.isOn = SO2 = settings3tests[17];
            VWM_T.isOn = VWM = settings3tests[18];
            VWMB_T.isOn = VWMB = settings3tests[19];
            VIS_T.isOn = VIS = settings3tests[20];
            SLC_T.isOn = SLC = settings3tests[21];
            GNG_T.isOn = GNG = settings3tests[22];
        }
    }

    public void ResetSettings3()
    {
        settings3tests = new bool[23];
        settings3 = false;
        foreach (Transform child in GameObject.Find("Canvas/Dots/Button 3").transform)
            child.gameObject.SetActive(false);
    }

    public void SetSettings4()
    {
        if (!settings4)
        {
            settings4 = true;
            if (settings4tests == null) settings4tests = new bool[23];
            GameObject.Find("Canvas/Dots/Button 4/LI").SetActive(settings4tests[0] = LI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/LS").SetActive(settings4tests[1] = LSSI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/WI").SetActive(settings4tests[2] = LW_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/SI").SetActive(settings4tests[3] = SI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/RF").SetActive(settings4tests[4] = RF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/RC").SetActive(settings4tests[5] = RC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/OV").SetActive(settings4tests[6] = OV_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/OC").SetActive(settings4tests[7] = OC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/PM").SetActive(settings4tests[8] = PM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/SB").SetActive(settings4tests[9] = SB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/RYM").SetActive(settings4tests[10] = RYM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/RAN").SetActive(settings4tests[11] = RAN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/RLNN").SetActive(settings4tests[12] = RLNNL_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/RLNN").SetActive(settings4tests[13] = RLNNN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/RLNN").SetActive(settings4tests[14] = RLNNM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/LF").SetActive(settings4tests[15] = LF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/CS1").SetActive(settings4tests[16] = SO1_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/CS2").SetActive(settings4tests[17] = SO2_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/NF").SetActive(settings4tests[18] = VWM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/NB").SetActive(settings4tests[19] = VWMB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/VS").SetActive(settings4tests[20] = VIS_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/SL").SetActive(settings4tests[21] = SLC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 4/GNG").SetActive(settings4tests[22] = GNG_T.isOn);
        }
        else
        {
            LI_T.isOn = LI = settings4tests[0];
            LSSI_T.isOn = LSSI = settings4tests[1];
            LW_T.isOn = LW = settings4tests[2];
            SI_T.isOn = SI = settings4tests[3];
            RF_T.isOn = RF = settings4tests[4];
            RC_T.isOn = RC = settings4tests[5];
            OV_T.isOn = OV = settings4tests[6];
            OC_T.isOn = OC = settings4tests[7];
            PM_T.isOn = PM = settings4tests[8];
            SB_T.isOn = SB = settings4tests[9];
            RYM_T.isOn = RYM = settings4tests[10];
            RAN_T.isOn = RAN = settings4tests[11];
            RLNNL_T.isOn = RLNNL = settings4tests[12];
            RLNNN_T.isOn = RLNNN = settings4tests[13];
            RLNNM_T.isOn = RLNNM = settings4tests[14];
            LF_T.isOn = LF = settings4tests[15];
            SO1_T.isOn = SO1 = settings4tests[16];
            SO2_T.isOn = SO2 = settings4tests[17];
            VWM_T.isOn = VWM = settings4tests[18];
            VWMB_T.isOn = VWMB = settings4tests[19];
            VIS_T.isOn = VIS = settings4tests[20];
            SLC_T.isOn = SLC = settings4tests[21];
            GNG_T.isOn = GNG = settings4tests[22];
        }
    }

    public void ResetSettings4()
    {
        settings4tests = new bool[23];
        settings4 = false;
        foreach (Transform child in GameObject.Find("Canvas/Dots/Button 4").transform)
            child.gameObject.SetActive(false);
    }

    public void SetSettings5()
    {
        if (!settings5)
        {
            settings5 = true;
            if (settings5tests == null) settings5tests = new bool[23];
            GameObject.Find("Canvas/Dots/Button 5/LI").SetActive(settings5tests[0] = LI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/LS").SetActive(settings5tests[1] = LSSI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/WI").SetActive(settings5tests[2] = LW_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/SI").SetActive(settings5tests[3] = SI_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/RF").SetActive(settings5tests[4] = RF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/RC").SetActive(settings5tests[5] = RC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/OV").SetActive(settings5tests[6] = OV_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/OC").SetActive(settings5tests[7] = OC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/PM").SetActive(settings5tests[8] = PM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/SB").SetActive(settings5tests[9] = SB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/RYM").SetActive(settings5tests[10] = RYM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/RAN").SetActive(settings5tests[11] = RAN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/RLNN").SetActive(settings5tests[12] = RLNNL_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/RLNN").SetActive(settings5tests[13] = RLNNN_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/RLNN").SetActive(settings5tests[14] = RLNNM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/LF").SetActive(settings5tests[15] = LF_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/CS1").SetActive(settings5tests[16] = SO1_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/CS2").SetActive(settings5tests[17] = SO2_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/NF").SetActive(settings5tests[18] = VWM_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/NB").SetActive(settings5tests[19] = VWMB_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/VS").SetActive(settings5tests[20] = VIS_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/SL").SetActive(settings5tests[21] = SLC_T.isOn);
            GameObject.Find("Canvas/Dots/Button 5/GNG").SetActive(settings5tests[22] = GNG_T.isOn);
        }
        else
        {
            LI_T.isOn = LI = settings5tests[0];
            LSSI_T.isOn = LSSI = settings5tests[1];
            LW_T.isOn = LW = settings5tests[2];
            SI_T.isOn = SI = settings5tests[3];
            RF_T.isOn = RF = settings5tests[4];
            RC_T.isOn = RC = settings5tests[5];
            OV_T.isOn = OV = settings5tests[6];
            OC_T.isOn = OC = settings5tests[7];
            PM_T.isOn = PM = settings5tests[8];
            SB_T.isOn = SB = settings5tests[9];
            RYM_T.isOn = RYM = settings5tests[10];
            RAN_T.isOn = RAN = settings5tests[11];
            RLNNL_T.isOn = RLNNL = settings5tests[12];
            RLNNN_T.isOn = RLNNN = settings5tests[13];
            RLNNM_T.isOn = RLNNM = settings5tests[14];
            LF_T.isOn = LF = settings5tests[15];
            SO1_T.isOn = SO1 = settings5tests[16];
            SO2_T.isOn = SO2 = settings5tests[17];
            VWM_T.isOn = VWM = settings5tests[18];
            VWMB_T.isOn = VWMB = settings5tests[19];
            VIS_T.isOn = VIS = settings5tests[20];
            SLC_T.isOn = SLC = settings5tests[21];
            GNG_T.isOn = GNG = settings5tests[22];
        }
    }

    public void ResetSettings5()
    {
        settings5tests = new bool[23];
        settings5 = false;
        foreach (Transform child in GameObject.Find("Canvas/Dots/Button 5").transform)
            child.gameObject.SetActive(false);
    }

    public void SetSettingsAtStart()
    {
        if (settings1)
        {
            GameObject.Find("Canvas/Dots/Button 1/LI").SetActive(settings1tests[0]);
            GameObject.Find("Canvas/Dots/Button 1/LS").SetActive(settings1tests[1]);
            GameObject.Find("Canvas/Dots/Button 1/WI").SetActive(settings1tests[2]);
            GameObject.Find("Canvas/Dots/Button 1/SI").SetActive(settings1tests[3]);
            GameObject.Find("Canvas/Dots/Button 1/RF").SetActive(settings1tests[4]);
            GameObject.Find("Canvas/Dots/Button 1/RC").SetActive(settings1tests[5]);
            GameObject.Find("Canvas/Dots/Button 1/OV").SetActive(settings1tests[6]);
            GameObject.Find("Canvas/Dots/Button 1/OC").SetActive(settings1tests[7]);
            GameObject.Find("Canvas/Dots/Button 1/PM").SetActive(settings1tests[8]);
            GameObject.Find("Canvas/Dots/Button 1/SB").SetActive(settings1tests[9]);
            GameObject.Find("Canvas/Dots/Button 1/RYM").SetActive(settings1tests[10]);
            GameObject.Find("Canvas/Dots/Button 1/RAN").SetActive(settings1tests[11]);
            GameObject.Find("Canvas/Dots/Button 1/RLNN").SetActive(settings1tests[12]);
            GameObject.Find("Canvas/Dots/Button 1/RLNN").SetActive(settings1tests[13]);
            GameObject.Find("Canvas/Dots/Button 1/RLNN").SetActive(settings1tests[14]);
            GameObject.Find("Canvas/Dots/Button 1/LF").SetActive(settings1tests[15]);
            GameObject.Find("Canvas/Dots/Button 1/CS1").SetActive(settings1tests[16]);
            GameObject.Find("Canvas/Dots/Button 1/CS2").SetActive(settings1tests[17]);
            GameObject.Find("Canvas/Dots/Button 1/NF").SetActive(settings1tests[18]);
            GameObject.Find("Canvas/Dots/Button 1/NB").SetActive(settings1tests[19]);
            GameObject.Find("Canvas/Dots/Button 1/VS").SetActive(settings1tests[20]);
            GameObject.Find("Canvas/Dots/Button 1/SL").SetActive(settings1tests[21]);
            GameObject.Find("Canvas/Dots/Button 1/GNG").SetActive(settings1tests[22]);

        }
        if (settings2)
        {
            GameObject.Find("Canvas/Dots/Button 2/LI").SetActive(settings2tests[0]);
            GameObject.Find("Canvas/Dots/Button 2/LS").SetActive(settings2tests[1]);
            GameObject.Find("Canvas/Dots/Button 2/WI").SetActive(settings2tests[2]);
            GameObject.Find("Canvas/Dots/Button 2/SI").SetActive(settings2tests[3]);
            GameObject.Find("Canvas/Dots/Button 2/RF").SetActive(settings2tests[4]);
            GameObject.Find("Canvas/Dots/Button 2/RC").SetActive(settings2tests[5]);
            GameObject.Find("Canvas/Dots/Button 2/OV").SetActive(settings2tests[6]);
            GameObject.Find("Canvas/Dots/Button 2/OC").SetActive(settings2tests[7]);
            GameObject.Find("Canvas/Dots/Button 2/PM").SetActive(settings2tests[8]);
            GameObject.Find("Canvas/Dots/Button 2/SB").SetActive(settings2tests[9]);
            GameObject.Find("Canvas/Dots/Button 2/RYM").SetActive(settings2tests[10]);
            GameObject.Find("Canvas/Dots/Button 2/RAN").SetActive(settings2tests[11]);
            GameObject.Find("Canvas/Dots/Button 2/RLNN").SetActive(settings2tests[12]);
            GameObject.Find("Canvas/Dots/Button 2/RLNN").SetActive(settings2tests[13]);
            GameObject.Find("Canvas/Dots/Button 2/RLNN").SetActive(settings2tests[14]);
            GameObject.Find("Canvas/Dots/Button 2/LF").SetActive(settings2tests[15]);
            GameObject.Find("Canvas/Dots/Button 2/CS1").SetActive(settings2tests[16]);
            GameObject.Find("Canvas/Dots/Button 2/CS2").SetActive(settings2tests[17]);
            GameObject.Find("Canvas/Dots/Button 2/NF").SetActive(settings2tests[18]);
            GameObject.Find("Canvas/Dots/Button 2/NB").SetActive(settings2tests[19]);
            GameObject.Find("Canvas/Dots/Button 2/VS").SetActive(settings2tests[20]);
            GameObject.Find("Canvas/Dots/Button 2/SL").SetActive(settings2tests[21]);
            GameObject.Find("Canvas/Dots/Button 2/GNG").SetActive(settings2tests[22]);
        }
        if (settings3)
        {
            GameObject.Find("Canvas/Dots/Button 3/LI").SetActive(settings3tests[0]);
            GameObject.Find("Canvas/Dots/Button 3/LS").SetActive(settings3tests[1]);
            GameObject.Find("Canvas/Dots/Button 3/WI").SetActive(settings3tests[2]);
            GameObject.Find("Canvas/Dots/Button 3/SI").SetActive(settings3tests[3]);
            GameObject.Find("Canvas/Dots/Button 3/RF").SetActive(settings3tests[4]);
            GameObject.Find("Canvas/Dots/Button 3/RC").SetActive(settings3tests[5]);
            GameObject.Find("Canvas/Dots/Button 3/OV").SetActive(settings3tests[6]);
            GameObject.Find("Canvas/Dots/Button 3/OC").SetActive(settings3tests[7]);
            GameObject.Find("Canvas/Dots/Button 3/PM").SetActive(settings3tests[8]);
            GameObject.Find("Canvas/Dots/Button 3/SB").SetActive(settings3tests[9]);
            GameObject.Find("Canvas/Dots/Button 3/RYM").SetActive(settings3tests[10]);
            GameObject.Find("Canvas/Dots/Button 3/RAN").SetActive(settings3tests[11]);
            GameObject.Find("Canvas/Dots/Button 3/RLNN").SetActive(settings3tests[12]);
            GameObject.Find("Canvas/Dots/Button 3/RLNN").SetActive(settings3tests[13]);
            GameObject.Find("Canvas/Dots/Button 3/RLNN").SetActive(settings3tests[14]);
            GameObject.Find("Canvas/Dots/Button 3/LF").SetActive(settings3tests[15]);
            GameObject.Find("Canvas/Dots/Button 3/CS1").SetActive(settings3tests[16]);
            GameObject.Find("Canvas/Dots/Button 3/CS2").SetActive(settings3tests[17]);
            GameObject.Find("Canvas/Dots/Button 3/NF").SetActive(settings3tests[18]);
            GameObject.Find("Canvas/Dots/Button 3/NB").SetActive(settings3tests[19]);
            GameObject.Find("Canvas/Dots/Button 3/VS").SetActive(settings3tests[20]);
            GameObject.Find("Canvas/Dots/Button 3/SL").SetActive(settings3tests[21]);
            GameObject.Find("Canvas/Dots/Button 3/GNG").SetActive(settings3tests[22]);
        }
        if (settings4)
        {
            GameObject.Find("Canvas/Dots/Button 4/LI").SetActive(settings4tests[0]);
            GameObject.Find("Canvas/Dots/Button 4/LS").SetActive(settings4tests[1]);
            GameObject.Find("Canvas/Dots/Button 4/WI").SetActive(settings4tests[2]);
            GameObject.Find("Canvas/Dots/Button 4/SI").SetActive(settings4tests[3]);
            GameObject.Find("Canvas/Dots/Button 4/RF").SetActive(settings4tests[4]);
            GameObject.Find("Canvas/Dots/Button 4/RC").SetActive(settings4tests[5]);
            GameObject.Find("Canvas/Dots/Button 4/OV").SetActive(settings4tests[6]);
            GameObject.Find("Canvas/Dots/Button 4/OC").SetActive(settings4tests[7]);
            GameObject.Find("Canvas/Dots/Button 4/PM").SetActive(settings4tests[8]);
            GameObject.Find("Canvas/Dots/Button 4/SB").SetActive(settings4tests[9]);
            GameObject.Find("Canvas/Dots/Button 4/RYM").SetActive(settings4tests[10]);
            GameObject.Find("Canvas/Dots/Button 4/RAN").SetActive(settings4tests[11]);
            GameObject.Find("Canvas/Dots/Button 4/RLNN").SetActive(settings4tests[12]);
            GameObject.Find("Canvas/Dots/Button 4/RLNN").SetActive(settings4tests[13]);
            GameObject.Find("Canvas/Dots/Button 4/RLNN").SetActive(settings4tests[14]);
            GameObject.Find("Canvas/Dots/Button 4/LF").SetActive(settings4tests[15]);
            GameObject.Find("Canvas/Dots/Button 4/CS1").SetActive(settings4tests[16]);
            GameObject.Find("Canvas/Dots/Button 4/CS2").SetActive(settings4tests[17]);
            GameObject.Find("Canvas/Dots/Button 4/NF").SetActive(settings4tests[18]);
            GameObject.Find("Canvas/Dots/Button 4/NB").SetActive(settings4tests[19]);
            GameObject.Find("Canvas/Dots/Button 4/VS").SetActive(settings4tests[20]);
            GameObject.Find("Canvas/Dots/Button 4/SL").SetActive(settings4tests[21]);
            GameObject.Find("Canvas/Dots/Button 4/GNG").SetActive(settings4tests[22]);
        }
        if (settings5)
        {
            GameObject.Find("Canvas/Dots/Button 5/LI").SetActive(settings5tests[0]);
            GameObject.Find("Canvas/Dots/Button 5/LS").SetActive(settings5tests[1]);
            GameObject.Find("Canvas/Dots/Button 5/WI").SetActive(settings5tests[2]);
            GameObject.Find("Canvas/Dots/Button 5/SI").SetActive(settings5tests[3]);
            GameObject.Find("Canvas/Dots/Button 5/RF").SetActive(settings5tests[4]);
            GameObject.Find("Canvas/Dots/Button 5/RC").SetActive(settings5tests[5]);
            GameObject.Find("Canvas/Dots/Button 5/OV").SetActive(settings5tests[6]);
            GameObject.Find("Canvas/Dots/Button 5/OC").SetActive(settings5tests[7]);
            GameObject.Find("Canvas/Dots/Button 5/PM").SetActive(settings5tests[8]);
            GameObject.Find("Canvas/Dots/Button 5/SB").SetActive(settings5tests[9]);
            GameObject.Find("Canvas/Dots/Button 5/RYM").SetActive(settings5tests[10]);
            GameObject.Find("Canvas/Dots/Button 5/RAN").SetActive(settings5tests[11]);
            GameObject.Find("Canvas/Dots/Button 5/RLNN").SetActive(settings5tests[12]);
            GameObject.Find("Canvas/Dots/Button 5/RLNN").SetActive(settings5tests[13]);
            GameObject.Find("Canvas/Dots/Button 5/RLNN").SetActive(settings5tests[14]);
            GameObject.Find("Canvas/Dots/Button 5/LF").SetActive(settings5tests[15]);
            GameObject.Find("Canvas/Dots/Button 5/CS1").SetActive(settings5tests[16]);
            GameObject.Find("Canvas/Dots/Button 5/CS2").SetActive(settings5tests[17]);
            GameObject.Find("Canvas/Dots/Button 5/NF").SetActive(settings5tests[18]);
            GameObject.Find("Canvas/Dots/Button 5/NB").SetActive(settings5tests[19]);
            GameObject.Find("Canvas/Dots/Button 5/VS").SetActive(settings5tests[20]);
            GameObject.Find("Canvas/Dots/Button 5/SL").SetActive(settings5tests[21]);
            GameObject.Find("Canvas/Dots/Button 5/GNG").SetActive(settings5tests[22]);
        }
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
        report_T.enabled = false;
        raw_T.enabled = false;
        email_T.enabled = false;
        harddrive_T.enabled = false;
        xlsx_T.enabled = false;

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
        string values = string.Format("update CARE1.student_test_info set LI = {0}, LSSI = {1}, WI = {2}, SI = {3}, RC = {4}, RF = {5}, OV = {6}, OC = {7}, SLC = {8}, RYM = {9}, SB = {10}, PM = {11}, VWM = {12}, VWMB = {13}, VIS = {14}, CSC = {15}, CSS = {16}, CSA = {17}, WCST = {18}",
            LI ? 9 : 0, LSSI ? 9 : 0, LW ? 9 : 0, SI ? 9 : 0, RC ? 9 : 0, RF ? 9 : 0, OV ? 9 : 0, OC ? 9 : 0, SLC ? 9 : 0, RYM ? 9 : 0, SB ? 9 : 0, PM ? 9 : 0, VWM ? 9 : 0, VWMB ? 9 : 0, VIS ? 9 : 0, SO1 ? 9 : 0, SO1 ? 9 : 0, SO1 ? 9 : 0, SO2 ? 9 : 0);
        string command = values + " where test_num = " + testID;
        print(command);
        SQLHandler.RunCommand(command);
    }

    // Use this for initialization
    void Start()
    {
        testToggles.AddRange(new Toggle[] { LI_T, LSSI_T, LW_T, SI_T, RF_T, RC_T, OV_T, OC_T, PM_T, SB_T, RYM_T, RAN_T, RLNNL_T, RLNNN_T, RLNNM_T, LF_T,
        SO1_T, SO2_T, VWM_T, VWMB_T, VIS_T, SLC_T, GNG_T});

        ClearAll();
        if (!hasPermissions)
        {
            NoPermissions();
        }
        if (InternetAvailable.internetAvailableStatic)
        {
            FillDropdown();
            FillDropDownStudents();
        }/*
        else
        {
            dropDown.SetActive(false);
            dropDownStudents.SetActive(false);
        }
        */
        SetSettingsAtStart();
        SceneHandler.currScene = 0;

        if (dateTime.Equals(""))
            dateTime = DateTime.Now.ToString("dd-MM-yyyy HH.mm.ss");
    }

    void Update()
    {
        if (teachers.value != currentTeacher)
        {
            if (InternetAvailable.internetAvailableStatic)
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

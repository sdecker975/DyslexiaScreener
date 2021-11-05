using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PanelHandler : MonoBehaviour {

    public Dropdown dropdownStudents;
    public InputField studentIdInput;
    public InputField firstNameInput;
    public InputField lastNameInput;
    public InputField adminEmailInput;
    public Text dateText;

    public Toggle grade1;
    public Toggle grade2;
    public Toggle grade3;
    public Toggle grade4;
    public Toggle sex1;
    public Toggle sex2;
    public Toggle race1;
    public Toggle race2;
    public Toggle race3;
    public Toggle race4;
    public Toggle race5;
    public Toggle race6;
    public Toggle specialEd1;
    public Toggle specialEd2;
    public Toggle specialEd3;
    public Toggle specialEd4;
    
    // public Button saveAndContinueButton;
    public StudentInfo info;
    [FormerlySerializedAs("AddChildCanvas")]
    public GameObject addChildCanvas;

    private string studentInfoString;

    private UnityEngine.EventSystems.EventSystem eventSystem;

    public void Start() {
        eventSystem = UnityEngine.EventSystems.EventSystem.current;
    }
    
    public void Update() {
        if (Input.GetKeyDown(KeyCode.Tab)) {
            var next = eventSystem.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null) {
                var inputField = next.GetComponent<InputField>();

                //if it's an input field, also set the text caret
                if (inputField != null) {
                    inputField.OnPointerClick(new PointerEventData(eventSystem));
                }

                eventSystem.SetSelectedGameObject(next.gameObject, new BaseEventData(eventSystem));
            } else if (next == null) {
                eventSystem.SetSelectedGameObject(studentIdInput.gameObject, new BaseEventData(eventSystem));
            }
        }
    }

    /*
    public void LoadProfileButton()
    {
        
        //info = JsonUtility.FromJson<StudentInfo>(studentInfoString);
        Debug.Log(info.StudentId);
        studentIdInput.text = info.StudentId;
        firstNameInput.text = info.FirstName;
        lastNameInput.text = info.LastName;
        adminEmailInput.text = info.AdminEmail;
        dateText.text = info.DateOfBirth;
        switch (info.Grade)
        {
            case Grade.Grade0:
                grade1.isOn = true;
                break;
            case Grade.Grade1:
                grade2.isOn = true;
                break;
            case Grade.Grade2:
                grade3.isOn = true;
                break;
            case Grade.Grade3:
                grade4.isOn = true;
                break;
            default:
                grade1.isOn = true;
                break;
        }
        switch (info.Sex)
        {
            case Sex.Male:
                sex1.isOn = true;
                break;
            case Sex.Female:
                sex2.isOn = true;
                break;
            default:
                sex1.isOn = true;
                break;
        }
        switch (info.Race)
        {
            case Race.AfricanAmericanOrBlack:
                race1.isOn = true;
                break;
            case Race.HawaiianOrOtherPacificIslander:
                race2.isOn = true;
                break;
            case Race.WhiteOrCaucasian:
                race3.isOn = true;
                break;
            case Race.NativeAmericanOrAlaskanNative:
                race4.isOn = true;
                break;
            case Race.HispanicOrLatino:
                race5.isOn = true;
                break;
            case Race.Asian:
                race6.isOn = true;
                break;
            default:
                race1.isOn = true;
                break;
        }
        switch (info.SpecialEducation)
        {
            case SpecialEducation.NotEligibleOrNotListed:
                specialEd1.isOn = true;
                break;
            case SpecialEducation.ReadingServicesTier2:
                specialEd2.isOn = true;
                break;
            case SpecialEducation.ReadingServicesTier3:
                specialEd3.isOn = true;
                break;
            case SpecialEducation.SpeechServices:
                specialEd4.isOn = true;
                break;
            default:
                specialEd1.isOn = true;
                break;
        }
    }
    */
    public void AddChildToDropDown() {
        
        info.SelectedStudent.StudentId = studentIdInput.text;
        info.SelectedStudent.FirstName = firstNameInput.text;
        info.SelectedStudent.LastName = lastNameInput.text;

        info.SelectedStudent.AdminEmail = adminEmailInput.text;
        info.SelectedStudent.DateOfBirth = dateText.text;
        studentInfoString = JsonUtility.ToJson(info);
        Debug.Log(studentInfoString);
        var filePath = Path.Combine(Application.persistentDataPath, "studentInfo.json");
        File.WriteAllText(filePath, studentInfoString);

        if (info.SelectedStudent.FirstName == "" && info.SelectedStudent.LastName == "") {
            info.SelectedStudent.FirstName = "(No name selected)";
        }
        /*
        if (dropdownStudents != null)
            dropdownStudents.ClearOptions();
        */

        var offlineDropDownOption = new Dropdown.OptionData {
            text = info.SelectedStudent.FirstName + " " + info.SelectedStudent.LastName
        };

        if (dropdownStudents != null) {
            dropdownStudents.options.Add(offlineDropDownOption);
            dropdownStudents.captionText.text = offlineDropDownOption.text;
            dropdownStudents.value = 0; // select first element
        }

        addChildCanvas.SetActive(false);
    }

}
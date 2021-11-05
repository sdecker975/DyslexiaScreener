using UnityEngine;

public class GradeToggle : MonoBehaviour {

    [SerializeField] private StudentInfo studentInfo;
    [SerializeField] private Grade grade;

    public void OnValueChanged(bool selected) {
        if (selected) {
            studentInfo.SelectedStudent.Grade = grade;
        }
    }

}
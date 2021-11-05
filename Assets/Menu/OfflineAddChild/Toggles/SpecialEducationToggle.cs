using UnityEngine;

public class SpecialEducationToggle : MonoBehaviour
{

    [SerializeField] private StudentInfo studentInfo;
    [SerializeField] private SpecialEducation specialEducation;

    public void OnValueChanged(bool selected)
    {
        if (selected)
        {
            studentInfo.SelectedStudent.SpecialEducation = specialEducation;
        }
    }

}
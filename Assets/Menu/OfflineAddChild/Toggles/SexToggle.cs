using UnityEngine;

public class SexToggle : MonoBehaviour
{

    [SerializeField] private StudentInfo studentInfo;
    [SerializeField] private Sex sex;

    public void OnValueChanged(bool selected)
    {
        if (selected)
        {
            studentInfo.SelectedStudent.Sex = sex;
        }
    }

}
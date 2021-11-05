using UnityEngine;

public class RaceToggle : MonoBehaviour
{

    [SerializeField] private StudentInfo studentInfo;
    [SerializeField] private Race race;

    public void OnValueChanged(bool selected)
    {
        if (selected)
        {
            studentInfo.SelectedStudent.Race = race;
        }
    }

}
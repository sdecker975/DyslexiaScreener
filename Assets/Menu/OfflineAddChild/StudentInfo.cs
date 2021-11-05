using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OfflineStudentInfo")]
public class StudentInfo : ScriptableObject {

    [SerializeField] private List<StudentData> studentDataList = new List<StudentData>();

    [SerializeField] private StudentData selectedStudent;

    public IList<StudentData> AllStudents => studentDataList.AsReadOnly();

    public StudentData SelectedStudent {
        get => selectedStudent;
        set => selectedStudent = value;
    }

    private void AddStudent(StudentData data) => studentDataList.Add(data);

    public void CreateAndSelectNewChild() {
        var newStudentData = new StudentData();
        AddStudent(newStudentData);
        SelectedStudent = newStudentData;
    }

}
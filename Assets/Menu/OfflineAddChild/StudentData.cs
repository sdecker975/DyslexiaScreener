using System;
using UnityEngine;

[Serializable]
public class StudentData {

    [SerializeField] private string studentId;
    [SerializeField] private Grade grade = Grade.Grade0;
    [SerializeField] private string dateOfBirth;
    [SerializeField] private string firstName;
    [SerializeField] private string lastName;
    [SerializeField] private Sex sex = Sex.Female;
    [SerializeField] private Race race = Race.AfricanAmericanOrBlack;
    [SerializeField] private SpecialEducation specialEducation;
    [SerializeField] private DateTime dob;
    [SerializeField] private string adminEmail;

    public string StudentId {
        get => studentId;
        set => studentId = value;
    }

    public Grade Grade {
        get => grade;
        set => grade = value;
    }

    public string DateOfBirth {
        get => dateOfBirth;
        set => dateOfBirth = value;
    }

    public string FirstName {
        get => firstName;
        set => firstName = value;
    }

    public string LastName {
        get => lastName;
        set => lastName = value;
    }

    public Sex Sex {
        get => sex;
        set => sex = value;
    }

    public Race Race {
        get => race;
        set => race = value;
    }

    public SpecialEducation SpecialEducation {
        get => specialEducation;
        set => specialEducation = value;
    }

    public DateTime Dob {
        get => dob;
        set => dob = value;
    }

    public string AdminEmail {
        get => adminEmail;
        set => adminEmail = value;
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{

    public string id;
    public Gender gender;
    public int age;

    public List<SubjectiveEvaluationData> subjectiveEvaluationResults;
    public List<DirectionGuessingData> directionGuessingResults;

    public SessionData(string id)
    {
        this.id = id;
        subjectiveEvaluationResults = new List<SubjectiveEvaluationData>();
        directionGuessingResults = new List<DirectionGuessingData>();
    }

}

[System.Serializable]
public enum Gender
{
    Male,
    Female,
    Diverse
}

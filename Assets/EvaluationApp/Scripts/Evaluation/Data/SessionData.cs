using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionData
{

    public string id;
    public Gender sex;
    public int age;
    public float volume;

    public List<ConcreteSubjectiveEvaluation> subjectiveEvaluationResults;
    public List<DirectionGuessingData> directionGuessingResults;

    public SessionData(string id)
    {
        this.id = id;
        subjectiveEvaluationResults = new List<ConcreteSubjectiveEvaluation>();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubjectiveEvalData
{
    public int evaluationID;
    public int spatializerID;
    public int evaluationAspectID;
    public string description;
    public string minValue;
    public string maxValue;

    public SubjectiveEvalData(int id, int spatID, int aspID, string desc, string min, string max) 
    {
        evaluationID = id;
        spatializerID = spatID;
        evaluationAspectID = aspID;
        description = desc;
        minValue = min; 
        maxValue = max;

    }
}

public class SpatializerInfo1
{
    public int spatializerID;
    public string spatializerName;
}


[System.Serializable]
public class Data
{

}
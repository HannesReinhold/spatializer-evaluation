using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SubjectiveEvaluationData1
{
    public int evaluationID;
    public string spatializerName;
    public string evaluationAspect;
    public float evaluationValue;

    public SubjectiveEvaluationData1(int id, string name, string a, float v)
    {
        evaluationID = id;
        spatializerName = name;
        evaluationAspect = a;
        evaluationValue = v;
    }
}

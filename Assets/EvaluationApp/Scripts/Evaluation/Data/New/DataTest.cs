using SteamAudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTest : MonoBehaviour
{

    public List<SpatializerInfo> spatializerInfo = new List<SpatializerInfo>();
    public List<SpeakerInfo> speakerInfo = new List<SpeakerInfo>();

    public List<SubjectiveEvaluationPartData> subjectiveEvaluationPartData = new List<SubjectiveEvaluationPartData>();


    private void Awake()
    {
    }

    void FillData()
    {
        // spatializer Info
        spatializerInfo.Add(new SpatializerInfo(0,"Oculus Spatializer", "Oculus", "Meta"));
        spatializerInfo.Add(new SpatializerInfo(1, "Resonance Spatializer", "Resonance", "Google"));
        spatializerInfo.Add(new SpatializerInfo(2, "Steam Spatializer", "Steam", "Valve"));
        spatializerInfo.Add(new SpatializerInfo(3, "My Own Spatializer", "Own", "Hannes Reinhold"));

        // speaker Info
        speakerInfo.Add(new SpeakerInfo(0, new UnityEngine.Vector3(), new Quaternion()));
        speakerInfo.Add(new SpeakerInfo(1, new UnityEngine.Vector3(), new Quaternion()));
        speakerInfo.Add(new SpeakerInfo(2, new UnityEngine.Vector3(), new Quaternion()));
        speakerInfo.Add(new SpeakerInfo(3, new UnityEngine.Vector3(), new Quaternion()));
        speakerInfo.Add(new SpeakerInfo(4, new UnityEngine.Vector3(), new Quaternion()));
        speakerInfo.Add(new SpeakerInfo(5, new UnityEngine.Vector3(), new Quaternion()));

        // subjective eval data
        subjectiveEvaluationPartData.Add(new SubjectiveEvaluationPartData(0,0, "Directivity", "", "How close in terms of directivity does the spatializer compare to the real world?", "Directivity", "Very Different", "Very Similar", 0, 100));
        subjectiveEvaluationPartData.Add(new SubjectiveEvaluationPartData(1,1, "Directivity", "", "How close in terms of directivity does the spatializer compare to the real world?", "Directivity", "Very Different", "Very Similar", 0, 100));
        subjectiveEvaluationPartData.Add(new SubjectiveEvaluationPartData(2,2, "Directivity", "", "How close in terms of directivity does the spatializer compare to the real world?", "Directivity", "Very Different", "Very Similar", 0, 100));
        subjectiveEvaluationPartData.Add(new SubjectiveEvaluationPartData(3,3, "Directivity", "", "How close in terms of directivity does the spatializer compare to the real world?", "Directivity", "Very Different", "Very Similar", 0, 100));
        subjectiveEvaluationPartData.Add(new SubjectiveEvaluationPartData(4,4, "Directivity", "", "How close in terms of directivity does the spatializer compare to the real world?", "Directivity", "Very Different", "Very Similar", 0, 100));


    }
}

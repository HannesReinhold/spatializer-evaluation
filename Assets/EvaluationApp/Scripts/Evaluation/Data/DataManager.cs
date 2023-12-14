using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class DataManager
{
    // Path to save the data
    private string persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar;

    public SessionData currentSessionData;

    public DataManager()
    {
        BuildDirectories();
    }

    public void InitializeSession()
    {
        Guid id = new Guid();
        currentSessionData = new SessionData(id.ToString());
        currentSessionData.gender = Gender.Female;
        currentSessionData.age = 18;
    }

    public void SaveSession()
    {
        SaveData(currentSessionData, persistentPath+"Sessions/session_"+currentSessionData.id);
    }

    public void BuildDirectories()
    {
        Directory.CreateDirectory(persistentPath + "Sessions/");

        ExtractData();
    }

    public void SaveData<T>(T data, string path)
    {
        string savePath = path + ".json";

        // Create directory with given path
        Directory.CreateDirectory(Path.GetDirectoryName(savePath));

        // Convert level data into json format
        string jsonFile = JsonUtility.ToJson(data);

        // Write data into file
        using (FileStream stream = new FileStream(savePath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(jsonFile);
            }
        }
    }

    private T LoadData<T>(string path)
    {
        string filePath = path + ".json";
        string jsonData = "";

        if (File.Exists(filePath))
        {
            using StreamReader reader = new StreamReader(filePath);
            jsonData = reader.ReadToEnd();
        }

        // Convert json to object
        return JsonUtility.FromJson<T>(jsonData);
    }

    private void ExtractData()
    {
        data = JsonUtility.FromJson<SpatializerEvaluationData>(json);
    }


    private SpatializerEvaluationData data = new SpatializerEvaluationData();
    private string json = "{\"spatializerInfo\":[{\"spatializerID\":0,\"name\":\"Real World Audio\",\"shortaName\":\"Real Audio\",\"creator\":\"none\"},{\"spatializerID\":1,\"name\":\"Unity Default Spatializer\",\"shortaName\":\"Unity Default\",\"creator\":\"Unity\"},{\"spatializerID\":2,\"name\":\"Oculus Spatializer\",\"shortaName\":\"Oculus\",\"creator\":\"Meta\"},{\"spatializerID\":3,\"name\":\"Resonance Spatializer\",\"shortaName\":\"Resonance\",\"creator\":\"Google\"},{\"spatializerID\":4,\"name\":\"Steam Audio Spatializer\",\"shortaName\":\"Steam Audio\",\"creator\":\"Valve\"},{\"spatializerID\":5,\"name\":\"Hannes Audio Spatializer\",\"shortaName\":\"Own Spatializer\",\"creator\":\"Hannes Reinhold\"}],\"speakerData\":[{\"speakerID\":0,\"position\":{\"x\":0,\"y\":0,\"z\":0},\"rotation\":{\"x\":0,\"y\":0,\"z\":0,\"w\":0}}],\"subjectiveEvaluationData\":{\"evaluationParts\":[{\"partID\":0,\"name\":\"Direction\",\"desription\":\"This evaluation is conducted in an mostly empty room. It tests directionality rather than phenomena such as occlusion or reverberation.\",\"question\":\"How well can you determine the direction of the spatialized noise compared to the real noise?\",\"comparisonAspect\":\"direction\",\"minValue\":\"Very Different\",\"maxValue\":\"Very Identical\",\"minNumber\":0,\"maxNumber\":100,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":0,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":4,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":6,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":2,\"value\":50},{\"evaluationID\":4,\"baseSpatializerID\":0,\"comparisonSpatializerID\":5,\"speakerID\":1,\"value\":50}]},{\"partID\":1,\"name\":\"Attenuation\",\"desription\":\"This evaluation is conducted in an mostly empty room. It tests the audio phenomena 'distance attenuation', which means that the sound gets quieter with increasing distance.\",\"question\":\"How well can you determine the distance of the spatialized noise compared to the real noise?\",\"comparisonAspect\":\"attenuation\",\"minValue\":\"Very Different\",\"maxValue\":\"Very Identical\",\"minNumber\":0,\"maxNumber\":100,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":5,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":2,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":1,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":3,\"value\":50},{\"evaluationID\":4,\"baseSpatializerID\":0,\"comparisonSpatializerID\":5,\"speakerID\":4,\"value\":50}]},{\"partID\":3,\"name\":\"Occlusion\",\"desription\":\"This time the room contains some obstacles which should obstruct the sound. This means if an obstacle is between you and the audio source, the sound gets quieter and more muffled.\",\"question\":\"How well does the spatialized occlusion effect match the real world?\",\"comparisonAspect\":\"occlusion\",\"minValue\":\"Very Different\",\"maxValue\":\"Very Identical\",\"minNumber\":0,\"maxNumber\":100,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":3,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":6,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":4,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":1,\"value\":50},{\"evaluationID\":4,\"baseSpatializerID\":0,\"comparisonSpatializerID\":5,\"speakerID\":2,\"value\":50}]},{\"partID\":3,\"name\":\"Reverb\",\"desription\":\"This time we will evaluate how good these spatializers can simulate audio reflections and reverb.\",\"question\":\"How realistic does the virtual reverb sound like in comparison to the real one?\",\"comparisonAspect\":\"reverb\",\"minValue\":\"Very Unrealistic\",\"maxValue\":\"Very Realistic\",\"minNumber\":0,\"maxNumber\":100,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":1,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":4,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":2,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":3,\"value\":50},{\"evaluationID\":4,\"baseSpatializerID\":0,\"comparisonSpatializerID\":5,\"speakerID\":5,\"value\":50}]}]}}";
}


[System.Serializable]
public struct SpatializerEvaluationData
{
    public SpatializerInfo[] spatializerInfo;
    public SpeakerInfo[] speakerInfo;
    SubjectiveEvaluationData data;
}

[System.Serializable]
public struct SpatializerInfo
{
    public int spatializerID;
    public string name;
    public string shortName;
    public string creator;
}

[System.Serializable]
public struct SpeakerInfo
{
    public int speakerID;
    public Vector3 position;
    public Quaternion rotation;
}

[System.Serializable]
public struct SubjectiveEvaluationData
{
    public SubjectiveEvaluationPartData[] evaluationParts;
}

[System.Serializable]
public struct SubjectiveEvaluationPartData
{
    public int partID;
    public string name;
    public string description;
    public string question;
    public string comparisonAspect;
    public string nimValue;
    public string maxValue;
    public float minValueNumer;
    public float maxValueNumer;
    public ConcreteSubjectiveEvaluation[] singleEvaluations;
}

[System.Serializable]
public struct ConcreteSubjectiveEvaluation
{
    public int evaluationID;
    public int baseSpatializerID;
    public int comparisonSpatializerID;
    public int speakerID;
    public float value;
}
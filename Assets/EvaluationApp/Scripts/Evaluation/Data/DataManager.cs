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
        
        currentSessionData = new SessionData(System.Guid.NewGuid().ToString());
        currentSessionData.sex = Gender.Male;
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
        Debug.Log(persistentPath);
        spatializerData = JsonUtility.FromJson<SpatializerEvaluationData>(json);
        directionGameData = spatializerData.directionGameData;
    }

    public DirectionGameData directionGameData = new DirectionGameData();
    public SpatializerEvaluationData spatializerData = new SpatializerEvaluationData();
    //private string json = "{\"spatializerInfo\":[{\"spatializerID\":0,\"name\":\"Real World Audio\",\"shortName\":\"Real Audio\",\"creator\":\"none\"},{\"spatializerID\":1,\"name\":\"Oculus Spatializer\",\"shortName\":\"Oculus\",\"creator\":\"Meta\"},{\"spatializerID\":2,\"name\":\"Resonance Spatializer\",\"shortName\":\"Resonance\",\"creator\":\"Google\"},{\"spatializerID\":3,\"name\":\"Steam Audio Spatializer\",\"shortName\":\"Steam Audio\",\"creator\":\"Valve\"},{\"spatializerID\":4,\"name\":\"Hannes Audio Spatializer\",\"shortName\":\"Own Spatializer\",\"creator\":\"Hannes Reinhold\"}],\"speakerData\":[{\"speakerID\":0,\"position\":{\"x\":0,\"y\":0,\"z\":0},\"rotation\":{\"x\":0,\"y\":0,\"z\":0,\"w\":0}},{\"speakerID\":1,\"position\":{\"x\":0,\"y\":0,\"z\":0},\"rotation\":{\"x\":0,\"y\":0,\"z\":0,\"w\":0}},{\"speakerID\":2,\"position\":{\"x\":0,\"y\":0,\"z\":0},\"rotation\":{\"x\":0,\"y\":0,\"z\":0,\"w\":0}},{\"speakerID\":3,\"position\":{\"x\":0,\"y\":0,\"z\":0},\"rotation\":{\"x\":0,\"y\":0,\"z\":0,\"w\":0}},{\"speakerID\":4,\"position\":{\"x\":0,\"y\":0,\"z\":0},\"rotation\":{\"x\":0,\"y\":0,\"z\":0,\"w\":0}},{\"speakerID\":5,\"position\":{\"x\":0,\"y\":0,\"z\":0},\"rotation\":{\"x\":0,\"y\":0,\"z\":0,\"w\":0}}],\"subjectiveEvaluationData\":{\"evaluationParts\":[{\"partID\":0,\"name\":\"Direction\",\"description\":\"This evaluation is conducted in an mostly empty room. It tests directionality rather than phenomena such as occlusion or reverberation.\",\"question\":\"How well can you determine the direction of the spatialized noise compared to the real noise?\",\"comparisonAspect\":\"direction\",\"minValue\":\"Very Different\",\"maxValue\":\"Very Similar\",\"minNumber\":0,\"maxNumber\":100,\"fileID\":0,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":0,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":0,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":1,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":1,\"value\":50}]},{\"partID\":1,\"name\":\"Attenuation\",\"description\":\"This evaluation is conducted in an mostly empty room. It tests the audio phenomena 'distance attenuation', which means that the sound gets quieter with increasing distance.\",\"question\":\"How well can you determine the distance of the spatialized noise compared to the real noise?\",\"comparisonAspect\":\"attenuation\",\"minValue\":\"Very Different\",\"maxValue\":\"Very Similar\",\"minNumber\":0,\"maxNumber\":100,\"fileID\":1,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":1,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":1,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":0,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":0,\"value\":50}]},{\"partID\":3,\"name\":\"Occlusion\",\"description\":\"This time the room contains some obstacles which should obstruct the sound. This means if an obstacle is between you and the audio source, the sound gets quieter and more muffled.\",\"question\":\"How well does the spatialized occlusion effect match the real world?\",\"comparisonAspect\":\"occlusion\",\"minValue\":\"Very Different\",\"maxValue\":\"Very Similar\",\"minNumber\":0,\"maxNumber\":100,\"fileID\":3,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":1,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":0,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":1,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":1,\"value\":50}]},{\"partID\":3,\"name\":\"Reverb\",\"description\":\"This time we will evaluate how good these spatializers can simulate audio reflections and reverb.\",\"question\":\"How realistic does the virtual reverb sound like in comparison to the real one?\",\"comparisonAspect\":\"reverb\",\"minValue\":\"Very Unrealistic\",\"maxValue\":\"Very Realistic\",\"minNumber\":0,\"maxNumber\":100,\"fileID\":4,\"evaluations\":[{\"evaluationID\":0,\"baseSpatializerID\":0,\"comparisonSpatializerID\":1,\"speakerID\":1,\"value\":50},{\"evaluationID\":1,\"baseSpatializerID\":0,\"comparisonSpatializerID\":2,\"speakerID\":0,\"value\":50},{\"evaluationID\":2,\"baseSpatializerID\":0,\"comparisonSpatializerID\":3,\"speakerID\":1,\"value\":50},{\"evaluationID\":3,\"baseSpatializerID\":0,\"comparisonSpatializerID\":4,\"speakerID\":1,\"value\":50}]}]},\"directionGameData\":{\"rounds\":[{\"roundID\":0,\"positionID\":0,\"spatializerID\":0},{\"roundID\":1,\"positionID\":1,\"spatializerID\":1},{\"roundID\":2,\"positionID\":2,\"spatializerID\":0},{\"roundID\":3,\"positionID\":3,\"spatializerID\":2},{\"roundID\":4,\"positionID\":4,\"spatializerID\":1}],\"positions\":[{\"x\":0,\"y\":1,\"z\":2},{\"x\":1,\"y\":1,\"z\":2},{\"x\":0,\"y\":1,\"z\":2},{\"x\":0,\"y\":0,\"z\":2},{\"x\":0,\"y\":1,\"z\":2},{\"x\":0,\"y\":1,\"z\":2},{\"x\":2,\"y\":1,\"z\":2},{\"x\":0,\"y\":1,\"z\":2}]}}";
    private string json = Resources.Load<TextAsset>("EvaluationData/EvaluationData").text;
}


[System.Serializable]
public struct SpatializerEvaluationData
{
    public SpatializerInfo[] spatializerInfo;
    public SpeakerInfo[] speakerInfo;
    public SubjectiveEvaluationData subjectiveEvaluationData;
    public DirectionGameData directionGameData;
}

[System.Serializable]
public struct SpatializerInfo
{
    public int spatializerID;
    public string name;
    public string shortName;
    public string creator;

    public SpatializerInfo(int id, string name, string sName, string creat)
    {
        spatializerID = id;
        this.name = name;
        this.shortName = sName;
        this.creator = creat;
    }
}

[System.Serializable]
public struct SpeakerInfo
{
    public int speakerID;
    public Vector3 position;
    public Quaternion rotation;

    public SpeakerInfo(int id, Vector3 pos, Quaternion rot)
    {
        speakerID = id;
        position = pos; 
        rotation = rot;
    }
}

[System.Serializable]
public class SubjectiveEvaluationData
{
    public List<SubjectiveEvaluationPartData> evaluationParts = new List<SubjectiveEvaluationPartData>();
}

[System.Serializable]
public struct SubjectiveEvaluationPartData
{
    public int partID;
    public int fileID;
    public string name;
    public string description;
    public string question;
    public string comparisonAspect;
    public string minValue;
    public string maxValue;
    public float minValueNumber;
    public float maxValueNumber;
    public ConcreteSubjectiveEvaluation[] evaluations;

    public SubjectiveEvaluationPartData(int id, int fileID, string name, string desc, string que, string asp, string min, string max, float minNum, float maxNum)
    {
        this.partID = id;
        this.fileID = fileID;
        this.name = name;
        this.description = desc;
        this.question = que;
        this.comparisonAspect = asp;
        this.minValue = min;
        this.maxValue = max;
        this.minValueNumber = minNum;
        this.maxValueNumber = maxNum;
        //singleEvaluations = new List<ConcreteSubjectiveEvaluation>();
        evaluations = new ConcreteSubjectiveEvaluation[10];
    }
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
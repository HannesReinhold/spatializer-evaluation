using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectiveAudioSwitch : MonoBehaviour
{
    public AudioTriggerEvent triggerEvent;
    public AudioSettingsEvent settingsEvent;

    private bool realSpeaker = false;
    private int speakerID = 0;
    private int spatializerID = 0;

    public void Play(int fileID)
    {
        triggerEvent.Emit(1,fileID);

        Debug.Log("Playing: " + fileID +" at: "+realSpeaker+","+speakerID+","+spatializerID);
    }

    public void Stop()
    {
        triggerEvent.Emit(0,0);
    }

    public void SetAll(bool real, int speakerID, int spatializerID)
    {
        realSpeaker = real;
        this.speakerID = speakerID;
        this.spatializerID = spatializerID;

        settingsEvent.SetSettings(speakerID, real ? 0:1, spatializerID);
    }

    public void SetSpeaker(int id)
    {
        speakerID = id;
        SetAll(realSpeaker, speakerID, spatializerID);
    }

    public void SetReal(int real)
    {
        this.realSpeaker = real==0;
        SetAll(realSpeaker, speakerID, spatializerID);
    }

    public void SetSpatializer(int id)
    {
        spatializerID = id;
        SetAll(realSpeaker, speakerID, spatializerID);
    }
}

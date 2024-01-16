using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualSpeakerSwitcher : MonoBehaviour
{
    public List<Transform> speakerPositionList;


    [SerializeField] public AudioEvent[] events;

    public FMODUnity.StudioEventEmitter emitterRef;

    FMOD.Studio.Bus bus;

    private int audioID = 0;
    private int spatializerID = 0;

    private void Awake()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
        //bus.getVolume(out maxVolume);

        SetSpeaker(0,0,0);
    }


    public void SetSpatializerID(int id)
    {
        spatializerID = id;
        emitterRef.EventReference = events[audioID].spatializedEvents[spatializerID];
    }

    public void SetAudioID(int id)
    {
        audioID = id;
        emitterRef.EventReference = events[audioID].spatializedEvents[spatializerID];
    }

    public void SetSpeaker(int speakerID, int spatializerID, int audioID)
    {
        this.audioID = audioID;
        this.spatializerID = spatializerID;
        emitterRef.transform.position = speakerPositionList[speakerID].position;
        emitterRef.transform.rotation = speakerPositionList[speakerID].rotation;
        emitterRef.EventReference = events[audioID].spatializedEvents[spatializerID];
    }

    public void SetSpeaker(int speakerID, int spatializerID)
    {
        this.spatializerID = spatializerID;
        emitterRef.transform.position = speakerPositionList[speakerID].position;
        emitterRef.transform.rotation = speakerPositionList[speakerID].rotation;
        emitterRef.EventReference = events[audioID].spatializedEvents[spatializerID];
    }

    private float maxVolume = 1f;

    public void Mute()
    {
        bus.setVolume(0);
    }

    public void UnMute()
    {
        bus.setVolume(maxVolume);
    }

    public void SetMute(bool mute)
    {
        if (mute) Mute();
        else UnMute();
    }

    public void Play()
    {
        emitterRef.Stop();
        emitterRef.Play();
    }

    public void Stop()
    {
        emitterRef.Stop();
    }
}

[System.Serializable]
public struct AudioEvent
{
    [SerializeField] public EventReference[] spatializedEvents;
}

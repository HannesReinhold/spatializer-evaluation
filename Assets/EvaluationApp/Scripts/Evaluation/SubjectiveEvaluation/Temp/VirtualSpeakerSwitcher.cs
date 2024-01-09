using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class VirtualSpeakerSwitcher : MonoBehaviour
{
    public List<Transform> speakerPositionList;


    [SerializeField] public AudioEvent[] events;

    public FMODUnity.StudioEventEmitter emitterRef;

    FMOD.Studio.Bus bus;

    private void Awake()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
    }


    public void SetSpeaker(int speakerID, int spatializerID, int audioID)
    {
        emitterRef.transform.position = speakerPositionList[speakerID].position;
        emitterRef.transform.rotation = speakerPositionList[speakerID].rotation;
        emitterRef.EventReference = events[audioID].spatializedEvents[spatializerID];
    }

    private float maxVolume = 0.5f;

    public void Mute()
    {
        bus.getVolume(out maxVolume);
        bus.setVolume(0);
    }

    public void UnMute()
    {
        bus.setVolume(maxVolume);
    }

    public void Play()
    {
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

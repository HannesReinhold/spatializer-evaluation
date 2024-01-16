using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AudioSync : MonoBehaviour
{
    public AudioSettingsEvent settings;
    public AudioTriggerEvent trigger;

    private int lastSpeakerID;
    private bool lastReal = true;


    public List<VirtualAudioSwitcher> virtualSpeaker;


    private void Start()
    {

    }




    public void PlayVirtualSpeaker(int speakerID, int spatializerID, int audioID)
    {
        virtualSpeaker[speakerID].Play(spatializerID, audioID);
    }



    public void SetAudioOutput(bool real, int speakerID, int spatializerID)
    {
        if(real) 

        virtualSpeaker[speakerID].SetSpatializer(spatializerID);
    }



    public void PlaySound()
    {
        //trigger.Emit();

    }

    public void PlaySpatializer()
    {

    }

    public void StopSpatializer()
    {
        for(int i=0; i<virtualSpeaker.Count; i++)
        {
            virtualSpeaker[i].Stop();
        }
    }

    public void SetSpatializer(int id)
    {
       // switcher.SetSpatializer(id);
    }

    public void SetSpeaker(int speakerID)
    {
        //settings.SetSettings(speakerID, lastReal ? 1 : 0);
        Debug.Log(settings);
        lastSpeakerID = speakerID;

        SetVirtualSpeaker(speakerID);
    }

    public void SetVirtualSpeaker(int speakerID)
    {
        //if (virtualSpeaker != null) virtualSpeaker.SetSpatializer(speakerID);
    }

    public void SetRealSound(bool real)
    {
        //settings.SetSettings(lastSpeakerID, real ? 1 : 0);
        lastReal = real;
    }
}

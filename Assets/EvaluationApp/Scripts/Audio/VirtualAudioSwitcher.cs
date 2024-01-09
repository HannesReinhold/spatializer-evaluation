using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAudioSwitcher : MonoBehaviour
{
    public List<FMODUnity.StudioEventEmitter> virtualAudioEmitter = new List<FMODUnity.StudioEventEmitter>();

    [Header("Events")]
    public List<EventReference> refOculus;
    public List<EventReference> refResonance;
    public List<EventReference> refSteam;



    int lastIndex = 0;
    double time = 0;

    public void SetSpatializer(int id)
    {
        double dif = Time.time*1000 - time;
        int d = (int) dif;
        
        

        virtualAudioEmitter[lastIndex].Stop();
        
        virtualAudioEmitter[id].Play();
        FMOD.RESULT res2 = virtualAudioEmitter[id].EventInstance.setTimelinePosition(d);

        lastIndex = id;
    }

    public void Play()
    {
        time = Time.time * 1000;
    }

    public void Stop()
    {
        virtualAudioEmitter[lastIndex].Stop();
    }

    public void Play(int spatializerID, int audioID)
    {
        //virtualAudioEmitter[spatializerID].EventReference = "";
        virtualAudioEmitter[spatializerID].Play();
    }
}

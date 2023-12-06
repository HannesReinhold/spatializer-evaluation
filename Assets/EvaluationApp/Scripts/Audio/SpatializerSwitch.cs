using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpatializerSwitch : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter realAudioEmitter;
    public FMODUnity.StudioEventEmitter virtualAudioEmitter;
    public AudioOutputChannelRouter outputChannelRouter;

    public void PrepareAudio(int fileID, int spatializerID)
    {
        //realAudioEmitter
    }

    public void SetAudioOutput(bool realAudio, int speakerID)
    {
        if(realAudio)
        {
            realAudioEmitter.SetParameter("Volume", 1);
            virtualAudioEmitter.SetParameter("Volume", 0);
            outputChannelRouter.SetOutputChannel(speakerID);
        }
        else
        {
            realAudioEmitter.SetParameter("Volume", 0);
            virtualAudioEmitter.SetParameter("Volume", 1);
        }

    }
}

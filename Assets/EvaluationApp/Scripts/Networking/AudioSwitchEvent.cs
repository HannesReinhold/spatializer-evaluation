using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class AudioSwitchEvent : RealtimeComponent<AudioSwitchEventModel>
{

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioOutputChannelRouter outputChannelRouter;

    private bool isVR;

    void Start()
    {
        audioSource = FindAnyObjectByType<AudioSource>();
        outputChannelRouter = audioSource.GetComponent<AudioOutputChannelRouter>();
        //isVR = FindAnyObjectByType<GameManager>().IsVR;
    }

    // When we connect to a room server, we'll be given an instance of our model to work with.
    protected override void OnRealtimeModelReplaced(AudioSwitchEventModel previousModel, AudioSwitchEventModel currentModel)
    {
        if (previousModel != null)
        {
            // Unsubscribe from events on the old model.
            previousModel.eventDidFire -= SettingsSet;
        }
        if (currentModel != null)
        {
            // Subscribe to events on the new model
            currentModel.eventDidFire += SettingsSet;
        }
    }

    // A public method we can use to fire the event
    public void SetSettings(int speakerID, int useRealSpeaker)
    {
        model.FireEvent(speakerID, useRealSpeaker);
    }

    int id = 0;

    // Called whenever our event fires
    private void SettingsSet(int speakerID, int realAudio)
    {
        id++;

        if (outputChannelRouter != null) outputChannelRouter.SetOutputChannel(speakerID);

        if (isVR)
            audioSource.volume = realAudio != 0 ? 0 : 1;
        else
            audioSource.volume = realAudio != 0 ? 1 : 0;

        if (realAudio == 0)
            Debug.Log("Playing virtual audio");
        else
            Debug.Log("Playing real speaker at index " + speakerID);
    }

}
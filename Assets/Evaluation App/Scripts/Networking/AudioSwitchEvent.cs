using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class AudioSwitchEvent : RealtimeComponent<AudioSwitchEventModel>
{

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    // TODO add channel switcher

    private bool isVR;

    void Start()
    {
        audioSource = FindAnyObjectByType<AudioSource>();
        //channelSplitter = audioSource.GetComponent<ChannelSplitter>();
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

    // Called whenever our event fires
    private void SettingsSet(int speakerID, int realAudio)
    {
        //if (channelSplitter != null) channelSplitter.selectedChannel = speakerID;

        if (isVR)
            audioSource.volume = realAudio != 0 ? 0 : 1;
        else
            audioSource.volume = realAudio != 0 ? 1 : 0;

        if (realAudio == 0)
            Debug.Log("Play virtual audio ");
        else
            Debug.Log("Play real speaker at index " + speakerID);
    }

}
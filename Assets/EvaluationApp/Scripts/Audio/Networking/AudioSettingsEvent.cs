using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class AudioSettingsEvent : RealtimeComponent<AudioSettingsEventModel>
{

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioOutputChannelRouter channelSplitter;

    private bool isVR;

    void Start()
    {
        audioSource = FindAnyObjectByType<AudioSource>();
        channelSplitter = audioSource.GetComponent<AudioOutputChannelRouter>();
        isVR = FindAnyObjectByType<GameManager>().IsVR;
    }

    // When we connect to a room server, we'll be given an instance of our model to work with.
    protected override void OnRealtimeModelReplaced(AudioSettingsEventModel previousModel, AudioSettingsEventModel currentModel) {
        if (previousModel != null) {
            // Unsubscribe from events on the old model.
            previousModel.eventDidFire -= SettingsSet;
        }
        if (currentModel != null) {
            // Subscribe to events on the new model
            currentModel.eventDidFire += SettingsSet;
        }
    }

    // A public method we can use to fire the event
    public void SetSettings(int speakerID, int useRealSpeaker) {
        model.FireEvent(speakerID, useRealSpeaker);
    }

    // Called whenever our event fires
    private void SettingsSet(int speakerID, int realAudio) {
        // Tell the particle system to trigger an explosion in response to the event
        if(channelSplitter!=null) channelSplitter.SetOutputChannel(speakerID);
        
        if(isVR)
            audioSource.volume = realAudio!=0 ? 0 : 1;
        else
            audioSource.volume = realAudio!=0 ? 1 : 0;

        if(realAudio==0)
            Debug.Log("Play virtual audio ");
        else
            Debug.Log("Play real speaker at index "+speakerID);
    }
    
}

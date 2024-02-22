using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class AudioSettingsEvent : RealtimeComponent<AudioSettingsEventModel>
{

    [SerializeField]
    private AudioSource realAudioSource;

    [SerializeField]
    private AudioOutputChannelRouter realChannelSplitter;

    [SerializeField]
    private VirtualSpeakerSwitcher spatializerSwitch;

    private bool isVR;

    void Start()
    {
        realChannelSplitter = realAudioSource.GetComponent<AudioOutputChannelRouter>();
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
    public void SetSettings(int speakerID, int useRealSpeaker, int spatializerID) {
        model.FireEvent(speakerID, useRealSpeaker, spatializerID);
    }

    // Called whenever our event fires
    private void SettingsSet(int speakerID, int realAudio, int spatializerID) {
        // Tell the particle system to trigger an explosion in response to the event
        if(realChannelSplitter!=null) realChannelSplitter.SetOutputChannel(speakerID);
        if (spatializerSwitch != null) spatializerSwitch.SetSpeaker(speakerID, spatializerID,0);

        isVR = GameManager.Instance.IsVR;

        if (isVR)
        {
            if(realAudioSource != null) realAudioSource.volume = 0;
            if (realAudioSource != null) realChannelSplitter.SetOutputChannel(speakerID);
            if (spatializerSwitch != null) spatializerSwitch.SetMute(realAudio != 0);
            if (spatializerSwitch != null) spatializerSwitch.SetSpeaker(speakerID, spatializerID);
            
        }
        else
        {
            if (realAudioSource != null) realAudioSource.volume = realAudio != 0 ? 1 : 0;
            if (realAudioSource != null) realChannelSplitter.SetOutputChannel(speakerID);
            if (spatializerSwitch != null) spatializerSwitch.SetMute(true);
        }

        if(realAudio==0)
            Debug.Log("Play virtual audio ");
        else
            Debug.Log("Play real speaker at index "+speakerID);
    }
    
}

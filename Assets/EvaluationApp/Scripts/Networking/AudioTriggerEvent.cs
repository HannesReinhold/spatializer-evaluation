using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using static UnityEngine.GraphicsBuffer;

public class AudioTriggerEvent : RealtimeComponent<AudioTriggerEventModel>
{

    [SerializeField]
    private AudioSource audioSource;
    public VirtualSpeakerSwitcher virtualSwitcher;

    public List<AudioClip> clipList;
    bool on = false;

    void Start()
    {
    }

    // When we connect to a room server, we'll be given an instance of our model to work with.
    protected override void OnRealtimeModelReplaced(AudioTriggerEventModel previousModel, AudioTriggerEventModel currentModel)
    {
        if (previousModel != null)
        {
            // Unsubscribe from events on the old model.
            previousModel.eventDidFire1 -= EventDidFire;
        }
        if (currentModel != null)
        {
            // Subscribe to events on the new model
            currentModel.eventDidFire1 += EventDidFire;
        }
    }

    // A public method we can use to fire the event
    public void Emit(int start, int fileID)
    {
        model.FireEvent(0,start, fileID);
    }

    // Called whenever our event fires
    private void EventDidFire(int senderID, int start, int fileID)
    {
        on = !on;
        if (start!=0)
        {
            if(audioSource != null) audioSource.time = 0;
            if (audioSource != null) audioSource.clip = clipList[fileID];
            if (audioSource != null) audioSource.Play();
            
            if(virtualSwitcher != null) virtualSwitcher.SetAudioID(fileID);
            if (virtualSwitcher != null) virtualSwitcher.Play();
            Debug.Log(fileID);
        }
        else
        {
            if (audioSource != null) audioSource.Stop();
            if (virtualSwitcher != null) virtualSwitcher.Stop();
        }

    }

    
}

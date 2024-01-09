using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;
using static UnityEngine.GraphicsBuffer;

public class AudioTriggerEvent : RealtimeComponent<AudioTriggerEventModel>
{

    [SerializeField]
    private AudioSource audioSource;
    public AudioSync sync;
    bool on = false;

    void Start()
    {
        audioSource = FindAnyObjectByType<AudioSource>();
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
    public void Emit()
    {
        model.FireEvent(realtime.clientID);
    }

    // Called whenever our event fires
    private void EventDidFire(int senderID)
    {
        on = !on;
        if (on)
        {
            audioSource.time = 0;
            audioSource.Play();
            sync.StopSpatializer();
        }
        else
        {
            audioSource.Stop();
            sync.PlaySpatializer();
        }

    }
}

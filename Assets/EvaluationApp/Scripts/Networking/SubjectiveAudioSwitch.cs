using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectiveAudioSwitch : MonoBehaviour
{
    public VirtualSpeakerSwitcher virtualSwitch;
    public RealSpeakerSwitcher realSwitch;

    public AudioTriggerEvent triggerEvent;
    public AudioSettingsEvent settingsEvent;

    public void SetupSpeakers(int speakerIndex, int fileIndex, int spatIndex)
    {
        virtualSwitch.SetSpeaker(speakerIndex,spatIndex, fileIndex);
        realSwitch.SetSpeaker(speakerIndex, fileIndex);
    }

    public void Switch(bool real)
    {
        //triggerEvent
    }
}

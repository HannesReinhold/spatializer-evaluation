using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealSpeakerSwitcher : MonoBehaviour
{
    public List<AudioClip> audioClipList;

    private AudioOutputChannelRouter router;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        router = GetComponent<AudioOutputChannelRouter>();
    }

    public void SetSpeaker(int speakerID, int fileID)
    {
        source.clip = audioClipList[fileID];
        router.SetOutputChannel(speakerID);
    }


    public void Play()
    {
        source.Play();
    }

    public void Stop()
    {
        source.Stop();
    }

    public void Mute()
    {
        source.volume = 0;
    }

    public void Unmute()
    {
        source.volume = GameManager.Instance.dataManager.currentSessionData.volume;
    }
}

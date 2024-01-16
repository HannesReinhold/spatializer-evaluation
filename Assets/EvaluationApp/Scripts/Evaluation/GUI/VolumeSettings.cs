using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public Slider slider;
    FMOD.Studio.Bus bus;


    private void Awake()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
    }
    private void Update()
    {
        bus.setVolume(slider.value);
        GameManager.Instance.dataManager.currentSessionData.volume = slider.value;
    }
}

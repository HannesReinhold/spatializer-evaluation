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
        //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Volume", slider.value);
        bus.setVolume(slider.value);
    }
}

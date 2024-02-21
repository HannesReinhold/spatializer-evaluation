using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergingWall : MonoBehaviour
{
    public ParticleSystem particles;
    public FMODUnity.StudioEventEmitter emitter;
    public GameObject wallObject;

    public float appearTime = 3;
    public float disappearTime = 3;
    public float wallHeight = 2;

    public void Appear()
    {
        wallObject.SetActive(true);
        particles.Play();
        LeanTween.moveLocalY(wallObject, wallHeight,appearTime);
        emitter.Play();
        Invoke("StopAppearing", appearTime);
    }

    private void StopAppearing()
    {
        particles.Stop();
        emitter.Stop();
    }

    public void Disappear() 
    {
        particles.Play();
        LeanTween.moveLocalY(wallObject, -wallHeight, disappearTime).setOnComplete(Disable);
        emitter.Play();
        Invoke("Disable", disappearTime);
    }

    public void Disable()
    {
        emitter.Stop();
        wallObject.SetActive(false);
    }
}

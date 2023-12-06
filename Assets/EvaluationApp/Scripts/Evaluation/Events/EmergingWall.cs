using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergingWall : MonoBehaviour
{
    public ParticleSystem particles;
    public FMODUnity.StudioEventEmitter emitter;

    public float appearTime = 5;
    public float disappearTime = 5;
    public float wallHeight = 2;

    public void Appear()
    {
        particles.Play();
        LeanTween.moveLocalY(gameObject,wallHeight,appearTime);
        emitter.Play();
        Invoke("StopAppearing", appearTime);
    }

    private void StopAppearing()
    {
        particles.Stop();
        emitter.Stop();
        Invoke("Disappear",3);
    }

    public void Disappear() 
    {
        particles.Play();
        LeanTween.moveLocalY(gameObject, -wallHeight, disappearTime).setOnComplete(Disable);
        emitter.Play();
        Invoke("StopAppearing", disappearTime);
    }

    private void Disable()
    {
        emitter.Stop();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Appear();
    }

    private void OnDisable()
    {
    }
}

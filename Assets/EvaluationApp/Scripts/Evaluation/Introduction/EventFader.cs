using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFader : MonoBehaviour
{
    public FMODUnity.StudioEventEmitter emitter;

    public float fadeInTime = 1;
    public float fadeOutTime = 1;
    public bool IsOn;
    private bool isOn2;

    private void Update()
    {
        if (IsOn!=isOn2)
        {
            
            if (IsOn) FadeIn();
            else FadeOut();
        }
        isOn2 = IsOn;
    }

    private void Start()
    {
        LeanTween.alpha(gameObject, 0, 0);
    }

    private void OnEnable()
    {
        FadeIn();
    }

    private void OnDisable()
    {
        FadeOut();
    }

    public void FadeIn()
    {
        LeanTween.alpha(gameObject,1,fadeInTime);
        if(emitter!=null) emitter.Play();
    }

    public void FadeOut()
    {
        LeanTween.alpha(gameObject, 0, fadeOutTime).setOnComplete(Disable);
        if (emitter != null) emitter.Stop();
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}

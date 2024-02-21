using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public CanvasGroup canvas;
    public bool Open = false;
    public GameObject audioSource;

    public float scaling = 1000;



    private void Start()
    {
        if(Open)LeanTween.scale(gameObject, Vector3.one * 1, 0).setEaseOutCubic();
        else LeanTween.scale(gameObject, Vector3.one * 0, 0).setEaseOutCubic();
        if (audioSource != null) audioSource.SetActive(Open);
    }

    public void OpenHint()
    {
        LeanTween.scale(gameObject, Vector3.one * scaling, 0.5f).setEaseOutCubic();
        if(audioSource!=null) audioSource.GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    public void CloseHint()
    {
        LeanTween.scale(gameObject, Vector3.one * 0, 0.5f).setEaseOutCubic();
        if (audioSource != null) audioSource.GetComponent<FMODUnity.StudioEventEmitter>().Stop();
    }

    public void HideHint()
    {
        //LeanTween.scale(gameObject, Vector3.one * 750, 0.5f).setEaseOutCubic();
        LeanTween.alphaCanvas(canvas,0.5f,0.5f).setEaseOutCubic();
    }
}

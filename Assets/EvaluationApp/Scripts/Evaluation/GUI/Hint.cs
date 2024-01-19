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
        audioSource.SetActive(Open);
    }

    public void OpenHint()
    {
        LeanTween.scale(gameObject, Vector3.one * scaling, 0.5f).setEaseOutCubic();
        audioSource.SetActive(true);
    }

    public void CloseHint()
    {
        LeanTween.scale(gameObject, Vector3.one * 0, 0.5f).setEaseOutCubic();
        audioSource.SetActive(false);
    }

    public void HideHint()
    {
        //LeanTween.scale(gameObject, Vector3.one * 750, 0.5f).setEaseOutCubic();
        LeanTween.alphaCanvas(canvas,0.5f,0.5f).setEaseOutCubic();
    }
}

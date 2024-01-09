using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    public CanvasGroup canvas;
    public bool Open = false;



    private void Start()
    {
        if(Open)LeanTween.scale(gameObject, Vector3.one * 1, 0).setEaseOutCubic();
        else LeanTween.scale(gameObject, Vector3.one * 0, 0).setEaseOutCubic();
    }

    public void OpenHint()
    {
        LeanTween.scale(gameObject, Vector3.one * 1000, 0.5f).setEaseOutCubic();
    }

    public void CloseHint()
    {
        LeanTween.scale(gameObject, Vector3.one * 0, 0.5f).setEaseOutCubic();
    }

    public void HideHint()
    {
        //LeanTween.scale(gameObject, Vector3.one * 750, 0.5f).setEaseOutCubic();
        LeanTween.alphaCanvas(canvas,0.5f,0.5f).setEaseOutCubic();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupWindow : MonoBehaviour
{
    public bool moveLocal;
    public bool openOnEnable=true;
    public bool closeOnComplete=true;

    public float openDuration = 0.25f;
    public float closeDuration = 0.3f;

    public float openDelay = 0;
    public float closeDelay = 0;

    public float minScale = 0.9f;
    public float maxScale = 1f;

    public float minY = -0.1f;
    public float maxY = 0;

    public float minAlpha = 0f;
    public float maxAlpha = 0f;

    public float tiltDeg = 30;

    public CanvasGroup canvas;

    private void Awake()
    {
        if(canvas==null) canvas = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        LeanTween.moveLocalY(gameObject, minY, 0);
        LeanTween.scale(gameObject, Vector3.one * minScale * 0.001f, 0);
        LeanTween.alphaCanvas(canvas, minAlpha, 0);
    }

    private void OnEnable()
    {
        LeanTween.moveLocalY(gameObject, minY, 0);
        LeanTween.scale(gameObject, Vector3.one*minScale*0.001f, 0);
        LeanTween.alphaCanvas(canvas, minAlpha, 0);

        if (openOnEnable) Open();
    }


    private void Update()
    {

    }

    private void OpenWindow()
    {
        LeanTween.scale(gameObject, Vector3.one * maxScale * 0.001f, openDuration).setEaseOutCubic();
        LeanTween.moveLocalY(gameObject, maxY, openDuration).setEaseOutCubic();
        LeanTween.alphaCanvas(canvas, maxAlpha, openDuration).setEaseOutCubic();
        if (!moveLocal) LeanTween.rotateX(gameObject, tiltDeg, openDuration).setEaseOutCubic();

    }

    private void CloseWindow()
    {
        if(closeOnComplete)LeanTween.moveLocalY(gameObject, minY, closeDuration).setEaseOutCubic().setOnComplete(Disable);
        else LeanTween.moveLocalY(gameObject, minY, closeDuration).setEaseOutCubic();
        LeanTween.scale(gameObject, Vector3.one * minScale * 0.001f, closeDuration).setEaseOutCubic();
        LeanTween.alphaCanvas(canvas, minAlpha, closeDuration).setEaseOutCubic();
        if (!moveLocal) LeanTween.rotateX(gameObject, -tiltDeg, closeDuration).setEaseOutCubic();
    }

    public void Open()
    {
        Invoke("OpenWindow", openDelay);
    }

    public void Close()
    {
        Invoke("CloseWindow", closeDelay);
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }
}

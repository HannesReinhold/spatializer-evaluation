using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEvent : MonoBehaviour
{
    public GameObject roomModel;

    public float minFog = 0;
    public float maxFog = 0.5f;

    private bool enableFog = false;

    private float currentFogTarget;
    private float currentFogValue;



    private void OnEnable()
    {
        foreach(MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject,1,2);
        }

        EnableFog();
    }

    private void OnDisable()
    {
        foreach (MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject, 1, 2);
        }

        currentFogTarget = 0;
    }

    public void EnableFog()
    {
        currentFogTarget = maxFog;
    }

    public void DisableFog()
    {
        currentFogTarget = minFog;
    }



    private void Update()
    {
        currentFogValue = 0.999f * currentFogValue + 0.001f * currentFogTarget;
        RenderSettings.fogDensity = currentFogValue;
    }

}

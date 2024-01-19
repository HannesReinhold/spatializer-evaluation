using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorrorEvent : MonoBehaviour
{
    public GameObject roomModel;


    private void Start()
    {
        foreach(MeshRenderer r in roomModel.GetComponentsInChildren<Renderer>())
        {
            LeanTween.alpha(r.gameObject,0,0);
        }
    }

    private void OnEnable()
    {
        LeanTween.alpha(roomModel,1,5);
    }

    private void OnDisable()
    {
        LeanTween.alpha(roomModel,0,3);
    }
}

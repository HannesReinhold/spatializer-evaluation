using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public FMODUnity.StudioEventEmitter emitter;

    public bool playClickSound = false;
    public FMODUnity.EventReference clickSound;
    public float clickVibration = 0.1f;

    public bool playEnterSound = false;
    public FMODUnity.EventReference enterSound;
    public float enterVibration = 0.1f;

    public bool playExitSound = false;
    public FMODUnity.EventReference exitSound;
    public float exitVibration = 0.1f;


    void Awake()
    {
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (playClickSound) FMODUnity.RuntimeManager.PlayOneShot(clickSound, transform.position);
        Vib(0.1f, clickVibration);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (playEnterSound) FMODUnity.RuntimeManager.PlayOneShot(enterSound, transform.position);
        Vib(0.1f, enterVibration);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (playExitSound) FMODUnity.RuntimeManager.PlayOneShot(exitSound, transform.position);
        Vib(0.1f, exitVibration);
    }

    public void Vib(float start, float end)
    {
        Invoke("startVib", start);
        Invoke("stopVib", end);
    }

    public void startVib()
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
    public void stopVib()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }
}

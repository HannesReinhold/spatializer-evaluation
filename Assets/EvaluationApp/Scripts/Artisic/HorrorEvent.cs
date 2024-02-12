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

    public GameObject ambience;
    public GameObject heartbeat;

    public EyesSpawner eyeSpawner;
    public GameObject jumpscareSpring;

    public PopupWindow popupWindow;
    public GameObject completeWindow;



    private void OnEnable()
    {
        GameManager.Instance.ShowRoomModel(2);

        ambience.SetActive(true);

        EnableFog();
        Invoke("CloseIntroWindow",5);
        Invoke("EnableEyes",2);
        Invoke("DisableAmbience",15);
        Invoke("DisableHeartbeat",15);
        Invoke("EnableJumpscare",15.5f);

        Invoke("OnComplete",17);
       

        GUIAudioManager.SetAmbientVolume(0);

        completeWindow.SetActive(false);
    }

    private void OnDisable()
    {
        GameManager.Instance.HideRoomModel(2);

        currentFogTarget = 0;

    }

    public void OnComplete()
    {
        DisableEyes();
        Invoke("DisableFog",2);
        Invoke("OpenCompleteWindow",4);
    }

    public void EnableFog()
    {
        currentFogTarget = maxFog;
    }

    public void DisableFog()
    {
        currentFogTarget = minFog;
    }

    public void EnableEyes()
    {
        eyeSpawner.SpawnEyes();
    }

    public void DisableEyes()
    {
        eyeSpawner.RemoveEyes();
    }

    public void EnableJumpscare()
    {
        jumpscareSpring.SetActive(true);
        FMODUnity.RuntimeManager.PlayOneShot("event:/Spooky/Boing", jumpscareSpring.transform.position);
    }

    private void DisableAmbience()
    {
        ambience.SetActive(false);
    }

    private void DisableHeartbeat()
    {
        heartbeat.SetActive(false);
    }

    private void CloseIntroWindow()
    {
        popupWindow.Close();
    }

    private void OpenCompleteWindow()
    {
        completeWindow.SetActive(true);
    }

    public void OnCompleteClick()
    {
        GUIAudioManager.SetAmbientVolume(0.5f);
        completeWindow.GetComponent<PopupWindow>().Close();
        Invoke("DisableEvent",1);
    }


    private void DisableEvent()
    {
        gameObject.SetActive(false);
    }



    private void Update()
    {
        currentFogValue = 0.99f * currentFogValue + 0.01f * currentFogTarget;
        RenderSettings.fogDensity = currentFogValue;
    }

}

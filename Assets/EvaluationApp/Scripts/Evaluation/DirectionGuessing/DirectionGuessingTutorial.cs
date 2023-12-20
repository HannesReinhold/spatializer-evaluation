using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;

public class DirectionGuessingTutorial : MonoBehaviour
{
    public List<Transform> positionList;

    public WindowManager windowManager;

    public GameObject target;

    private int roundID = 0;

    private bool enableInput = false;


    private void OnEnable()
    {
        roundID = 0;
        GUIAudioManager.SetAmbientVolume(0.1f);
        Invoke("StartTutorial", 2);
        target.SetActive(false);
        
    }

    private void Update()
    {
        if (enableInput && OVRInput.GetDown(OVRInput.Button.One)) Shoot();
    }

    public void StartTutorial()
    {
        StartRound();
    }

    public void StartRound()
    {
        enabled = true;

        target.transform.position = positionList[roundID].position;
        target.SetActive(true);

        // Play 3 times audio cue
        PlayAudioCue();
        Invoke("PlayAudioCue", 1);
        Invoke("PlayAudioCue", 2);

        Invoke("Shoot", 3);
    }

    void Shoot()
    {
        enableInput = false;
        Vib();
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Gunshot", transform.position);
        CancelInvoke("PlayAudioCue");
        Invoke("Evaluate", 1);
    }

    public void Vib()
    {
        startVib();
        Invoke("stopVib", 0.05f);
        Invoke("startVib", 0.1f);
        Invoke("stopVib", 0.15f);
    }
    public void startVib()
    {
        OVRInput.SetControllerVibration(1, 1, OVRInput.Controller.RTouch);
    }
    public void stopVib()
    {
        OVRInput.SetControllerVibration(0, 0, OVRInput.Controller.RTouch);
    }

    private void PlayAudioCue()
    {
        GUIAudioManager.PlayMenuSubmit(target.transform.position);
    }

    void Evaluate()
    {
        roundID++;
        windowManager.NextPage();
        Debug.Log("Shoot");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DirectionGuessingGame : MonoBehaviour
{
    public GameObject target;
    public Transform controllerTransform;

    public LineRenderer lineRendererGuessed;
    public LineRenderer lineRendererActual;

    public TextMeshProUGUI textMesh;

    public PopupWindow startWindow;
    public PopupWindow finishWindow;
    public GameObject countdown;

    private Vector3 guessedDirection;

    private int currentRound = 0;
    public int numRounds = 25;
    public int countdownTime = 3;

    private bool enableInput = false;


    void Start()
    {
        StartGame();
    }


    void Update()
    {
        // Shoot if pressed
        if (enableInput && target.activeSelf && OVRInput.GetDown(OVRInput.Button.One)) Shoot();

        
    }

    public void StartGame()
    {
        startWindow.Open();
        target.SetActive(false);
    }

    public void FinishGame()
    {
        finishWindow.Open();
    }

    public void StartCountdown()
    {
        startWindow.Close();
        Invoke("StartRound", countdownTime);
    }

    private void StartRound()
    {
        RespawnTarget();
        EnableControllerInput();
    }

    private void EnableControllerInput()
    {
        enableInput = true;
    }

    private void DisableControllerInput()
    {
        enableInput = false;
    }
    

    void RespawnTarget()
    {
        // select a random position
        Vector2 dir = Random.insideUnitCircle.normalized;
        Vector3 position = new Vector3(controllerTransform.position.x,0,controllerTransform.position.z) +new Vector3(dir.x,0.6f,dir.y)*3;
        target.transform.position = position;
        target.SetActive(true);

        // Play 3 times audio cue
        PlayAudioCue();
        Invoke("PlayAudioCue",1);
        Invoke("PlayAudioCue", 2);
    }

    void Shoot()
    {
        // auditive and tactile feedback
        Vib();
        CancelInvoke("PlayAudioCue");

        // disable target and input
        target.SetActive(false);
        DisableControllerInput();

        Invoke("EvaluateShot",2);
    }

    private void PlayAudioCue()
    {
        GUIAudioManager.PlayMenuSubmit(target.transform.position);
    }

    private void DrawDirectionLines(Vector3 guessed, Vector3 actual, Color c1, Color c2) 
    {
        lineRendererGuessed.SetColors(c1,c1);
        lineRendererGuessed.positionCount = 2;
        lineRendererGuessed.SetPosition(0, controllerTransform.position);
        lineRendererGuessed.SetPosition(1, controllerTransform.position + guessed);

        lineRendererActual.SetColors(c1, c1);
        lineRendererActual.positionCount = 2;
        lineRendererActual.SetPosition(0, controllerTransform.position);
        lineRendererActual.SetPosition(1, controllerTransform.position + actual);
    }

    void EvaluateShot()
    {
        // calculate azimuth and elevation error
        guessedDirection = controllerTransform.forward;
        Vector3 actualDirection = (target.transform.position - controllerTransform.position).normalized;

        Vector3 dirProjAz = Vector3.ProjectOnPlane(actualDirection, controllerTransform.up);
        Vector3 dirProjEl = Vector3.ProjectOnPlane(actualDirection, controllerTransform.right);

        Vector3 gueProjAz = Vector3.ProjectOnPlane(guessedDirection, controllerTransform.up);
        Vector3 gueProjEl = Vector3.ProjectOnPlane(guessedDirection, controllerTransform.right);

        float azimuth = Vector2.SignedAngle(new Vector2(gueProjAz.x, gueProjAz.z).normalized, new Vector2(dirProjAz.x, dirProjAz.z).normalized);
        float elevation = Vector2.SignedAngle(new Vector2(gueProjEl.y, gueProjEl.z).normalized, new Vector2(dirProjEl.y, dirProjEl.z).normalized);

        // draw azimuth projection of guessed and actual
        DrawDirectionLines(new Vector3(guessedDirection.x, guessedDirection.y, guessedDirection.z).normalized, new Vector3(actualDirection.x, actualDirection.y, actualDirection.z).normalized, new Color(0, 255, 0), new Color(0, 255, 100));

        // show error as text
        textMesh.text = "Azimuth: " + azimuth + "\n, Elevation: " + elevation;

        currentRound++;

        if (currentRound <= numRounds)
            Invoke("StartCountdown", 0);
        else
            FinishGame();
    }

    public void Vib()
    {
        startVib();
        Invoke("stopVib", 0.05f);
        Invoke("startVib",0.1f);
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
}

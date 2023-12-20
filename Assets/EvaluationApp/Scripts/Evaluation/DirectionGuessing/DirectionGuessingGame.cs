using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DirectionGuessingGame : MonoBehaviour
{
    public GameObject target;
    public GameObject targetVisualizer;
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

    private float startTime = 0;

    public bool tutorial = false;


    private List<Vector3> guessList = new List<Vector3>();


    void Start()
    {
        StartGame();
        controllerTransform = GameObject.Find("RightHandAnchor").transform;
        GUIAudioManager.SetAmbientVolume(0.1f);
    }


    void Update()
    {
        // Shoot if pressed
        if (enableInput && target.activeSelf && OVRInput.GetDown(OVRInput.Button.One)) Shoot();

    }

    /// <summary>
    /// Initial start game
    /// </summary>
    public void StartGame()
    {
        startWindow.gameObject.SetActive(true);
        startWindow.Open();
        target.SetActive(false);
    }

    /// <summary>
    /// end the game
    /// </summary>
    public void FinishGame()
    {
        finishWindow.gameObject.SetActive(true);
        targetVisualizer.SetActive(false);
        lineRendererGuessed.gameObject.SetActive(false);
        lineRendererActual.gameObject.SetActive(false);
        DisableControllerInput();
    }

    public void OnFinishClick()
    {
        finishWindow.Close();
        GUIAudioManager.SetAmbientVolume(1);
    }

    /// <summary>
    /// Starts a small waiting period until the round starts
    /// </summary>
    public void StartCountdown()
    {
        startWindow.Close();
        Invoke("StartRound", countdownTime);
        // hide target
        targetVisualizer.SetActive(false);
        lineRendererGuessed.gameObject.SetActive(false);
        lineRendererActual.gameObject.SetActive(false);
    }

    /// <summary>
    /// Start the round
    /// </summary>
    private void StartRound()
    {
        RespawnTarget();
        EnableControllerInput();

        startTime = Time.time;
    }

    /// <summary>
    /// Enable controller input
    /// </summary>
    private void EnableControllerInput()
    {
        enableInput = true;
    }

    /// <summary>
    /// Disable controller input
    /// </summary>
    private void DisableControllerInput()
    {
        enableInput = false;
    }
    

    /// <summary>
    /// Spawn the target at a new random direction
    /// </summary>
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

    /// <summary>
    /// Shoots a ray in the guessed direction
    /// </summary>
    void Shoot()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Gunshot", controllerTransform.position);
        // auditive and tactile feedback
        Vib();
        CancelInvoke("PlayAudioCue");

        // disable target and input
        target.SetActive(false);
        DisableControllerInput();

        EvaluateShot();
    }

    /// <summary>
    /// Plays an audio cue
    /// </summary>
    private void PlayAudioCue()
    {
        GUIAudioManager.PlayMenuSubmit(target.transform.position);
    }

    /// <summary>
    /// Draws 2 lines for the guessed and actual direction (currently for debug only)
    /// </summary>
    /// <param name="guessed"></param>
    /// <param name="actual"></param>
    /// <param name="c1"></param>
    /// <param name="c2"></param>
    private void DrawDirectionLines(Vector3 guessed, Vector3 actual) 
    {
        lineRendererGuessed.positionCount = 2;
        lineRendererGuessed.SetPosition(0, controllerTransform.position);
        lineRendererGuessed.SetPosition(1, controllerTransform.position + guessed);

        lineRendererActual.positionCount = 2;
        lineRendererActual.SetPosition(0, controllerTransform.position);
        lineRendererActual.SetPosition(1, controllerTransform.position + actual);
    }

    /// <summary>
    /// Calculates the error for the guessed direction and starts a new round
    /// </summary>
    void EvaluateShot()
    {
        // calculate azimuth and elevation error
        guessedDirection = controllerTransform.forward;
        Vector3 actualDirection = (target.transform.position - controllerTransform.position).normalized;

        Quaternion dif = Quaternion.LookRotation(controllerTransform.InverseTransformPoint(target.transform.position), Vector3.up);
        float azimuth = dif.y;
        float elevation = dif.x;

        // draw azimuth projection of guessed and actual
        DrawDirectionLines(new Vector3(guessedDirection.x, guessedDirection.y, guessedDirection.z).normalized, new Vector3(actualDirection.x, actualDirection.y, actualDirection.z).normalized);

        // show error as text
        textMesh.text = "Azimuth: " + azimuth + "\n Elevation: " + elevation;


        // show target
        targetVisualizer.SetActive(true);
        lineRendererGuessed.gameObject.SetActive(true);
        lineRendererActual.gameObject.SetActive(true);

        float elapsedTime = Time.time - startTime;

        guessList.Add(new Vector3(azimuth,elevation,elapsedTime));


        currentRound++;

        // if all rounds are over, finish game
        if (currentRound < numRounds)
            Invoke("StartCountdown", 2);
        else
            Invoke("FinishGame",1);
    }

    /// <summary>
    /// Vibration of the controller
    /// </summary>
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

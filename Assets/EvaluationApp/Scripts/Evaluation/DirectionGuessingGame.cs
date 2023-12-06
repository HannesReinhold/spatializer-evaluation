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

    // Start is called before the first frame update
    void Start()
    {
        RespawnTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if (target.activeSelf && OVRInput.GetDown(OVRInput.Button.One)) Shoot();
    }

    void RespawnTarget()
    {
        Vector3 position = new Vector3(Random.Range(-5,5),Random.Range(0,3), Random.Range(-5,5)).normalized;
        target.transform.position = position;
        target.SetActive(true);
    }

    void Shoot()
    {
        Vib();

        Quaternion guessedDirection = controllerTransform.rotation;
        Vector3 dir = (target.transform.position - controllerTransform.position).normalized;
        Quaternion actualDirection = Quaternion.LookRotation(dir,Vector3.up);

        lineRendererGuessed.positionCount = 2;
        lineRendererGuessed.SetPosition(0, controllerTransform.position);
        lineRendererGuessed.SetPosition(1, controllerTransform.position + Quaternion.Euler(guessedDirection.eulerAngles)*Vector3.forward);
        lineRendererActual.positionCount = 2;
        lineRendererActual.SetPosition(0, controllerTransform.position);
        lineRendererActual.SetPosition(1, controllerTransform.position + Quaternion.Euler(actualDirection.eulerAngles) * Vector3.forward);





        target.SetActive(false);
        Invoke("EvaluateShot",3);
    }

    void EvaluateShot()
    {
        
        Invoke("RespawnTarget",3);
    }

    public void Vib()
    {
        startVib();
        Invoke("stopVib", .1f);
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

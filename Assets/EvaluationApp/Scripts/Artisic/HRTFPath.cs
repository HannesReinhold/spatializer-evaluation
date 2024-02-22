using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HRTFPath : MonoBehaviour
{

    private float targetAzimuth=0;
    private float targetElevation=0;

    public float currentAzimuth;
    public float currentElevation;

    public float radius;

    public float transitionTime = 1;

    public Transform mover;

    int index;

    public List<Vector2> directionList = new List<Vector2>();

    private float t = 0;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        t = 0;
        targetAzimuth = directionList[0].x * Mathf.PI;
        targetElevation = directionList[0].y * Mathf.PI;
        Invoke("SetTargetToNext", 4);
    }

    private void OnDisable()
    {
        CancelInvoke("SetTargetToNext");
    }


    void Update()
    {

        currentAzimuth = Mathf.Lerp(currentAzimuth, targetAzimuth, t);
        currentElevation = Mathf.Lerp(currentElevation, targetElevation, t);

        t += Time.deltaTime / transitionTime;

        mover.localPosition = new Vector3(Mathf.Cos(currentAzimuth)*Mathf.Cos(currentElevation), Mathf.Sin(currentElevation),Mathf.Cos(currentElevation) *Mathf.Sin(currentAzimuth)) * radius;


    }

    private void SetTargetToNext()
    {
        index++;
        index%= directionList.Count;

        targetAzimuth = directionList[index].x * Mathf.PI;
        targetElevation = directionList[index].y * Mathf.PI;

        t = 0;
        Invoke("SetTargetToNext", 4);
    }
}

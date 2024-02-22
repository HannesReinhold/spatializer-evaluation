using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spatializer : MonoBehaviour
{

    public Transform cameraTransform;

    private HRTF hrtf=new HRTF();

    void Start()
    {
        
    }


    void Update()
    {
        Vector3 relativeDirection = cameraTransform.InverseTransformDirection((transform.position - cameraTransform.position).normalized);
        float sourceDistance = Vector3.Distance(cameraTransform.position, transform.position);




        hrtf.UpdateHRTF(relativeDirection,sourceDistance);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        hrtf.Process(data, channels);
    }
}

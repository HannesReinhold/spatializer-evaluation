using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicReverb : MonoBehaviour
{
    public Transform camTransform;

    public int numRays;
    private int lastNumRays;

    public float maxDistance = 1000;

    public Vector3[] rayDirections;
    public float[] rayDistances;

    private float averageRoomSize=1;
    public float minRoomSize;
    public float maxRoomSize;

    public float minDelay;
    public float maxDelay;

    public float wallAbsorption = 0.6f;

    public bool occluded = false;



    // DSP
    RealisticReverb reverb=new RealisticReverb(8,2);



    void Start()
    {
        camTransform = Camera.main.transform;

        lastNumRays = numRays;
        GenerateRays();
    }


    void Update()
    {
        if(lastNumRays != numRays) GenerateRays();
        lastNumRays = numRays;

        averageRoomSize = 0;
        minRoomSize = 1000;
        maxRoomSize = 0;


        for(int i=0; i < numRays; i++)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position, rayDirections[i], out hit, maxDistance))
            {
                averageRoomSize += hit.distance;
                rayDistances[i] = hit.distance;
                if(hit.distance < minRoomSize) minRoomSize = hit.distance;
                if(hit.distance > maxRoomSize) maxRoomSize = hit.distance;
            }

            if(hit.distance!= 0)Debug.DrawLine(transform.position, hit.point);
            else Debug.DrawLine(transform.position, transform.position+rayDirections[i]*maxDistance);

        }

        averageRoomSize /= numRays;
        float standartDeviation=0;
        float sum = 0;

        for(int i=0; i < numRays; i++)
        {
            sum += Mathf.Pow((rayDistances[i] - averageRoomSize), 2);
        }
        standartDeviation = Mathf.Sqrt(sum/(numRays-1));

        minDelay = Mathf.Min(minRoomSize, Mathf.Clamp(averageRoomSize-standartDeviation,0,maxDistance));
        maxDelay = Mathf.Clamp(averageRoomSize + standartDeviation, 0, maxDistance);

        reverb.diffuser.setDelays(minDelay,maxDelay*0.33f);
        reverb.delaynetwork.setDelays(minDelay, maxDelay*10);

        reverb.delaynetwork.setFeedback(wallAbsorption);
        reverb.delaynetwork.setCutoffFrequencies(wallAbsorption);

        RaycastHit occlusionHit;
        Vector3 direction = (camTransform.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position,camTransform.position);
        if (Physics.Raycast(transform.position, (camTransform.position - transform.position).normalized, out occlusionHit, distance))
        {
            reverb.dry = 0;
            occluded = true;
        }
        else
        {
            occluded = false;
            reverb.dry = 1;
        }
        Debug.DrawLine(transform.position, transform.position+(camTransform.position - transform.position).normalized);
        

    }

    public void GenerateRays()
    {
        rayDirections = new Vector3[numRays];
        rayDistances = new float[numRays];

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i=0; i < numRays; i++)
        {
            float t = (float)i / numRays;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);
            rayDirections[i] = new Vector3(x, y, z);

        }
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        reverb.Process(data);
    }
}

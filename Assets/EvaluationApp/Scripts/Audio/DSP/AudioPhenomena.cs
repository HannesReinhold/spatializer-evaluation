using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPhenomena : MonoBehaviour
{
    [System.Serializable]
    public struct headVolume
    {
        public float forward;
        public float backward;
        public float left;
        public float right;
        public float up;
        public float down;
    }

    public ParticleSystem particles;
    public Transform speakerModel;
    public Light light;


    [Header("Source Properties")]
    public float loudnessDb = 70;

    [Header("HRTF")]
    public float earDelayDifference;
    public headVolume earVolumeDifference;
    public float earFilterDifference;




    public bool enableITD;
    public bool enableIID;
    public bool enableAttenuation;
    public bool enableOcclusion;
    public bool enableDoppler;
    public bool enableReverb;

    public Transform trans;



    private VariableDelay itdDelayLeft = new VariableDelay(1000);
    private VariableDelay itdDelayRight = new VariableDelay(1000);

    private FirstOrderLowpass iidFilterLeft = new FirstOrderLowpass();
    private FirstOrderLowpass iidFilterRight = new FirstOrderLowpass();

    private FirstOrderLowpass airAttenuationFilterLeft = new FirstOrderLowpass();
    private FirstOrderLowpass airAttenuationFilterRight = new FirstOrderLowpass();

    private FirstOrderLowpass occlusionFilterLeft = new FirstOrderLowpass();
    private FirstOrderLowpass occlusionFilterRight = new FirstOrderLowpass();

    private RealisticReverb reverb = new RealisticReverb(8, 4);

    // Audio parameters

    private float volumeLeft;
    private float volumeRight;

    private float filterCutoffLeft;
    private float filterCutoffRight;

    private float delayLeft;
    private float delayRight;

    private float airAttenuation = 1;
    private float airAttenuationVolume = 1;
    private float airAttenuationFilterCutoff = 1;

    private float occlusionVolume = 1;
    private float occlusionFilterCutoff = 1;



    [Range(0, 1)] public float dry = 1;
    [Range(0, 1)] public float wet = 1;

    [Range(0, 1)] public float feedback = 0.5f;
    [Range(0, 1)] public float feedbackCutoff = 0.5f;

    [Range(0, 50)] public float diffDelaysMin = 20, diffDelaysMax = 40;
    [Range(0, 1000)] public float revDelaysMin = 12, revDelaysMax = 234;

    [Range(0, 1)] public float diffuserOutputCutoff = 0.9f;

    [Range(0, 4)] public int numDiffusionStages = 1;

    public bool enableDiffusion = true;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relativePosition = trans.InverseTransformDirection((transform.position - trans.position).normalized);

        float sourceDistance = Vector3.Distance(trans.position, transform.position);
        float near = 1 - Sigmoid(sourceDistance, -2, 2);
        float nearExp = Mathf.Lerp(0.9f, 1.25f, near);

        // calc hrtf
        float dotLeft = Mathf.Pow((Vector3.Dot(-Vector3.right, relativePosition) + 1) * 0.5f, nearExp);
        float dotRight = Mathf.Pow((Vector3.Dot(Vector3.right, relativePosition) + 1) * 0.5f, nearExp);
        float dotForward = Mathf.Pow((Vector3.Dot(Vector3.forward, relativePosition) + 1) * 0.5f, nearExp);
        float dotUpward = Mathf.Pow((Vector3.Dot(Vector3.up, relativePosition) + 1) * 0.5f, nearExp);

        volumeLeft = (Mathf.Lerp(Mathf.Lerp(0.3f, 0.15f, near), 1, dotLeft) * 0.6f + dotForward * 0.3f + dotUpward * 0.1f) * 1.3f;
        volumeRight = (Mathf.Lerp(Mathf.Lerp(0.3f, 0.15f, near), 1, dotRight) * 0.6f + dotForward * 0.3f + dotUpward * 0.1f) * 1.3f;

        filterCutoffLeft = Mathf.Lerp(0.4f, 1, volumeLeft);
        filterCutoffRight = Mathf.Lerp(0.4f, 1, volumeRight);

        delayLeft = (Vector3.Dot(Vector3.right, relativePosition) + 1);
        delayRight = (Vector3.Dot(-Vector3.right, relativePosition) + 1);
        itdDelayLeft.SetDelay(delayLeft* earDelayDifference);
        itdDelayRight.SetDelay(delayRight* earDelayDifference);

        // calc air attenuation
        airAttenuation = calculateAirAttenuation(sourceDistance);
        airAttenuationVolume = Mathf.Pow(airAttenuation, 0.9f);
        airAttenuationFilterCutoff = Mathf.Lerp(0.1f, 1, Mathf.Pow(airAttenuation, 0.5f));



        // calc occlusion
        float occ = calcOcclusion();
        occlusionVolume = occ;
        occlusionFilterCutoff = Mathf.Lerp(0, 1, occ);

        // calc reverb parameters


        reverb.dry = occlusionVolume;
        reverb.wet = wet;

        reverb.delaynetwork.setFeedback(feedback);
        reverb.delaynetwork.setCutoffFrequencies(feedbackCutoff);
        reverb.diffuser.setDelays(diffDelaysMin, diffDelaysMax);
        reverb.delaynetwork.setDelays(revDelaysMin, revDelaysMax);

        reverb.enableReverb = enableReverb;
        reverb.diffuser.SetOtputCutoffFrequency(diffuserOutputCutoff);

        reverb.diffuser.numStages = numDiffusionStages;



        Debug.Log("Volume:[L: " + volumeLeft + ", R: " + volumeRight + "], " +
                  "Filter:[L: " + filterCutoffLeft + ", R: " + filterCutoffRight + "]" +
                  "Delay:[L: " + delayLeft + ", R: " + delayRight + "]" +
                  "Near Field: " + near + "]" +
                  "Attenuation:[ " + airAttenuation + "]" +
                  "Occlusion:[ " + occlusionVolume + "]"
                  );


        iidFilterLeft.cutoffFrequency = filterCutoffLeft;
        iidFilterRight.cutoffFrequency = filterCutoffRight;

        airAttenuationFilterLeft.cutoffFrequency = airAttenuationFilterCutoff;
        airAttenuationFilterRight.cutoffFrequency = airAttenuationFilterCutoff;

        occlusionFilterLeft.cutoffFrequency = occlusionFilterCutoff;
        occlusionFilterRight.cutoffFrequency= occlusionFilterCutoff;


        //var emission = particles.emission;
        //emission.rateOverTimeMultiplier = rms*25;
        if(rms>0.3f && !playParticles)
        {
            playParticles = true;
            particles.Play();
        }
        if (rms < 0.3f)
        {
            playParticles = false;
        }

        speakerModel.localScale = Vector3.one*0.1f+Vector3.one*rms*0.05f;
        light.intensity = rms * 1;
    }

    bool playParticles = false;


    float calcOcclusion()
    {
        float occlusion = 0;
        int n = 0;

        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < 10; i++)
        {
            float t = (float)i / 100;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);


            Vector3 dir = new Vector3(x, y, z);
            //Vector3 dir = Random.onUnitSphere;
            RaycastHit hit = new RaycastHit();
            float att = 2;
            Vector3 lastHit = Vector3.zero;

            for (int j = 0; j < 2; j++)
            {
                lastHit = hit.point;
                if (Physics.Raycast(transform.position + dir * 0.02f, dir, out hit, Mathf.Infinity))
                {
                    Debug.DrawLine(transform.position, hit.point);
                    RaycastHit hit2;
                    att *= 0.5f;
                    dir = Vector3.Reflect(dir, hit.normal);
                    if (Physics.Raycast(hit.point - (trans.position - hit.point).normalized * 0.02f, (trans.position - hit.point).normalized, out hit2, Vector3.Distance(trans.position, hit.point)))
                    {
                        att = 0;
                        //Debug.DrawLine(hit.point, hit2.point);
                    }

                    n++;
                    occlusion += att;
                }
                Debug.DrawLine(lastHit, hit.point);

            }
        }

        return Mathf.Pow(occlusion / n, 0.5f);
    }

    Vector3[] preferredDirections = new Vector3[5];


    float calcOcclusion2()
    {
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < 100; i++)
        {
            float t = (float)i / 100;
            float inclination = Mathf.Acos(1 - 2 * t);
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);


            Vector3 dir = new Vector3(x,y,z);
            dir += preferredDirections[0] * 0.5f;
            dir.Normalize();

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(transform.position - dir * 0.02f, dir, out hit, Mathf.Infinity))
            {
                //Debug.DrawLine(transform.position, hit.point);
                RaycastHit hit2;


                if (Physics.Raycast(hit.point - (trans.position - hit.point).normalized * 0.05f, (trans.position - hit.point).normalized, out hit2, Vector3.Distance(trans.position, hit.point)))
                {

                }
                else
                {
                    Debug.DrawLine(hit.point, hit2.point);
                    preferredDirections[0] += dir * 0.3f;
                    preferredDirections[0].Normalize();
                }
            }
        }

        Debug.DrawLine(transform.position, transform.position + preferredDirections[0]);

        return 0.4f;
    }




    private float Sigmoid(float x, float offset, float size)
    {
        return 1f / (1 + Mathf.Exp(-size * (x + offset)));
    }

    private float calculateAirAttenuation(float distance)
    {
        float att = DbToLin(loudnessDb - 70) / Mathf.Pow(distance + 1, 1.5f);

        return att;
    }

    private float DbToLin(float x)
    {
        return Mathf.Pow(10, x / 20);
    }



    public float sum = 0;
    private float rms = 0;


    private void OnAudioFilterRead(float[] data, int channels)
    {
        sum = 0;
        for(int i=0; i<data.Length; i += channels)
        {
            sum += Mathf.Max(Mathf.Abs(data[i]), Mathf.Abs(data[i+1]));
        }
        rms = Mathf.Sqrt(sum/(data.Length/2));



        for (int i = 0; i < data.Length; i += channels)
        {
            // ITD
            if (enableITD)
            {
                data[i] = itdDelayLeft.Process(data[i]);
                data[i + 1] = itdDelayRight.Process(data[i + 1]);
            }



            // IID
            if (enableIID)
            {
                data[i] = iidFilterLeft.Process(data[i]) * volumeLeft;
                data[i + 1] = iidFilterRight.Process(data[i + 1]) * volumeRight;
            }



            // Attenuation
            if (enableAttenuation)
            {
                data[i] = airAttenuationFilterLeft.Process(data[i]) * airAttenuationVolume;
                data[i + 1] = airAttenuationFilterRight.Process(data[i + 1]) * airAttenuationVolume;
            }



            // Occlusion
            if (enableOcclusion)
            {
                data[i] = occlusionFilterLeft.Process(data[i]) * occlusionVolume;
                data[i + 1] = occlusionFilterRight.Process(data[i + 1]) * occlusionVolume;
            }


            

        }

        // Reverb
        if (enableReverb)
        {
            reverb.Process(data);
        }


    }

    public void EnableITD()
    {
        enableITD = true;
    }

    public void EnableIID()
    {
        enableIID = true;
    }

    public void EnableAttenuation()
    {
        enableAttenuation = true;
    }

    public void EnableOcclusion()
    {
        enableOcclusion = true;
    }

    public void EnableDoppler()
    {
        enableDoppler = true;
    }

    public void EnableReverb()
    {
        enableReverb = true;
    }
}

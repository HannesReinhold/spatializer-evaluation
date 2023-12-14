using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FilterTest : MonoBehaviour
{

    private Biquad biquad1 = new Biquad();
    private Biquad biquad2 = new Biquad();
    private Biquad biquad3 = new Biquad();
    private Biquad biquad4 = new Biquad();

    [Range(0.0001f, 0.5f)] public float fC;
    [Range(0.001f, 10)] public float q;
    [Range(-10, 10)] public float pG;

    [Range(0,1)] public float t = 0;


    float[] coeffs1 = new float[5];
    float[] coeffs2 = new float[5];
    


    // Start is called before the first frame update
    void Start()
    {
        coeffs1 = biquad1.GetCalculatedCoefficients(0.2f,10,10);
        coeffs2 = biquad1.GetCalculatedCoefficients(0.2f, 10, -10);
    }

    // Update is called once per frame
    void Update()
    {
        coeffs1 = biquad1.GetCalculatedCoefficients(fC, 10, 10);
        coeffs2 = biquad1.GetCalculatedCoefficients(fC, 10, -10);

        float[] coeffs = new float[5];
        for(int i=0; i < 5; i++)
        {
            coeffs[i] = (1 - t) * coeffs1[i] + t* coeffs2[i];
        }
        biquad1.SetCoefficients(coeffs);
    }

    private void OnAudioFilterRead(float[] data, int channels)
    {
        biquad1.Process(data);
    }
}

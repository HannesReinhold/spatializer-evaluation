 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface AudioFilter
{

    public void Process(float[] input);

}


public class Biquad : AudioFilter
{

    int type;
    float a0, a1, a2, b1, b2;
    float fc, q, peakGain;
    float z1L, z2L;
    float z1R, z2R;

    public void Process(float[] inputBuffer)
    {
        float outL = 0;
        float inL = 0;

        float outR = 0;
        float inR = 0;

        for(int i=0; i<inputBuffer.Length; i+=2)
        {
            inL = inputBuffer[i];
            inR = inputBuffer[i+1];

            outL = inL * a0 + z1L;
            outR = inR * a0 + z1R;

            z1L = inL * a1 + z2L - b1 * outL;
            z1R = inR * a1 + z2R - b1 * outR;

            z2L = inL * a2 - b2 * outL;
            z2R = inR * a2 - b2 * outR;

            inputBuffer[i] = outL;
            inputBuffer[i + 1] = outR;
        }
    }

    public void CalculateCoefficients(float fc, float q, float peakGain)
    {
        float norm;
        float V = Mathf.Pow(10, Mathf.Abs(peakGain) / 20f);
        float K = Mathf.Tan(Mathf.PI * fc);

        float kk = K * K;
        float qk = q * K;
        float vqk = V / qk;
        float qk2 = 1 / qk + kk;

        if (peakGain >= 0)
        {    // boost
            norm = 1 / (1 + 1 / q * K + kk);
            a0 = (1 + V / q * K + kk) * norm;
            a1 = 2 * (kk - 1) * norm;
            a2 = (1 - V / q * K + kk) * norm;
            b1 = a1;
            b2 = (1 - 1 / q * K + kk) * norm;
        }
        else
        {    // cut
            norm = 1 / (1 + V / q * K + kk);
            a0 = (1 + 1 / q * K + kk) * norm;
            a1 = 2 * (kk - 1) * norm;
            a2 = (1 - 1 / q * K + kk) * norm;
            b1 = a1;
            b2 = (1 - V / q * K + kk) * norm;
        }
    }

    public void SetCoefficients(float[] coeffs)
    {
        a0 = coeffs[0];
        a1 = coeffs[1];
        a2 = coeffs[2];
        b1 = coeffs[3];
        b2 = coeffs[4];
    }

    public float[] GetCalculatedCoefficients(float fc, float q, float peakGain)
    {
        float[] coeffs = new float[5];

        float norm;
        float V = Mathf.Pow(10, Mathf.Abs(peakGain) / 20f);
        float K = Mathf.Tan(Mathf.PI * fc);

        float kk = K * K;
        float qk = q * K;
        float vqk = V / qk;
        float qk2 = 1 / qk + kk;

        if (peakGain >= 0)
        {    // boost
            norm = 1 / (1 + 1 / q * K + kk);
            coeffs[0] = (1 + V / q * K + kk) * norm;
            coeffs[1] = 2 * (kk - 1) * norm;
            coeffs[2] = (1 - V / q * K + kk) * norm;
            coeffs[3] = a1;
            coeffs[4] = (1 - 1 / q * K + kk) * norm;
        }
        else
        {    // cut
            norm = 1 / (1 + V / q * K + kk);
            coeffs[0] = (1 + 1 / q * K + kk) * norm;
            coeffs[1] = 2 * (kk - 1) * norm;
            coeffs[2] = (1 - 1 / q * K + kk) * norm;
            coeffs[3] = a1;
            coeffs[4] = (1 - V / q * K + kk) * norm;
        }
        return coeffs;
    }

}
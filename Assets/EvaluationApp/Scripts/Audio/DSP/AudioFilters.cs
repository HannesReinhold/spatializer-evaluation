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

    public Biquad()
    {

    }

    public void Process(float[] inputBuffer)
    {
        float outL = 0;
        float inL = 0;

        float outR = 0;
        float inR = 0;

        for(int i=0; i<inputBuffer.Length; i+=2)
        {
            CalculateCoefficients(fc,q,peakGain);

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

public class BiquadSingleChannel
{
    float a0, a1, a2, b1, b2;

    float z1=0, z2=0;

    public void SetCoeffs(float[] coeffs)
    {
        a0 = coeffs[0];
        a1 = coeffs[1];
        a2 = coeffs[2];
        b1 = coeffs[3];
        b2 = coeffs[4];
    }

    public BiquadSingleChannel()
    {
        a0 = 1;
        a1 = a2 = b1 = b2 = 0;
    }

    public float Process(float input)
    {
        float output = input *a0 + z1;
        z1 = input *a1 + z2 - b1 * output;
        z2 = input *a2 - b2 * output;
        return output;
    }




}

public enum BiquadType
{
    Lowpass,
    Highpass,
    Bandpass,
    Bandstop,
    Peak,
    Lowshelf,
    Highshelf
}

public static class BiquadCalculator
{
    public static float[] CalculateCoefficients(BiquadType type,float fc, float Q, float pG)
    {

        float a0=0, a1=0, a2=0, b1=0, b2=0;

        float norm;
        float V = Mathf.Pow(10, Mathf.Abs(pG) / 20f);
        float K = Mathf.Tan(Mathf.PI * fc*0.5f);
        switch (type)
        {
            case BiquadType.Lowpass:
                norm = 1 / (1 + K / Q + K * K);
                a0 = K * K * norm;
                a1 = 2 * a0;
                a2 = a0;
                b1 = 2 * (K * K - 1) * norm;
                b2 = (1 - K / Q + K * K) * norm;
                break;

            case BiquadType.Highpass:
                norm = 1 / (1 + K / Q + K * K);
                a0 = 1 * norm;
                a1 = -2 * a0;
                a2 = a0;
                b1 = 2 * (K * K - 1) * norm;
                b2 = (1 - K / Q + K * K) * norm;
                break;

            case BiquadType.Bandpass:
                norm = 1 / (1 + K / Q + K * K);
                a0 = K / Q * norm;
                a1 = 0;
                a2 = -a0;
                b1 = 2 * (K * K - 1) * norm;
                b2 = (1 - K / Q + K * K) * norm;
                break;

            case BiquadType.Bandstop:
                norm = 1 / (1 + K / Q + K * K);
                a0 = (1 + K * K) * norm;
                a1 = 2 * (K * K - 1) * norm;
                a2 = a0;
                b1 = a1;
                b2 = (1 - K / Q + K * K) * norm;
                break;

            case BiquadType.Peak:
                if (pG >= 0)
                {    // boost
                    norm = 1 / (1 + 1 / Q * K + K * K);
                    a0 = (1 + V / Q * K + K * K) * norm;
                    a1 = 2 * (K * K - 1) * norm;
                    a2 = (1 - V / Q * K + K * K) * norm;
                    b1 = a1;
                    b2 = (1 - 1 / Q * K + K * K) * norm;
                }
                else
                {    // cut
                    norm = 1 / (1 + V / Q * K + K * K);
                    a0 = (1 + 1 / Q * K + K * K) * norm;
                    a1 = 2 * (K * K - 1) * norm;
                    a2 = (1 - 1 / Q * K + K * K) * norm;
                    b1 = a1;
                    b2 = (1 - V / Q * K + K * K) * norm;
                }
                break;
            case BiquadType.Lowshelf:
                if (pG >= 0)
                {    // boost
                    norm = 1 / (1 + Mathf.Sqrt(2) * K + K * K);
                    a0 = (1 + Mathf.Sqrt(2 * V) * K + V * K * K) * norm;
                    a1 = 2 * (V * K * K - 1) * norm;
                    a2 = (1 - Mathf.Sqrt(2 * V) * K + V * K * K) * norm;
                    b1 = 2 * (K * K - 1) * norm;
                    b2 = (1 - Mathf.Sqrt(2) * K + K * K) * norm;
                }
                else
                {    // cut
                    norm = 1 / (1 + Mathf.Sqrt(2 * V) * K + V * K * K);
                    a0 = (1 + Mathf.Sqrt(2) * K + K * K) * norm;
                    a1 = 2 * (K * K - 1) * norm;
                    a2 = (1 - Mathf.Sqrt(2) * K + K * K) * norm;
                    b1 = 2 * (V * K * K - 1) * norm;
                    b2 = (1 - Mathf.Sqrt(2 * V) * K + V * K * K) * norm;
                }
                break;
            case BiquadType.Highshelf:
                if (pG >= 0)
                {    // boost
                    norm = 1 / (1 + Mathf.Sqrt(2) * K + K * K);
                    a0 = (V + Mathf.Sqrt(2 * V) * K + K * K) * norm;
                    a1 = 2 * (K * K - V) * norm;
                    a2 = (V - Mathf.Sqrt(2 * V) * K + K * K) * norm;
                    b1 = 2 * (K * K - 1) * norm;
                    b2 = (1 - Mathf.Sqrt(2) * K + K * K) * norm;
                }
                else
                {    // cut
                    norm = 1 / (V + Mathf.Sqrt(2 * V) * K + K * K);
                    a0 = (1 + Mathf.Sqrt(2) * K + K * K) * norm;
                    a1 = 2 * (K * K - 1) * norm;
                    a2 = (1 - Mathf.Sqrt(2) * K + K * K) * norm;
                    b1 = 2 * (K * K - V) * norm;
                    b2 = (V - Mathf.Sqrt(2 * V) * K + K * K) * norm;
                }
                break;

                
        }
        return new float[] { a0, a1, a2, b1, b2 };


    }
}
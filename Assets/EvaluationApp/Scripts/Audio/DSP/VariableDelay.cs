using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDelay
{

    private float delaySamples;
    private float smoothedDelaySamples;

    private int bufferLength;
    private float[] buffer;

    private float readPointer;
    private int writePointer;

    public VariableDelay(int n)
    {
        bufferLength = n;
        buffer = new float[n];

    }

    public void SetDelay(float ms)
    {
        delaySamples = 44800 * ms / 1000;
        if (delaySamples >= bufferLength) delaySamples = bufferLength - 1;
        if (delaySamples < 0) delaySamples = 0;
    }

    public float Process(float input)
    {

        smoothedDelaySamples = smoothedDelaySamples * 0.9995f + delaySamples * (1 - 0.9995f);

        readPointer = writePointer - (int)smoothedDelaySamples;
        while (readPointer < 0) readPointer += bufferLength;

        buffer[writePointer] = input;

        writePointer = (writePointer + 1);
        if (writePointer >= bufferLength) writePointer -= bufferLength;

        return buffer[(int)readPointer];
    }
}

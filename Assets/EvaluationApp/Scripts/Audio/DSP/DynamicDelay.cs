using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicDelay
{
    private float dry;
    private float wet;
    private float delayInSamplesLeft;
    private float delayInSamplesRight;
    private float feedback;

    private int bufferLength;
    private float[] bufferLeft;
    private float[] bufferRight;

    private float readPointerLeft;
    private float readPointerRight;

    private int writePointerLeft;
    private int writePointerRight;

    private float smoothedDelayInSamplesLeft;
    private float smoothedDelayInSamplesRight;


    private int sampleRate;

    public float Dry { get { return dry; } set { dry = value; } }
    public float Wet { get { return wet; } set { wet = value; } }
    public float DelayInSamplesLeft { get { return delayInSamplesLeft; } set { delayInSamplesLeft = value; CheckIfTeleport(); } }
    public float DelayInSamplesRight { get { return delayInSamplesRight; } set { delayInSamplesRight = value; CheckIfTeleport(); } }
    public float DelayInSamples { set { delayInSamplesLeft = value; delayInSamplesRight = value; CheckIfTeleport(); } }
    public float Feedback { get { return feedback; } set { feedback = value; } }

    public float delayChangeTime = 0.99f;

    public DynamicDelay(int nBuffer, int sampleRate)
    {
        dry = 0f;
        wet = 1f;
        delayInSamplesLeft = 20000f;
        feedback = 0.9f;

        bufferLength = nBuffer;
        bufferLeft = new float[nBuffer];
        bufferRight = new float[nBuffer];

        this.sampleRate = sampleRate;
    }

    public void CheckIfTeleport()
    {
        if (Mathf.Abs(delayInSamplesLeft - smoothedDelayInSamplesLeft) > 10000)
        {
            smoothedDelayInSamplesLeft = delayInSamplesLeft;
            smoothedDelayInSamplesRight = delayInSamplesRight;
        }
    }

    public void SetSmoothedDelay(float smoothedDelay)
    {
        smoothedDelayInSamplesLeft = smoothedDelay;
        smoothedDelayInSamplesRight = smoothedDelay;
    }

    public void SetDelays(float delLeft, float delRight)
    {
        delayInSamplesLeft = delLeft;
        delayInSamplesRight = delRight;
    }

    public void ProcessBlock(float[] data, int channels)
    {


        for (int i = 0; i < data.Length; i += channels)
        {
            float inputLeft = data[i];
            float inputRight = data[i + 1];

            smoothedDelayInSamplesLeft = smoothedDelayInSamplesLeft * delayChangeTime + delayInSamplesLeft * (1 - delayChangeTime);
            smoothedDelayInSamplesRight = smoothedDelayInSamplesRight * delayChangeTime + delayInSamplesRight * (1 - delayChangeTime);

            readPointerLeft = writePointerLeft - (int)smoothedDelayInSamplesLeft;
            while (readPointerLeft < 0) readPointerLeft += bufferLength;
            readPointerRight = writePointerRight - (int)smoothedDelayInSamplesRight;
            while (readPointerRight < 0) readPointerRight += bufferLength;

            //bufferLeft[writePointerLeft] *= feedback;
            //bufferRight[writePointerRight] *= feedback;

            //bufferLeft[writePointerLeft] = bufferLeft[(int)readPointerLeft] * feedback + inputLeft;
            //bufferRight[writePointerRight] = bufferRight[(int)readPointerRight] * feedback + inputRight;

            bufferLeft[writePointerLeft] = inputLeft;
            bufferRight[writePointerRight] = inputRight;

            writePointerLeft = (writePointerLeft + 1) % bufferLength;
            writePointerRight = (writePointerRight + 1) % bufferLength;

            data[i] = bufferLeft[(int)readPointerLeft];
            data[i + 1] = bufferRight[(int)readPointerRight];
        }

    }

    public float processLeft(float inputLeft)
    {
        smoothedDelayInSamplesLeft = smoothedDelayInSamplesLeft * delayChangeTime + delayInSamplesLeft * (1 - delayChangeTime);

        readPointerLeft = writePointerLeft - (int)smoothedDelayInSamplesLeft;
        while (readPointerLeft < 0) readPointerLeft += bufferLength;

        bufferLeft[writePointerLeft] *= feedback;

        bufferLeft[writePointerLeft] = bufferLeft[(int)readPointerLeft] * feedback + inputLeft;

        writePointerLeft = (writePointerLeft + 1) % bufferLength;

        return dry * inputLeft + wet * bufferLeft[(int)readPointerLeft];
    }

    public float processRight(float inputRight)
    {
        smoothedDelayInSamplesRight = smoothedDelayInSamplesRight * delayChangeTime + delayInSamplesRight * (1 - delayChangeTime);

        readPointerRight = writePointerRight - (int)smoothedDelayInSamplesRight;
        while (readPointerRight < 0) readPointerRight += bufferLength;

        bufferRight[writePointerRight] *= feedback;

        bufferRight[writePointerRight] = bufferRight[(int)readPointerRight] * feedback + inputRight;

        writePointerRight = (writePointerRight + 1) % bufferLength;

        return dry * inputRight + wet * bufferRight[(int)readPointerRight];
    }



}

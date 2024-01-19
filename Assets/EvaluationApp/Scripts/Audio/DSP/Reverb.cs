using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverb : MonoBehaviour
{
    AllpassDiffusionNetwork diff1 = new AllpassDiffusionNetwork(1,8,60);
    AllpassDiffusionNetwork diff2 = new AllpassDiffusionNetwork(1, 8, 120);
    AllpassDiffusionNetwork diff3 = new AllpassDiffusionNetwork(1, 8, 180);
    AllpassDiffusionNetwork diff4 = new AllpassDiffusionNetwork(1, 8, 240);



    private void OnAudioFilterRead(float[] data, int channels)
    {
        diff1.Process(data, data);
        diff2.Process(data, data);
        diff3.Process(data, data);
        diff4.Process(data, data);
    }

}


public class AllpassDiffusionNetwork
{

    public int numStages;
    public int numChannels;


    // diff multitap delay
    int writePointer;
    float[] readPointerLeft, readPointerRight;
    float[] bufferLeft, bufferRight;
    int bufferSize;
    public float[] delays;

    // hadamard matrix



    public AllpassDiffusionNetwork(int numStages, int numChannels, float max)
    {
        float msInSamples = max * 48;
        bufferSize = (int)msInSamples + 1;
        this.numChannels = numChannels;
        this.numStages = numStages;

        bufferLeft = new float[bufferSize];
        bufferRight = new float[bufferSize];

        readPointerLeft = new float[bufferSize];
        readPointerRight = new float[bufferSize];

        delays = new float[bufferSize];

        System.Random random = new System.Random();

        for(int i=1; i<numChannels+1; i++)
        {
            //delays[i] = Mathf.Lerp(0,msInSamples,(float)i/(float)(numChannels));
            delays[i] = random.Next((int)msInSamples);
        }
    }


    public void Process(float[] input, float[] output)
    {
        int n = input.Length;
        float[] channelBufferLeft = new float[numChannels];
        float[] channelBufferRight = new float[numChannels];

        

        for (int i = 0; i < n; i+=2)
        {
            // delay
            for(int j = 0; j < numChannels; j++)
            {
                channelBufferLeft[j] = 0;
                channelBufferRight[j]=0;

                readPointerLeft[j] = writePointer - delays[j];
                while (readPointerLeft[j] < 0) readPointerLeft[j] += bufferSize;

                readPointerRight[j] = writePointer - delays[j];
                while (readPointerRight[j] < 0) readPointerRight[j] += bufferSize;

                channelBufferLeft[j] += bufferLeft[(int)readPointerLeft[j]];
                channelBufferRight[j] += bufferRight[(int)readPointerRight[j]];

                
            }

            bufferLeft[writePointer] = input[i]*0.7f + input[i+1]*0.3f;
            bufferRight[writePointer] = input[i + 1] * 0.7f + input[i]*0.3f;

            writePointer++;
            if (writePointer >= bufferSize) writePointer -= bufferSize;


            // shuffling + polarity
            (channelBufferLeft[0], channelBufferLeft[3]) = (channelBufferLeft[3],-channelBufferLeft[0]);
            (channelBufferLeft[1], channelBufferLeft[4]) = (-channelBufferLeft[1], channelBufferLeft[4]);
            (channelBufferLeft[2], channelBufferLeft[7]) = (channelBufferLeft[7], -channelBufferLeft[2]);
            (channelBufferLeft[5], channelBufferLeft[6]) = (-channelBufferLeft[5], -channelBufferLeft[5]);

            (channelBufferRight[0], channelBufferRight[3]) = (channelBufferRight[3], -channelBufferRight[0]);
            (channelBufferRight[1], channelBufferRight[4]) = (-channelBufferRight[1], channelBufferRight[4]);
            (channelBufferRight[2], channelBufferRight[7]) = (channelBufferRight[7], -channelBufferRight[2]);
            (channelBufferRight[5], channelBufferRight[6]) = (-channelBufferRight[5], -channelBufferRight[5]);

            // mixing matrix
            output[i] = channelBufferLeft[0] * 0.34f +
                -channelBufferRight[1] * 0.65f +
                channelBufferLeft[2] * 0.49f +
                channelBufferLeft[3] * 0.56f +
                -channelBufferLeft[4] * 0.23f +
                channelBufferLeft[5] * 0.31f +
                -channelBufferRight[6] * 0.43f +
                channelBufferLeft[7] * 0.56f;

            output[i+1] = channelBufferRight[0] * 0.34f +
                channelBufferRight[1] * 0.65f +
                channelBufferLeft[2] * 0.49f +
                -channelBufferRight[3] * 0.56f +
                channelBufferLeft[4] * 0.23f +
                -channelBufferLeft[5] * 0.31f +
                channelBufferRight[6] * 0.43f +
                -channelBufferRight[7] * 0.56f;


        }
    }
}


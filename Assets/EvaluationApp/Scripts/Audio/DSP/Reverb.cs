using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverb : MonoBehaviour
{

    public RealisticReverb reverb = new RealisticReverb(8,4);

    [Range(0, 1)] public float dry = 1;
    [Range(0, 1)] public float wet = 1;

    [Range(0, 1)] public float feedback = 0.5f;
    [Range(0, 1)] public float feedbackCutoff = 0.5f;

    [Range(0, 50)] public float diffDelaysMin = 20, diffDelaysMax=40;
    [Range(0, 1000)] public float revDelaysMin = 200, revDelaysMax = 500;

    [Range(0,1)] public float diffuserOutputCutoff = 1;

    [Range(0, 4)] public int numDiffusionStages = 4;

    public bool enableDiffusion = true;
    public bool enableReverb = true;

    private void Update()
    {
        reverb.dry = dry;
        reverb.wet = wet;

        reverb.delaynetwork.setFeedback(feedback);
        reverb.delaynetwork.setCutoffFrequencies(feedbackCutoff);
        reverb.diffuser.setDelays(diffDelaysMin, diffDelaysMax);
        reverb.delaynetwork.setDelays(revDelaysMin, revDelaysMax);

        reverb.enableDiffusion = enableDiffusion;
        reverb.enableReverb = enableReverb;
        reverb.diffuser.SetOtputCutoffFrequency(diffuserOutputCutoff);

        reverb.diffuser.numStages = numDiffusionStages;
    }


    private void OnAudioFilterRead(float[] data, int channels)
    {
        reverb.Process(data);
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

public class RealisticReverb
{

    public int numChannels;

    public Diffuser diffuser;
    public DelayNetwork delaynetwork;

    public float dry = 1;
    public float wet = 1;
    public float delaySmoothing = 0.999f;

    public bool enableDiffusion = true;
    public bool enableReverb = true;



    public RealisticReverb(int numChannels, int numDiffusionSteps)
    {
        this.numChannels = numChannels;
        diffuser = new Diffuser(numChannels, numDiffusionSteps);
        delaynetwork = new DelayNetwork(numChannels,48000);
    }

    public void Process(float[] buffer)
    {
        float[] channels = new float[numChannels];
        for(int i=0; i< buffer.Length; i+=2)
        {

            float inputLeft = buffer[i];
            float inputRight = buffer[i+1];

            for(int j=0; j<numChannels; j++)
            {
                //channels[j] = buffer[i]+buffer[i+1] * 0.5f;
                if (j<numChannels/2) channels[j] = buffer[i];
                else channels[j] = buffer[i+1];
            }

            if(enableDiffusion) diffuser.Process(channels);
            if(enableReverb) delaynetwork.Process(channels);


            buffer[i] = 0;
            buffer[i+1] = 0;
            for (int j = 0; j < numChannels; j++)
            {
                if (j < numChannels / 2) buffer[i] = 0.5f*channels[j]+ 0.5f * channels[j+4];
                else buffer[i+1] = 0.5f * channels[j] + 0.5f * channels[j -4];
                //buffer[i]+= channels[j];
                //buffer[i+1] += channels[j];
            }

            buffer[i] = inputLeft * dry + buffer[i] * wet;
            buffer[i+1] = inputRight * dry + buffer[i+1] * wet;
        }
    }
}

public class DelayNetwork
{
    public int numChannels;

    public float[] delays;
    public float[] smoothedDelays;
    

    public float[] feedbacks;
    public float[] cutoffFrequencies;

    private float[,] buffer;
    private int bufferSize;
    private int writePointer;
    private float[] readPointers;

    float[] outputBuffer;
    float[] output;
    float[] feedbackBuffer;

    private FirstOrderLowpass[] lowpasses;
    private FirstOrderLowpass lowpassInput;

    private float[] delayOffsets;

    public float delaySmoothing;



    public DelayNetwork(int numChannels, int maxDelays)
    {
        this.numChannels = numChannels;

        this.delays = new float[numChannels];
        this.smoothedDelays = new float[numChannels];
        this.feedbacks = new float[numChannels];
        this.cutoffFrequencies = new float[numChannels];
        this.buffer = new float[numChannels,maxDelays];

        this.readPointers = new float[numChannels];
        outputBuffer = new float[numChannels];
        output = new float[numChannels];
        feedbackBuffer = new float[numChannels];
        lowpasses = new FirstOrderLowpass[numChannels];

        bufferSize = maxDelays;

        delayOffsets = new float[numChannels];

        

        System.Random random = new System.Random();
        for (int i = 0; i < numChannels; i++)
        {
            lowpasses[i] = new FirstOrderLowpass();
            delayOffsets[i] = Mathf.Pow((float)i/(float)(numChannels-1),1.2f);
            feedbacks[i] = 0.99f;
            lowpasses[i].cutoffFrequency = 0.9f;
        }
    }

    public void setFeedback(float f)
    {
        for (int i = 0; i < numChannels; i++)
        {
            feedbacks[i] = f;
        }
    }

    public void setCutoffFrequencies(float fc)
    {
        for (int i = 0; i < numChannels; i++)
        {
            lowpasses[i].cutoffFrequency = fc;
        }
    }

    public void setDelays(float min, float max)
    {
        System.Random random = new System.Random();
        for (int i = 0; i < numChannels; i++)
        {

            delays[i] = Mathf.Lerp(min*48,max*48, delayOffsets[i]);
        }
        Debug.Log(delays[0] + ", " + delays[1] + ", " + delays[2] + ", " + delays[3] + ", " + delays[4] + ", " + delays[5] + ", " + delays[6] + ", " + delays[7]);
    }

    public void Process(float[] input)
    {
       
        
        for (int i=0; i<numChannels; i++)
        {
            smoothedDelays[i] = delaySmoothing * smoothedDelays[i] + (1 - delaySmoothing) * delays[i];

            readPointers[i] = writePointer - smoothedDelays[i];
            while (readPointers[i] < 0) readPointers[i] += bufferSize;

            output[i] = buffer[i, (int)readPointers[i]];

            buffer[i, writePointer] = lowpasses[i].Process(outputBuffer[i]) * feedbacks[i] + input[i];

            input[i] = (input[i]+output[i])*0.5f;
        }

        /*
        outputBuffer[0] = buffer[0, (int)readPointers[0]] + buffer[5, (int)readPointers[5]] - buffer[3, (int)readPointers[3]] + buffer[6, (int)readPointers[6]];
        outputBuffer[1] = buffer[4, (int)readPointers[4]] + buffer[2, (int)readPointers[2]] - buffer[7, (int)readPointers[7]] + buffer[0, (int)readPointers[0]];
        outputBuffer[2] = buffer[2, (int)readPointers[2]] + buffer[6, (int)readPointers[7]] - buffer[4, (int)readPointers[4]] + buffer[5, (int)readPointers[5]];
        outputBuffer[3] = buffer[6, (int)readPointers[6]] + buffer[3, (int)readPointers[3]] - buffer[5, (int)readPointers[5]] + buffer[1, (int)readPointers[1]];
        outputBuffer[4] = buffer[2, (int)readPointers[2]] + buffer[6, (int)readPointers[6]] - buffer[1, (int)readPointers[1]] + buffer[4, (int)readPointers[4]];
        outputBuffer[5] = buffer[1, (int)readPointers[1]] + buffer[4, (int)readPointers[4]] - buffer[0, (int)readPointers[0]] + buffer[5, (int)readPointers[5]];
        outputBuffer[6] = buffer[5, (int)readPointers[5]] + buffer[7, (int)readPointers[7]] - buffer[2, (int)readPointers[2]] + buffer[0, (int)readPointers[0]];
        outputBuffer[7] = buffer[7, (int)readPointers[7]] + buffer[4, (int)readPointers[4]] - buffer[5, (int)readPointers[5]] + buffer[2, (int)readPointers[2]];
        */

        float sum = 0;
        for (int i = 0; i < numChannels; i++)
        {
            sum += buffer[i, (int)readPointers[i]];
        }
        sum *= 2f / numChannels;

        for(int i = 0; i < numChannels; i++)
        {
            outputBuffer[i] = sum;
        }

        writePointer++;
        if (writePointer >= bufferSize) writePointer -= bufferSize;


    }
}

public class FirstOrderLowpass
{
    public float cutoffFrequency;
    private float z=0;

    public float Process(float input)
    {
        float output = (1 - cutoffFrequency) * z + cutoffFrequency * input;
        z = output;
        return output;
    }


}




public class Diffuser
{

    public int numStages;
    public int numChannels;

    public DiffusionStep[] diffusionSteps;

    public BiquadSingleChannel[] outputLowpasses;


    public Diffuser(int numChannels, int numStages)
    {
        this.numChannels=numChannels;
        this.numStages=numStages;

        diffusionSteps = new DiffusionStep[numStages];
        outputLowpasses = new BiquadSingleChannel[numChannels];
        for(int i=0; i<numStages; i++)
        {
            diffusionSteps[i] = new DiffusionStep(numChannels,100*(i+1),i);
        }

        float[] coeffs = BiquadCalculator.CalculateCoefficients(BiquadType.Lowpass,0.3f,0.7f,0);
        for(int i=0; i<numChannels; i++)
        {
            outputLowpasses[i] = new BiquadSingleChannel();
            outputLowpasses[i].SetCoeffs(coeffs);
        }
    }

    public void setDelays(float min, float max)
    {
        for(int i=0; i<numStages; i++)
        {
            diffusionSteps[i].setDelays(min,max,i);
        }
    }

    public void SetOtputCutoffFrequency(float fc)
    {
        float[] coeffs = BiquadCalculator.CalculateCoefficients(BiquadType.Lowpass, fc, 0.7f, 0);
        for (int i = 0; i < numChannels; i++)
        {
            outputLowpasses[i].SetCoeffs(coeffs);
        }
        Debug.Log(coeffs[0]+", "+ coeffs[1] + ", " + coeffs[2] + ", " + coeffs[3] + ", " + coeffs[4]);
    }


    public void Process(float[] input)
    {
        for(int i=0; i<numStages; i++)
        {
            diffusionSteps[i].Process(input);
        }

        for(int i=0; i<numChannels; i++)
        {
            input[i] = outputLowpasses[i].Process(input[i]);
        }
    }



}

public class DiffusionStep
{

    public int numChannels;


    int writePointer;
    float[] readPointer;
    float[,] buffer;
    int bufferSize;
    public float[] delays;
    public float[] smoothedDelays;
    public float[] delayOffsets;

    float[] outputBuffer;

    public float delaySmoothing = 0.999f;

    private Hadamart hadamart=new Hadamart();

    public DiffusionStep(int numChannels, int maxDelay, int index)
    {
        this.numChannels = numChannels;
        float msInSamples = maxDelay * 48;
        bufferSize = (int)msInSamples + 1;
        buffer = new float[numChannels,bufferSize];
        readPointer = new float[numChannels];
        delays = new float[numChannels];
        smoothedDelays = new float[numChannels];
        delayOffsets = new float[numChannels];
        outputBuffer = new float[numChannels];

        System.Random random = new System.Random();
        for (int i = 0; i < numChannels; i++)
        {
            delayOffsets[i] = (float)random.Next(10000)*0.0001f;
            delayOffsets[i] = Mathf.Pow((float)(i+1) / (numChannels),2);
        }

        setDelays(0, maxDelay,index);
    }

    public void setDelays(float min, float max, int i)
    {

        for (int j = 0; j < numChannels; j++)
        {
            delays[j] = Mathf.Lerp(min*48,max*(i+1)*48,delayOffsets[j]);
            
        }
        //Debug.Log(delays[0]+","+ delays[1] + "," + delays[2] + "," + delays[3] + "," + delays[4] + "," + delays[5] + "," + delays[6] + "," + delays[7]);
    }

    public void Process(float[] inputBuffer)
    {

        for (int j = 0; j < numChannels; j++)
        {
            smoothedDelays[j] = delaySmoothing * smoothedDelays[j] + (1 - delaySmoothing) * delays[j];
            buffer[j, writePointer] = inputBuffer[j];

            readPointer[j] = writePointer - smoothedDelays[j];
            while ((int)readPointer[j] < 0) readPointer[j] += bufferSize;

            outputBuffer[j] = buffer[j,(int)readPointer[j]];
        }

        

        writePointer++;
        if (writePointer >= bufferSize) writePointer -= bufferSize;



        
        outputBuffer[0] = inputBuffer[0] + inputBuffer[5] - inputBuffer[3] + inputBuffer[6];
        outputBuffer[1] = inputBuffer[4] + inputBuffer[2] - inputBuffer[7] + inputBuffer[1];
        outputBuffer[2] = inputBuffer[2] + inputBuffer[6] - inputBuffer[4] + inputBuffer[1];
        outputBuffer[3] = inputBuffer[6] + inputBuffer[3] - inputBuffer[6] + inputBuffer[1];
        outputBuffer[4] = inputBuffer[2] + inputBuffer[1] - inputBuffer[1] + inputBuffer[4];
        outputBuffer[5] = inputBuffer[1] + inputBuffer[4] - inputBuffer[0] + inputBuffer[5];
        outputBuffer[6] = inputBuffer[5] + inputBuffer[0] - inputBuffer[2] + inputBuffer[0];
        outputBuffer[7] = inputBuffer[3] + inputBuffer[4] - inputBuffer[5] + inputBuffer[2];
        
        
        //hadamart.Process(outputBuffer);

        for(int i=0; i < numChannels; i++)
        {
            inputBuffer[i] = outputBuffer[i];
        }

    }

    public class Hadamart
    {
        public float[] output;

        float factor = Mathf.Sqrt(1f/8f);

        public void RecursiveScale(float[] data, int offset, int size)
        {
            if (size<=1) return;
            int hsize = size / 2;

            RecursiveScale(data, 0, hsize);
            RecursiveScale(data, hsize, hsize);

            for(int i=0; i< hsize; i++)
            {
                float a = data[i+offset];
                float b = data[i + hsize+offset];
                data[i + offset] = a + b;
                data[i + hsize + offset] = a - b;
            }
        }


        public void Process(float[] input)
        {
            RecursiveScale(input, 0,input.Length);

            for(int i=0; i<input.Length; i++)
            {
                input[i] *= factor;
            }
        }

    }


}


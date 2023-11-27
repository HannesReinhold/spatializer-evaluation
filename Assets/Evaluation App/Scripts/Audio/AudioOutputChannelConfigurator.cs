using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioOutputChannelRouter : MonoBehaviour
{
    private int outputChannel;
    public void SetOutputChannel(int channel)
    {
        outputChannel = channel;
    }

    public int GetOutputChannel()
    {
        return outputChannel;
    }


    void OnAudioFilterRead(float[] data, int numChannels)
    {
        RouteToChannel(data, numChannels, outputChannel);
    }

    private void RouteToChannel(float[] data, int numChannels, int channelID)
    {
        for (int i = 0; i < data.Length; i += numChannels)
        {
            for (int channel = 0; channel < numChannels; channel++)
            {
                data[i + channel] *= channel == channelID ? 1 : 0;
            }
        }
    }
}

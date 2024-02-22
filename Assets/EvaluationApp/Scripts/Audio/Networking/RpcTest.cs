using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpcTest : MonoBehaviour
{
    public AudioSettingsEvent settings;

    int id = 0;
    public void Switch(int i)
    {
        id++;
        //settings.SetSettings(id,0);

    }
}

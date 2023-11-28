using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GUIAudioManager
{
    public static void PlayMenuOpen(Vector3 pos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Menu_Open",pos);
    }
}

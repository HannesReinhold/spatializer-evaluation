using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class GUIAudioManager
{
    public static void PlayMenuFirstOpen(Vector3 pos)
    {

    }

    public static void PlayMenuOpen(Vector3 pos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Menu_Open",pos);
    }
    public static void PlayMenuNext(Vector3 pos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/WindowOpen", pos);
    }

    public static void PlayMenuSubmit(Vector3 pos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Submit", pos);
    }

    public static void PlayMenuStart(Vector3 pos)
    {

    }


}

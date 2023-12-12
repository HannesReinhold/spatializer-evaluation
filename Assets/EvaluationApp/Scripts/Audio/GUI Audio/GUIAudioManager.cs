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

    public static void PlaySelect(Vector3 pos)
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Select", pos);
    }

    public static void SetAmbientVolume(float v)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("AmbientVolume",v);
    }


}

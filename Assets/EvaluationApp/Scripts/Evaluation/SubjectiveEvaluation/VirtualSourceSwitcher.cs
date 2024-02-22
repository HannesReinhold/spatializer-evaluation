using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualSourceSwitcher : MonoBehaviour
{
    public List<FMODUnity.StudioEventEmitter> emitters; 


    public void HideAll()
    {
        for(int i=0; i< emitters.Count; i++)
        {
            emitters[i].gameObject.SetActive(false);    
        }
    }

    public void SetOutput(int index)
    {
        for(int j=0; j<emitters.Count; j++) 
        {
            emitters[j].gameObject.SetActive(j==index);
            emitters[j].SetParameter("Volume",j==index?1:0);
        }
    }
}

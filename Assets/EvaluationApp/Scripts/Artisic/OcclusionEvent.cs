using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionEvent : MonoBehaviour
{
    public EmergingWall wall1;
    public EmergingWall wall2;

    public AudioPhenomena audioPhenomena;




    void Start()
    {
    }


    void Update()
    {
        
    }

    private void OnEnable()
    {
        wall1.Disable();
        wall2.Disable();

        Invoke("ShowWall1",2);
        Invoke("ShowWall2", 4);

        audioPhenomena.EnableOcclusion();
    }

    private void OnDisable()
    {
        
    }

    public void OnComplete()
    {
        HideWall1();
        HideWall2();
    }

    private void ShowWall1()
    {
        wall1.Appear();
    }

    private void HideWall1()
    {
        wall1.Disappear();
    }

    private void ShowWall2()
    {
        wall2.Appear();
    }

    private void HideWall2()
    {
        wall2.Disappear();
    }
}


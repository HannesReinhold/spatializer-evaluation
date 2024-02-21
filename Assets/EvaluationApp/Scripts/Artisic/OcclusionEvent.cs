using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OcclusionEvent : MonoBehaviour
{
    public MainIntroductionManager mainIntroductionManager;

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

        Invoke("ShowWall1",0.5f);
        Invoke("ShowWall2", 1.5f);

        audioPhenomena.EnableOcclusion();
    }

    private void OnDisable()
    {
        
    }

    public void OnComplete()
    {
        HideWall1();
        HideWall2();
        Invoke("Disable",3);
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

    private void Disable()
    {
        mainIntroductionManager.StartEvent(8);
    }
}


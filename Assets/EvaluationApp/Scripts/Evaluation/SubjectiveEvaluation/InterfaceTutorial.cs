using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceTutorial : MonoBehaviour
{

    public List<Hint> hints = new List<Hint>();

    void Start()
    {
        for(int i = 0; i < hints.Count; i++)
        {
            hints[i].CloseHint();
        }
    }

    public void Open()
    {
        Invoke("Open1", 5);
        Invoke("Open2", 10);
        Invoke("Open3", 15);
        Invoke("Open4", 20);
        Invoke("Open5", 25);
        Invoke("Open6", 30);
    }

    private void OnEnable()
    {

    }

    public void Close(int i)
    {
        hints[i].CloseHint();
    }

    public void Open(int i)
    {
        hints[i].OpenHint();
    }


    private void Open1()
    {
        hints[0].OpenHint();
    }

    private void Open2()
    {
        hints[1].OpenHint();
        hints[0].HideHint();
    }

    private void Open3()
    {
        hints[2].OpenHint();
        hints[1].HideHint();
    }

    private void Open4()
    {
        hints[3].OpenHint();
        hints[2].HideHint();
    }

    private void Open5()
    {
        hints[4].OpenHint();
        hints[3].HideHint();
    }

    private void Open6()
    {
        hints[5].OpenHint();
        hints[4].HideHint();
    }


}

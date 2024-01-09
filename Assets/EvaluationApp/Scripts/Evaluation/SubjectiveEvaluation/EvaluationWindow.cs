using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class EvaluationWindow : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI minValueText;
    public TextMeshProUGUI maxValueText;
    public TextMeshProUGUI currentValueText;

    public AudioSync sync;

    private SubjectiveEvaluationRound roundManager;

    public RectTransform maskTransform;
    public RectTransform textTransform;

    [Range(0,1)]public float a=1;
    private float targetA = 0;
    private bool descriptionOpen = true;

    public GameObject closeImage;
    public GameObject openImage;

    public void SetRoundManager(SubjectiveEvaluationRound roundManager)
    {
        this.roundManager = roundManager;
        SetupText();
    }

    public void Submit()
    {
        roundManager.SaveRound();
    }

    private void SetupText()
    {
        
    }
    private void Update()
    {
        maskTransform.localPosition = new Vector3(0, -1520+a*440, 0);
        textTransform.localPosition = new Vector3(0, 100+a * 440, 0);

        a = Mathf.Lerp(a,targetA,0.1f);
    }

    private void Start()
    {
        OpenDescription();
    }

    public void OpenDescription()
    {
        targetA = 1;
        closeImage.SetActive(true);
        openImage.SetActive(false);
        Debug.Log("Open desc");
    }

    public void CloseDescription()
    {
        targetA = 0;
        closeImage.SetActive(false);
        openImage.SetActive(true);
        Debug.Log("Close desc");
    }

    public void ToggleDescription()
    {
        descriptionOpen = !descriptionOpen;

        if (descriptionOpen) OpenDescription();
        else CloseDescription();
    }
    
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubjectiveEvaluationRound : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI minText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI aspectText;

    public AudioSync sync;

    public ToggleGroup spatializerSwitch;
    public Slider ratingSlider;

    private SubjectiveEvaluationPartData partData;
    private ConcreteSubjectiveEvaluation roundData;
    public SubjectiveEvaluationManager manager;
    private WindowManager windowManager;

    public void UpdateInterface(SubjectiveEvaluationPartData data, int roundID)
    {
        this.partData = data;
        this.roundData = data.evaluations[roundID];

        nameText.text = data.name + " - "+GameManager.Instance.dataManager.spatializerData.spatializerInfo[roundData.comparisonSpatializerID].shortName;
        descriptionText.text = data.description;
        aspectText.text = "This part compares the aspect: "+data.comparisonAspect;
        questionText.text = data.question;
        minText.text = data.minValue; 
        maxText.text = data.maxValue;

    }

    private void Start()
    {
        windowManager = GetComponent<WindowManager>();
    }

    public void SaveRound()
    {

    }

    public void StartRound(bool nextAspect)
    {
        //sync.SetAudioOutput(false,);


        if (nextAspect) windowManager.OpenPage(0);
        else
        {
            windowManager.OpenPage(1);
            sync.PlaySound();
        }

    }

    public void UpdateRatingValue()
    {
        roundData.value = ratingSlider.value;
    }


    public void ToggleSpatializer(bool real)
    {
        sync.SetAudioOutput(real,roundData.comparisonSpatializerID,0);
    }

}

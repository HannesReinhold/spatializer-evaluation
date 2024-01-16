using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SubjectiveEvaluationRound : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI minText;
    public TextMeshProUGUI maxText;
    public TextMeshProUGUI aspectText;

    public SubjectiveAudioSwitch audioSwitch;

    public ToggleGroup spatializerSwitch;
    public UnityEngine.UI.Slider ratingSlider;

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

    FMOD.Studio.Bus bus;

    private void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/MainSounds");
        bus.setVolume(1);
        windowManager = GetComponent<WindowManager>();
        StartRound(false);

    }

    public void SaveRound()
    {
        audioSwitch.Stop();
    }

    public void StartRound(bool nextAspect)
    {
        //sync.SetAudioOutput(false,);
        audioSwitch.SetAll(true,roundData.speakerID, roundData.comparisonSpatializerID);

        if (nextAspect) windowManager.OpenPage(0);
        else
        {
            windowManager.OpenPage(1);
            
        }
        
        audioSwitch.Play(partData.fileID);

    }

    public void UpdateRatingValue()
    {
        roundData.value = ratingSlider.value;
    }


    public void ToggleSpatializer(bool real)
    {
        audioSwitch.SetReal(real?1:0);
    }

    public void SetSpeaker(int id)
    {
        audioSwitch.SetSpeaker(id);
    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationManager : MonoBehaviour
{
    public List<GameObject> MenuPages;

    //public SubjectiveStartDialog Dialog;
    public SubjectiveEvaluationInterface1 EvaluationInterface;

    public int currentEvaluationIndex = 0;

    public SubjectiveEvaluationData evaluationData;



    //public SpatializerSwitcher spatializerSwitcher;

    private void Awake()
    {

        toggle = GetComponent<ToggleGroup>();
    }

    private void OnEnable()
    {
        SetEvaluationMenuState(0);
    }

    public void EnableAudioSource(bool enable)
    {
        //spatializerSwitcher.gameObject.SetActive(enable);
    }


    //public Loudspeaker Speaker1;
    //public Loudspeaker Speaker2;

    ToggleGroup toggle;


    

    public void UpdateToggle(int i)
    {

        SwitchDirect(i*100);
    }

    public void StartSpeakers(int i)
    {
        //Speaker1.SetActive(true);
        //Speaker1.SetSpatializer(6);
        //Speaker2.SetActive(true);
        //Speaker2.SetSpatializer(i);
    }
    public void StopSpeakers()
    {
        //Speaker1.SetActive(false);
        //Speaker2.SetActive(false);
    }

    public void SwitchDirect(float vol)
    {
        //AudioSource audioEvent1 = Speaker1.switcher.UnitySource.GetComponent<AudioSource>();
        //audioEvent1.volume = (1f - vol / 100f);
        //FMODUnity.StudioEventEmitter audioEvent2 = Speaker2.GetActiveEmitter();
        //audioEvent2.EventInstance.setVolume(vol / 100f);
    }



    public void SetEvaluationMenuState(int i)
    {
        for(int j = 0; j < MenuPages.Count; j++)
        {
            MenuPages[j].SetActive(i==j);
        }

        switch (i)
        {
            case 0:
                //EnableAudioSource(false);
                StopSpeakers();
                break;
            case 1:
                //EnableAudioSource(true);
                StartSpeakers(evaluationData.evaluationParts[0].evaluations[currentEvaluationIndex].baseSpatializerID - 1);
                break;
            default:
                //EnableAudioSource(false);
                StopSpeakers();
                break;
        }
        //spatializerSwitcher.SetSource(evaluationData.);
    }

    public void SetupEvaluation()
    {
        //evaluationData = Evaluations[currentEvaluationIndex];
        //Dialog.SetHeader(evaluationData);
        //EvaluationInterface.SetInterface(evaluationData);
        EvaluationInterface.SetEvaluationData(currentEvaluationIndex);

        //spatializerSwitcher.SetSource(Evaluations[currentEvaluationIndex].spatializerID);
    }

    public void SetNextEvaluation()
    {
        currentEvaluationIndex++;
        //evaluationData = Evaluations[currentEvaluationIndex];
        //Dialog.SetHeader(evaluationData);
        //EvaluationInterface.SetInterface(evaluationData);
        EvaluationInterface.SetEvaluationData(currentEvaluationIndex);
    }
}

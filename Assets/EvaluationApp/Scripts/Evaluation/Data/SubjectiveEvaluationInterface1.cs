using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubjectiveEvaluationInterface1 : MonoBehaviour
{
    public TextMeshProUGUI Header;
    public TextMeshProUGUI Description;


    public SubjectiveEvaluationData evaluationData;
    private int currentRoundID = 0;

    public EvaluationManager evaluationManager;

    public Slider slider;

    public TextMeshProUGUI toggleA, toggleB;
    public TextMeshProUGUI sliderMin, sliderMax;



    public void Reset()
    {

    }

    private void OnEnable()
    {
        slider.value = 50;
    }

    public void ShowNextEvaluation(int partID, int roundID)
    {
        currentRoundID = roundID;
        SetInterface(evaluationData.evaluationParts[partID]);
    }

    public void SetInterface(SubjectiveEvaluationPartData evaluationInfo)
    {
        ConcreteSubjectiveEvaluation eval = evaluationInfo.evaluations[currentRoundID];
        

        Header.text = evaluationInfo.name;
        Description.text = evaluationInfo.description;

        sliderMin.text = evaluationInfo.minValue;
        sliderMax.text = evaluationInfo.maxValue;
    }

    public void SetEvaluationData(int index)
    {
        //evaluationData = new SubjectiveEvaluationData(index,"","","");
        //evaluationData.spatializerName = evaluationManager.Evaluations[index].spatializerName;
        //evaluationData.evaluationAspect = evaluationManager.Evaluations[index].evaluationAspect;
        //GameManager.Instance.dataManager.currentSessionData.subjectiveEvaluationResults.Add(evaluationData);
    }

    public void SaveEvaluationData()
    {
        //GameManager.Instance.dataManager.SaveSession();
    }

    public void OnSliderChanged(float value)
    {
        evaluationData.evaluationParts[0].evaluations[0].value = value;
        //GameManager.Instance.dataManager.currentSessionData.subjectiveEvaluationResults
    }

    public void OnNextClicked()
    {
        SaveEvaluationData();
        //if (evaluationManager.currentEvaluationIndex >= evaluationManager.Evaluations.Count-1)
        //{
        //    evaluationManager.SetEvaluationMenuState(2);
            
        //    return;
        //}
        
        
        evaluationManager.SetEvaluationMenuState(0);
        evaluationManager.SetNextEvaluation();
        //SetInterface(evaluationManager.evaluationData);
        slider.value = 50;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectiveEvaluationManager : MonoBehaviour
{

    public GameObject introduction;
    public GameObject tutorial;
    public GameObject evaluationRound;

    private void Start()
    {
        HideAll();
        StartEvalution();
    }


    private void HideAll()
    {
        introduction.SetActive(false);
        tutorial.SetActive(false);
        evaluationRound.SetActive(false);
    }

    
    public void StartEvalution()
    {
        introduction.SetActive(true);
    }

    public void FinishEvaluation()
    {

    }

    public void StartRound()
    {
        tutorial.SetActive(false);
        evaluationRound.SetActive(true);
    }


    public void SaveData()
    {

    }

    public void StartTutorial()
    {
        tutorial.SetActive(true);
        introduction.SetActive(false);
    }

}

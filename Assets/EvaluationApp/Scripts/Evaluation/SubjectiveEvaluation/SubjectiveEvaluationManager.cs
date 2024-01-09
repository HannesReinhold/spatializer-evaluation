using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectiveEvaluationManager : MonoBehaviour
{

    public GameObject introduction;
    public GameObject tutorial;
    public GameObject evaluationRound;
    public GameObject finish;

    public SubjectiveEvaluationRound roundManager;
    //public SubjectiveEvaluationInterface1 subjectiveEvalInterface;

    public List<GameObject> speakers = new List<GameObject>();

    private int numParts;

    private int roundID = 0;
    private int partID = 0;

    public bool skipTutorial = false;

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
        numParts = GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts.Count;
        if (!skipTutorial) introduction.SetActive(true);
        else { 
            StartRound(); 
            roundID++;
        }
    }

    public void FinishEvaluation()
    {
        evaluationRound.SetActive(false);
        finish.SetActive(true);
    }

    public void StartRound()
    {
        //subjectiveEvalInterface.ShowNextEvaluation(partID, roundID);
        numParts = GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts.Count;
        roundManager.UpdateInterface(GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts[partID], roundID);
        tutorial.SetActive(false);
        evaluationRound.SetActive(true);
    }

    public void NextRound()
    {
        bool nextAspect=false;
        if (roundID >= 4)
        {
            roundID = 0;
            partID++;
            nextAspect = true;
        }

        if (partID>=numParts) FinishEvaluation();
        else {
            roundManager.UpdateInterface(GameManager.Instance.dataManager.spatializerData.subjectiveEvaluationData.evaluationParts[partID], roundID);
            roundManager.StartRound(nextAspect); 
            
        }
        roundID++;
    }



    public void SaveData()
    {

    }

    public void StartTutorial()
    {
        tutorial.SetActive(true);
        introduction.SetActive(false);
    }


    int currentSpeaker = 0;
    public void EmergeSpeakers()
    {
        Invoke("EmergeSpeaker1",1);
        Invoke("EmergeSpeaker2", 2);
        Invoke("EmergeSpeaker3", 3);
        Invoke("EmergeSpeaker4", 4);
        Invoke("EmergeSpeaker5", 5);

        Invoke("DisableHighlighting",8);
    }

    public void DisableHighlighting()
    {
        //speakers[]
        for(int i=0; i<speakers.Count; i++)
        {
            speakers[i].GetComponentInChildren<Hint>().CloseHint();
        }
    }

    private void EmergeSpeaker1()
    {
        LeanTween.moveY(speakers[0], 0, 1).setEaseOutCubic();

    }

    private void EmergeSpeaker2()
    {
        LeanTween.moveY(speakers[1], 0, 1).setEaseOutCubic();

    }

    private void EmergeSpeaker3()
    {
        LeanTween.moveY(speakers[2], 0, 1).setEaseOutCubic();

    }

    private void EmergeSpeaker4()
    {
        LeanTween.moveY(speakers[3], 0, 1).setEaseOutCubic();

    }

    private void EmergeSpeaker5()
    {
        LeanTween.moveY(speakers[4], 0, 1).setEaseOutCubic();

    }

}

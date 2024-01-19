using FMODUnity;
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

    FMOD.Studio.Bus bus;

    private int currentEmergingSpeaker = 0;

    private void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/Ambience");
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
        //GUIAudioManager.SetAmbientVolume(0);


    }

    public void FinishEvaluation()
    {
        evaluationRound.SetActive(false);
        finish.SetActive(true);
        GUIAudioManager.SetAmbientVolume(0.1f);
    }

    public void StartRound()
    {
        //subjectiveEvalInterface.ShowNextEvaluation(partID, roundID);
        GUIAudioManager.SetAmbientVolume(0);
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
        roundManager.SaveRound();
    }

    public void StartTutorial()
    {
        tutorial.SetActive(true);
        introduction.SetActive(false);
    }


    int currentSpeaker = 0;
    public void EmergeSpeakers()
    {
        
        Invoke("EmergeSpeaker",1);
        Invoke("EmergeSpeaker", 2);
        Invoke("EmergeSpeaker", 3);
        Invoke("EmergeSpeaker", 4);
        Invoke("EmergeSpeaker", 5);
        Invoke("EmergeSpeaker", 6);
        
        Invoke("DisableHighlighting",8);
    }

    public void DisableHighlighting()
    {
        for(int i=0; i<speakers.Count; i++)
        {
            speakers[i].GetComponentInChildren<Hint>().CloseHint();
        }
    }

    private void EmergeSpeaker()
    {
        LeanTween.moveY(speakers[currentEmergingSpeaker], 0, 1).setEaseOutCubic();
        speakers[currentEmergingSpeaker].GetComponentInChildren<Hint>().OpenHint();
        currentEmergingSpeaker++;
    }

}

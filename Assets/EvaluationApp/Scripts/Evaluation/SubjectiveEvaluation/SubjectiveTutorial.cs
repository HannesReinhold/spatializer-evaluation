using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubjectiveTutorial : MonoBehaviour
{
    private SubjectiveEvaluationPartData evaluationData = new SubjectiveEvaluationPartData();


    void Start()
    {
        SetupData();
    }


    void Update()
    {
        
    }

    private void SetupData()
    {
        evaluationData.partID = -1;
        evaluationData.name = "Evaluation Tutorial";
        evaluationData.description = "An Introduction on how to use the subjective evaluation interface.";
        evaluationData.question = "Which adio source sounds more realistic?";
        evaluationData.comparisonAspect = "Realism";
        evaluationData.minValue = "A";
        evaluationData.maxValue = "B";
        evaluationData.minValueNumber = 0;
        evaluationData.maxValueNumber = 100;
    }


}

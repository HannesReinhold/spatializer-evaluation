using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubjectiveEvaluationRound : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    private SubjectiveEvaluationPartData data;

    public void SetData(SubjectiveEvaluationPartData data)
    {
        this.data = data;
    }

    public void SaveRound()
    {

    }

}

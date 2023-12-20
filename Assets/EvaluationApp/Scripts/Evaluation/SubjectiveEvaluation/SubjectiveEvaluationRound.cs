using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubjectiveEvaluationRound : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    private SubjectiveEvaluationPartData data;
    private WindowManager windowManager;

    public void SetData(SubjectiveEvaluationPartData data)
    {
        this.data = data;
    }

    private void Start()
    {
        windowManager = GetComponent<WindowManager>();
    }

    public void SaveRound()
    {

    }

    public void StartRound()
    {
        windowManager.OpenPage(0);
    }

    

}

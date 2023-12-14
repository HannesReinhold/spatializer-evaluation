using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroductionManager : MonoBehaviour
{
    public List<GameObject> eventList;
    private int currentEvent;

    public void StartSubjectiveEvaluation()
    {
        GameManager.Instance.StartSubjectiveEvaluation();
    }

    public void StartDirectionGuessingGame()
    {
        GameManager.Instance.StartDirectionGuessing();
    }



    public void StartEvent(int i)
    {
        eventList[currentEvent].SetActive(false);
        currentEvent = i;
        eventList[i].SetActive(true);
    }

}

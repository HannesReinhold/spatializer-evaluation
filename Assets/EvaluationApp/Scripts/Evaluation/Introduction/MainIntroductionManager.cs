using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroductionManager : MonoBehaviour
{
    public List<GameObject> eventList;
    private int currentEvent;


    private void Start()
    {
        ResetEvents();
    }

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

    public void ResetEvents()
    {
        foreach(GameObject e in eventList)
        {
            e.SetActive(false);
        }
    }


}

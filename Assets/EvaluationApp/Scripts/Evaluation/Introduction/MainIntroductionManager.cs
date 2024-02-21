using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroductionManager : MonoBehaviour
{
    public List<GameObject> eventList;
    private int currentEvent;
    public int startEvent;
    public bool hideWindowsOnStart = false;
    public bool eventOnStart = false;

    public PopupWindow additionalInfoWindow;

    public WindowManager windowManager;


    private void OnEnable()
    {
        ResetEvents();
        additionalInfoWindow.gameObject.SetActive(false);

        GUIAudioManager.SetAmbientVolume(0.5f);

        if (hideWindowsOnStart)
        {
            Invoke("HideAllWindows",0.3f);
        }

        if(eventOnStart) StartEvent(startEvent);
    }

    private void HideAllWindows()
    {
        SetAdditionalInfoWindow(false);
        SetMainWindow(false);
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

    public void SetAdditionalInfoWindow(bool visibility)
    {
        if (visibility) additionalInfoWindow.gameObject.SetActive(true);
        if(visibility) additionalInfoWindow.Open();
        else additionalInfoWindow.Close();
    }

    public void SetMainWindow(bool visibility)
    {
        if (visibility) windowManager.windows[windowManager.currentWindowIndex].GetComponent<PopupWindow>().Open();
        else windowManager.windows[windowManager.currentWindowIndex].GetComponent<PopupWindow>().Close();
    }


}

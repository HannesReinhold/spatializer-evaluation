using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowManager : MonoBehaviour
{
    public List<GameObject> windows = new List<GameObject>();

    GameObject currentWindow = null;
    public int currentWindowIndex = 0;
    public int startWindowIndex = 0;

    private void Awake()
    {
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            //NextPage();
        }

        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            //PreviousPage();
        }
    }

    void OnEnable()
    {
        ResetValues();
        currentWindowIndex = startWindowIndex;

        currentWindow = windows[currentWindowIndex];
        currentWindow.SetActive(true);
        //currentWindow.GetComponent<PopupWindow>().Open();

        GUIAudioManager.PlayMenuOpen(transform.position);
    }

    public void ResetValues()
    {
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
    }

    public void ResetSlow()
    {
        if (currentWindow != null) currentWindow.GetComponent<PopupWindow>().Close();
        currentWindowIndex = startWindowIndex;
        currentWindow = windows[currentWindowIndex];
}

    public void NextPage()
    {
        if (currentWindowIndex < windows.Count) currentWindowIndex++;
        if (currentWindow != null) currentWindow.GetComponent<PopupWindow>().Close();
        currentWindow = windows[currentWindowIndex];
        currentWindow.SetActive(true);
    }

    public void PreviousPage()
    {
        if (currentWindowIndex > 0) currentWindowIndex--;
        if (currentWindow != null) currentWindow.GetComponent<PopupWindow>().Close();
        currentWindow = windows[currentWindowIndex];
        currentWindow.SetActive(true);
    }

    public void OpenPage(int pageIndex)
    {
        currentWindowIndex = pageIndex;
        if (currentWindow != null) currentWindow.GetComponent<PopupWindow>().Close();
        currentWindow = windows[pageIndex];
        currentWindow.SetActive(true);
        GUIAudioManager.PlayMenuOpen(transform.position);
        Debug.Log("Open Menu"+currentWindow);

    }

    public void CloseCurrentWindow()
    {
        currentWindow.GetComponent<PopupWindow>().Close();
    }

    public void OpenCurrentWindow()
    {
        currentWindow.GetComponent<PopupWindow>().Open();
    }
}

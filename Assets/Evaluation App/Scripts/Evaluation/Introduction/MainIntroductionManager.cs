using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainIntroductionManager : MonoBehaviour
{

    public List<GameObject> pages = new List<GameObject>();

    GameObject currentPage = null;
    public int currentPageIndex = 0;

    private void Awake()
    {
        Reset();
    }

    void Start()
    {
        OpenPage(0);
    }

    public void Reset()
    {
        foreach(GameObject page in pages)
        {
            page.SetActive(false);
        }
    }

    public void NextPage()
    {
        if(currentPageIndex<pages.Count) currentPageIndex++;
        if (currentPage != null) currentPage.GetComponent<PopupWindow>().Close();
        currentPage = pages[currentPageIndex];
        currentPage.SetActive(true);
    }

    public void PreviousPage()
    {
        if (currentPageIndex > 0) currentPageIndex--;
        if (currentPage != null) currentPage.GetComponent<PopupWindow>().Close();
        currentPage = pages[currentPageIndex];
        currentPage.SetActive(true);
    }

    public void OpenPage(int pageIndex)
    {
        currentPageIndex = pageIndex;
        if(currentPage != null) currentPage.GetComponent<PopupWindow>().Close();
        currentPage = pages[pageIndex];
        currentPage.SetActive(true);

    }

}

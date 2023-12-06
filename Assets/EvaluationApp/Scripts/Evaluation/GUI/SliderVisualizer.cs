using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderVisualizer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Slider slider;
    public TextMeshProUGUI textMesh;
    public PopupWindow popupWindow;

    private bool alreadyOpen = false;
    private float lastValue = 0;

    private void Start()
    {
        popupWindow.Close();
    }

    public void OnHover()
    {
        popupWindow.Open();
    }

    public void OnHoverExit()
    {
        popupWindow.Close();
    }


    void Update()
    {
        textMesh.text = linToDb(slider.value).ToString("F1") + " dB";
    }

    private float linToDb(float input)
    {
        return Mathf.Log10(Mathf.Max(0.00000001f,input))*10;
    }

    private float dbToLin(float input)
    {
        return Mathf.Pow(10,input/10f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnHoverExit();
    }
}

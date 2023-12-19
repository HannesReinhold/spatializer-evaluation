using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderVisualizer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public Slider slider;
    public TextMeshProUGUI textMesh;
    public PopupWindow popupWindow;
    public bool allowDiscreteLabels = false;
    public List<string> discreteLabels = new List<string>() {"Very Different","Rather Different","Neutral","Rather Similar","Very Similar"};


    public string suffix = "dB";

    public bool logScale = true;

    private bool alreadyOpen = false;
    private float lastValue = 0;
    private bool pressed = false;
    private bool hover = false;

    private void Start()
    {
        popupWindow.Close();
        
    }


    public void OnHover()
    {
        popupWindow.Open();
        Debug.Log("open");
    }

    public void OnHoverExit()
    {
        popupWindow.Close();
        Debug.Log("Close");
    }


    void Update()
    {
        float value = logScale ? linToDb(slider.value) : slider.value;
        int labelNum = (int)(Mathf.Min(discreteLabels.Count-1,slider.value/slider.maxValue*discreteLabels.Count));
        string text = allowDiscreteLabels ? discreteLabels[labelNum] : value.ToString("F1") + " " + suffix;
        textMesh.text = text;
        //pressed = Input.GetMouseButtonDown(0);
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
        hover = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!pressed) OnHoverExit();
        hover = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!pressed) return;
        pressed = false;
        if(!hover) OnHoverExit();

    }
}

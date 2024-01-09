using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class PersonalInformationManager : MonoBehaviour
{
    public TMP_InputField ageInputField;
    public ToggleGroup sexToggle;
    public Slider volumeSlider;

    public void SaveAge()
    {
        GameManager.Instance.dataManager.currentSessionData.age = int.Parse(ageInputField.text);
    }

    public void SaveSex()
    {
        var allToggles = sexToggle.GetComponentsInChildren<Toggle>();
        var activeToggle = sexToggle.ActiveToggles().First();

        Gender sex;
        if (allToggles[0] == activeToggle) sex = Gender.Male;
        else if (allToggles[0] == activeToggle) sex = Gender.Female;
        else sex = Gender.Diverse;

        GameManager.Instance.dataManager.currentSessionData.sex = sex;
    }

    public void SaveVolume()
    {
        GameManager.Instance.dataManager.currentSessionData.volume = volumeSlider.value;
    }
}

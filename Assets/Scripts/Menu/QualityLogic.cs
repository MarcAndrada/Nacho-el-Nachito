using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityLogic : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown dropdownQuality;

    [SerializeField]
    int quality;

    // Start is called before the first frame update
    void Start()
    {
        quality = PlayerPrefs.GetInt("qualityNumber", 3);
        dropdownQuality.value = quality;
        AdjustQuality();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustQuality()
    {
        QualitySettings.SetQualityLevel(dropdownQuality.value);
        PlayerPrefs.SetInt("qualityNumber", dropdownQuality.value);
        quality = dropdownQuality.value;
    }
}

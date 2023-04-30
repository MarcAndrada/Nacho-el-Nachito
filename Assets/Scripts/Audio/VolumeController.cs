using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider SFXSlider;

    private void Start()
    {
        // Initialize Music volume value
        if(PlayerPrefs.HasKey("MusicVolume"))
        {
            LoadMusicValue();
        }
        else
        {
            SetMusicVolume();
        }

        // Initialize SFX volume value
        if(PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadSFXValue();
        }
        else
        {
            SetSFXVolume();
        }
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume()
    {
        float SFXVolume = SFXSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    private void LoadMusicValue()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");

        SetMusicVolume();
    }

    private void LoadSFXValue()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetSFXVolume();
    }
}

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
    private Slider masterSlider;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider SFXSlider;

    private void Start()
    {
        // Initialize Master volume value
        if(PlayerPrefs.HasKey("MasterValue"))
        {
            LoadMasterValue();
        }
        else
        {
            SetMasterVolume();
        }

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

    public void SetMasterVolume()
    {
        float masterVolume = masterSlider.value;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(masterVolume) * 30);
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
    }

    public void SetMusicVolume()
    {
        float musicVolume = musicSlider.value;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 30);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume()
    {
        float SFXVolume = SFXSlider.value;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolume) * 30);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    private void LoadMasterValue()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");

        SetMasterVolume();
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

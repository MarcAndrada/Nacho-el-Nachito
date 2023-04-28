using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    [SerializeField]
    private AudioMixer mixer;

    private AudioSource[] actionsAS;
    [SerializeField]
    private int totalActionsAS;
    [SerializeField]
    GameObject actionsASObj;

    private void Awake()
    {
        if (_instance != null)
        {
            if (_instance != this)
            {
                Destroy(gameObject);
            }
            
        }

        _instance = this;

    }

    private void Start()
    {
        actionsAS = new AudioSource[totalActionsAS];
        AudioMixerGroup mixerGroup = mixer.FindMatchingGroups("SFX")[0];
        for (int i = 0; i < totalActionsAS; i++)
        {
            actionsAS[i] = actionsASObj.AddComponent<AudioSource>();
            actionsAS[i].playOnAwake = false;
            actionsAS[i].outputAudioMixerGroup = mixerGroup;
        }
    }

    private void Update()
    {
        int unusedAS = 0;

        foreach (AudioSource item in actionsAS)
        {
            if (!item.isPlaying)
            {
                unusedAS++;
            }
        }

        Debug.Log(unusedAS);
    }

    public AudioSource GetUnusedAS() 
    {
        foreach (AudioSource item in actionsAS)
        {
            if (!item.isPlaying)
            {
                return item;
            }
        }

        return null;
    }

    public void PlayOneShotSound(AudioClip _clip, float _minPitch = 1, float _maxPitch = 1, float _volume = 1)
    {
        AudioSource _as = GetUnusedAS();
        if (_as != null)
        {
            _as.loop = false;
            _as.pitch = Random.Range(_minPitch, _maxPitch);
            _as.volume = _volume;
            _as.PlayOneShot(_clip);

        }
    }

    public AudioSource PlayLoopSound(AudioClip _clip, float _minPitch = 1, float _maxPitch = 1, float _volume = 1)
    {
        AudioSource _as = GetUnusedAS();

        _as.loop = true;
        if (_as != null)
        {
            _as.loop = true;
            _as.pitch = Random.Range(_minPitch, _maxPitch);
            _as.volume = _volume;
            _as.clip = _clip;
            _as.Play();
        }

        return _as;
    }


    public void StopLoopSound(AudioSource _as) 
    {
        _as.Stop();
    }



}

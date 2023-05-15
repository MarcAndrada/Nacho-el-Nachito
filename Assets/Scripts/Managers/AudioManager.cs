using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    [SerializeField]
    private AudioMixer mixer;

    private AudioSource[] actions2dAS;
    private List<AudioSource> actions3dAS;
    [SerializeField]
    private int total2DAS; 
    [SerializeField]
    GameObject actions2dASObj; 

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

        actions2dAS = new AudioSource[total2DAS];
        actions3dAS = new List<AudioSource>();
    }

    private void Start()
    {
        
        AudioMixerGroup mixerGroup = mixer.FindMatchingGroups("SFX")[0];
        for (int i = 0; i < total2DAS; i++)
        {
            actions2dAS[i] = actions2dASObj.AddComponent<AudioSource>();
            actions2dAS[i].playOnAwake = false;
            actions2dAS[i].outputAudioMixerGroup = mixerGroup;
        }
    }

    public AudioSource GetUnused2dAS() 
    {
        foreach (AudioSource item in actions2dAS)
        {
            if (!item.isPlaying)
            {
                return item;
            }
        }

        return null;
    }
    public AudioSource GetUnused3dAS()
    {
        foreach (AudioSource item in actions3dAS)
        {
            if (!item.isPlaying)
            {
                return item;
            }
        }

        return null;
    }
    public void AddNew3dAS() 
    {
        GameObject _obj = new GameObject();
        AudioMixerGroup mixerGroup = mixer.FindMatchingGroups("SFX")[0];

        actions3dAS.Add(_obj.AddComponent<AudioSource>());
        _obj.transform.parent = transform;
        actions3dAS[actions3dAS.Count - 1].playOnAwake = false;
        actions3dAS[actions3dAS.Count - 1].outputAudioMixerGroup = mixerGroup;
        actions3dAS[actions3dAS.Count - 1].spatialBlend = 1;
        actions3dAS[actions3dAS.Count - 1].rolloffMode = AudioRolloffMode.Linear;
    }

    public void Play2dOneShotSound(AudioClip _clip, float _volume = 1, float _minPitch = 0.75f, float _maxPitch = 1.25f)
    {
        AudioSource _as = GetUnused2dAS();
        PlayOneShotSound(_as, _clip, _minPitch, _maxPitch, _volume);
    }
    public void Play3dOneShotSound(AudioClip _clip, float _radius, Vector2 _pos, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 1)
    {
        AudioSource _as = GetUnused3dAS();
        _as.minDistance = _radius;
        _as.maxDistance = _radius * 5;
        _as.gameObject.transform.position = new Vector3(_pos.x, _pos.y, -10);
        PlayOneShotSound(_as, _clip, _minPitch, _maxPitch, _volume);
    }
    private void PlayOneShotSound(AudioSource _as, AudioClip _clip, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 1)
    {
        if (_as != null)
        {
            _as.loop = false;
            _as.pitch = Random.Range(_minPitch, _maxPitch);
            _as.volume = _volume;
            _as.PlayOneShot(_clip);

        }
    }

    
    public void PlayOneRandomShotSound(AudioClip[] _clips, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 1) 
    {
        Play2dOneShotSound(_clips[Random.Range(0, _clips.Length)], _minPitch, _maxPitch, _volume);
    }

    public AudioSource Play2dLoop(AudioClip _clip, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 0.7f) 
    {
        AudioSource _as = GetUnused2dAS();
        
        PlayLoopSound(_as, _clip, _minPitch, _maxPitch, _volume);

        return _as;
    }
    public AudioSource Play3dLoop(AudioClip _clip, float _radius, Vector2 _pos, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 0.4f) 
    {
        AudioSource _as = GetUnused3dAS();
        _as.minDistance = _radius;
        _as.maxDistance = _radius * 5;
        _as.gameObject.transform.position = new Vector3(_pos.x, _pos.y, -10);
        PlayLoopSound(_as, _clip, _minPitch, _maxPitch, _volume);
        return _as;
    }
    private void PlayLoopSound(AudioSource _as, AudioClip _clip, float _minPitch = 0.75f, float _maxPitch = 1.25f, float _volume = 0.4f)
    {

        _as.loop = true;
        if (_as != null)
        {
            _as.loop = true;
            _as.pitch = Random.Range(_minPitch, _maxPitch);
            _as.volume = _volume;
            _as.clip = _clip;
            _as.Play();
        }

    }
    public void StopLoopSound(AudioSource _as) 
    {
        _as.loop = false;
        _as.clip = null;
        _as.Stop();
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBoss : MonoBehaviour
{
    [SerializeField]
    private AudioClip musicClip;
    private void Awake()
    {
        FindObjectOfType<MusicController>().GetComponent<AudioSource>().clip = musicClip;
        FindObjectOfType<MusicController>().GetComponent<AudioSource>().Play();
    }
}

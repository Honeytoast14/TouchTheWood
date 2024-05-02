using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;

public class SoundPlayer : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] AudioSource bgmAudioSource;
    [SerializeField] AudioSource sfxAudioSource;

    [Header("Audio Clip")]
    public AudioClip bgmMusic;
    public AudioClip buttonClick;
    public AudioClip buttonSelect;

    void Start()
    {
        if (bgmMusic != null)
            PlayerMusic();
    }

    public void PlayerSFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void PlayerMusic()
    {
        bgmAudioSource.clip = bgmMusic;
        bgmAudioSource.Play();
    }
}

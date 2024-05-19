using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class SoundPlayer : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] public AudioSource bgmAudioSource;
    [SerializeField] public AudioSource sfxAudioSource;

    [Header("Audio Clip")]
    public AudioClip CaveMusic;
    public AudioClip ForestMusic;
    public AudioClip buttonClick;
    public AudioClip buttonSelect;
    public AudioClip switchUsed;
    public AudioClip paperSound;
    public AudioClip getItem;
    public AudioClip sortingOre;
    public AudioClip correct;

    void Start()
    {
        if (CaveMusic != null)
        {
            PlayMusic(CaveMusic);
        }

        if (ForestMusic != null)
        {
            PlayMusic(ForestMusic);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxAudioSource.PlayOneShot(clip);
    }

    public void StopSFX()
    {
        sfxAudioSource.Stop();
    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();
    }

    public void PlayMusic(AudioClip clip)
    {
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }
        Debug.Log("Playing BGM: " + clip.name);
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }
}

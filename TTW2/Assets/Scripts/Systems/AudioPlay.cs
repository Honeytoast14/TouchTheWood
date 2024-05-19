using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
    }

    public void AudioPlaySound()
    {
        if (aud.enabled)
            aud.Play();
    }
}

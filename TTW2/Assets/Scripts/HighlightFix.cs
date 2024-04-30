using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class HighlightFix : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    [SerializeField] GameObject border;
    SoundPlayer soundPlayer;

    private bool isSelected = false;

    void Start()
    {
        border.SetActive(false);

        soundPlayer = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundPlayer>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        isSelected = true;
        border.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (isSelected)
        {
            isSelected = false;
            border.SetActive(false);
            if (soundPlayer != null)
            {
                soundPlayer.PlayerSFX(soundPlayer.buttonClick);
            }
        }
    }
}

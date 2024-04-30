using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class OptionSetting : MonoBehaviour
{
    public bool optionIsOpen = false;
    [SerializeField] GameObject optionPanel;
    public GameObject musicButton;

    [Header("Music")]
    [SerializeField] AudioMixer musicAudioMixer;
    [SerializeField] Slider musicSlider;
    [Header("SFX")]
    [SerializeField] AudioMixer sfxAudioMixer;
    [SerializeField] Slider sfxSlider;
    GameController gameController;

    // [Header("SFX")]
    // [SerializeField] AudioMixer audioMixer;
    // [SerializeField] Slider slider;
    public static OptionSetting Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        gameController = FindObjectOfType<GameController>();

        gameObject.SetActive(false);
    }

    public void HandleUpdate(Action onBack)
    {
        var titleScene = GameObject.Find("TitleScene");
        if (Input.GetKeyDown(KeyCode.X) && optionIsOpen)
        {
            onBack?.Invoke();
            CloseSetting(optionPanel);
            if (titleScene)
            {
                // SetSelect(gameObject);
                Debug.Log("Test");
            }
        }
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicAudioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolome()
    {
        float volume = sfxSlider.value;
        sfxAudioMixer.SetFloat("SFX", MathF.Log10(volume) * 20);
    }

    public void OpenSetting(GameObject gameObject)
    {
        gameObject.SetActive(true);
        gameController.state = GameState.Setting;
        optionIsOpen = true;
        SetSelect(musicButton);
    }

    public void CloseSetting(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void SetSelect(GameObject selectButton)
    {
        var eventSystem = EventSystem.current;
        selectButton = eventSystem.currentSelectedGameObject;
    }
}

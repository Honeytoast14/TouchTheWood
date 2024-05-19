using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrueFalseSwitch : MonoBehaviour
{
    [Header("All Switches")]
    [SerializeField] List<TrueFalseSwitch> trueFalseSwitches;

    [Header("Wall")]
    [SerializeField] List<GameObject> hideObjects;

    [Header("Sound Wall")]
    [SerializeField] AudioClip sfx;

    SoundPlayer soundPlayer;
    bool soundPlayed = false;

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }

    void Update()
    {
        UpdateWallVisibility();
    }

    void UpdateWallVisibility()
    {
        bool allSwitchesActivated = true;
        foreach (TrueFalseSwitch trueFalseSwitch in trueFalseSwitches)
        {
            if (!trueFalseSwitch.IsSwitchActivated())
            {
                allSwitchesActivated = false;
                break;
            }
        }

        if (allSwitchesActivated && !soundPlayed)
        {
            foreach (TrueFalseSwitch trueFalseSwitch in trueFalseSwitches)
            {
                trueFalseSwitch.SetInputEnabled(false);
            }

            foreach (GameObject hideObject in hideObjects)
            {
                hideObject.SetActive(false);
            }

            soundPlayer.PlaySFX(sfx);
            soundPlayed = true;
        }
    }
}

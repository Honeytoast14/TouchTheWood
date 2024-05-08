using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallTrueFalseSwitch : MonoBehaviour
{
    [Header("All Switches")]
    [SerializeField] List<TrueFalseSwitch> trueFalseSwitches;
    [Header("Wall")]
    [SerializeField] List<GameObject> hideObjects;

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

        if (allSwitchesActivated)
        {
            foreach (TrueFalseSwitch trueFalseSwitch in trueFalseSwitches)
            {
                trueFalseSwitch.SetInputEnabled(false); // Disable input for all switches
            }

            foreach (GameObject hideObject in hideObjects)
            {
                hideObject.SetActive(false);
            }
        }
    }
}

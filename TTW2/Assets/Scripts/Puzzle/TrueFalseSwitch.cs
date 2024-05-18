using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrueFalseSwitch : MonoBehaviour
{
    private bool inZone = false;
    public bool switchActivated { get; private set; } = false;
    private bool propagateReset = true;
    private bool inputEnabled = true;
    SoundPlayer soundPlayer;

    [Header("Switch")]
    [SerializeField] List<TrueFalseSwitch> connectedSwitches;
    [Header("Tile Switch")]
    [SerializeField] Tile initialTile;
    [SerializeField] Tile tileChange;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector3Int position;

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }

    void Update()
    {
        if (inputEnabled && inZone && Input.GetKeyDown(KeyCode.Z))
        {
            if (!switchActivated)
            {
                Activate();
            }
            else
            {
                ResetSwitch();
            }
        }
    }

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;
    }

    public void Activate()
    {
        if (!switchActivated)
        {
            switchActivated = !switchActivated;

            tilemap.SetTile(position, switchActivated ? tileChange : initialTile);
            soundPlayer.PlaySFX(soundPlayer.switchUsed);

            if (propagateReset)
            {
                foreach (TrueFalseSwitch switchScript in connectedSwitches)
                {
                    if (switchScript != null)
                    {
                        switchScript.ToggleSwitchState();
                    }
                }
            }
        }
    }

    public void ResetSwitch()
    {
        if (switchActivated)
        {
            switchActivated = !switchActivated;

            tilemap.SetTile(position, initialTile);
            soundPlayer.PlaySFX(soundPlayer.switchUsed);
            if (propagateReset)
            {
                foreach (TrueFalseSwitch switchScript in connectedSwitches)
                {
                    if (switchScript != null)
                    {
                        switchScript.ResetSwitch();
                    }
                }
            }
        }
    }

    public void ToggleSwitchState()
    {
        if (switchActivated)
        {
            ResetSwitch();
        }
        else
        {
            Activate();
        }
    }

    public bool IsSwitchActivated()
    {
        return switchActivated;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && GameController.Instance.state == GameState.FreeRoam)
        {
            inZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player") && GameController.Instance.state == GameState.FreeRoam)
        {
            inZone = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ParrotPuzzle : MonoBehaviour
{
    SoundPlayer soundPlayer;
    [SerializeField] AudioClip bouceSound;
    [SerializeField] int orderNumber;
    [Header("Tile Switch")]
    [SerializeField] Tile initialTile;
    [SerializeField] Tile tileChange;
    [SerializeField] Tilemap tilemap;
    [SerializeField] Vector3Int position;
    [SerializeField] ParrotManager parrotManager;
    bool inZone = false;
    bool canInteract = true;

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
        tilemap.SetTile(position, initialTile);
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.Z) && inZone && GameController.Instance.state == GameState.FreeRoam)
        {
            soundPlayer.PlaySFX(bouceSound);
            parrotManager.RegisterInteraction(orderNumber);
            ChangeTile();
            parrotManager.AddInteractedPuzzle(this);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (canInteract && collider.gameObject.tag == "Player")
        {
            inZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (canInteract && collider.gameObject.tag == "Player")
        {
            inZone = false;
        }
    }

    public void DisableInteraction()
    {
        canInteract = false;
    }

    private void ChangeTile()
    {
        if (tilemap.GetTile(position) == initialTile)
        {
            tilemap.SetTile(position, tileChange);
        }
    }

    public void ResetTile()
    {
        tilemap.SetTile(position, initialTile);
    }
}

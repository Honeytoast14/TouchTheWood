using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingPuzzleTrigger : MonoBehaviour
{
    [SerializeField] GameObject puzzle;
    [SerializeField] Camera puzzleCam;
    [SerializeField] Camera mainCam;
    // [SerializeField] Texture2D cursor;
    bool inZone = false;

    void Start()
    {
        puzzleCam.enabled = false;
    }

    void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.Z) && GameController.Instance.state == GameState.FreeRoam)
        {
            puzzle.SetActive(true);
            puzzleCam.enabled = true;
            mainCam.enabled = false;
            GameController.Instance.state = GameState.SortingPuzzle;

            // Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (inZone && Input.GetKeyDown(KeyCode.X) && GameController.Instance.state == GameState.SortingPuzzle)
        {
            puzzleCam.enabled = false;
            mainCam.enabled = true;
            GameController.Instance.state = GameState.FreeRoam;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            inZone = false;
        }
    }
}

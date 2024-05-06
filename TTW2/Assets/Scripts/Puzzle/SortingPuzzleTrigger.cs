using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class SortingPuzzleTrigger : MonoBehaviour
{
    [SerializeField] GameObject puzzle;
    [SerializeField] Camera puzzleCam;
    [SerializeField] Camera mainCam;
    [SerializeField] List<GameObject> objectGroup;
    [SerializeField] GameObject doorOpen;
    [SerializeField] GameObject doorClose;
    [SerializeField] GameObject doorColider;
    [SerializeField] GameObject button;
    // [SerializeField] Texture2D cursor;
    bool inZone = false;
    public static SortingPuzzleTrigger Instance { get; private set; }
    TimelineController timelineController;
    IsometricPlayerMovementController playerController;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        puzzleCam.enabled = false;

        playerController = FindObjectOfType<IsometricPlayerMovementController>();
        timelineController = FindObjectOfType<TimelineController>();
    }

    void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.Z) && GameController.Instance.state == GameState.FreeRoam)
        {
            button.SetActive(true);
            puzzle.SetActive(true);
            puzzleCam.enabled = true;
            mainCam.enabled = false;
            GameController.Instance.state = GameState.SortingPuzzle;

            // Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HandleUpdate(Action onBack)
    {
        if (inZone && Input.GetKeyDown(KeyCode.X) && GameController.Instance.state == GameState.SortingPuzzle)
        {
            button.gameObject.SetActive(false);
            puzzleCam.enabled = false;
            mainCam.enabled = true;
            GameController.Instance.state = GameState.FreeRoam;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            onBack?.Invoke();
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

    public void CloseSortingPuzzle()
    {
        GameController.Instance.state = GameState.FreeRoam;
        button.gameObject.SetActive(false);
        puzzleCam.enabled = false;
        mainCam.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.ResumeMoving();
    }
}

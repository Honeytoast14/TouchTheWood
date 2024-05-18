using System;
using System.Collections;
using System.Collections.Generic;
using MyApp.MyBSP;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;
using Yarn;
using Yarn.Unity;

public class SortingPuzzleTrigger : MonoBehaviour
{
    [SerializeField] List<AudioSource> audioSources;
    [SerializeField] GameObject puzzle;
    [SerializeField] Camera puzzleCam;
    Camera mainCam;
    [SerializeField] GameObject button;
    // [SerializeField] Texture2D cursor;
    bool inZone = false;
    public static SortingPuzzleTrigger Instance { get; private set; }
    IsometricPlayerMovementController playerController;
    CapsuleCollider2D capColider;
    BSPGameManager bSPGameManager;
    SoundPlayer soundPlayer;
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        playerController = FindObjectOfType<IsometricPlayerMovementController>();
        capColider = GetComponent<CapsuleCollider2D>();
        bSPGameManager = FindObjectOfType<BSPGameManager>();
        soundPlayer = FindObjectOfType<SoundPlayer>();

        puzzleCam.enabled = false;
        mainCam = Camera.main;
        capColider.enabled = false;
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

            if (audioSources != null)
                foreach (AudioSource aud in audioSources)
                {
                    aud.enabled = false;
                }

            // Cursor.SetCursor(cursor, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void HandleUpdate(Action onBack)
    {
        if (inZone && Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape) && GameController.Instance.state == GameState.SortingPuzzle)
        {
            button.gameObject.SetActive(false);
            puzzleCam.enabled = false;
            mainCam.enabled = true;
            GameController.Instance.state = GameState.FreeRoam;

            if (audioSources != null)
                foreach (AudioSource aud in audioSources)
                {
                    aud.enabled = true;
                }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // bSPGameManager.RestartGame();

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

        if (audioSources != null)
            foreach (AudioSource aud in audioSources)
            {
                aud.enabled = true;
            }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerController.ResumeMoving();
    }
}

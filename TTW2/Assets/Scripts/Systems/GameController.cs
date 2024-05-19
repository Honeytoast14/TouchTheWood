using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using Yarn.Unity;
using Debug = UnityEngine.Debug;

public enum GameState { FreeRoam, Menu, Inventory, Dialogue, PuzzlePicture, Timeline, SortingPuzzle, Passcode, ItemReward }
public class GameController : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] NewLineView newLineView;
    public GameState state;

    public static GameController Instance { get; private set; }
    public SceneDetails currentScene { get; private set; }
    public SceneDetails previousScene { get; private set; }
    MenuController menuController;
    TitleGame titleGame;
    IsometricPlayerMovementController playerController;
    void Awake()
    {
        Time.timeScale = 1;
        menuController = GetComponent<MenuController>();
        titleGame = FindObjectOfType<TitleGame>();
        playerController = FindObjectOfType<IsometricPlayerMovementController>();

        if (menuController != null)
        {
            menuController.onBack += () =>
            {
                state = GameState.FreeRoam;
            };
            menuController.onMenuSelected += onMenuSelected;
        }

        ItemDB.Init();

        Instance = this;
    }

    void Update()
    {
        if (state == GameState.FreeRoam)
        {
            if (menuController != null)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    menuController.ShowMenuPage();
                    state = GameState.Menu;

                    inventoryUI.cover.enabled = true;
                }
            }
        }
        else if (state == GameState.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameState.Inventory)
        {
            Action onBack = () =>
            {
                state = GameState.Menu;
                menuController.cover.enabled = false;
            };
            inventoryUI.HandleUpdate(onBack);
        }
        else if (state == GameState.Dialogue)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                StartCoroutine(PressDialogue());
            }
        }
        else if (state == GameState.PuzzlePicture)
        {
            playerController.StopMoving();
            Action onBack = () =>
            {
                state = GameState.Inventory;
            };

            if (PuzzleRotatePicFirst.Instance != null)
            {
                if (PuzzleRotatePicFirst.Instance.firstIsOpen)
                {
                    PuzzleRotatePicFirst.Instance.HandleUpdate(onBack);
                }
            }
            if (PuzzleRotatePicSecond.Instance != null)
            {
                if (PuzzleRotatePicSecond.Instance.secondIsOpen)
                {
                    PuzzleRotatePicSecond.Instance.HandleUpdate(onBack);
                }
            }
            if (PuzzleRotatePicThird.Instance != null)
            {
                if (PuzzleRotatePicThird.Instance.thirdIsOpen)
                {
                    PuzzleRotatePicThird.Instance.HandleUpdate(onBack);
                }
            }
            if (PuzzleRail.Instance != null)
            {
                if (PuzzleRail.Instance.railIsOpen)
                {
                    PuzzleRail.Instance.HandleUpdate();
                }
            }
            if (PuzzleHoneyEasy.Instance != null)
            {
                if (PuzzleHoneyEasy.Instance.honeyEasyIsOpen)
                {
                    PuzzleHoneyEasy.Instance.HandleUpdate();
                }
            }
            if (PuzzleHoneyMed.Instance != null)
            {
                if (PuzzleHoneyMed.Instance.medIsOpen)
                {
                    PuzzleHoneyMed.Instance.HandleUpdate();
                }
            }
            if (PuzzleHoneyHard.Instance != null)
            {
                if (PuzzleHoneyHard.Instance.hardIsOpen)
                {
                    PuzzleHoneyHard.Instance.HandleUpdate();
                }
            }
        }

        else if (state == GameState.SortingPuzzle)
        {
            playerController.StopMoving();

            Action onBack = () =>
            {
                playerController.ResumeMoving();
            };
            SortingPuzzleTrigger.Instance.HandleUpdate(onBack);
        }

        else if (state == GameState.Passcode)
        {
            playerController.StopMoving();

            Action onBack = () =>
            {
                playerController.ResumeMoving();
            };
            Passcode.Instance.HandleUpdate(onBack);
        }

        else if (state == GameState.ItemReward)
        {
            if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
            {
                state = GameState.Inventory;
                inventoryUI.HideImageReward();
            }
        }
    }

    void onMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)  //Inventory
        {
            state = GameState.Inventory;
            inventoryUI.openInventory = true;
            inventoryUI.UpdateItemSelectionInventory();

            menuController.cover.enabled = true;
            inventoryUI.cover.enabled = false;
        }
        if (selectedItem == 1) // Exit Game
        {
            EssentialObjects essentialObjects = FindObjectOfType<EssentialObjects>();
            if (essentialObjects != null)
            {
                Destroy(essentialObjects.gameObject);
            }
            titleGame.LoadScene("TitleGame");
        }
    }

    private IEnumerator PressDialogue()
    {
        yield return new WaitForSeconds(0.2f);
        // Debug.Log("Z is pressed from GameController(Dialogue State)");
        //newLineView.HandleUpdate();
        newLineView.UserRequestedViewAdvancement();
        yield return new WaitForSeconds(0.1f);
    }

    public void CurrrentScene(SceneDetails curScene)
    {
        previousScene = currentScene;
        currentScene = curScene;
    }
}

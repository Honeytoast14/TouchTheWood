using System;
using System.Collections;
using UnityEngine;
using Yarn.Unity;

public enum GameState { FreeRoam, Menu, Inventory, Dialogue }
public class GameController : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] NewLineView newLineView;
    [SerializeField] DialogueRunner dialogueRunner;
    public GameState state;

    MenuController menuController;
    TitleGame titleGame;
    void Awake()
    {
        Time.timeScale = 1;
        menuController = GetComponent<MenuController>();
        titleGame = GetComponent<TitleGame>();

        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };
        menuController.onMenuSelected += onMenuSelected;

        ItemDB.Init();
    }

    void Update()
    {
        if (state == GameState.FreeRoam)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuController.ShowMenuPage();
                state = GameState.Menu;

                inventoryUI.cover.enabled = true;
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
                //newLineView.HandleUpdate();
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
        if (selectedItem == 4)
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
        yield return new WaitForSeconds(0.1f);
        //Debug.Log("Z is pressed from GameController(Dialogue State)");
        newLineView.HandleUpdate();
    }
}
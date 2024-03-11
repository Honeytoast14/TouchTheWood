using System;
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
    void Awake()
    {
        menuController = GetComponent<MenuController>();

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
                newLineView.HandleUpdate();
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
    }
}

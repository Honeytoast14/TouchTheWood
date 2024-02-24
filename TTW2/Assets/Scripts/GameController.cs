using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Yarn.Unity;

public enum GameSate { FreeRoam, Paused, Menu, Inventory }
public class GameController : MonoBehaviour
{
    [SerializeField] IsometricPlayerMovementController playerController;
    [SerializeField] DialogueRunner dialogueRunner;
    [SerializeField] InventoryUI inventoryUI;
    GameSate state;

    MenuController menuController;
    void Start()
    {
        menuController.onBack += () =>
        {
            state = GameSate.FreeRoam;
        };

        menuController.onMenuSelected += onMenuSelected;
    }
    private void Awake()
    {
        menuController = GetComponent<MenuController>();
    }
    void Update()
    {
        if (state == GameSate.FreeRoam)
        {
            playerController.HandleUpdate();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuController.ShowMenuPage();
                state = GameSate.Menu;
            }
        }
        else if (state == GameSate.Menu)
        {
            menuController.HandleUpdate();
        }
        else if (state == GameSate.Inventory)
        {
            Action onBack = () =>
            {
                state = GameSate.Menu;
            };
            inventoryUI.HandleUpdate(onBack);
        }
    }

    void onMenuSelected(int selectedItem)
    {
        if (selectedItem == 0)  //Inventory
        {
            state = GameSate.Inventory;
            inventoryUI.openInventory = true;
            inventoryUI.UpdateItemSelectionInventory();
        }
    }
}

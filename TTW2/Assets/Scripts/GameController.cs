using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Yarn.Unity;

public enum GameState { FreeRoam, Paused, Menu, Inventory }
public class GameController : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;
    public GameState state;

    MenuController menuController;
    void Start()
    {
        menuController.onBack += () =>
        {
            state = GameState.FreeRoam;
        };
        menuController.onMenuSelected += onMenuSelected;
    }
    private void Awake()
    {
        menuController = GetComponent<MenuController>();
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

using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] List<GameObject> borderButton;
    [SerializeField] public GameObject MenuPage;
    [SerializeField] public Image cover;
    public event Action<int> onMenuSelected;
    public event Action onBack;
    List<Button> menuButtons;
    IsometricPlayerMovementController playerController;
    SoundPlayer soundPlayer;
    int selectedItem = 0;
    void Awake()
    {
        menuButtons = GameObject.Find("Button Group").GetComponentsInChildren<Button>().ToList();
        playerController = FindObjectOfType<IsometricPlayerMovementController>();
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }
    private void Start()
    {
        MenuPage.SetActive(false);
    }

    public void HandleUpdate()
    {
        int preSelection = selectedItem;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            soundPlayer.PlaySFX(soundPlayer.buttonClick);
            --selectedItem;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            soundPlayer.PlaySFX(soundPlayer.buttonClick);
            ++selectedItem;
        }

        selectedItem = Mathf.Clamp(selectedItem, 0, menuButtons.Count - 1);
        if (preSelection != selectedItem)
        {
            UpdateItemSelection();
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            soundPlayer.PlaySFX(soundPlayer.buttonSelect);
            onMenuSelected?.Invoke(selectedItem);
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            onBack?.Invoke();
            HideMenuPage();
        }
    }

    void UpdateItemSelection()
    {
        for (int i = 0; i < menuButtons.Count; i++)
        {
            // ColorBlock colors = menuButtons[i].colors;

            if (i == selectedItem)
            {
                // colors.normalColor = GlobalSetting.i.HighlightColor;
                borderButton[i].SetActive(true);
            }
            else
            {
                // colors.normalColor = GlobalSetting.i.NormalColor;
                borderButton[i].SetActive(false);
            }

            // menuButtons[i].colors = colors;
        }
    }

    public void ShowMenuPage()
    {
        MenuPage.SetActive(true);
        Time.timeScale = 0;
        UpdateItemSelection();
        cover.enabled = false;

        playerController.StopMoving();
    }
    public void HideMenuPage()
    {
        MenuPage.SetActive(false);
        Time.timeScale = 1;
        selectedItem = 0;

        playerController.ResumeMoving();
    }
}

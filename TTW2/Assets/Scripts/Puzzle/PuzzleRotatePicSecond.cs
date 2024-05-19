using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PuzzleRotatePicSecond : MonoBehaviour
{
    [SerializeField] GlobalSetting globalSetting;
    [SerializeField] GameObject cover;
    [SerializeField] private int rowSize = 3;
    [SerializeField] private List<Button> pictures;
    [Header("Required Item")]
    [SerializeField] public ItemData requiredItems;
    [SerializeField] public int requiredCounts;
    [Header("Reward Item")]
    [SerializeField] ItemData rewardItem;
    [SerializeField] int rewardCount;
    public bool win { get; private set; }
    int selectedPicture = 0;
    public bool secondIsOpen = false;
    SoundPlayer soundPlayer;
    public static PuzzleRotatePicSecond Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        win = false;
        gameObject.SetActive(false);
        cover.SetActive(false);
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }
    public void HandleUpdate(Action onBack)
    {
        Win();

        int preSelection = selectedPicture;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            soundPlayer.PlaySFX(soundPlayer.buttonClick);
            ++selectedPicture;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            soundPlayer.PlaySFX(soundPlayer.buttonClick);
            --selectedPicture;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selectedPicture >= rowSize)
        {
            soundPlayer.PlaySFX(soundPlayer.buttonClick);
            selectedPicture -= rowSize;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && selectedPicture + rowSize < pictures.Count)
        {
            soundPlayer.PlaySFX(soundPlayer.buttonClick);
            selectedPicture += rowSize;
        }

        selectedPicture = Mathf.Clamp(selectedPicture, 0, pictures.Count - 1);

        if (preSelection != selectedPicture)
        {
            UpdatedPictureSelection();
        }

        var pic = pictures[selectedPicture];
        if (Input.GetKeyDown(KeyCode.Z) && !win)
        {
            soundPlayer.PlaySFX(soundPlayer.paperSound);
            pic.transform.Rotate(0, 0, 90);
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            onBack?.Invoke();
            HidePuzzlePage();
        }
    }

    void Win()
    {
        bool allZero = true;
        foreach (Button pic in pictures)
        {
            if (pic.transform.rotation.z != 0)
            {
                allZero = false;
                break;
            }
        }

        if (allZero)
        {
            win = true;

            UpdatedPictureSelection();
        }

        var inventory = Inventory.GetInventory();
        if (win)
        {
            if (rewardItem != null && rewardCount > 0)
            {
                inventory.AddItem(rewardItem, rewardCount);
            }
            rewardItem = null;
            rewardCount = 0;

            if (requiredItems != null)
            {
                inventory.RemoveItem(requiredItems, requiredCounts);
            }
            requiredItems = null;
        }
    }

    void UpdatedPictureSelection()
    {
        for (int i = 0; i < pictures.Count; i++)
        {
            ColorBlock colors = pictures[i].colors;
            if (i == selectedPicture)
            {
                colors.normalColor = GlobalSetting.i.PuzzleSelect;
            }
            else
            {
                colors.normalColor = GlobalSetting.i.PuzzleDeselect;
                if (win)
                    colors.normalColor = GlobalSetting.i.PuzzleSelect;
            }

            pictures[i].colors = colors;
        }
    }
    public void OpenPicturePuzzle()
    {
        if (CheckItemInInventory(requiredItems, requiredCounts))
        {
            gameObject.SetActive(true);
            GameController.Instance.state = GameState.PuzzlePicture;
            UpdatedPictureSelection();
            Time.timeScale = 0;
            cover.SetActive(true);
            secondIsOpen = true;
        }
    }

    public void HidePuzzlePage()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        selectedPicture = 0;
        cover.SetActive(false);
        secondIsOpen = false;
    }

    bool CheckItemInInventory(ItemData item, int count)
    {
        var inventory = Inventory.GetInventory();
        if (item != null && count > 0)
        {
            var itemCount = inventory.GetItemCount(item);
            if (itemCount < count)
            {
                return false;
            }
        }
        return true;
    }
}

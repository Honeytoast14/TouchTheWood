using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PuzzleHoneyEasy : MonoBehaviour
{
    [SerializeField] List<GameObject> bgAndPic;
    [SerializeField] private int rowSize = 3;
    [SerializeField] private List<Button> pictures;
    [SerializeField] private List<Button> truePics;
    [SerializeField] private List<Button> twoWayPics;
    public bool win { get; private set; }
    int selectedPicture = 0;
    public bool honeyEasyIsOpen = false;
    SoundPlayer soundPlayer;
    public static PuzzleHoneyEasy Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        win = false;
        gameObject.SetActive(false);

        foreach (GameObject child in bgAndPic)
        {
            child.SetActive(false);
        }

        soundPlayer = FindObjectOfType<SoundPlayer>();
    }
    public void HandleUpdate()
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
            soundPlayer.PlaySFX(soundPlayer.switchUsed);
            pic.transform.Rotate(0, 0, 90);
            Debug.Log(pic.transform.name + pic.transform.rotation.z);
        }

        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            GameController.Instance.state = GameState.FreeRoam;
            HideEasyHon();
        }
    }

    void Win()
    {
        bool allZero = true;

        foreach (Button pic in truePics)
        {
            if (pic.transform.rotation.z != 0)
            {
                allZero = false;
                break;
            }
        }

        foreach (Button pictwoway in twoWayPics)
        {
            if (pictwoway.transform.rotation.z != 1 && pictwoway.transform.rotation.z != 0 && pictwoway.transform.rotation.z != -1)
            {
                allZero = false;
                break;
            }
        }

        if (allZero)
        {
            soundPlayer.PlaySFX(soundPlayer.correct);
            win = true;
            UpdatedPictureSelection();
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

    public void OpenEasyHon()
    {
        gameObject.SetActive(true);
        GameController.Instance.state = GameState.PuzzlePicture;
        UpdatedPictureSelection();
        Time.timeScale = 0;
        honeyEasyIsOpen = true;

        foreach (GameObject child in bgAndPic)
        {
            child.SetActive(true);
        }
    }

    public void HideEasyHon()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        selectedPicture = 0;
        honeyEasyIsOpen = false;

        foreach (GameObject child in bgAndPic)
        {
            child.SetActive(false);
        }
    }
}

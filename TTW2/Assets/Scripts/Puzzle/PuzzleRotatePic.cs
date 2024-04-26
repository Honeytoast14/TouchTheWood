using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows;
using Input = UnityEngine.Input;

public class PuzzleRotatePic : MonoBehaviour
{
    [SerializeField] GameObject puzzlePage;
    [SerializeField] private int rowSize = 3;
    [SerializeField] private List<Button> pictures;
    public bool win { get; private set; }
    int selectedPicture = 0;

    public static PuzzleRotatePic Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        win = false;
        gameObject.SetActive(false);
    }
    public void HandleUpdate(Action onBack)
    {
        Win();

        int preSelection = selectedPicture;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++selectedPicture;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --selectedPicture;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selectedPicture >= rowSize)
        {
            selectedPicture -= rowSize;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && selectedPicture + rowSize < pictures.Count)
        {
            selectedPicture += rowSize;
        }

        selectedPicture = Mathf.Clamp(selectedPicture, 0, pictures.Count - 1);

        if (preSelection != selectedPicture)
        {
            UpdatedPictureSelection();
        }

        var pic = pictures[selectedPicture];
        if (Input.GetKeyDown(KeyCode.Z) && !win)
            pic.transform.Rotate(0, 0, 90);

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
    }

    void UpdatedPictureSelection()
    {
        for (int i = 0; i < pictures.Count; i++)
        {
            ColorBlock colors = pictures[i].colors;
            if (i == selectedPicture)
            {
                if (win)
                {
                    colors.normalColor = GlobalSetting.i.NormalColor;
                }
                else
                {
                    colors.normalColor = GlobalSetting.i.HighlightColor;
                }
            }
            else
            {
                colors.normalColor = GlobalSetting.i.NormalColor;
            }

            pictures[i].colors = colors;
        }
    }

    public void OpenPicturePuzzle()
    {
        puzzlePage.SetActive(true);
        GameController.Instance.state = GameState.PuzzlePicture;
        UpdatedPictureSelection();
        Time.timeScale = 0;
    }

    public void HidePuzzlePage()
    {
        puzzlePage.SetActive(false);
        Time.timeScale = 1f;
        selectedPicture = 0;
    }
}

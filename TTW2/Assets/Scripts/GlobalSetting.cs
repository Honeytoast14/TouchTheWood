using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlobalSetting : MonoBehaviour
{
    [Header("Set Colors")]
    [SerializeField] Color normalColor;
    [SerializeField] Color highlightColor;
    [SerializeField] Color puzzleSelect;
    [SerializeField] Color puzzleDeselect;
    public Color NormalColor => normalColor;
    public Color HighlightColor => highlightColor;
    public Color PuzzleSelect => puzzleSelect;
    public Color PuzzleDeselect => puzzleDeselect;

    public static GlobalSetting i
    {
        get; private set;
    }
    public void Awake()
    {
        i = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckHoneyPuzzle : MonoBehaviour
{
    TriggerEvent trigger;
    bool isQuestComplete = false;

    void Start()
    {
        trigger = FindObjectOfType<TriggerEvent>();
    }

    void Update()
    {
        if (!isQuestComplete &&
            PuzzleHoneyEasy.Instance != null &&
            PuzzleHoneyMed.Instance != null &&
            PuzzleHoneyHard.Instance != null)
        {
            if (PuzzleHoneyEasy.Instance.win &&
                PuzzleHoneyMed.Instance.win &&
                PuzzleHoneyHard.Instance.win)
            {
                trigger.AddQuesCompletetInYarn("ChefPanda");
                isQuestComplete = true;
                Debug.Log("Honey puzzle completed!");
            }
        }
    }
}

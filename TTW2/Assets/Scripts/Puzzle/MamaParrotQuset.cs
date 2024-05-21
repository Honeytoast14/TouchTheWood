using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Yarn.Unity;

public class MamaParrotQuset : MonoBehaviour
{
    [SerializeField] List<ParrotManager> parrotManagers;
    [SerializeField] TMP_Text completeCountText;
    private int totalCompletedPuzzles = 0;

    void Start()
    {
        UpdateCompleteCountText();
    }

    void Update()
    {
        if (AllPuzzlesCompleted())
        {
            var variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();

            bool parrotQuest;
            variableStorage.TryGetValue("$parrotQuest", out parrotQuest);
            variableStorage.SetValue("$parrotQuest", parrotQuest = true);
        }
    }

    private bool AllPuzzlesCompleted()
    {
        foreach (ParrotManager parrotManager in parrotManagers)
        {
            if (!parrotManager.IsPuzzleCompleted())
            {
                return false;
            }
        }
        return true;
    }

    public void UpdateCompleteCount(int completeCount)
    {
        totalCompletedPuzzles += completeCount;
        completeCountText.text = "Completed Puzzles: " + totalCompletedPuzzles;
    }

    private void UpdateCompleteCountText()
    {
        totalCompletedPuzzles = 0;
        foreach (ParrotManager parrotManager in parrotManagers)
        {
            totalCompletedPuzzles += parrotManager.GetNumberCompleteChild();
        }
        completeCountText.text = "Completed Puzzles: " + totalCompletedPuzzles;
    }
}

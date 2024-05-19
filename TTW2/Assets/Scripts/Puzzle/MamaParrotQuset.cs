using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class MamaParrotQuset : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    [SerializeField] List<ParrotManager> parrotManagers;

    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
    }
    void Update()
    {
        if (AllPuzzlesCompleted())
        {
            // Debug.Log("All puzzles are completed!");
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
}

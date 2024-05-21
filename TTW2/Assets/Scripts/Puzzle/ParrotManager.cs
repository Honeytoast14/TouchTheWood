using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

public class ParrotManager : MonoBehaviour
{
    SoundPlayer soundPlayer;
    private List<int> interactionOrder = new List<int>();
    [SerializeField] List<int> correctOrder;
    [SerializeField] AudioClip correct;
    [SerializeField] AudioClip incorrect;
    [SerializeField] PlayableDirector timelineCorrect;
    private bool puzzleCompleted = false;
    private int numberCompleteChild = 0;
    private List<ParrotPuzzle> interactedPuzzles = new List<ParrotPuzzle>();

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundPlayer>();
    }

    public void RegisterInteraction(int orderNumber)
    {
        if (puzzleCompleted) return;

        interactionOrder.Add(orderNumber);
        if (interactionOrder.Count == correctOrder.Count)
        {
            CheckOrder();
        }
    }

    private void CheckOrder()
    {
        for (int i = 0; i < correctOrder.Count; i++)
        {
            if (interactionOrder[i] != correctOrder[i])
            {
                Debug.Log("Incorrect Order");
                soundPlayer.PlaySFX(incorrect);
                StartCoroutine(ResetPuzzle());
                return;
            }
        }
        Debug.Log("Puzzle Completed!");
        soundPlayer.PlaySFX(correct);
        StartCoroutine(PlayTimeline());
        puzzleCompleted = true;
        DisableInteractedPuzzles();

        numberCompleteChild += correctOrder.Count;

        var variableStorage = GameObject.FindObjectOfType<InMemoryVariableStorage>();

        float numberOfChild;
        variableStorage.TryGetValue("$numberChild", out numberOfChild);
        variableStorage.SetValue("$numberChild", numberOfChild + correctOrder.Count);

        // Notify MamaParrotQuset about the completion
        var mamaParrotQuest = FindObjectOfType<MamaParrotQuset>();
        if (mamaParrotQuest != null)
        {
            mamaParrotQuest.UpdateCompleteCount(correctOrder.Count);
        }
    }

    private void DisableInteractedPuzzles()
    {
        foreach (ParrotPuzzle puzzle in interactedPuzzles)
        {
            puzzle.DisableInteraction();
        }
    }

    private IEnumerator ResetPuzzle()
    {
        yield return new WaitForSeconds(0.5f);
        interactionOrder.Clear();
        foreach (ParrotPuzzle puzzle in interactedPuzzles)
        {
            puzzle.ResetTile();
        }
        interactedPuzzles.Clear();
    }

    private IEnumerator PlayTimeline()
    {
        yield return new WaitForSeconds(0.7f);
        timelineCorrect.Play();
    }

    public void AddInteractedPuzzle(ParrotPuzzle puzzle)
    {
        interactedPuzzles.Add(puzzle);
    }

    public bool IsPuzzleCompleted()
    {
        return puzzleCompleted;
    }

    public int GetNumberCompleteChild()
    {
        return numberCompleteChild;
    }
}

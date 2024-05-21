using UnityEngine;
using Yarn.Unity;

public class YarnSpinnerFunction : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    PickUp pickUp;
    ItemGiver itemGiver;
    PuzzleWall puzzleWall;
    [SerializeField] NewLineView newLineView;
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        pickUp = FindObjectOfType<PickUp>();
        itemGiver = FindObjectOfType<ItemGiver>();
        puzzleWall = FindObjectOfType<PuzzleWall>();

        LoadYarnCommand();
    }

    public void LoadYarnCommand()
    {
        dialogueRunner.AddCommandHandler<bool>("setGroupTalk", newLineView.SetGroupTalk);

        if (puzzleWall != null)
        {
            dialogueRunner.AddCommandHandler<string, string>("SetEmojiWall", puzzleWall.SetEmoji);
            dialogueRunner.AddCommandHandler<string, string>("SetEmoji", puzzleWall.SetEmojiNoSwitch);
        }
    }
}

using UnityEngine;
using Yarn.Unity;

public class YarnSpinnerFunction : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    PickUp pickUp;
    ItemGiver itemGiver;
    PuzzleTrigger puzzleTrigger;
    [SerializeField] NewLineView newLineView;
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        pickUp = FindObjectOfType<PickUp>();
        itemGiver = FindObjectOfType<ItemGiver>();
        puzzleTrigger = FindObjectOfType<PuzzleTrigger>();

        if (itemGiver != null)
        {
            dialogueRunner.AddCommandHandler<string>("SetUseItemGiver", itemGiver.SetUsedInYarn);
        }

        if (pickUp != null)
        {
            dialogueRunner.AddCommandHandler<string>("SetUsePickUp", pickUp.SetUsedInYarn);
        }
        dialogueRunner.AddCommandHandler<bool>("setGroupTalk", newLineView.SetGroupTalk);

    }
}

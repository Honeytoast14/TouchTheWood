using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class YarnSpinnerFunction : MonoBehaviour
{
    DialogueRunner dialogueRunner;
    PickUp pickUp;
    ItemGiver itemGiver;
    void Start()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();
        pickUp = FindObjectOfType<PickUp>();
        itemGiver = FindObjectOfType<ItemGiver>();

        dialogueRunner.AddCommandHandler<string>("SetUseItemGiver", itemGiver.SetUsedInYarn);
        dialogueRunner.AddCommandHandler<string>("SetUsePickUp", pickUp.SetUsedInYarn);
    }
}

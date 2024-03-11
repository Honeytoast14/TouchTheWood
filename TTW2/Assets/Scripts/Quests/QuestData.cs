using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestData", menuName = "Quest Data", order = 51)]
public class QuestData : ScriptableObject
{
    [Header("About Quest")]
    [SerializeField] new string name;
    [SerializeField] string description;

    [Header("Dialogue")]
    [SerializeField] string dialogueNodeStart;
    [SerializeField] string dialogueNodeInprogress;
    [SerializeField] string dialogueNodeComplete;

    [Header("Item and RewardItem")]
    [SerializeField] ItemData requiredItem;
    [SerializeField] int requiredCount;
    [SerializeField] ItemData rewardItem;
    [SerializeField] int rewardCount;


    public string Name => name;
    public string Description => description;
    public string DialogueNodeStart => dialogueNodeStart;
    public string DialogueNodeInprogress => dialogueNodeInprogress;
    public string DialogueNodeComplete => dialogueNodeComplete;
    public ItemData RequiredItem => requiredItem;
    public int RequiredCount => requiredCount;
    public ItemData RewardItem => rewardItem;
    public int RewardCount => rewardCount;
}

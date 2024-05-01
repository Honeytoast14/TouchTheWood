using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using Yarn;
using Yarn.Unity;

[System.Serializable]
public class Quest
{
    public QuestData Base { get; private set; }
    public QuestStatus Status { get; private set; }
    public Quest(QuestData _Base)
    {
        Base = _Base;
    }

    public Quest(QuestSaveData saveData)
    {
        Base = QuestDB.GetObjectByName(saveData.name);
        Status = saveData.status;
    }

    public QuestSaveData GetSaveData()
    {
        var saveData = new QuestSaveData()
        {
            name = Base.Name,
            status = Status
        };
        return saveData;
    }

    public void StartQuest(string dialogueStartQuest)
    {
        Status = QuestStatus.Started;
        TriggerEvent.Instance.StartDialogue(dialogueStartQuest);


        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
    }

    public void CompleteQuest(Transform player, string dialogueCompleteQuest)
    {
        TriggerEvent.Instance.StartDialogue(dialogueCompleteQuest);
        Debug.Log("complete quest");

        var inventory = Inventory.GetInventory();
        if (Base.RequiredItem != null && Base.RequiredCount > 0)
        {
            inventory.RemoveItem(Base.RequiredItem, Base.RequiredCount);
        }

        if (Base.RewardItem != null && Base.RewardCount > 0)
        {
            inventory.AddItem(Base.RewardItem, Base.RewardCount);
        }

        Status = QuestStatus.Completed;
    }

    public void InProgressQuest(string dialogueInProgress)
    {
        TriggerEvent.Instance.StartDialogue(Base.DialogueNodeInprogress);
    }

    public bool canBeComplete()
    {
        var inventory = Inventory.GetInventory();

        if (Base.RequiredItem != null && Base.RequiredCount > 0)
        {
            int itemCount = inventory.GetItemCount(Base.RequiredItem);

            if (itemCount < Base.RequiredCount)
            {
                return false;
            }
        }
        return true;
    }
}

[System.Serializable]
public class QuestSaveData
{
    public string name;
    public QuestStatus status;
}

public enum QuestStatus { None, Started, Completed }

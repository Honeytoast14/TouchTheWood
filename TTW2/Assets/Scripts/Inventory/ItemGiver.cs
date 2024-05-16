using System;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] List<ItemDataList> itemDataLists;
    GameObject parent;
    public bool used = false;
    bool canPick = false;
    bool collisionCheck;
    public bool setYarn = false;
    DialogueRunner dialogueRunner;

    void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (setYarn)
            dialogueRunner.AddCommandHandler<string>("SetUseItemGiver", SetUsedInYarn);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "NPC")
        {
            collisionCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "NPC")
        {
            collisionCheck = false;
        }
    }

    public void GiveItem(IsometricPlayerMovementController player)
    {
        foreach (var itemDataList in itemDataLists)
        {
            player.GetComponent<Inventory>().AddItem(itemDataList.Item, itemDataList.Count);
            Debug.Log($"Give {itemDataList.Item} x{itemDataList.Count}.");
        }
        used = true;
        canPick = false;

        GetComponent<ItemGiver>().enabled = false;
    }

    public bool CanBeGiven()
    {
        foreach (var itemDataList in itemDataLists)
        {
            if (!(collisionCheck && itemDataList.Item != null && !used && itemDataList.Count > 0 && canPick))
            {
                return false;
            }
        }
        return true;
    }


    public void SetUsedInYarn(string parentName)
    {
        ItemGiver itemGiver;
        parent = GameObject.Find(parentName);
        itemGiver = parent.gameObject.GetComponentInChildren<ItemGiver>();
        itemGiver.canPick = true;
    }
}

[Serializable]
public class ItemDataList
{
    [SerializeField] ItemData item;
    [SerializeField] int count = 1;
    public ItemData Item
    {
        get { return item; }
    }

    public int Count
    {
        get { return count; }
    }
}
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Yarn.Unity;

public class ItemGiver : MonoBehaviour
{
    [SerializeField] ItemData item;
    [SerializeField] int count = 1;
    GameObject parent;
    bool used = false;
    bool collisionCheck;
    [SerializeField] bool isItem;

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
        player.GetComponent<Inventory>().AddItem(item, count);
        used = false;

        Debug.Log($"Give {item} x{count}. set used to {used}");
        GetComponent<ItemGiver>().enabled = false;
    }

    public bool CanBeGiven()
    {
        return collisionCheck && item != null && used && count > 0;
    }

    public void SetUsedInYarn(string parentName)
    {
        ItemGiver itemGiver;
        parent = GameObject.Find(parentName);
        itemGiver = parent.gameObject.GetComponentInChildren<ItemGiver>();
        itemGiver.used = true;

        Debug.Log($"you recive {item} x{count}");
    }
}
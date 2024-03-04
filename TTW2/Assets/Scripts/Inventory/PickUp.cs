using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Yarn.Unity;

public class PickUp : MonoBehaviour, ISavable
{
    [SerializeField] ItemData item;
    [SerializeField] int count = 1;
    GameObject parent;
    public bool Used { get; set; } = false;
    bool canPick = false;
    bool collisionCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "Item")
        {
            collisionCheck = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "Item")
        {
            collisionCheck = false;
        }
    }

    public void GiveItem(IsometricPlayerMovementController player)
    {
        player.GetComponent<Inventory>().AddItem(item, count);
        Used = true;
        canPick = false;

        Debug.Log($"Give {item} x{count}. set used to {Used}");

        transform.parent.GetComponent<CapsuleCollider2D>().enabled = false;
        transform.parent.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<TriggerEvent>().enabled = false;
        GetComponent<PickUp>().enabled = false;
    }

    public bool CanBeGiven()
    {
        return collisionCheck && item != null && !Used && count > 0 && canPick;
    }

    public void SetUsedInYarn(string parentName)
    {
        PickUp pickUp;
        parent = GameObject.Find(parentName);
        pickUp = parent.gameObject.GetComponentInChildren<PickUp>();
        pickUp.canPick = true;

        Debug.Log($"Give {item} x{count}.");
    }

    public object CaptureState()
    {
        return Used;
    }

    public void RestoreState(object state)
    {
        Used = (bool)state;

        if (Used)
        {
            transform.parent.GetComponent<CapsuleCollider2D>().enabled = false;
            transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<TriggerEvent>().enabled = false;
            GetComponent<PickUp>().enabled = false;
        }
    }
}
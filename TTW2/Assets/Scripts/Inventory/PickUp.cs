using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using Yarn.Unity;

public class PickUp : MonoBehaviour
{
    [SerializeField] ItemData item;
    [SerializeField] int count = 1;
    GameObject parent;
    bool used = false;
    bool collisionCheck;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "Item")
        {
            collisionCheck = true;
            Debug.Log($"collison is {collisionCheck}, parentName = {transform.parent.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && transform.parent.tag == "Item")
        {
            collisionCheck = false;
            Debug.Log($"collison is {collisionCheck}, parentName = {transform.parent.name}");
        }
    }

    public void GiveItem(IsometricPlayerMovementController player)
    {
        player.GetComponent<Inventory>().AddItem(item, count);
        used = false;

        Debug.Log($"Give {item} x{count}. set used to {used}");

        transform.parent.GetComponent<CapsuleCollider2D>().enabled = false;
        transform.parent.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GetComponent<PickUp>().enabled = false;
    }

    public bool CanBeGiven()
    {
        return collisionCheck && item != null && used && count > 0;
    }

    public void SetUsedInYarn(string parentName)
    {
        PickUp pickUp;
        parent = GameObject.Find(parentName);
        pickUp = parent.gameObject.GetComponentInChildren<PickUp>();
        pickUp.used = true;

        Debug.Log($"you recive {item} x{count}");
    }
}
using UnityEngine;
using Yarn.Unity;

public class PickUp : MonoBehaviour
{
    [SerializeField] ItemData item;
    [SerializeField] int count = 1;
    GameObject parent;
    public bool Used { get; set; } = false;
    bool canPick = false;
    bool collisionCheck;
    public bool setYarn = false;
    DialogueRunner dialogueRunner;
    void Awake()
    {
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (setYarn)
            dialogueRunner.AddCommandHandler<string>("SetUsePickUp", SetUsedInYarn);

    }

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
        var inventory = Inventory.GetInventory();
        var canUseItem = inventory.CanUseItem(item);
        if (inventory != null)
        {
            inventory.AddItem(item, count);

            Used = true;
            canPick = false;

            Debug.Log($"Give {item} x{count}. set used to {Used}");

            transform.parent.GetComponent<CapsuleCollider2D>().enabled = false;
            transform.parent.GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<CapsuleCollider2D>().enabled = false;
            GetComponent<TriggerEvent>().enabled = false;
            GetComponent<PickUp>().enabled = false;
        }
        else
        {
            Debug.LogError("Cant find inventory for pick up items");
        }
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
}
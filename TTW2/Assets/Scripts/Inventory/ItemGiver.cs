using UnityEngine;

public class ItemGiver : MonoBehaviour, ISavable
{
    [SerializeField] ItemData item;
    [SerializeField] int count = 1;
    GameObject parent;
    public bool used = false;
    bool canPick = false;
    bool collisionCheck;

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
        used = true;
        canPick = false;

        Debug.Log($"Give {item} x{count}.");
        GetComponent<ItemGiver>().enabled = false;
    }

    public bool CanBeGiven()
    {
        return collisionCheck && item != null && !used && count > 0 && canPick;
    }

    public void SetUsedInYarn(string parentName)
    {
        ItemGiver itemGiver;
        parent = GameObject.Find(parentName);
        itemGiver = parent.gameObject.GetComponentInChildren<ItemGiver>();
        itemGiver.canPick = true;

        Debug.Log($"you recive {item} x{count}");
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }

}
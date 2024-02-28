using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<ItemSlot> slots;
    public List<ItemSlot> Slots => slots;
    public event Action onUpdated;
    public static Inventory GetInventory()
    {
        return FindObjectOfType<IsometricPlayerMovementController>().GetComponent<Inventory>();
    }

    public void AddItem(ItemData item, int count = 1)
    {
        var itemSlot = slots.FirstOrDefault(slot => slot.Item == item);

        if (itemSlot != null)
        {
            itemSlot.Count += count;
        }
        else
        {
            slots.Add(new ItemSlot()
            {
                Item = item,
                Count = count
            });
        }

        onUpdated?.Invoke();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemData item;
    [SerializeField] int count;

    public ItemData Item
    {
        get => item;
        set => item = value;
    }

    public int Count
    {
        get => count;
        set => count = value;
    }
}
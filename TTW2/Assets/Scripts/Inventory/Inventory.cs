using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour, ISavable
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

    public void RemoveItem(ItemData item, int count)
    {
        var itemSlot = slots.FirstOrDefault(slot => slot.Item == item);
        if (count > 1)
        {
            itemSlot.Count -= count;
        }
        else
        {
            itemSlot.Count--;
        }

        if (itemSlot.Count == 0)
        {
            slots.Remove(itemSlot);
        }

        onUpdated?.Invoke();
    }

    public bool HasItem(ItemData item)
    {
        var itemSlot = slots.Exists(slot => slot.Item == item);

        return itemSlot;
    }

    public int GetItemCount(ItemData item)
    {
        var itemSlot = slots.FirstOrDefault(slot => slot.Item == item);

        return itemSlot != null && itemSlot.Count > 0 ? itemSlot.Count : 0;
    }

    public object CaptureState()
    {
        var saveData = new InventorySaveData()
        {
            items = slots.Select(i => i.GetSaveData()).ToList()
        };
        return saveData;
    }

    public void RestoreState(object state)
    {
        var saveData = state as InventorySaveData;
        slots = saveData.items.Select(i => new ItemSlot(i)).ToList();

        onUpdated?.Invoke();
    }
}

[Serializable]
public class ItemSlot
{
    [SerializeField] ItemData item;
    [SerializeField] int count;

    public ItemSlot(ItemSaveData saveData)
    {
        item = ItemDB.GetItemByName(saveData.name);
        count = saveData.count;
    }

    public ItemSlot()
    {

    }

    public ItemSaveData GetSaveData()
    {
        var saveData = new ItemSaveData()
        {
            name = item.name,
            count = count
        };
        return saveData;
    }

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
[Serializable]
public class ItemSaveData
{
    public string name;
    public int count;
}

[Serializable]
public class InventorySaveData
{
    public List<ItemSaveData> items;
}
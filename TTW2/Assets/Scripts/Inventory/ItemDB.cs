using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB
{
    static Dictionary<string, ItemData> items;

    public static void Init()
    {
        items = new Dictionary<string, ItemData>();

        var itemList = Resources.LoadAll<ItemData>("");
        foreach (var item in itemList)
        {
            if (items.ContainsKey(item.name))
            {
                Debug.LogError($"There two items with name: {item.name}");
                continue;
            }

            items[item.name] = item;
        }
    }

    public static ItemData GetItemByName(string name)
    {
        if (!items.ContainsKey(name))
        {
            Debug.LogError($"Item with name {name} not found in the database");
            return null;
        }
        return items[name];
    }
}

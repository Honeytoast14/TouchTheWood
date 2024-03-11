using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDB
{
    static Dictionary<string, QuestData> quests;
    public static QuestData GetObjectByName(string name)
    {
        if (!quests.ContainsKey(name))
        {
            Debug.LogError($"Item with name {name} not found in the database");
            return null;
        }
        return quests[name];
    }
}

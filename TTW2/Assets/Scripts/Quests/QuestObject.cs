using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestObject : MonoBehaviour
{
    [SerializeField] QuestData questToCheck;
    [SerializeField] ObjectActions onStart;
    [SerializeField] ObjectActions onComplete;

    QuestList questList;
    void Start()
    {
        questList = QuestList.GetQuestList();
    }

    public void UpdateObjectStatus()
    {
        if (onStart != ObjectActions.DoNothing && questList.IsStarted(questToCheck.name))
        {
            foreach (Transform child in transform)
            {

            }
        }
    }
}

public enum ObjectActions { DoNothing, Enable, Disable }
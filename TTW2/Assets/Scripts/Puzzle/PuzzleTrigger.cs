using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PuzzleTrigger : MonoBehaviour
{
    [Header("Required Item")]
    [SerializeField] ItemData requiredItem;
    [SerializeField] int requiredCount;

    [Header("Reward Item")]
    [SerializeField] ItemData rewardItem;
    [SerializeField] int rewardCount;

    [Header("Puzzle Scene")]
    [SerializeField] string sceneName;

    void Update()
    {
        var inventory = Inventory.GetInventory();
        var item = inventory.HasItem(requiredItem);

        if (requiredItem == item)
        {
            SceneDetails.Instance.LoadAdditiveScene(sceneName);
        }
    }

    bool CheckItemInInventory()
    {
        var inventory = Inventory.GetInventory();
        if (requiredItem != null && requiredCount > 0)
        {
            var itemCount = inventory.GetItemCount(requiredItem);
            if (itemCount < requiredCount)
            {
                return false;
            }
        }
        return true;
    }

    void GiveReward()
    {
        var inventory = Inventory.GetInventory();
        if (PuzzleRotatePic.Instance.win)
        {
            if (requiredItem != null && requiredCount > 0)
            {
                inventory.RemoveItem(requiredItem, requiredCount);
            }

            if (rewardItem != null && rewardCount > 0)
            {
                inventory.AddItem(rewardItem, rewardCount);
            }
        }
    }

    public void PlayerPicturePuzzle()
    {
        if (requiredItem != null && requiredCount > 0)
        {
            if (!PuzzleRotatePic.Instance.win)
            {
                if (CheckItemInInventory())
                {
                    Debug.Log("Open Puzzle Picture");
                    PuzzleRotatePic.Instance.OpenPicturePuzzle();
                }
            }
            else
            {
                Debug.Log("Puzzle Complete");
                GiveReward();
            }
        }
        else
        {
            Debug.LogError("Need requiredItem and requiredCount");
        }
    }
}

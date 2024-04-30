using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Item Data", order = 51)]
public class ItemData : ScriptableObject
{
    public Sprite itemImage;
    public string itemName;
    public string itemDescription;
    public bool canUse;
}
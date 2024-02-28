using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image border;
    [SerializeField] TMP_Text count;
    RectTransform rectTransform;
    public float Height => rectTransform.rect.height;

    public void SetData(ItemSlot itemSlot)
    {
        rectTransform = GetComponent<RectTransform>();
        image.sprite = itemSlot.Item.itemImage;
        count.text = $"{itemSlot.Count}";
    }

    public void ShowSelectBorder()
    {
        border.enabled = true;
    }

    public void HideSelectBorder()
    {
        border.enabled = false;
    }

    public void ShowCount()
    {
        count.enabled = true;
    }
    public void HideCount()
    {
        count.enabled = false;
    }
}

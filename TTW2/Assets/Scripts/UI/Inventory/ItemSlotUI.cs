using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Image border;
    RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public float Height => rectTransform.rect.height;

    public void SetData(ItemSlot itemSlot)
    {
        image.sprite = itemSlot.Item.itemImage;
    }

    public void ShowSelectBorder()
    {
        border.enabled = true;
    }

    public void HideSelectBorder()
    {
        border.enabled = false;
    }
}

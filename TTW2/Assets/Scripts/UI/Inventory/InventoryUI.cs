using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;
    [SerializeField] Image image;
    [SerializeField] TMP_Text itemTitle;
    [SerializeField] TMP_Text description;
    Inventory inventory;
    RectTransform itemListRect;
    public List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();
    private int selectedItem = 0;
    const int itemInViewPort = 7;
    public bool openInventory = false;
    private void Awake()
    {
        inventory = Inventory.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();
    }
    private void Start()
    {
        UpdateItemList();
    }
    void UpdateItemList()
    {
        foreach (Transform child in itemList.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (var itemSlot in inventory.Slots)
        {
            var slotUIObj = Instantiate(itemSlotUI, itemList.transform);
            slotUIObj.SetData(itemSlot);

            slotUIList.Add(slotUIObj);
        }

        UpdateItemSelectionInventory();
    }

    public void HandleUpdate(Action onBack)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            onBack?.Invoke();
            for (int i = 0; i < slotUIList.Count; i++)
            {
                slotUIList[i].HideSelectBorder();
            }
            openInventory = false;
            selectedItem = 0;
        }

        int preSelection = selectedItem;
        int rowSize = 4;

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ++selectedItem;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            --selectedItem;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && selectedItem >= rowSize)
        {
            selectedItem -= rowSize;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && selectedItem + rowSize < inventory.Slots.Count)
        {
            selectedItem += rowSize;
        }

        selectedItem = Mathf.Clamp(selectedItem, 0, inventory.Slots.Count - 1);

        if (preSelection != selectedItem)
        {
            UpdateItemSelectionInventory();
        }
    }

    public void UpdateItemSelectionInventory()
    {
        for (int i = 0; i < slotUIList.Count; i++)
        {
            if (i == selectedItem && openInventory)
            {
                slotUIList[i].ShowSelectBorder();
            }
            else
            {
                slotUIList[i].HideSelectBorder();
            }
        }

        var slot = inventory.Slots[selectedItem];
        image.sprite = slot.Item.itemImage;
        itemTitle.text = slot.Item.itemName;
        description.text = slot.Item.itemDescription;

        HandleScrolling();
    }

    private void HandleScrolling()
    {
        float scrollPos = Mathf.Clamp(selectedItem - itemInViewPort, 0, selectedItem) * slotUIList[0].Height;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);
    }

}

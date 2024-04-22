using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class InventoryUI : MonoBehaviour
{
    [Header("Item Box")]
    [SerializeField] GameObject itemList;
    [SerializeField] ItemSlotUI itemSlotUI;
    [Header("Item Description")]
    [SerializeField] Image image;
    [SerializeField] TMP_Text itemTitle;
    [SerializeField] TMP_Text description;
    [Header("")]
    [SerializeField] public Image cover;
    Inventory inventory;
    RectTransform itemListRect;
    List<ItemSlotUI> slotUIList = new List<ItemSlotUI>();
    private int selectedItem = 0;
    public bool openInventory = false;

    private void Awake()
    {
        inventory = Inventory.GetInventory();
        itemListRect = itemList.GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdateItemList();
        inventory.onUpdated += UpdateItemList;
    }

    private void OnDestroy()
    {
        inventory.onUpdated -= UpdateItemList;
    }

    void UpdateItemList()
    {
        slotUIList.Clear();
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

        for (int i = 0; i < inventory.Slots.Count; i++)
        {
            var slot = inventory.Slots[i];
            var slotUIObj = slotUIList[i];

            if (slot.Count > 1)
            {
                slotUIObj.ShowCount();
            }
            else
            {
                slotUIObj.HideCount();
            }
        }

        UpdateItemSelectionInventory();
    }

    public void HandleUpdate(Action onBack)
    {
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Escape))
        {
            onBack?.Invoke();
            foreach (var slotUI in slotUIList)
            {
                slotUI.HideSelectBorder();
            }
            cover.enabled = true;
            ResetItemData();
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

        HandleScrolling(14);
    }

    void ResetItemData()
    {
        var slot = inventory.Slots[0];
        image.sprite = slot.Item.itemImage;
        itemTitle.text = slot.Item.itemName;
        description.text = slot.Item.itemDescription;

        openInventory = false;
        selectedItem = 0;

        HandleScrolling(1);
    }

    private void HandleScrolling(int itemInViewPort)
    {
        float scrollPos = Mathf.Clamp(selectedItem - itemInViewPort / 2, 0, inventory.Slots.Count - 1) * slotUIList[0].Height;
        itemListRect.localPosition = new Vector2(itemListRect.localPosition.x, scrollPos);
    }
}
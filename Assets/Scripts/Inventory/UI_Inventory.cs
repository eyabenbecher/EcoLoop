using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using CodeMonkey.Utils;
public class UI_Inventory : MonoBehaviour
{
    public Transform pfUI_Item;
    private Inventory inventory;
    [SerializeField] private Transform itemSlotContainer;
    [SerializeField] private Transform itemSlotTemplate;
    private PlayerInventory playerInventory;

    private void Awake()
    {
        itemSlotContainer = transform.Find("itemSlotContainer");
        itemSlotTemplate = itemSlotContainer.Find("itemSlotTemplate");
        itemSlotTemplate.gameObject.SetActive(false);
    }
    public void SetInventory(Inventory inventory)
    {


        this.inventory = inventory;
        inventory.OnItemListChanged += Inventory_OnItemListChanged;
        RefreshInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, EventArgs e)
    {
        RefreshInventoryItems();
    }
    public void SetPlayer(PlayerInventory player)
    {
        this.playerInventory = player;
    }
    public void RefreshInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x = 0;
        int y = 0;
        float itemSlotCellSize = 100f;


        foreach (Inventory.InventorySlot inventorySlot in inventory.GetInventorySlotArray())
        {
            Item item = inventorySlot.GetItem();

            RectTransform itemSlotRectTransform = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);

            itemSlotRectTransform.GetComponent<Button_UI>().ClickFunc = () => {
                // Use item
                //inventory.UseItem(item);
            };
            itemSlotRectTransform.GetComponent<Button_UI>().MouseRightClickFunc = () => {
                // Drop item
                //Item duplicateItem = new Item { itemType = item.itemType, amount = item.amount };
                //inventory.RemoveItem(item);
                //ItemWorld.DropItem(player.GetPosition(), duplicateItem);
            };

            itemSlotRectTransform.anchoredPosition = new Vector2(x * itemSlotCellSize, -y * itemSlotCellSize);

            if (!inventorySlot.IsEmpty())
            {
                // Not Empty, has Item
                Transform uiItemTransform = Instantiate(pfUI_Item, itemSlotContainer);
                uiItemTransform.GetComponent<RectTransform>().anchoredPosition = itemSlotRectTransform.anchoredPosition;
                UI_Item uiItem = uiItemTransform.GetComponent<UI_Item>();
                uiItem.SetItem(item);
                uiItem.SetSprite(item.GetSprite());
            }

            Inventory.InventorySlot tmpInventorySlot = inventorySlot;

            UI_ItemSlot uiItemSlot = itemSlotRectTransform.GetComponent<UI_ItemSlot>();
            uiItemSlot.SetOnDropAction(() => {
                // Dropped on this UI Item Slot
                Item draggedItem = UI_ItemDrag.Instance.GetItem();
                inventory.AddItem(draggedItem, tmpInventorySlot);
            });
            //foreach(Item item in inventory.GetItemList())
            //{
            //   RectTransform itemSlotRectTransform= Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            //    itemSlotRectTransform.gameObject.SetActive(true);

            //    itemSlotRectTransform.anchoredPosition=new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);


            //    Image image= itemSlotRectTransform.Find("image").GetComponent<Image>();
            //    image.sprite =item.GetSprite();
            //    TextMeshProUGUI uiText= itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            //    if(item.amount>1 )
            //    {
            //        uiText.SetText(item.amount.ToString());
            //    }
            //    else { uiText.SetText(""); }
            x++;
            if (x > 2)
            {
                x = 0;
                y--;
            }
        }
    }
}
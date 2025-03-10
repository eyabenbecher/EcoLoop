///* 
//    ------------------- Code Monkey -------------------

//    Thank you for downloading this package
//    I hope you find it useful in your projects
//    If you have any questions let me know
//    Cheers!

//               unitycodemonkey.com
//    --------------------------------------------------
// */

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class UI_CraftingSystem : MonoBehaviour {

//    [SerializeField] private Transform pfUI_Item;

//    private Transform[,] slotTransformArray;
//    private Transform outputSlotTransform;
//    private Transform itemContainer;
//    private CraftingSystem craftingSystem;

//    private void Awake() {
//        Transform gridContainer = transform.Find("gridContainer");
//        itemContainer = transform.Find("itemContainer");

//        slotTransformArray = new Transform[CraftingSystem.GRID_SIZE, CraftingSystem.GRID_SIZE];

//        for (int x = 0; x < CraftingSystem.GRID_SIZE; x++) {
//            for (int y = 0; y < CraftingSystem.GRID_SIZE; y++) {
//                slotTransformArray[x, y] = gridContainer.Find("grid_" + x + "_" + y);
//                UI_CraftingItemSlot craftingItemSlot = slotTransformArray[x, y].GetComponent<UI_CraftingItemSlot>();
//                craftingItemSlot.SetXY(x, y);
//                craftingItemSlot.OnItemDropped += UI_CraftingSystem_OnItemDropped;
//            }
//        }

//        outputSlotTransform = transform.Find("outputSlot");

//        //CreateItem(0, 0, new Item { itemType = Item.ItemType.Diamond });
//        //CreateItem(1, 2, new Item { itemType = Item.ItemType.Wood });
//        //CreateItemOutput(new Item { itemType = Item.ItemType.Sword_Wood });
//    }

//    public void SetCraftingSystem(CraftingSystem craftingSystem) {
//        this.craftingSystem = craftingSystem;
//        craftingSystem.OnGridChanged += CraftingSystem_OnGridChanged;

//        UpdateVisual();
//    }

//    private void CraftingSystem_OnGridChanged(object sender, System.EventArgs e) {
//        UpdateVisual();
//    }

//    private void UI_CraftingSystem_OnItemDropped(object sender, UI_CraftingItemSlot.OnItemDroppedEventArgs e) {
//        //craftingSystem.TryAddItem(e.item, e.x, e.y);
//        if (craftingSystem == null)
//        {
//            Debug.LogError("Dropped item is null!");
//            return;
//        }
//        craftingSystem.TryAddItem(e.item, e.x, e.y);
//    }

//    private void UpdateVisual() {
//        // Clear old items
//        foreach (Transform child in itemContainer) {
//            Destroy(child.gameObject);
//        }

//        // Cycle through grid and spawn items
//        for (int x = 0; x < CraftingSystem.GRID_SIZE; x++) {
//            for (int y = 0; y < CraftingSystem.GRID_SIZE; y++) {
//                if (!craftingSystem.IsEmpty(x, y)) {
//                    CreateItem(x, y, craftingSystem.GetItem(x, y));
//                }
//            }
//        }

//        if (craftingSystem.GetOutputItem() != null) {
//            CreateItemOutput(craftingSystem.GetOutputItem());
//        }
//    }

//    private void CreateItem(int x, int y, Item item) {
//        Transform itemTransform = Instantiate(pfUI_Item, itemContainer);
//        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
//        itemRectTransform.anchoredPosition = slotTransformArray[x, y].GetComponent<RectTransform>().anchoredPosition;
//        itemTransform.GetComponent<UI_Item>().SetItem(item);
//    }

//    private void CreateItemOutput(Item item) {
//        Transform itemTransform = Instantiate(pfUI_Item, itemContainer);
//        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
//        itemRectTransform.anchoredPosition = outputSlotTransform.GetComponent<RectTransform>().anchoredPosition;
//        itemTransform.localScale = Vector3.one * 1.5f;
//        itemTransform.GetComponent<UI_Item>().SetItem(item);
//    }

//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public class UI_CraftingSystem : MonoBehaviour
{
    [SerializeField] private Transform pfUI_Item;

    private Transform[,] slotTransformArray;
    private Transform outputSlotTransform;
    private Transform itemContainer;
    private CraftingSystem craftingSystem;
    public InventorySlot[] craftingSlots; // ✅ Declare crafting slots

    private void Awake()
    {
        Transform gridContainer = transform.Find("gridContainer");
        itemContainer = transform.Find("itemContainer");

        slotTransformArray = new Transform[CraftingSystem.GRID_SIZE, CraftingSystem.GRID_SIZE];

        for (int x = 0; x < CraftingSystem.GRID_SIZE; x++)
        {
            for (int y = 0; y < CraftingSystem.GRID_SIZE; y++)
            {
                slotTransformArray[x, y] = gridContainer.Find("grid_" + x + "_" + y);
                UI_CraftingItemSlot craftingItemSlot = slotTransformArray[x, y].GetComponent<UI_CraftingItemSlot>();
                craftingItemSlot.SetXY(x, y);
                craftingItemSlot.OnItemDropped += UI_CraftingSystem_OnItemDropped;
            }
        }

        outputSlotTransform = transform.Find("outputSlot");
        
    }

    public void SetCraftingSystem(CraftingSystem craftingSystem)
    {
        if(craftingSystem == null)
        {
            Debug.LogError("Crafting system is NULL! Initialization failed.");
            return;
        }

        this.craftingSystem = craftingSystem;
        craftingSystem.OnGridChanged += CraftingSystem_OnGridChanged;

        Debug.Log("✅ Crafting system successfully set in UI_CraftingSystem."); // ✅ Debugging line

        UpdateVisual();
    }

    private void CraftingSystem_OnGridChanged(object sender, System.EventArgs e)
    {

        RefreshUI();
    }

    private void UI_CraftingSystem_OnItemDropped(object sender, UI_CraftingItemSlot.OnItemDroppedEventArgs e)
    {
        if (craftingSystem == null)
        {
            Debug.LogError("Crafting system is NULL! Make sure SetCraftingSystem() is called.");
            return;
        }

        if (e.item == null)
        {
            Debug.LogError("Dropped item is NULL! UI_ItemDrag did not provide an item.");
            return;
        }

        Inventory.Instance.RemoveItem(e.item);

        if (!craftingSystem.TryAddItem(e.item, e.x, e.y))
        {
            Inventory.Instance.AddItem(e.item); // Return item if placement fails
        }

        RefreshUI(); // Refresh UI after item drop
    }



    private void UpdateVisual()
    {
        if (itemContainer == null)
        {
            Debug.LogError("Item container is NULL! Check UI hierarchy.");
            return;
        }

        // Clear old items
        foreach (Transform child in itemContainer)
        {
            Destroy(child.gameObject);
        }

        // Cycle through grid and spawn items
        for (int x = 0; x < CraftingSystem.GRID_SIZE; x++)
        {
            for (int y = 0; y < CraftingSystem.GRID_SIZE; y++)
            {
                if (!craftingSystem.IsEmpty(x, y))
                {
                    CreateItem(x, y, craftingSystem.GetItem(x, y));
                }
            }
        }

        if (craftingSystem.GetOutputItem() != null)
        {
            CreateItemOutput(craftingSystem.GetOutputItem());
        }
    }

    private void CreateItem(int x, int y, Item item)
    {
        if (item == null)
        {
            Debug.LogError($"Trying to create a NULL item at slot ({x},{y})");
            return;
        }

        Transform itemTransform = Instantiate(pfUI_Item, itemContainer);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        itemRectTransform.anchoredPosition = slotTransformArray[x, y].GetComponent<RectTransform>().anchoredPosition;
        itemTransform.localScale = Vector3.one * 0.40f;
        itemTransform.GetComponent<UI_Item>().SetItem(item);
    }

    private void CreateItemOutput(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Trying to create a NULL output item.");
            return;
        }

        Transform itemTransform = Instantiate(pfUI_Item, itemContainer);
        RectTransform itemRectTransform = itemTransform.GetComponent<RectTransform>();
        itemRectTransform.anchoredPosition = outputSlotTransform.GetComponent<RectTransform>().anchoredPosition;
        itemTransform.localScale = Vector3.one * 1.5f;
        itemTransform.GetComponent<UI_Item>().SetItem(item);
    }
    public void MoveItemBackToInventory(int craftingSlotIndex)
    {
        if (craftingSlots[craftingSlotIndex].IsEmpty()) return; // If empty, do nothing

        Item itemToMove = craftingSlots[craftingSlotIndex].GetItem();

        // Try adding item back to inventory
        InventorySlot availableSlot = Inventory.Instance.GetFirstAvailableSlot();
        if (availableSlot != null)
        {
            Inventory.Instance.AddItem(itemToMove, availableSlot);
            craftingSlots[craftingSlotIndex].RemoveItem(); // ✅ Remove from crafting slot
            UpdateCraftingUI();  // ✅ Refresh UI to reflect changes
        }
        else
        {
            Debug.LogWarning("No available inventory slot for returning item.");
        }
    }
    private void UpdateCraftingUI()
    {
        RefreshUI();  // Ensure UI is updated properly
        Debug.Log("Crafting UI updated.");
    }
    public void RefreshUI()
    {
        UpdateVisual();  // Calls the function that updates the crafting grid UI
        Debug.Log("Crafting UI refreshed.");
    }


}

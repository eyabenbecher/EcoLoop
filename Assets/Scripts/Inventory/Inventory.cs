using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Inventory;

public class Inventory
{
    private static Inventory _Instance;
    public static Inventory Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new Inventory(null, 10); // Default inventory size
            }
            return _Instance;
        }
    }
    public event EventHandler OnItemListChanged;

    [SerializeField] private List<Item> itemList;
    private Action<Item> useItemAction;
    public InventorySlot[] inventorySlotArray;
    public Inventory(Action<Item> useItemAction, int inventorySlotCount)
    {
        this.useItemAction = useItemAction;
        itemList = new List<Item>();
        //AddItem(new Item { itemType=Item.ItemType.Paper,amount=1});
        //AddItem(new Item { itemType = Item.ItemType.PlasticCan, amount = 1 });
        //AddItem(new Item { itemType = Item.ItemType.Garbage, amount = 1 });
        inventorySlotArray = new InventorySlot[inventorySlotCount];
        for (int i = 0; i < inventorySlotCount; i++)
        {
            inventorySlotArray[i] = new InventorySlot(i);
        }
        _Instance = this;
    }
    //public Inventory(Action<Item> useItemAction, int inventorySlotCount)
    //{
    //    this.useItemAction = useItemAction;
    //    itemList = new List<Item>();

    //    // Initialize the inventorySlotArray with the given count
    //    inventorySlotArray = new InventorySlot[inventorySlotCount];

    //    // Loop through the slots and initialize each one
    //    for (int i = 0; i < inventorySlotCount; i++)
    //    {
    //        inventorySlotArray[i] = new InventorySlot(i); // Initialize each slot here
    //    }
    //}
    public InventorySlot GetFirstAvailableSlot()
    {
        foreach (InventorySlot slot in inventorySlotArray)
        {
            if (slot.IsEmpty())
            {
                return slot;
            }
        }
        return null; // No available slots
    }

    public InventorySlot GetInventorySlotWithItem(Item item)
    {
        foreach (InventorySlot inventorySlot in inventorySlotArray)
        {
            if (inventorySlot.GetItem() == item)
            {
                return inventorySlot;
            }
        }
        Debug.LogError("Cannot find Item " + item + " in a InventorySlot!");
        return null;
    }

    public void RemoveItem(Item item)
    {
        InventorySlot slot = GetInventorySlotWithItem(item);
        if (slot == null)
        {
            Debug.LogWarning("Tried to remove an item that doesn't exist in inventory.");
            return;
        }

        if (item.IsStackable())
        {
            slot.GetItem().amount -= item.amount;
            if (slot.GetItem().amount <= 0)
            {
                slot.RemoveItem(); // Clear the slot only when the count reaches zero
                itemList.Remove(item);
            }
        }
        else
        {
            slot.RemoveItem();
            itemList.Remove(item);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool TryAddItem(Item item)
    {
        if (item == null) return false;

        // Try to stack the item in an existing slot
        foreach (InventorySlot inventorySlot in inventorySlotArray) // Corrected name
        {
            if (inventorySlot.GetItem() != null && inventorySlot.GetItem().itemType == item.itemType)
            {
                inventorySlot.GetItem().amount += item.amount;
                return true;
            }
        }

        // If no suitable slot is found, return false
        return false;
    }

    //public void RemoveItem(Item item)
    //{
    //    if (item.IsStackable())
    //    {
    //        Item itemInInventory = null;
    //        foreach (Item inventoryItem in itemList)
    //        {
    //            if (inventoryItem.itemType == item.itemType)
    //            {
    //                inventoryItem.amount -= item.amount;
    //                itemInInventory = inventoryItem;
    //            }
    //        }
    //        if (itemInInventory != null && itemInInventory.amount <= 0)
    //        {
    //            GetInventorySlotWithItem(itemInInventory).RemoveItem();
    //            itemList.Remove(itemInInventory);
    //        }
    //    }
    //    else
    //    {
    //        GetInventorySlotWithItem(item).RemoveItem();
    //        itemList.Remove(item);
    //    }
    //    OnItemListChanged?.Invoke(this, EventArgs.Empty);
    //}
    ////public void AddItem(Item item, InventorySlot inventorySlot)
    ////{
    ////RemoveItem(item);

    ////itemList.Add(item);
    ////inventorySlot.SetItem(item);

    ////OnItemListChanged?.Invoke(this, EventArgs.Empty);public void AddItem(Item item, InventorySlot inventorySlot)

    ////if (item.IsStackable())
    ////{
    ////    foreach (Item inventoryItem in itemList)
    ////    {
    ////        if (inventoryItem.itemType == item.itemType)
    ////        {
    ////            inventoryItem.amount += item.amount; // Increase the stack count

    ////            // Update the slot that already holds this item
    ////            InventorySlot existingSlot = GetInventorySlotWithItem(inventoryItem);
    ////            if (existingSlot != null)
    ////            {
    ////                existingSlot.SetItem(inventoryItem);
    ////            }

    ////            OnItemListChanged?.Invoke(this, EventArgs.Empty);
    ////            return; // Exit to avoid adding a new entry
    ////        }
    ////    }
    ////}

    ////// If the item is new (not stacked), add it to an empty slot
    ////if (inventorySlot.IsEmpty())
    ////{
    ////    inventorySlot.SetItem(item);
    ////    itemList.Add(item);
    ////}
    ////else
    ////{
    ////    Debug.LogError("Tried to add item to a non-empty slot: " + inventorySlot.GetItem().itemType);
    ////}

    ////OnItemListChanged?.Invoke(this, EventArgs.Empty);
    ////}
    //public void AddItem(Item item, InventorySlot inventorySlot)
    //{
    //    if (item == null)
    //    {
    //        Debug.LogError("Cannot add a null item to the inventory.");
    //        return; // Exit early if the item is null
    //    }

    //    if (item.IsStackable())
    //    {
    //        foreach (Item inventoryItem in itemList)
    //        {
    //            if (inventoryItem.itemType == item.itemType)
    //            {
    //                inventoryItem.amount += item.amount; // Increase the stack count

    //                // Update the slot that already holds this item
    //                InventorySlot existingSlot = GetInventorySlotWithItem(inventoryItem);
    //                if (existingSlot != null)
    //                {
    //                    existingSlot.SetItem(inventoryItem);
    //                }

    //                OnItemListChanged?.Invoke(this, EventArgs.Empty);
    //                return; // Exit to avoid adding a new entry
    //            }
    //        }
    //    }

    //    // If the item is new (not stacked), add it to an empty slot
    //    if (inventorySlot.IsEmpty())
    //    {
    //        inventorySlot.SetItem(item);
    //        itemList.Add(item);
    //    }
    //    else
    //    {
    //        Debug.LogError("Tried to add item to a non-empty slot: " + inventorySlot.GetItem().itemType);
    //    }

    //    OnItemListChanged?.Invoke(this, EventArgs.Empty);
    //}
    public void AddItem(Item item, InventorySlot inventorySlot)
    {
        if (item == null)
        {
            Debug.LogError("Cannot add a null item to the inventory.");
            return;
        }

        // Check if the item already exists in a slot and remove it first
        InventorySlot oldSlot = GetInventorySlotWithItem(item);
        if (oldSlot != null)
        {
            oldSlot.RemoveItem();
        }

        if (item.IsStackable())
        {
            foreach (InventorySlot slot in inventorySlotArray)
            {
                if (!slot.IsEmpty() && slot.GetItem().itemType == item.itemType)
                {
                    slot.GetItem().amount += item.amount; // Stack items
                    OnItemListChanged?.Invoke(this, EventArgs.Empty);
                    return; // Exit early to avoid duplication
                }
            }
        }

        // Find an empty slot and assign the item
        if (inventorySlot.IsEmpty())
        {
            inventorySlot.SetItem(item);
            itemList.Add(item);
        }
        else
        {
            Debug.LogError("Tried to add item to a non-empty slot: " + inventorySlot.GetItem().itemType);
        }

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }



    public void AddItem(Item item) {
        if (item.IsStackable())
        {   bool itemAlreadyInInventory = false;
            foreach (Item inventoryItem in itemList)
            {
                if (inventoryItem.itemType == item.itemType)
                {
                    inventoryItem.amount += item.amount;
                    itemAlreadyInInventory = true;
                }
            }
            if (!itemAlreadyInInventory) {  itemList.Add(item); }
        }
        
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    
    }
    public List<Item> GetItemList() {
        return itemList;
    }
    public InventorySlot[] GetInventorySlotArray()
    {
        return inventorySlotArray;
    }
    public class InventorySlot
    {
        private int index;
        private Item item;

        public InventorySlot(int index)
        {
            this.index = index;
        }

        public Item GetItem()
        {
            return item;
        }

        public void SetItem(Item item)
        {
            this.item = item;
        }

        public void RemoveItem()
        {
            item = null;
        }

        public bool IsEmpty()
        {
            return item == null;
        }

        public int GetIndex()
        {
            return index;  // Returns the slot index for reference
        }
    }

    public void UseItem(Item item)
    {
        useItemAction(item); 

    }
    public void SwapItems(InventorySlot slotA, InventorySlot slotB)
    {
        if (slotA == slotB) return; // No need to swap same slot

        Item tempItem = slotA.GetItem();
        slotA.SetItem(slotB.GetItem());
        slotB.SetItem(tempItem);

        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }

}

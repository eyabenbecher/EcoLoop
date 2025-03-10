using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : IItemHolder
{

    public const int GRID_SIZE = 3;

    public event EventHandler OnGridChanged;

    private Dictionary<Item.ItemType, Item.ItemType[,]> recipeDictionary;

    private Item[,] itemArray;
    private Item outputItem;

    public CraftingSystem()
    {
        itemArray = new Item[GRID_SIZE, GRID_SIZE];

        recipeDictionary = new Dictionary<Item.ItemType, Item.ItemType[,]>();

        // Garbage
        Item.ItemType[,] recipe = new Item.ItemType[GRID_SIZE, GRID_SIZE];
        recipe[0, 2] = Item.ItemType.Bag; recipe[1, 2] = Item.ItemType.None; recipe[2, 2] = Item.ItemType.None;
        recipe[0, 1] = Item.ItemType.None; recipe[1, 1] = Item.ItemType.None; recipe[2, 1] = Item.ItemType.None;
        recipe[0, 0] = Item.ItemType.None; recipe[1, 0] = Item.ItemType.None; recipe[2, 0] = Item.ItemType.Paper;
        recipeDictionary[Item.ItemType.Garbage] = recipe;

        // Bag
        recipe = new Item.ItemType[GRID_SIZE, GRID_SIZE];
        recipe[0, 2] = Item.ItemType.Paper; recipe[1, 2] = Item.ItemType.None; recipe[2, 2] = Item.ItemType.None;
        recipe[0, 1] = Item.ItemType.None; recipe[1, 1] = Item.ItemType.None; recipe[2, 1] = Item.ItemType.None;
        recipe[0, 0] = Item.ItemType.None; recipe[1, 0] = Item.ItemType.None; recipe[2, 0] = Item.ItemType.Lpaper;
        recipeDictionary[Item.ItemType.Bag] = recipe;

        // Diamond Sword
        recipe = new Item.ItemType[GRID_SIZE, GRID_SIZE];
        recipe[0, 2] = Item.ItemType.Paper; recipe[1, 2] = Item.ItemType.None; recipe[2, 2] = Item.ItemType.None;
        recipe[0, 1] = Item.ItemType.None; recipe[1, 1] = Item.ItemType.Scissors; recipe[2, 1] = Item.ItemType.None;
        recipe[0, 0] = Item.ItemType.None; recipe[1, 0] = Item.ItemType.None; recipe[2, 0] = Item.ItemType.None;
        recipeDictionary[Item.ItemType.Lpaper] = recipe;
    }

    public bool IsEmpty(int x, int y)
    {
        return itemArray[x, y] == null;
    }

    public Item GetItem(int x, int y)
    {
        return itemArray[x, y];
    }

    //public void SetItem(Item item, int x, int y) {
    //    if (item != null) {
    //        item.RemoveFromItemHolder();
    //        item.SetItemHolder(this);
    //    }
    //    itemArray[x, y] = item;
    //    CreateOutput();
    //    OnGridChanged?.Invoke(this, EventArgs.Empty);
    //}
    public void SetItem(Item item, int x, int y)
    {
        if (item != null)
        {
            item.RemoveFromItemHolder();
            item.SetItemHolder(this);

            // Ensure the slot gets a NEW instance of the item, preventing shared references
            itemArray[x, y] = new Item { itemType = item.itemType, amount = item.amount };
        }
        else
        {
            itemArray[x, y] = null;
        }

        CreateOutput();
        OnGridChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseItemAmount(int x, int y)
    {
        GetItem(x, y).amount++;
        OnGridChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseItemAmount(int x, int y)
    {
        if (GetItem(x, y) != null)
        {
            GetItem(x, y).amount--;
            if (GetItem(x, y).amount == 0)
            {
                RemoveItem(x, y);
            }
            OnGridChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void RemoveItem(int x, int y)
    {
        SetItem(null, x, y);
    }

    //public bool TryAddItem(Item item, int x, int y) {
    //    if (IsEmpty(x, y)) {
    //        SetItem(item, x, y);
    //        return true;
    //    } else {
    //        if (item.itemType == GetItem(x, y).itemType) {
    //            IncreaseItemAmount(x, y);
    //            return true;
    //        } else {
    //            return false;
    //        }
    //    }
    //}
    public bool TryAddItem(Item item, int x, int y)
    {
        if (item == null) return false;

        // Remove item from any other slots before placing it here
        for (int i = 0; i < GRID_SIZE; i++)
        {
            for (int j = 0; j < GRID_SIZE; j++)
            {
                if (GetItem(i, j) == item)
                {
                    RemoveItem(i, j);
                }
            }
        }

        if (IsEmpty(x, y))
        {
            SetItem(item, x, y);
            return true;
        }
        else if (item.itemType == GetItem(x, y).itemType)
        {
            IncreaseItemAmount(x, y);
            return true;
        }

        return false;
    }


    //public void RemoveItem(Item item) {
    //    if (item == outputItem) {
    //        // Removed output item
    //        ConsumeRecipeItems();
    //        CreateOutput();
    //        OnGridChanged?.Invoke(this, EventArgs.Empty);
    //    } else {
    //        // Removed item from grid
    //        for (int x = 0; x < GRID_SIZE; x++) {
    //            for (int y = 0; y < GRID_SIZE; y++) {
    //                if (GetItem(x, y) == item) {
    //                    // Removed this one
    //                    RemoveItem(x, y);
    //                }
    //            }
    //        }
    //    }
    //}
    public void RemoveItem(Item item)
    {
        if (item == outputItem)
        {
            // 🔥 Output item is removed; consume ingredients
            ConsumeRecipeItems();
            CreateOutput();
            OnGridChanged?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            // 🔥 Return item back to inventory if removed from crafting grid
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    if (GetItem(x, y) == item)
                    {
                        Inventory.Instance.AddItem(item); // Return item to inventory
                        RemoveItem(x, y); // Remove from crafting grid
                    }
                }
            }
        }
    }

    public void AddItem(Item item) { }

    public bool CanAddItem() { return false; }


    //private Item.ItemType GetRecipeOutput() {
    //    foreach (Item.ItemType recipeItemType in recipeDictionary.Keys) {
    //        Item.ItemType[,] recipe = recipeDictionary[recipeItemType];

    //        bool completeRecipe = true;
    //        for (int x = 0; x < GRID_SIZE; x++) {
    //            for (int y = 0; y < GRID_SIZE; y++) {
    //                if (recipe[x, y] != Item.ItemType.None) {
    //                    // Recipe has Item in this position
    //                    if (IsEmpty(x, y) || GetItem(x, y).itemType != recipe[x, y]) {
    //                        // Empty position or different itemType
    //                        completeRecipe = false;
    //                    }
    //                }
    //            }
    //        }

    //        if (completeRecipe) {
    //            return recipeItemType;
    //        }
    //    }
    //    return Item.ItemType.None;
    //}
    private Item.ItemType GetRecipeOutput()
    {
        foreach (Item.ItemType recipeItemType in recipeDictionary.Keys)
        {
            Item.ItemType[,] recipe = recipeDictionary[recipeItemType];

            bool completeRecipe = true;
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    if (recipe[x, y] != Item.ItemType.None)
                    {
                        // Compare item types instead of checking direct references
                        if (IsEmpty(x, y) || GetItem(x, y).itemType != recipe[x, y])
                        {
                            completeRecipe = false;
                        }
                    }
                }
            }

            if (completeRecipe)
            {
                Debug.Log($"✅ Recipe matched! Output: {recipeItemType}");
                return recipeItemType;
            }
        }
        return Item.ItemType.None;
    }


    private void CreateOutput()
    {
        Item.ItemType recipeOutput = GetRecipeOutput();
        if (recipeOutput == Item.ItemType.None)
        {
            outputItem = null;
        }
        else
        {
            outputItem = new Item { itemType = recipeOutput };
            outputItem.SetItemHolder(this);
        }
    }

    public Item GetOutputItem()
    {
        return outputItem;
    }

    //public void ConsumeRecipeItems() {
    //    for (int x = 0; x < GRID_SIZE; x++) {
    //        for (int y = 0; y < GRID_SIZE; y++) {
    //            DecreaseItemAmount(x, y);
    //        }
    //    }
    //}
    public void ConsumeRecipeItems()
    {
        for (int x = 0; x < GRID_SIZE; x++)
        {
            for (int y = 0; y < GRID_SIZE; y++)
            {
                if (!IsEmpty(x, y))
                {
                    RemoveItem(x, y); // 🔥 Remove item from crafting grid
                }
            }
        }
        OnGridChanged?.Invoke(this, EventArgs.Empty);
    }

}
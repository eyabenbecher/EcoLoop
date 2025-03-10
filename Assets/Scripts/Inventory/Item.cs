using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class Item 
{
  public enum ItemType
    {
        None,
        Paper,
        PlasticCan,
        Garbage,
        Lpaper,
        Bag,
        Scissors,
    }
    public ItemType itemType;
    public int amount;
    private IItemHolder itemHolder;
    public static Sprite GetSprite(ItemType itemType)
    {
        switch(itemType)
        {
            default:
            case ItemType.Paper:return ItemAssets.Instance.paperSprite;
            case ItemType.PlasticCan:return ItemAssets.Instance.plasticcanSprite;
            case ItemType.Garbage: return ItemAssets.Instance.garbageSprite;
            case ItemType.Lpaper: return ItemAssets.Instance.lpaperSprite;
            case ItemType.Bag: return ItemAssets.Instance.bagSprite;
            case ItemType.Scissors: return ItemAssets.Instance.scissorsSprite;

        }
    }
    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Paper: 
            case ItemType.PlasticCan:
            case ItemType.Garbage:
            case ItemType.Lpaper:
            case ItemType.Bag:
            case ItemType.Scissors:
                return true;

        }
    }
    public void RemoveFromItemHolder()
    {
        if (itemHolder != null)
        {
            // Remove from current Item Holder
            itemHolder.RemoveItem(this);
        }
    }
    public void SetItemHolder(IItemHolder itemHolder)
    {
        this.itemHolder = itemHolder;
    }
    public IItemHolder GetItemHolder()
    {
        return itemHolder;
    }
    public Sprite GetSprite()
    {
        return GetSprite(itemType);
    }
}

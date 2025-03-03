using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[Serializable]
public class Item 
{
  public enum ItemType
    {
        Paper,
        PlasticCan,
        Garbage,
        Lpaper,
        Bag,
        Scissors,
    }
    public ItemType itemType;
    public int amount;
    public Sprite GetSprite()
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
}

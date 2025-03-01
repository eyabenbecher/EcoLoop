using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
  public enum ItemType
    {
        Paper,
        PlasticCan,
        Garbage,
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
        }
    }
}

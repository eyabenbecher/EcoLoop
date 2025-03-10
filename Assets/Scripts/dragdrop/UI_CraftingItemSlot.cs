/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_CraftingItemSlot : MonoBehaviour, IDropHandler
{

    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
    public class OnItemDroppedEventArgs : EventArgs
    {
        public Item item;
        public int x;
        public int y;
    }

    private int x;
    private int y;

    public void OnDrop(PointerEventData eventData)
    {
        UI_ItemDrag.Instance.Hide();
        Item item = UI_ItemDrag.Instance.GetItem();
        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = item, x = x, y = y });

                Debug.Log($"Item dropped at ({x},{y}): {item.itemType}");
    }

    public void SetXY(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}
//using System;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class UI_CraftingItemSlot : MonoBehaviour, IDropHandler
//{
//    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;

//    public class OnItemDroppedEventArgs : EventArgs
//    {
//        public Item item;
//        public int x;
//        public int y;
//    }

//    private int x;
//    private int y;

//    public void OnDrop(PointerEventData eventData)
//    {
//        if (UI_ItemDrag.Instance == null)
//        {
//            Debug.LogError("UI_ItemDrag.Instance is NULL! Make sure it exists in the scene.");
//            return;
//        }

//        UI_ItemDrag.Instance.Hide();
//        Item item = UI_ItemDrag.Instance.GetItem();

//        if (item == null)
//        {
//            Debug.LogError("Dropped item is NULL! UI_ItemDrag did not provide an item.");
//            return;
//        }

//        Debug.Log($"Item dropped at ({x},{y}): {item.itemType}");
//        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = item, x = x, y = y });
//    }

//    public void SetXY(int x, int y)
//    {
//        this.x = x;
//        this.y = y;
//    }
//}


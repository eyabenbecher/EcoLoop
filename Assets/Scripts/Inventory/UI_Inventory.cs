using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Inventory : MonoBehaviour
{
   private Inventory inventory;
    [SerializeField] private Transform itemSlotContainer;  
    [SerializeField] private Transform itemSlotTemplate;

    private void Awake()
    {
        
       
      
      
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

    private void RefreshInventoryItems()

    {
        foreach(Transform child in itemSlotContainer)
        {
            if(child==itemSlotTemplate) continue;
            Destroy(child.gameObject);
        }
        int x= 0;
        int y= 0;
        float itemSlotCellSize = 100f;
        foreach(Item item in inventory.GetItemList())
        {
           RectTransform itemSlotRectTransform= Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRectTransform.gameObject.SetActive(true);
            itemSlotRectTransform.anchoredPosition=new Vector2(x * itemSlotCellSize, y * itemSlotCellSize);
            Image image= itemSlotRectTransform.Find("image").GetComponent<Image>();
            image.sprite =item.GetSprite();
            TextMeshProUGUI uiText= itemSlotRectTransform.Find("text").GetComponent<TextMeshProUGUI>();
            if(item.amount>1 )
            {
                uiText.SetText(item.amount.ToString());
            }
            else { uiText.SetText(""); }
            x++;
            if(x>2) { 
             x = 0;
            y--;}
        }
    }
}

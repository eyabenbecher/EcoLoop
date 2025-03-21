﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour {

     private PlayerInventory player;
    [SerializeField] private UI_Inventory uiInventory;

    //[SerializeField] private UI_CharacterEquipment uiCharacterEquipment;
    //[SerializeField] private CharacterEquipment characterEquipment;

    [SerializeField] private UI_CraftingSystem uiCraftingSystem;

    private void Start() {
        //uiInventory.SetPlayer(player);
        //uiInventory.SetInventory(player.GetInventory());

        //uiCharacterEquipment.SetCharacterEquipment(characterEquipment);

        CraftingSystem craftingSystem = new CraftingSystem();
        //Item item = new Item { itemType = Item.ItemType.Diamond, amount = 1 };
        //craftingSystem.SetItem(item, 0, 0);
        //Debug.Log(craftingSystem.GetItem(0, 0));

        uiCraftingSystem.SetCraftingSystem(craftingSystem);
    }

}

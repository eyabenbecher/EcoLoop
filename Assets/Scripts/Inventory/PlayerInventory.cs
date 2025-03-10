using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update
    private Inventory inventory;
    [SerializeField] private UI_Inventory uiInventory;
    private void Awake()
    {
        inventory=new Inventory(UseItem,6);
        uiInventory.SetInventory(inventory);
       
        for (int i = 0; i < 50; i++) 
        {
            // Paper
            ItemWorld.SpawnItemWorld(new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), 0), new Item { itemType = Item.ItemType.Paper, amount = 1 });

            // Garbage
            ItemWorld.SpawnItemWorld(new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), 0), new Item { itemType = Item.ItemType.Garbage, amount = 1 });

            // PlasticCan
            ItemWorld.SpawnItemWorld(new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), 0), new Item { itemType = Item.ItemType.PlasticCan, amount = 1 });

            // Lpaper
            ItemWorld.SpawnItemWorld(new Vector3(Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f), 0), new Item { itemType = Item.ItemType.Lpaper, amount = 1 });

            // Bag - Increase its size by scaling
            var bagItemWorld = ItemWorld.SpawnItemWorld(new Vector3(Random.Range(-1000f, 1000f), Random.Range(-500f, 500f), 0), new Item { itemType = Item.ItemType.Bag, amount = 1 });
            bagItemWorld.transform.localScale = new Vector3(50f, 50f, 30f); // Increase the size of the Bag
            
            // Scissors
            ItemWorld.SpawnItemWorld(new Vector3(Random.Range(-1000f, 1000f), Random.Range(-500f, 500f), 0), new Item { itemType = Item.ItemType.Scissors, amount = 1 });
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        //if (itemWorld != null )
        //{
        //    int index = 2;
        //    //inventory.AddItem(itemWorld.GetItem());
        //    inventory.AddItem(itemWorld.GetItem(),ui);
        //    itemWorld.DestroySelf();
        //}
        ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
        if (itemWorld != null)
        {
            // Find the first empty slot in inventory
            Inventory.InventorySlot availableSlot = null;
            foreach (Inventory.InventorySlot slot in inventory.GetInventorySlotArray())
            {
                if (slot.IsEmpty())
                {
                    availableSlot = slot;
                    break;
                }
            }

            if (availableSlot != null)
            {
                inventory.AddItem(itemWorld.GetItem(), availableSlot);
                itemWorld.DestroySelf();
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }

    }
    public Inventory GetInventory()
    {
        return inventory;
    }
    private void UseItem(Item item)
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

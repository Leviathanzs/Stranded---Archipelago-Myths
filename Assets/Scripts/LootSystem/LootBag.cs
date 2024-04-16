using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public Inventory inventory;

    public GameObject droppedItemPrefab;
    public List<Item> lootList = new List<Item>();

    protected virtual Item GetDroppedItem()
    {
        int randomNumber = Random.Range(1, 101);
        List<Item> possibleItems = new List<Item>();
        foreach(Item item in lootList)
        {
            if(randomNumber <= item.DropChance)
            {
                possibleItems.Add(item);
            }
        } 
        if(possibleItems.Count > 0)
        {
            Item droppedItem = possibleItems[Random.Range(0, possibleItems.Count)]; 
            return droppedItem;
        }
        Debug.Log("No loot dropped");
        return null;
    }

    virtual public void InstantiateLoot(Vector2 spawnPosition)
    {
        Item droppedItem = GetDroppedItem();
        if(droppedItem != null)
        {
           inventory.AddItem(Instantiate(droppedItem));
        }
    }
}

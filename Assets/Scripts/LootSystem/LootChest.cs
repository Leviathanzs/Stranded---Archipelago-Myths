using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootChest : LootBag
{
    List<Item> GetDroppedItems()
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
        Debug.Log("No loot dropped");
        return possibleItems;
    }

    override public void InstantiateLoot(Vector2 spawnPosition)
    {
        List<Item> droppedItems = GetDroppedItems();
        if(droppedItems != null)
        {
            foreach (Item loot in droppedItems)
            {
                GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponent<SpriteRenderer>().sprite = loot.Icon;
            }
        }
    }
}

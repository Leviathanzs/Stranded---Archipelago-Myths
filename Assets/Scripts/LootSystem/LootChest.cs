using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootChest : LootBag
{
    List<Loot> GetDroppedItems()
    {
        int randomNumber = Random.Range(1, 101);
        List<Loot> possibleItems = new List<Loot>();
        foreach(Loot item in lootList)
        {
            if(randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        } 
        Debug.Log("No loot dropped");
        return possibleItems;
    }

    override public void InstantiateLoot(Vector2 spawnPosition)
    {
        List<Loot> droppedItems = GetDroppedItems();
        if(droppedItems != null)
        {
            foreach (Loot loot in droppedItems)
            {
                GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                lootGameObject.GetComponent<SpriteRenderer>().sprite = loot.lootSprite;
            }
        }
    }
}

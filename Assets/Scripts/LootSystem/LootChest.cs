using System.Collections.Generic;
using UnityEngine;

public class LootChest : LootBag
{
    protected override List<Item> GetDroppedItems()
    {
        List<Item> possibleItems = new List<Item>();

        foreach (LootEntry entry in lootList)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= entry.dropChance)
            {
                possibleItems.Add(entry.item);
            }
        }

        if (possibleItems.Count == 0)
        {
            Debug.Log("No loot dropped from chest.");
        }

        return possibleItems;
    }

    public virtual void InstantiateLoot(Vector2 spawnPosition)
    {
        List<Item> droppedItems = GetDroppedItems();

        foreach (Item item in droppedItems)
        {
            inventory.AddItem(item.GetCopy());
            ShowLootNotification(item); 
        }

        if (droppedItems.Count == 0)
        {
            Debug.Log("Tidak ada item yang didapat.");
        }
    }

}

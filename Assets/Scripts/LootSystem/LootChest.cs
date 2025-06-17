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
            if (entry.item != null && roll <= entry.dropChance)
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

    public override void InstantiateLoot(Vector2 spawnPosition)
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
        }

        if (inventory == null)
        {
            Debug.LogWarning("Inventory tidak ditemukan.");
            return;
        }

        List<Item> droppedItems = GetDroppedItems();

        foreach (Item item in droppedItems)
        {
            if (item == null)
            {
                Debug.LogWarning("Item null ditemukan di loot chest.");
                continue;
            }

            Item itemCopy = item.GetCopy();
            if (itemCopy == null)
            {
                Debug.LogWarning("GetCopy() dari item menghasilkan null.");
                continue;
            }

            bool added = inventory.AddItem(itemCopy);
            Debug.Log($"Item {item.name} ditambahkan: {added}");

            if (added)
            {
                ShowLootNotification(item);
            }
            else
            {
                Debug.LogWarning($"Inventory penuh atau gagal menambahkan item: {item.name}");
            }
        }

        if (droppedItems.Count == 0)
        {
            Debug.Log("Tidak ada item yang didapat dari chest.");
        }
    }
}

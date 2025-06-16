using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LootEntry
{
    public Item item;
    [Range(0f, 100f)] public float dropChance;
}

public class LootBag : MonoBehaviour
{
    public Inventory inventory;
    public List<LootEntry> lootList = new List<LootEntry>();

    // Mengembalikan list item yang berhasil didapat berdasarkan drop chance
    protected virtual List<Item> GetDroppedItems()
    {
        List<Item> droppedItems = new List<Item>();

        foreach (LootEntry entry in lootList)
        {
            float roll = Random.Range(0f, 100f);
            if (roll <= entry.dropChance)
            {
                droppedItems.Add(entry.item);
            }
        }

        return droppedItems;
    }

    // Fungsi ini dipanggil saat loot dilakukan
    public virtual void GiveLootToPlayer()
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

    // Menampilkan notifikasi loot
    protected virtual void ShowLootNotification(Item item)
    {
        if (LootNotificationManager.Instance != null)
        {
            LootNotificationManager.Instance.ShowNotification(item);
        }
        else
        {
            Debug.Log("Mendapat item: " + item.ItemName);
        }
    }

    public virtual void InstantiateLoot(Vector2 spawnPosition)
    {
        GiveLootToPlayer();
    }

}

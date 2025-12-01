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
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();

        if (inventory == null)
        {
            Debug.LogWarning("Inventory tidak ditemukan.");
            return;
        }

        List<Item> droppedItems = GetDroppedItems();

        // Ambil referensi PlayerStats
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        int playerLevel = playerStats != null ? playerStats.CurrentLevel : 1;

        foreach (Item item in droppedItems)
        {
            if (item == null) continue;

            Item itemCopy = item.GetCopy();
            if (itemCopy == null) continue;

            // Jika item adalah EquippableItem → randomize stats dengan fuzzy
            EquippableItem equip = itemCopy as EquippableItem;
            if (equip != null)
            {
                float rarityValue = (float)equip.Rarity;

                FuzzyItemStatGenerator generator = new FuzzyItemStatGenerator();
                var stats = generator.GenerateStats(playerLevel, rarityValue);

                // Flat bonus
                equip.StrenghtBonus = stats.strength;
                equip.AgilityBonus = stats.agility;
                equip.IntelligenceBonus = stats.intelligence;
                equip.VitalityBonus = stats.vitality;

                // Percent bonus
                equip.StrenghtPercentBonus = stats.strengthPercent;
                equip.AgilityPercentBonus = stats.agilityPercent;
                equip.IntelligencePercentBonus = stats.intelligencePercent;
                equip.VitalityPercentBonus = stats.vitalityPercent;
            }

            bool added = inventory.AddItem(itemCopy);
            if (added) ShowLootNotification(itemCopy);
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

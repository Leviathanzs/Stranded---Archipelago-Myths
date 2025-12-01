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

        // Ambil level player dari PlayerStats
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        int playerLevel = playerStats != null ? playerStats.CurrentLevel : 1;

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

            // 🔥 Integrasi fuzzy di sini
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
            Debug.Log($"Item {item.name} ditambahkan: {added}");

            if (added)
            {
                ShowLootNotification(itemCopy);
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

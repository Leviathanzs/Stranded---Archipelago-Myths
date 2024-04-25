using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public PlayerBaseStats Strenght;
    public PlayerBaseStats Agility;
    public PlayerBaseStats Intelligence;
    public PlayerBaseStats Vitality;

    private int originalMaxHealth = 100;

    // Current max health value after considering all equipped items
    private int currentMaxHealth = 100;

    // Current health value
    private int currentHealth = 100;

    private int originalMaxMana = 100;
    private int currentMaxMana = 100;
    private int currentMana = 100;


    private float _strenghtFinalValue;
    private float _agilityFinalValue;
    private float _intelligenceFinalValue;
    private float _vitalityFinalValue;
    public float StrenghtFinalValue {get { return _strenghtFinalValue; } set { _strenghtFinalValue = value;}}
    public float AgilityFinalValue {get { return _agilityFinalValue; } set { _agilityFinalValue = value;}}
    public float IntelligenceFinalValue {get { return _intelligenceFinalValue; } set { _intelligenceFinalValue = value;}}
    public float VitalityFinalValue {get { return _vitalityFinalValue; } set { _vitalityFinalValue = value;}}

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;
    [SerializeField] Damageable damageable;

    private ItemSlot draggedSlot;
    private Dictionary<EquipmentType, EquippableItem> equippedItems = new Dictionary<EquipmentType, EquippableItem>();

    private void OnValidate()
    {
        if(itemTooltip == null)
            itemTooltip = FindObjectOfType<ItemTooltip>();
    }

    private void Awake()
    {
        statPanel.SetStats(Strenght, Agility, Intelligence, Vitality);
        statPanel.UpdateStatValues();
        _strenghtFinalValue = Strenght.BaseValue;

        //setup Events:
        //Right Click
        inventory.OnRightClickEvent += Equip;
        equipmentPanel.OnRightClickEvent += Unequip;
        //Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        //PointerExit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        //Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        //End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        //Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        //Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
    }

    // Method to recalculate base stats based on equipped items
    public void RecalculateBaseStats()
    {
        // Accumulate bonuses from equipped items
        float strengthBonusTotal = 0;
        float agilityBonusTotal = 0;
        float intelligenceBonusTotal = 0;
        float vitalityBonusTotal = 0;

        foreach (var item in equippedItems.Values)
        {
            strengthBonusTotal += item.StrenghtBonus;
            agilityBonusTotal += item.AgilityBonus;
            intelligenceBonusTotal += item.IntelligenceBonus;
            vitalityBonusTotal += item.VitalityBonus;
        }

        // Apply accumulated bonuses
        _strenghtFinalValue = Strenght.BaseValue + strengthBonusTotal;
        _agilityFinalValue = Agility.BaseValue + agilityBonusTotal;
        _intelligenceFinalValue = Intelligence.BaseValue + intelligenceBonusTotal;
        _vitalityFinalValue = Vitality.BaseValue + vitalityBonusTotal;
    }

    private void Equip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;

        if(equippableItem != null)
        {
            Equip(equippableItem);
        }
    }

    private void Unequip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if(equippableItem != null)
        {
            Unequip(equippableItem);
        }
    }

    private void ShowTooltip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if(equippableItem != null)
        {
            itemTooltip.ShowTooltip(equippableItem);
        }
    }

    private void HideTooltip(ItemSlot itemSlot)
    {
        itemTooltip.HideTooltip();
    }

    private void BeginDrag(ItemSlot itemSlot)
    {
        if(itemSlot.Item != null)
        {
            draggedSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(ItemSlot itemSlot)
    {
        draggedSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(ItemSlot itemSlot)
    {
        if(draggableItem.enabled)
        {
             draggableItem.transform.position = Input.mousePosition;
        }
    }

    private void Drop(ItemSlot dropItemSlot)
    {
        if(draggedSlot == null) return;

        if(dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item))
        {
            EquippableItem dragItem = draggedSlot.Item as EquippableItem;
            EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

            if(draggedSlot is EquipmentSlot)
            {
                if(dragItem != null) dragItem.Unequip(this);
                if(dropItem != null) dragItem.Equip(this);
            }
            if(dropItemSlot is EquipmentSlot)
            {
                if(dragItem != null) dragItem.Equip(this);
                if(dropItem != null) dragItem.Unequip(this);
            }
            statPanel.UpdateStatValues();

            Item draggedItem = draggedSlot.Item;
            draggedSlot.Item = dropItemSlot.Item;
            dropItemSlot.Item = draggedItem;
        }
    }

    public void Equip(EquippableItem item)
    {
        EquipmentType equipmentType = item.EquipmentType;
        // Store the current health before equipping the item
        int previousHealth = currentHealth;
        int previousMana = currentMana;

        // Unequip previous item of the same type, if any
        if (equippedItems.ContainsKey(equipmentType))
        {
            Unequip(equippedItems[equipmentType]);
        }

        // Equip the new item
        item.Equip(this);
        equippedItems[equipmentType] = item;
        statPanel.UpdateStatValues();

        // Calculate the new maximum health
        int newMaxHealth = CalculateMaxHealth();

        if (previousHealth > currentMaxHealth)
        {
            // If previous health exceeds the new maximum health, set current health to the new maximum health
            currentHealth = currentMaxHealth;
        }
        else
        {
            // Otherwise, adjust current health proportionally to maintain the same percentage of health
           currentHealth = damageable.Health;
        }

        // Update the current and maximum health values
        currentMaxHealth = newMaxHealth;

        int newMaxMana = CalculateMaxMana();

        if (previousMana > currentMaxMana)
        {
            currentMana = currentMaxMana;
        }
        else
        {
            currentMana = damageable.Mana;
        }

        currentMaxMana = newMaxMana;

        // Update the inventory
        if (inventory.RemoveItem(item.ID))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    // Adjust current health if needed after unequipping the previous item
                    if (currentHealth > currentMaxHealth)
                    {
                        currentHealth = currentMaxHealth;
                    }

                    if (currentMana > currentMaxMana)
                    {
                        currentMana = currentMaxMana;
                    }

                    statPanel.UpdateStatValues();
                }
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }

        // Recalculate base stats and update health
        RecalculateBaseStats();
        // Update the UI and other necessary components
        statPanel.UpdateStatValues();
        UpdateBarInstance();
    }

    public void Unequip(EquippableItem item)
    {
        if(!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            inventory.AddItem(item);

            // Remove the item from the dictionary of equipped items
            equippedItems.Remove(item.EquipmentType);

            // Update the current max health value
            currentMaxHealth = CalculateMaxHealth();

            // Restore the current health value if it exceeds the new max health value
            currentHealth = Mathf.Min(currentHealth, currentMaxHealth);

            currentMaxMana = CalculateMaxMana();

            currentMana = Mathf.Min(currentMana, currentMaxMana);

            RecalculateBaseStats();
            statPanel.UpdateStatValues();
            UpdateBarInstance();
        }
    }

    private int CalculateMaxHealth()
    {
        int totalMaxHealth = originalMaxHealth;
        foreach (EquippableItem item in equippedItems.Values)
        {
            totalMaxHealth += item.StrenghtBonus * 2; // Add the HP increase from each equipped item
        }
        return totalMaxHealth;
    }

    private int CalculateMaxMana()
    {
        int totalMaxMana = originalMaxMana;
        foreach (EquippableItem item in equippedItems.Values)
        {
            totalMaxMana += item.IntelligenceBonus * 5;
        }

        return totalMaxMana;
    }

    void UpdateBarInstance()
    {
        HealthBar.barInstance.SetMaxHealth(CalculateMaxHealth(), currentHealth);
        HealthBar.barInstance.SetMaxMana(CalculateMaxMana(), currentMana);
    }
}

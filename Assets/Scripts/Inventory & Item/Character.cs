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

    public int originalMaxHealth;

    // Current max health value after considering all equipped items
    private int currentMaxHealth;

    // Current health value
    public int currentHealth;

    public int originalMaxMana;
    private int currentMaxMana;
    private int currentMana;


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
        
        _strenghtFinalValue = Strenght.BaseValue;

        //setup Events:
        //Right Click
        inventory.OnRightClickEvent += InventoryRightClick;
        equipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
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

   private void Start() 
    {
        originalMaxHealth = damageable.MaxHealth;
        currentMaxHealth = damageable.MaxHealth;
        currentHealth = damageable.Health;
        originalMaxMana = damageable.MaxMana;
        currentMaxMana = damageable.MaxMana;
        currentMaxMana = damageable.Mana;
        SetStartingHpMana();
        UpdateStatValues();
    }


    // Method to recalculate base stats based on equipped items
    public void RecalculateBaseStats()
    {
       // Clear current modifiers from equipped items
    Strenght.RemoveAllModifiersFromSource(this);
    Agility.RemoveAllModifiersFromSource(this);
    Intelligence.RemoveAllModifiersFromSource(this);
    Vitality.RemoveAllModifiersFromSource(this);

    // Accumulate bonuses from equipped items and add as modifiers
    foreach (var item in equippedItems.Values)
    {
        if (item.StrenghtBonus != 0)
            Strenght.AddModifier(new StatModifier(item.StrenghtBonus, StatModType.Flat, this));
        if (item.AgilityBonus != 0)
            Agility.AddModifier(new StatModifier(item.AgilityBonus, StatModType.Flat, this));
        if (item.IntelligenceBonus != 0)
            Intelligence.AddModifier(new StatModifier(item.IntelligenceBonus, StatModType.Flat, this));
        if (item.VitalityBonus != 0)
            Vitality.AddModifier(new StatModifier(item.VitalityBonus, StatModType.Flat, this));
    }

    // Update final values based on modifiers
    _strenghtFinalValue = Strenght.Value;
    _agilityFinalValue = Agility.Value;
    _intelligenceFinalValue = Intelligence.Value;
    _vitalityFinalValue = Vitality.Value;

    // Update UI or other dependent systems
    UpdateStatValues();
    }

    private void InventoryRightClick(ItemSlot itemSlot)
    {
        if(itemSlot.Item is EquippableItem)
        {
            Equip((EquippableItem)itemSlot.Item);
        }
        else if(itemSlot.Item is UsableItem)
        {
            UsableItem usableItem = (UsableItem)itemSlot.Item;
            usableItem.Use(damageable);
            usableItem.Use2(this);

            if(damageable.Health > damageable.MaxHealth)
            {
                damageable.Health = damageable.MaxHealth;
            }

            if(usableItem.IsConsumable)
            {
                inventory.RemoveItem(usableItem);
                usableItem.Destroy();
            }
        }
    }

    private void EquipmentPanelRightClick(ItemSlot itemSlot)
    {
        if(itemSlot.Item is EquippableItem)
        {
            Unequip((EquippableItem)itemSlot.Item);
        }
    }

    private void ShowTooltip(ItemSlot itemSlot)
    {
        if(itemSlot.Item != null)
        {
            itemTooltip.ShowTooltip(itemSlot.Item);
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
        statPanel.UpdateStatValues();   
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
            int draggedItemAmount = draggedSlot.Amount;

            draggedSlot.Item = dropItemSlot.Item;
            draggedSlot.Amount = dropItemSlot.Amount;

            dropItemSlot.Item = draggedItem;
            dropItemSlot.Amount = draggedItemAmount;

        }
    }

    public void Equip(EquippableItem item)
    {
        EquipmentType equipmentType = item.EquipmentType;

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

        // Update the current and maximum health values
        currentMaxHealth = newMaxHealth;

        // Restore the current health value if it exceeds the new maximum health value
        currentHealth = Mathf.Min(currentHealth, currentMaxHealth);

        int newMaxMana = CalculateMaxMana();

        // Update the current and maximum mana values
        currentMaxMana = newMaxMana;

        // Restore the current mana value if it exceeds the new maximum mana value
        currentMana = Mathf.Min(currentMana, currentMaxMana);

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
                }
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

        // Set damageable MaxHealth
        damageable.MaxHealth = newMaxHealth;
        damageable.MaxMana = newMaxMana;
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

            // Set damageable MaxHealth to original value
            damageable.MaxHealth = currentMaxHealth;

            // Restore the current health value if it exceeds the new max health value
            currentHealth = Mathf.Min(currentHealth, currentMaxHealth);

            currentMaxMana = CalculateMaxMana();
            currentMana = Mathf.Min(currentMana, currentMaxMana);
            damageable.MaxMana = currentMaxMana;

            RecalculateBaseStats();
            statPanel.UpdateStatValues();
            UpdateBarInstance();
        }
    }

    private int CalculateMaxHealth()
    {
        int totalMaxHealth = originalMaxHealth;
        currentHealth = damageable.Health;
        foreach (EquippableItem item in equippedItems.Values)
        {
            totalMaxHealth += item.StrenghtBonus * 2; // Ubah ke StrengthBonus
        }   
        
        return totalMaxHealth;
    }

    private int CalculateMaxMana()
    {
        int totalMaxMana = originalMaxMana;
        currentMana = damageable.Mana;
        foreach (EquippableItem item in equippedItems.Values)
        {
            totalMaxMana += item.IntelligenceBonus * 3;
        }

        return totalMaxMana;
    }

    void UpdateBarInstance()
    {
        HealthBar.barInstance.SetMaxHealth(CalculateMaxHealth(), currentHealth);
        HealthBar.barInstance.SetMaxMana(CalculateMaxMana(), currentMana);
    }

    void SetStartingHpMana()
    {
        int Hp = originalMaxHealth + (int)Strenght.BaseValue * 2;
        Hp += (int)Vitality.BaseValue * 5;
        damageable.MaxHealth = Hp;
        damageable.Health = Hp;
        currentHealth = Hp;

        int Mana = originalMaxMana + (int)Intelligence.BaseValue * 3;
        damageable.MaxMana = Mana;
        damageable.Mana = Mana;
        currentMana = Mana;

        originalMaxHealth = damageable.MaxHealth; 
        originalMaxMana = damageable.MaxMana;
        HealthBar.barInstance.SetMaxHealth(Hp, currentHealth);
        HealthBar.barInstance.SetMaxMana(Mana, currentMana);
    }

    public void UpdateStatValues()
    {
        statPanel.UpdateStatValues();
    }

    public float CalculateDamage()
    {
        return StrenghtFinalValue; // Adjust this based on how you calculate damage
    }

    public void RecalculatStatValues()
    {
        RecalculateBaseStats();
    }
}

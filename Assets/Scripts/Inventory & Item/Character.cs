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

    private float _strenghtFinalValue;
    public float StrenghtFinalValue {get { return _strenghtFinalValue; } set { _strenghtFinalValue = value;}}

    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentPanel equipmentPanel;
    [SerializeField] StatPanel statPanel;
    [SerializeField] ItemTooltip itemTooltip;
    [SerializeField] Image draggableItem;

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
        Agility.BaseValue += agilityBonusTotal;
        Intelligence.BaseValue += intelligenceBonusTotal;
        Vitality.BaseValue += vitalityBonusTotal;
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

        // Unequip previous item of the same type, if any
        if (equippedItems.ContainsKey(equipmentType))
        {
            Unequip(equippedItems[equipmentType]);
        }

        //Equip the new item
        item.Equip(this);
        equippedItems[equipmentType] = item;
        statPanel.UpdateStatValues();

        if(inventory.RemoveItem(item.ID))
        {
            EquippableItem previousItem;
            if(equipmentPanel.AddItem(item, out previousItem))
            {
                if(previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    item.Equip(this);
                    statPanel.UpdateStatValues();
                }
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
        RecalculateBaseStats();
    }

    public void Unequip(EquippableItem item)
    {
        if(!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            inventory.AddItem(item);
            equippedItems.Remove(item.EquipmentType);
            RecalculateBaseStats();
            statPanel.UpdateStatValues();
        }
    }
}

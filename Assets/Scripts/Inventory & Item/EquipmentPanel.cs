using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPanel : MonoBehaviour
{
    [SerializeField] Transform equipmentSlotParent;
    [SerializeField] EquipmentSlot[] equipmentSlots;
    
    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    private void Start() 
    {
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            equipmentSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            equipmentSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
            equipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
            equipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
            equipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
            equipmentSlots[i].OnDragEvent += slot => OnDragEvent(slot);
            equipmentSlots[i].OnDropEvent += slot => OnDropEvent(slot);
        }    
    }

    private void OnValidate()
    {
        equipmentSlots = equipmentSlotParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public bool AddItem(EquippableItem item, out EquippableItem previousItem)
    {
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].EquipmentType == item.EquipmentType)
            {
                previousItem = (EquippableItem)equipmentSlots[i].Item;
                equipmentSlots[i].Item = item;
                return true;
            }
        }
        previousItem = null;
        return false;
    }

    public bool RemoveItem(EquippableItem item)
    {
        for(int i = 0; i < equipmentSlots.Length; i++)
        {
            if(equipmentSlots[i].Item == item)
            {
                equipmentSlots[i].Item = null;
                return true;
            }
        }
        return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class Inventory : MonoBehaviour
{
    [FormerlySerializedAs("items")] 
    [SerializeField] List<Item> startingItems;
    [SerializeField] Transform itemsParent;
    [SerializeField] ItemSlot[] itemSlots;
    
    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    private void Start() 
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
            itemSlots[i].OnPointerExitEvent += slot => OnPointerExitEvent(slot);
            itemSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
            itemSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
            itemSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
            itemSlots[i].OnDragEvent += slot => OnDragEvent(slot);
            itemSlots[i].OnDropEvent += slot => OnDropEvent(slot);
        }    

        SetStartingItems();
    }

    private void OnValidate()
    {
        if(itemsParent != null)
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>();

        SetStartingItems();    
    }

    private void SetStartingItems()
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }

        for(int i = 0; i < startingItems.Count; i++)
        {
           AddItem(startingItems[i].GetCopy());
        }
    }

    public void ClearInventory()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i] != null)
            {
                itemSlots[i].Clear(); 
            }
        }
    }

    public bool AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogWarning("Tried to add null item to inventory");
            return false;
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            // Jika slot kosong atau stackable
            if (itemSlots[i].Item == null ||
                (itemSlots[i].Item.ID == item.ID && itemSlots[i].Amount < item.MaximumStacks))
            {
                if (itemSlots[i].Item == null)
                {
                    itemSlots[i].Item = item.GetCopy(); 
                    itemSlots[i].Amount = 1;
                }
                else
                {
                    itemSlots[i].Amount++;
                }

                return true;
            }
        }

        Debug.LogWarning($"Inventory penuh atau tidak bisa menampung item: {item.ID}");
        return false;
    }

    public bool RemoveItem(Item item)
    {
        for(int i =  0; i <itemSlots.Length; i++)
        {
            if(itemSlots[i].Item == item)
            {
                itemSlots[i].Amount--;

                if(itemSlots[i].Amount == 0)
                {
                    itemSlots[i].Item = null;
                }
                return true;
            }
        }
        return false;
    }

    public Item RemoveItem(string itemID)
    {
        for(int i =  0; i <itemSlots.Length; i++)
        {
            Item item = itemSlots[i].Item;
            if(item != null && item.ID == itemID)
            {
                itemSlots[i].Amount--;
                if(itemSlots[i].Amount == 0) 
                {
                    itemSlots[i].Item = null;
                }
                return item;
            }
        }
        return null;
    }

    public bool IsFull()
    {
        for(int i =  0; i <itemSlots.Length; i++)
        {
            if(itemSlots[i].Item == null)
            {
                return false;
            }
        }
        return true;
    }

    public int ItemCount(string itemID)
    {
        int number = 0;

        for(int i  = 0; i < itemSlots.Length; i++)
        {
            if(itemSlots[i].Item.ID == itemID)
            {
                number++;
            }
        }
        return number;
    }
}

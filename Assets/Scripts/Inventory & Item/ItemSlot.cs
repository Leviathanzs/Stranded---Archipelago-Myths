using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Data.Common;
using TMPro;

public class ItemSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField] Image Image;
    [SerializeField] TextMeshProUGUI amountText;

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    protected bool isPointerOver;

    private Color normalColor = Color.white;
    private Color disabledColor = new Color(1, 1, 1, 0);

    private Item _item;
    public Item Item {
        get {return _item;}
        set {
            _item = value;
            if (_item == null && Amount != 0) Amount = 0;

            if(_item == null)
            {
                Image.color = disabledColor;
            } else {
                Image.sprite = _item.Icon;
                Image.color = normalColor;
            }

            if (isPointerOver)
            {
                OnPointerExit(null);
                OnPointerEnter(null);
            }
        }
    }

    private int _amount;
    public int Amount {
        get { return _amount;}
        set {
            _amount = value;
            if (_amount < 0) _amount = 0;
            if (_amount == 0 && Item != null) Item = null;
            
            if(amountText != null)
            {
                amountText.enabled = _item != null && _item.MaximumStacks > 1 && _amount > 1;
                if (amountText.enabled) {
                    amountText.text = _amount.ToString();
                }
            }
        }
    }

    protected virtual void OnValidate()
    {
        if(Image == null)
            Image = GetComponent<Image>();

        if(amountText == null)
            amountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected private void OnDisable() 
    {
        if (isPointerOver)
        {
            OnPointerExit(null);
        }
    }

    public virtual bool CanReceiveItem(Item item)
    {
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData != null && eventData.button == PointerEventData.InputButton.Right)
        {
            if(OnRightClickEvent != null)
                OnRightClickEvent(this);
            
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;

        if(OnPointerEnterEvent != null)
            OnPointerEnterEvent(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;

        if(OnPointerExitEvent != null)
            OnPointerExitEvent(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(OnBeginDragEvent != null)
            OnBeginDragEvent(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(OnEndDragEvent != null)
            OnEndDragEvent(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(OnDragEvent != null)
            OnDragEvent(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(OnDropEvent != null)
            OnDropEvent(this);
    }
}

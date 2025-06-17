using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour,
    IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler,
    IBeginDragHandler,
    IEndDragHandler,
    IDragHandler,
    IDropHandler
{
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI amountText;

    public event Action<ItemSlot> OnPointerEnterEvent;
    public event Action<ItemSlot> OnPointerExitEvent;
    public event Action<ItemSlot> OnRightClickEvent;
    public event Action<ItemSlot> OnBeginDragEvent;
    public event Action<ItemSlot> OnEndDragEvent;
    public event Action<ItemSlot> OnDragEvent;
    public event Action<ItemSlot> OnDropEvent;

    protected bool isPointerOver;

    private readonly Color normalColor = Color.white;
    private readonly Color disabledColor = new Color(1, 1, 1, 0);

    private Item _item;
    public Item Item
    {
        get => _item;
        set
        {
            _item = value;

            // Hindari akses komponen jika object sudah dihancurkan
            if (this == null || gameObject == null || !gameObject.activeInHierarchy)
                return;

            if (Image != null)
            {
                if (_item == null)
                {
                    Image.color = disabledColor;
                    Image.sprite = null;
                }
                else
                {
                    Image.sprite = _item.Icon;
                    Image.color = normalColor;
                }
            }

            if (_item == null && Amount != 0)
                Amount = 0;

            if (isPointerOver)
            {
                OnPointerExit(null);
                OnPointerEnter(null);
            }
        }
    }

    private int _amount;
    public int Amount
    {
        get => _amount;
        set
        {
            _amount = Mathf.Max(0, value);

            if (_amount == 0 && _item != null)
                _item = null;

            if (this == null || gameObject == null || !gameObject.activeInHierarchy)
                return;

            if (amountText != null)
            {
                bool shouldShow = _item != null && _item.MaximumStacks > 1 && _amount > 1;
                amountText.enabled = shouldShow;
                if (shouldShow)
                    amountText.text = _amount.ToString();
            }
        }
    }

    protected virtual void OnValidate()
    {
        if (Image == null)
            Image = GetComponent<Image>();

        if (amountText == null)
            amountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected void OnDisable()
    {
        if (isPointerOver)
            OnPointerExit(null);
    }

    public virtual bool CanReceiveItem(Item item) => true;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData != null && eventData.button == PointerEventData.InputButton.Right)
            OnRightClickEvent?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;
        OnPointerEnterEvent?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;
        OnPointerExitEvent?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragEvent?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropEvent?.Invoke(this);
    }

    public void Clear()
    {
        _item = null;
        _amount = 0;

        if (Image != null)
        {
            Image.sprite = null;
            Image.color = disabledColor;
        }

        if (amountText != null)
        {
            amountText.enabled = false;
        }
    }

}

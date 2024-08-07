using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IOnOff, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Text countText;
    private Image slotImage;
    private Canvas canvas;
    private Transform original;
    private Vector2 originalPos;
    public string itemName;
    public int count;

    public void Awake()
    {
        slotImage = GetComponent<Image>();
        canvas = GetComponentInParent<Canvas>();

        UpdateUI();
    }

    public void InsertItem(string name, Sprite sprite)
    {
        itemName = name;
        slotImage.sprite = sprite;
        count++;

        UpdateUI();
    }

    public void InsertItem()
    {
        count++;

        UpdateUI();
    }

    public void RemoveItem()
    {
        count--;

        if (count == 0)
        {
            itemName = null;
            slotImage.sprite = null;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        OnOff(countText.gameObject, count > 1);

        if (count > 0)
            transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        else
            transform.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
        countText.text = $"{count}";
    }

    public void OnOff(GameObject gameObject, bool value)
    {
        gameObject.SetActive(value);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        original = transform.parent;
        originalPos = transform.position;
        transform.SetParent(canvas.transform);
        slotImage.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(original);
        transform.position = originalPos;
        slotImage.raycastTarget = true;

        if (eventData.pointerEnter != null)
        {
            Slot targetSlot = eventData.pointerEnter.GetComponent<Slot>();
            if (targetSlot != null)
            {
                SwapItems(targetSlot);
            }
        }
    }

    private void SwapItems(Slot targetSlot)
    {
        if (targetSlot.itemName == itemName)
        {
            int total = targetSlot.count + count;
            if (total <= 64)
            {
                targetSlot.count = total;
                itemName = null;
                slotImage.sprite = null;
                count = 0;
            }
            else
            {
                int amount = total - 64;
                targetSlot.count = 64;
                count = amount;
            }
        }
        else
        {
            string name = targetSlot.itemName;
            Sprite sprite = targetSlot.slotImage.sprite;
            int count = targetSlot.count;

            targetSlot.itemName = itemName;
            targetSlot.slotImage.sprite = slotImage.sprite;
            targetSlot.count = this.count;

            itemName = name;
            slotImage.sprite = sprite;
            this.count = count;
        }
        targetSlot.UpdateUI();
        UpdateUI();
    }
}
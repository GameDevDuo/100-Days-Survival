using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IOnOff
{
    [SerializeField] private Text countText;
    private Image slotImage;
    public string itemName;
    public int count;

    public void Awake()
    {
        slotImage = GetComponent<Image>();

        UpdateUI();
    }

    public void InsertItem(Sprite item)
    {
        slotImage.sprite = item;
        count++;

        UpdateUI();
    }
    public void InsertItem() => count++;

    public void RemoveItem()
    {
        count--;

        UpdateUI();
    }

    private void UpdateUI()
    {
        OnOff(countText.gameObject, count != 0);

        countText.text = $"{count}";
    }

    public void OnOff(GameObject gameObject, bool value)
    {
        gameObject.SetActive(value);
    }
}
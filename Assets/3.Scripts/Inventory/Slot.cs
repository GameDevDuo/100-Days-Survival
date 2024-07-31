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
        OnOff(countText.gameObject, count != 0);

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
}
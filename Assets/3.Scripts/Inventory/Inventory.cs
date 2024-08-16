using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite currentSlotSprite;
    [SerializeField] private Sprite previousSlotSprite;
    private PlayerController playerController;
    private FirstPersonCamera firstPersonCamera;
    private ResourceItemRaycaster resourceItemRaycaster;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    public GameObject inventoryUI;
    public Image[] hotbarSlots;
    public Slot[] slots;
    public int selectedSlot = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        firstPersonCamera = player.transform.GetChild(0).GetComponent<FirstPersonCamera>();
        resourceItemRaycaster = player.transform.GetChild(0).GetComponent<ResourceItemRaycaster>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();
    }

    private void Update()
    {
        HandleHotbarInput();
    }

    private void HandleHotbarInput()
    {
        float scrollValue = Mouse.current.scroll.ReadValue().y;
        float scrollSensitivity = 0.1f;

        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (Keyboard.current[Key.Digit1 + i].wasPressedThisFrame)
            {
                selectedSlot = i;
                break;
            }
        }
        if (Mathf.Abs(scrollValue) >= scrollSensitivity)
        {
            selectedSlot = (selectedSlot - (int)Mathf.Sign(scrollValue) + hotbarSlots.Length) % hotbarSlots.Length;
        }
        UpdateHotbarUI();
    }

    private void UpdateHotbarUI()
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (i == selectedSlot)
            {
                hotbarSlots[i].sprite = currentSlotSprite;
                hotbarSlots[i].color = new Color(1f, 1f, 1f, 0.392f);

                if (hotbarSlots[i].transform.childCount > 0)
                {
                    Transform childTransform = hotbarSlots[i].transform.GetChild(0);
                    if (childTransform != null)
                    {
                        Image childImage = childTransform.GetComponent<Image>();
                        if (childImage != null)
                        {
                            Sprite sprite = childImage.sprite;
                            resourceItemRaycaster.toolSprite = sprite;
                        }
                        else
                        {
                            resourceItemRaycaster.toolSprite = null;
                        }
                    }
                }
            }
            else
            {
                hotbarSlots[i].sprite = previousSlotSprite;
                hotbarSlots[i].color = Color.white;
            }
        }
    }

    public void ToggleInventory()
    {
        if (resourceItemRaycaster.isCollecting)
        {
            return;
        }
        bool isActive = !inventoryUI.activeSelf;
        inventoryUI.SetActive(isActive);

        if (!Craftingtable.Instance.craftingTableUI.activeSelf)
        {
            playerController.enabled = !isActive;
            firstPersonCamera.enabled = !isActive;
            resourceItemRaycaster.enabled = !isActive;

            Cursor.visible = isActive;
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
        if (inventoryUI.activeSelf)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerAnimator.Play("Idle");
        }
    }

    public void AddCraftingItem(string name, Sprite itemSprite)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName == name)
            {
                if (slots[i].count < 64)
                {
                    slots[i].InsertItem();
                    return;
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].count == 0)
            {
                slots[i].InsertItem(name, itemSprite);
                return;
            }
        }
    }

    public void AddResourceItem(ResourceItem item)
    {
        string itemName = item.itemData.ItemName;
        Sprite sprite = item.itemData.Sprite;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].itemName == itemName && slots[i].count < 64)
            {
                slots[i].InsertItem();
                return;
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].count == 0)
            {
                slots[i].InsertItem(itemName, sprite);
                return;
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Craft
{
    public Button craftingSlot;
}

public class Craftingtable : MonoBehaviour
{
    public static Craftingtable Instance;
    [SerializeField] private List<Craft> slots;
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    private FirstPersonCamera firstPersonCamera;
    private ResourceItemRaycaster resourceItemRaycaster;
    private Rigidbody playerRigidbody;
    private Animator playerAnimator;
    public GameObject craftingTableUI;

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

    void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        firstPersonCamera = player.transform.GetChild(0).GetComponent<FirstPersonCamera>();
        resourceItemRaycaster = player.transform.GetChild(0).GetComponent<ResourceItemRaycaster>();
        playerRigidbody = player.GetComponent<Rigidbody>();
        playerAnimator = player.GetComponent<Animator>();

        for (int i = 0; i < slots.Count; i++)
        {
            int index = i;
            slots[i].craftingSlot.onClick.AddListener(() => IsCraftable(index));
        }
    }

    public void OnToggleCraftingTable()
    {
        if (resourceItemRaycaster.isCollecting)
        {
            return;
        }
        bool isActive = !craftingTableUI.activeSelf;
        craftingTableUI.SetActive(isActive);

        if (!Inventory.Instance.inventoryUI.activeSelf)
        {
            playerController.enabled = !isActive;
            firstPersonCamera.enabled = !isActive;
            resourceItemRaycaster.enabled = !isActive;

            Cursor.visible = isActive;
            Cursor.lockState = isActive ? CursorLockMode.None : CursorLockMode.Locked;
        }
        if (craftingTableUI.activeSelf)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerAnimator.Play("Idle");
        }
    }

    private void IsCraftable(int index)
    {
        string itemName = slots[index].craftingSlot.GetComponent<Image>().sprite.name;

        CraftingData data = Resources.Load<CraftingData>($"Prefabs/CraftingData/{itemName}");

        if (data == null)
        {
            return;
        }

        foreach (Recipe recipe in data.Recipes)
        {
            if (Craft(recipe))
            {
                Inventory.Instance.AddCraftingItem(itemName, recipe.resultItem);
                UseIngredients(recipe);
            }
        }
    }

    private bool Craft(Recipe recipe)
    {
        foreach (Ingredient ingredient in recipe.ingredients)
        {
            int totalCount = 0;

            foreach (Slot slot in Inventory.Instance.slots)
            {
                if (slot.itemName == ingredient.itemSprite.name)
                {
                    totalCount += slot.count;
                }
            }
            if (totalCount < ingredient.count)
            {
                return false;
            }
        }
        return true;
    }

    private void UseIngredients(Recipe recipe)
    {
        foreach (Ingredient ingredient in recipe.ingredients)
        {
            int count = ingredient.count;

            foreach (Slot slot in Inventory.Instance.slots)
            {
                if (slot.itemName == ingredient.itemSprite.name)
                {
                    if (slot.count >= count)
                    {
                        for (int i = 0; i < count; i++)
                        {
                            slot.RemoveItem();
                        }
                        count = 0;
                        break;
                    }
                    else
                    {
                        for (int i = 0; i < slot.count; i++)
                        {
                            slot.RemoveItem();
                        }
                        count -= slot.count;
                    }
                }
            }
        }
    }
}
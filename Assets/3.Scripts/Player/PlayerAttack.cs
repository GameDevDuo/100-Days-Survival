using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float distance;
    private Camera playerCamera;
    private PlayerInput playerInput;
    private ResourceItemRaycaster resourceItemRaycaster;
    public int damage;

    void Start()
    {
        playerCamera = Camera.main;

        playerInput = GetComponent<PlayerInput>();
        resourceItemRaycaster = GetComponent<ResourceItemRaycaster>();
        playerInput.actions["Attack"].performed += OnAttack;
    }

    void Update()
    {
        UpdateDamage();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Attack();
    }

    void Attack()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance, layerMask))
        {
            if (hit.collider.TryGetComponent<AttackAnimal>(out var attackAnimal))
            {
                attackAnimal.TakeDamage(damage);
            }
            else if (hit.collider.TryGetComponent<PassiveAnimal>(out var passiveAnimal))
            {
                passiveAnimal.TakeDamage(damage);
            }
        }
    }

    void UpdateDamage()
    {
        if (resourceItemRaycaster.toolSprite != null)
        {
            string itemName = resourceItemRaycaster.toolSprite.name;
            ItemData itemData = Resources.Load<ItemData>($"Prefabs/ItemData/{itemName}");

            if (itemData != null)
            {
                damage = itemData.Damage;
            }
            else
            {
                damage = 2;
            }
        }
        else
        {
            damage = 2;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * distance);
    }

    private void OnDestroy()
    {
        playerInput.actions["Attack"].performed -= OnAttack;
    }
}
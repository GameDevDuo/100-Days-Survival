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
        AttackDamageUpdate();
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
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<AttackAnimal>())
                {
                    AttackAnimal animal = hit.collider.GetComponent<AttackAnimal>();
                    animal.TakeDamage(damage);
                }
                else
                {
                    PassiveAnimal animal = hit.collider.GetComponent<PassiveAnimal>();
                    animal.TakeDamage(damage);
                }
            }
        }
    }

    void AttackDamageUpdate()
    { 
        if (resourceItemRaycaster.toolSprite.name != null)
        {
            damage = Resources.Load<ItemData>($"Prefabs/ItemData/{resourceItemRaycaster.toolSprite.name}").Damage;
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
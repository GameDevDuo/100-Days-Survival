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

    void Start()
    {
        playerCamera = Camera.main;

        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Attack"].performed += OnAttack;
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
                Debug.Log("PlayerAttack");
            }
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
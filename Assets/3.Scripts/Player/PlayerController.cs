using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    private Rigidbody rb;
    private Vector3 moveInput;
    private bool isGround;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Run();
        CheckGround();
    }

    void OnMove(InputValue value)
    {
        Vector2 inputVector = value.Get<Vector2>();
        moveInput = new Vector3(inputVector.x, 0, inputVector.y);
    }

    void OnJump()
    {
        if (isGround)
        {
            Jump();
        }
    }

    private void Run()
    {
        Vector3 velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.z * speed);
        rb.velocity = transform.TransformDirection(velocity);
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, 1.25f, LayerMask.GetMask("Ground"));
    }

    private void OnToggleInventory()
    {
        Inventory.Instance.ToggleInventory();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector3.down * 1.25f);
    }
}
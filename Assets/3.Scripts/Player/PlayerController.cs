using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    private Animator anim;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Run();
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        Vector3 velocity = new Vector3(moveInput.x * speed, rb.velocity.y, moveInput.y * speed);
        rb.velocity = transform.TransformDirection(velocity);

        if (moveInput.x != 0 || moveInput.y != 0)
        {
            anim.Play("Run");
        }
        else
        {
            anim.Play("Idle");
        }
    }
}
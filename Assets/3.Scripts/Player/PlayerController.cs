using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runMultiplier = 2f;
    [SerializeField] private float jumpForce;
    [SerializeField] private LayerMask layerMask;
    private Rigidbody rb;
    private Animator anim;
    private Player player;
    private Vector3 moveInput;
    private bool isGround;
    public bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
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

    void OnRun(InputValue value)
    {
        if (player.curStamina > 25 && value.isPressed)
        {
            isRunning = true;
            player.isRun = true;
        }
        else
        {
            isRunning = false;
            player.isRun = false;
        }
    }

    private void Run()
    {
        float currentSpeed = speed;

        if (isRunning)
        {
            currentSpeed *= runMultiplier;
        }
        Vector3 velocity = new Vector3(moveInput.x * currentSpeed, rb.velocity.y, moveInput.z * currentSpeed);
        rb.velocity = transform.TransformDirection(velocity);

        if (moveInput.x != 0 || moveInput.z != 0)
        {
            anim.Play("Run");
        }
        else
        {
            anim.Play("Idle");
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void CheckGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, 0.25f, layerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, Vector3.down * 0.25f);
    }
}
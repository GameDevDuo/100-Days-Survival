using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : PlayerMoveBase
{
    private CharacterController characterController;
    private Vector2 moveInput;

    private float moveSpeed = 5f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        CursorUpdate(false, CursorLockMode.Locked);
    }

    private void OnEnable()
    {
        var inputAction = new PlayerAction();
        inputAction.PlayerActions.Enable();

        inputAction.PlayerActions.WASD.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerActions.WASD.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        var inputAction = new PlayerAction();
        inputAction.PlayerActions.Disable();
    }

    private void Update()
    {
        Move();
    }

    public override void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move) * moveSpeed * Time.deltaTime;
        characterController.Move(move);
    }
}

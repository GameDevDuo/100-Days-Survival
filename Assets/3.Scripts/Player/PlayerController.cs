using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : PlayerMoveBase
{
    [SerializeField]
    private Transform cameraPos;

    private Camera cam;
    private CharacterController characterController;
    private Rigidbody rigid;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private float moveSpeed = 5f;
    private float sensitivity = 3f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        rigid = GetComponent<Rigidbody>();
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

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Move();
        Follow();
        Rotate();
    }

    public override void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move) * moveSpeed * Time.deltaTime;
        characterController.Move(move);
    }

    public override void Rotate()
    {
        float xRotation = lookInput.y * sensitivity * Time.deltaTime;
        float yRotation = lookInput.x * sensitivity * Time.deltaTime;

        transform.Rotate(Vector3.up * yRotation);

        cameraPos.localRotation = Quaternion.Euler(cameraPos.localEulerAngles.x - xRotation, 0f, 0f);
    }

    public override void Follow()
    {
        cam.transform.position = cameraPos.position;
        cam.transform.rotation = cameraPos.rotation;
    }
}

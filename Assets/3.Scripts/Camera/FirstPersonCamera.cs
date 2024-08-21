using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float mouseSensitivity = 2f;
    private float cameraVerticalRotation = 0f;
    private Vector2 lookInput;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraLook();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void CameraLook()
    {
        float inputX = lookInput.x * mouseSensitivity;
        float inputY = lookInput.y * mouseSensitivity;

        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -80f, 60f);
        transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0f, 0f);

        player.Rotate(Vector3.up * inputX);
    }
}
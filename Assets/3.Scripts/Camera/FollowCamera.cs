using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : FollowBase
{
    [SerializeField]
    private Transform cameraPos;

    private Camera cam;

    private float Sensitivity = 3f;
    private float currentCamera;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Follow();
    }

    public override void Follow()
    {
        cam.transform.position = cameraPos.position;
    }

    private void RotateCamera()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotation = xRotation * Sensitivity;

        currentCamera -= cameraRotation;
        currentCamera = Mathf.Clamp(currentCamera, -85f, 85f);

        cam.transform.localEulerAngles = new Vector3(currentCamera, 0f, 0f);
    }
}

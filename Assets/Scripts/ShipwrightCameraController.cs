using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShipwrightCameraController : MonoBehaviour
{
    InputWrapper input;

    bool rotationEnabled = false;

    public float movementSpeed = 100f;
    public float rotationSpeed = 25f;

    private float targetYRot = 0;
    private float targetXRot = 0;

    private void OnEnable()
    {
        input = new();
        input.ShipwrightCamera.Enable();
        input.ShipwrightCamera.Rotate.performed += Rotate;
        input.ShipwrightCamera.EnableRotation.started += EnableRotation;
        input.ShipwrightCamera.EnableRotation.canceled += DisableRotation;
    }

    private void OnDisable()
    {
        input.ShipwrightCamera.Disable();
    }

    private void Rotate(InputAction.CallbackContext context)
    {
        if (!rotationEnabled)
        {
            return;
        }

        var rotation = context.ReadValue<Vector2>();

        targetYRot += rotation.x * Time.deltaTime * rotationSpeed;
        targetXRot -= rotation.y * Time.deltaTime * rotationSpeed;
        targetXRot = Mathf.Clamp(targetXRot, -85, 85);
    }

    void Move(Vector2 direction)
    {
        var forward = direction.y;
        var right = direction.x;
        transform.position += transform.forward * forward * Time.deltaTime * movementSpeed;
        transform.position += transform.right * right * Time.deltaTime * movementSpeed;
    }
    void EnableRotation(InputAction.CallbackContext context)
    {
        rotationEnabled = true;
    }

    void DisableRotation(InputAction.CallbackContext context)
    {
        rotationEnabled = false;
    }

    //void Rotate(Vector2 rotationDelta)
    //{
    //    //var rotation = transform.localEulerAngles;
    //    var yaw = rotationDelta.x;
    //    var pitch = rotationDelta.y;
    //    //rotation.y += yaw;
    //    //rotation.x -= pitch;
    //    //transform.localEulerAngles = rotation;

    //    transform.Rotate(Vector3.up, yaw, Space.World);
    //    transform.Rotate(Vector3.right, -pitch, Space.World);
    //}

    // Update is called once per frame
    void Update()
    {
        if(input.ShipwrightCamera.Move.IsPressed())
        {
            var direction = input.ShipwrightCamera.Move.ReadValue<Vector2>();
            Move(direction * Time.deltaTime * movementSpeed);
        }

        transform.rotation = Quaternion.Euler(targetXRot, targetYRot, 0.0f);
    }
}

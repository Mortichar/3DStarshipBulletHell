using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private InputWrapper input;

    public float zoomSpeed = 25f;
    public float rotateSpeed = 25f;

    public float minZoomDistance = 1f;
    public float maxZoomDistance = 100f;

    
    public GameObject player;

    
    // target distance from the player in units (meters)
    private float targetDistance = 10f;
    private float targetXRot = 0f;
    private float targetYRot = 0f;

    // offset, calculated from targetDistance and rotation
    private Vector3 offset;


    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            //get the player
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Awake()
    {
    }

    private void OnEnable()
    {
        input = new();
        input.Camera.Zoom.performed += Zoom;
        input.Camera.Rotate.performed += Rotate;
        input.Camera.Enable();
        targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);
        offset = new Vector3(0, 0, -targetDistance);
    }

    private void OnDisable()
    {
        input.Camera.Disable();
    }

    void Rotate(InputAction.CallbackContext context)
    {
        if (!input.Camera.EnableRotation.IsPressed())
        {
            return;
        }
        
        var rotation = context.ReadValue<Vector2>();
        targetYRot += rotation.x * Time.deltaTime * rotateSpeed;
        targetYRot = Mathf.Clamp(targetYRot, -80, 80);
        targetXRot += rotation.y * Time.deltaTime * rotateSpeed;
        targetXRot = Mathf.Clamp(targetXRot, -80, 80);

    }

    void Zoom(InputAction.CallbackContext context)
    {
        var zoom = context.ReadValue<float>();

        targetDistance -= zoom * zoomSpeed * Time.deltaTime;
        targetDistance = Mathf.Clamp(targetDistance, minZoomDistance, maxZoomDistance);
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player != null)
        {
            // rotate z, y, x
            offset = Quaternion.AngleAxis(player.transform.rotation.z, Vector3.forward) * new Vector3(0, 0, -targetDistance);
            offset = Quaternion.AngleAxis(targetYRot, Vector3.up) * offset;
            offset = Quaternion.AngleAxis(targetXRot, Vector3.right) * offset;
            //var newPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - targetDistance);

            //transform.position = newPosition;
            //transform.RotateAround(player.transform.position, Vector3.up, targetYRot);
            //transform.RotateAround(player.transform.position, transform.right, targetXRot);

            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform);

        }
    }
}

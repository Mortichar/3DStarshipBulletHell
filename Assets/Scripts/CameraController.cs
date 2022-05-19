using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    private new Camera camera;
    private InputWrapper input;
    
    public float zoomSpeed;
    public float minZoomDistance;
    public float mazZoomDistance;

    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();

        //get the player
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        input = new();
        input.Camera.Zoom.performed += Zoom;
    }

    private void OnEnable()
    {
        input.Camera.Enable();
    }

    private void OnDisable()
    {
        input.Camera.Disable();
    }

    void Zoom(InputAction.CallbackContext context)
    {
        var zoom = context.ReadValue<float>();

        var currentZ = Mathf.Abs(camera.transform.position.z);
        var newZ = currentZ - zoom * zoomSpeed;
        newZ = -Mathf.Clamp(newZ, minZoomDistance, mazZoomDistance);

        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, newZ);        
    }

    // Update is called once per frame
    void Update()
    {

        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if(player != null)
        {
            // move camera to player
            camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
        }
    }
}

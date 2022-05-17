using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;

    public float zoomSpeed;
    public float minZoomDistance;
    public float mazZoomDistance;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var zoom = Input.GetAxis("Zoom");

        var currentZ = Mathf.Abs(camera.transform.position.z);
        var newZ = currentZ - zoom * zoomSpeed;
        newZ = -Mathf.Clamp(newZ, minZoomDistance, mazZoomDistance);

        camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, newZ);

    }
}

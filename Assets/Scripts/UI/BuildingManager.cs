using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    // Layers to place objects on (raycast target)
    public LayerMask targetLayers;

    // The material to show on the object being placed
    public Material placementMaterial;
    // The layer applied to objects placed
    public LayerMask placedLayer;



    // The object the manager is currently placing at the cursor
    private GameObject objectToPlace;
    // the actual material of the object to be placed, before we swapped it out
    private Material cachedMaterial;

    // Generated class to query for input
    private InputWrapper input;

    // Target rotation of the placing object
    private float placementRotation = 0f;


    private void OnEnable()
    {
        input = new();
        input.Shipwright.Remove.performed += Remove;
        input.Shipwright.Rotate.performed += Rotate;
        input.Shipwright.Counterrotate.performed += Counterrotate;
        input.Shipwright.Enable();
    }

    private void OnDisable()
    {
        input.Shipwright.Disable();
    }

    private bool Clicked()
    {
        // ignore clicks through UI
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }
        return input.Shipwright.Place.WasPressedThisFrame();
    }

    void Place()
    {
        if (this.objectToPlace != null)
        {
            GameObject placedObject = Instantiate(this.objectToPlace, this.objectToPlace.transform.position, this.objectToPlace.transform.rotation);
            MeshRenderer renderer = placedObject.GetComponent<MeshRenderer>();
            if (renderer != null && cachedMaterial != null)
            {
                renderer.material = cachedMaterial;
            }
            placedObject.layer = (int) Mathf.Log(placedLayer.value, 2);
        }
    }

    /// <summary>
    /// Remove the ship part currently under the cursor
    /// </summary>
    void Remove(InputAction.CallbackContext _)
    {
        // cast ray from the main camera and check for collisions with ship parts
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out hit, 10000f, placedLayer))
        {
            // if we hit a ship part, destroy it
            Destroy(hit.collider.gameObject);
        }
    }

    void Rotate(InputAction.CallbackContext _)
    {
        placementRotation += 90f;
        while (placementRotation > 360f)
        {
            placementRotation -= 360f;
        }
    }

    void Counterrotate(InputAction.CallbackContext _)
    {
        placementRotation -= 90f;
        while (placementRotation < 0f)
        {
            placementRotation += 360f;
        }
    }

    private Vector3 ClosestAttachment(Vector3 point, Vector3 normal)
    {
        return new Vector3(
            Mathf.Round(point.x + normal.x / 2),
            Mathf.Round(point.y + normal.y / 2),
            Mathf.Round(point.z + normal.z / 2));
    }

    // Update is called once per frame
    void Update()
    {
        // if currently building something (something has been selected in the building menu)
        if (objectToPlace != null)
        {
            // do raycast from main camera to determine where to place block
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, 10000f, targetLayers))
            {
                if(!objectToPlace.activeInHierarchy)
                {
                    objectToPlace.SetActive(true);
                }
                // if the raycast hit a collider
                if (hit.collider != null)
                {
                    var closestAttachment = ClosestAttachment(hit.point, hit.normal);
                    objectToPlace.transform.position = closestAttachment;

                    //rotate in direction of hit normal; objects rotate with their "bottom" on the object hovered by the cursor
                    objectToPlace.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    //rotate around the normal by however much the current placement rotation is; allows rotating to face 4 directions
                    objectToPlace.transform.rotation = Quaternion.AngleAxis(placementRotation, hit.normal) * objectToPlace.transform.rotation;

                    if (Clicked())
                    {
                        Place();
                    }
                }
            }
            else
            {
                if(objectToPlace.activeInHierarchy)
                {
                    objectToPlace.SetActive(false);
                }
            }

        }
    }

    public void SetPlacingObject(GameObject objectToPlace)
    {
        if (this.objectToPlace != null)
        {
            Destroy(this.objectToPlace);
        }


        this.objectToPlace = Instantiate(objectToPlace, new Vector3(), Quaternion.identity);
        this.objectToPlace.layer = LayerMask.NameToLayer("Default");


        MeshRenderer renderer = this.objectToPlace.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            cachedMaterial = renderer.material;
            renderer.material = placementMaterial;
        }
    }
}

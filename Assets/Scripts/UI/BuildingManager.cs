using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    public GameObject objectToPlace;

    public LayerMask targetLayers;

    public Material placementMaterial;


    // the actual material of the object to be placed, before we swapped it out
    private Material cachedMaterial;

    private InputWrapper input;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        input = new();
        input.ShipwrightCamera.Enable();
    }

    private void OnDisable()
    {
        input.ShipwrightCamera.Disable();
    }

    private bool Clicked()
    {
        return input.ShipwrightCamera.Place.WasPressedThisFrame();
    }

    void Place()
    {
        if(this.objectToPlace != null)
        {
            GameObject PlacedObject = Instantiate(this.objectToPlace, this.objectToPlace.transform.position, this.objectToPlace.transform.rotation);
            MeshRenderer renderer = PlacedObject.GetComponent<MeshRenderer>();
            if(renderer != null && cachedMaterial != null)
            {
                renderer.material = cachedMaterial;
            }
            PlacedObject.layer = LayerMask.NameToLayer("ShipPiece");
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
        if(objectToPlace != null)
        {
            // do raycast from main camera to determine where to place block
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (Physics.Raycast(ray, out hit, 10000f, targetLayers))
            {
                // if the raycast hit a collider
                if (hit.collider != null)
                {
                    objectToPlace.transform.position = ClosestAttachment(hit.point, hit.normal);
                    objectToPlace.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                    
                    if(Clicked())
                    {
                        Place();
                    }
                }
            }

        }
    }

    public void SetPlacingObject(GameObject objectToPlace)
    {
        if(this.objectToPlace != null)
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

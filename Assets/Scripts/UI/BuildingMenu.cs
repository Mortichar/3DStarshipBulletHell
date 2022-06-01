using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingMenu : MonoBehaviour
{
    public Button buttonPrefab;
    public List<ShipPart> shipParts;
    public Material placementMaterial;
    public BuildingManager buildingManager;

    private List<Button> buildingButtons;

    void Awake()
    {
        // get a list of all of the scriptable objects in the ship parts folder
        buildingButtons = new List<Button>();

        if(buttonPrefab != null)
        {
            foreach (var part in shipParts)
            {
                // instantiate new button, parented to this
                var button = Instantiate(buttonPrefab, transform);            
                button.GetComponentInChildren<TextMeshProUGUI>().text = part.displayedName;
                button.onClick.AddListener(() => {

                    buildingManager.SetPlacingObject(part.prefab);
                });
                buildingButtons.Add(button);
            }
        }

    }
}

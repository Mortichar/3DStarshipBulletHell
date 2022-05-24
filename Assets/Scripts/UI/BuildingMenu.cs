using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class BuildingMenu : MonoBehaviour
{
    public Button buttonPrefab;
    public List<ShipPart> shipParts;

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
                buildingButtons.Add(button.GetComponent<Button>());
            }
        }

    }
}

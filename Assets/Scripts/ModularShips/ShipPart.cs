using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Ship Part")]
public class ShipPart : ScriptableObject
{
    public string displayedName = "PLACEHOLDER";
    public GameObject prefab;
    public Vector2Int dimensions = new Vector2Int(1, 1);
    
}


using UnityEngine;

public class PlacementSpot : MonoBehaviour
{
    [Header("Placement Settings")]
    public string itemID;

    private void OnValidate()
    {
        itemID = itemID.Trim();
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUp : MonoBehaviour
{
    [Header("Pickup Settings")]
    public Camera playerCamera;
    public Transform holdPosition;
    public float interactDistance = 5f;

    private KitchenItem heldItem;

    void Start()
    {
        Debug.Log("PickUp script is running!");
    }

    void Update()
    {
        // Check if the New Input System detects E.
        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E KEY PRESSED!");

            if (heldItem == null)
            {
                Debug.Log("Trying to pick up item...");
                TryPickup();
            }
            else
            {
                Debug.Log("Trying to place item...");
                TryPlace();
            }
        }
    }

    void TryPickup()
    {
        if (playerCamera == null)
        {
            Debug.LogError("Player Camera is missing!");
            return;
        }

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide))
        {
            KitchenItem item =
                hit.collider.GetComponentInParent<KitchenItem>();

            if (item != null && item.isCollected && !item.isPlaced)
            {
                if (holdPosition == null)
                {
                    Debug.LogError("Hold Position is missing!");
                    return;
                }

                heldItem = item;

                item.PickUp(holdPosition);

                Debug.Log("Picked up: " + item.itemID);
            }
            else
            {
                Debug.Log("No available kitchen item detected.");
            }
        }
        else
        {
            Debug.Log("Raycast did not hit an item.");
        }
    }

    void TryPlace()
    {
        if (playerCamera == null)
            return;

        Ray ray = playerCamera.ViewportPointToRay(
            new Vector3(0.5f, 0.5f, 0f)
        );

        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            interactDistance,
            Physics.DefaultRaycastLayers,
            QueryTriggerInteraction.Collide
        );

        System.Array.Sort(hits,
            (a, b) => a.distance.CompareTo(b.distance));

        PlacementSpot foundSpot = null;

        foreach (RaycastHit hit in hits)
        {
            KitchenItem hitItem =
                hit.collider.GetComponentInParent<KitchenItem>();

            if (hitItem == heldItem)
                continue;

            PlacementSpot spot =
                hit.collider.GetComponentInParent<PlacementSpot>();

            if (spot != null)
            {
                Debug.Log("Detected Spot: " + spot.itemID);

                if (heldItem.correctSpot == spot.transform)
                {
                    foundSpot = spot;
                    break;
                }
            }
        }

        if (foundSpot != null)
        {
            KitchenItem itemToPlace = heldItem;

            bool placed = itemToPlace.Place(foundSpot.transform);

            if (placed)
            {
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddItem(itemToPlace.gameObject);
                }

                heldItem = null;

                Debug.Log("Correctly organized: " + itemToPlace.itemID);
            }
        }
        else
        {
            Debug.Log("No matching placement spot found for: "
                + heldItem.itemID);
        }
    }
}
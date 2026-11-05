using UnityEngine;

public class KitchenItem : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemID;

    [Header("Item Locations")]
    public Transform correctSpot;
    public Transform trayPosition;

    [Header("Pickup Size")]
    public Vector3 heldScale = new Vector3(0.3f, 0.3f, 0.3f);

    [Header("Item Status")]
    public bool isCollected = false;
    public bool isPlaced = false;

    private Rigidbody rb;
    private Vector3 originalWorldScale;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        originalWorldScale = transform.lossyScale;
    }

    void SetWorldScale(Vector3 worldScale)
    {
        transform.localScale = transform.parent != null
            ? new Vector3(
                worldScale.x / transform.parent.lossyScale.x,
                worldScale.y / transform.parent.lossyScale.y,
                worldScale.z / transform.parent.lossyScale.z
            )
            : worldScale;
    }

    public void Collect()
    {
        if (isCollected || isPlaced)
            return;

        if (trayPosition == null)
        {
            Debug.LogError(itemID + " has no Tray Position!");
            return;
        }

        isCollected = true;

        transform.SetParent(null);
        transform.position = trayPosition.position;
        transform.rotation = trayPosition.rotation;

        SetWorldScale(originalWorldScale);

        if (rb != null)
            rb.isKinematic = true;

        Debug.Log(itemID + " moved to tray!");
    }

    public void PickUp(Transform holdPosition)
    {
        if (!isCollected || isPlaced)
            return;

        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        SetWorldScale(heldScale);

        Debug.Log(itemID + " picked up!");
    }

    // Place item directly at the detected PlacementSpot.
    public bool Place(Transform placementPosition)
    {
        if (placementPosition == null)
        {
            Debug.LogError(itemID + " has no placement position!");
            return false;
        }

        transform.SetParent(null);

        transform.position = placementPosition.position;
        transform.rotation = placementPosition.rotation;

        SetWorldScale(originalWorldScale);

        isCollected = false;
        isPlaced = true;

        if (rb != null)
            rb.isKinematic = true;

        Debug.Log(itemID + " correctly placed!");

        return true;
    }
}
using UnityEngine;

public class PlayerCollector : MonoBehaviour

{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player touched: " + other.name);

        KitchenItem item =
            other.GetComponentInParent<KitchenItem>();

        if (item != null)
        {
            Debug.Log("Found kitchen item: " + item.itemID);

            item.Collect();

            Debug.Log("Collected: " + item.itemID);
        }
    }
}
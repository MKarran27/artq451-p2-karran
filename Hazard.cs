using UnityEngine;

public class Hazard : MonoBehaviour
{
    [SerializeField] int damage = 1;   // How much health one hit takes.

    // Keeps running while the player is inside this trigger.
    // Anything else needs a Rigidbody to count.
    void OnTriggerStay(Collider other)
    {
        // Does the thing inside have Health? The player does.
        // A crate doesn't, so nothing happens to it.
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        // It has Health: take the hit. Health's safe time keeps it to one hit a second.
        if (health != null) health.TakeDamage(damage);
    }
}
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    // Runs once when something walks into this trigger.
    void OnTriggerEnter(Collider other)
    {
        // Look for Health on the thing that came in. The player has it.
        // Anything else gives null, which means nothing.
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        // This pad's position is the new respawn point.
        if (health != null)
            health.SetCheckpoint(transform.position);
    }
}

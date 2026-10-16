using UnityEngine;   // Unity's tools: MonoBehaviour, Vector3, Time and the rest.
using TMPro;         // TMP_Text, TextMeshPro text, lives here.

// PlayerHealth.cs, on PlayerCapsule. The Oct 15 slides Health, Invincibility time, and Respawn and checkpoints,
// in one script. The slides call it Health. It has its own name here, so Health.cs from Week06_Live_Scripts
// can go into this project in class without a clash.
// Two extras the slides don't show: the health text, and the red flash when you get hit.
// Like the slides, the respawn point comes from a checkpoint: Checkpoint Start is under the player at Play.
// The class name must match the file name. MonoBehaviour makes it a component.
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;               // How many hits you can take.
    [SerializeField] float safeTime = 1f;             // Seconds you're safe after a hit.
    [SerializeField] CharacterController controller;  // Drag the player in here.

    [Header("Extras")]                                // A bold label in the Inspector.
    [SerializeField] TMP_Text healthText;             // The Health text at the top left.
    [SerializeField] HurtFlash flash;                 // The red screen flash.

    int health;              // Health right now.
    float safeLeft;          // Seconds of safe time left.
    Vector3 respawnPoint;    // Where you come back. A checkpoint sets it.

    // Unity runs Start once, when you press Play, before the first frame.
    void Start()
    {
        // Start with full health.
        health = maxHealth;
        // Put it on screen.
        ShowHealth();
    }

    // Unity runs Update every frame.
    void Update()
    {
        // Count the safe time down a little every frame.
        if (safeLeft > 0f) safeLeft -= Time.deltaTime;
    }

    // Public, so the lava and the enemy can call it.
    public void TakeDamage(int amount)
    {
        // Still safe from the last hit? Ignore this one.
        if (safeLeft > 0f) return;
        // Take the hit.
        health -= amount;
        // Start the safe time.
        safeLeft = safeTime;
        // Flash the screen red, if a flash was dragged in.
        if (flash != null) flash.Flash();
        // Out of health? Go back to the last checkpoint.
        if (health <= 0) Respawn();
        // Update the text on screen.
        ShowHealth();
    }

    // A checkpoint calls this when you walk onto it.
    public void SetCheckpoint(Vector3 point)
    {
        // Save where the checkpoint is.
        respawnPoint = point;
    }

    // Sends you back to the last checkpoint with full health.
    void Respawn()
    {
        // The teleport from week 5: controller off, move, controller on.
        controller.enabled = false;          // While it's on, it keeps its own position.
        transform.position = respawnPoint;   // Move to the checkpoint.
        controller.enabled = true;           // Back on.
        // Full health again.
        health = maxHealth;
    }

    // Writes the health on screen.
    void ShowHealth()
    {
        // Only if a text was dragged in.
        if (healthText != null) healthText.text = "Health: " + health;
    }
}
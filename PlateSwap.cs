

using UnityEngine;

public class PlateSwap : MonoBehaviour
{
    [Header("Resources")]
    public Transform player;
    public Transform cube;

    [Header("Settings")]
    public float playerHeightOffset = 0f;
    public float cubeHeightOffset = 1f;

    private bool activated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (activated) return;

        SwapPlayerAndCube();
    }

    private void SwapPlayerAndCube()
    {
        if (player == null || cube == null)
        {
            Debug.LogError("Assign Player and Cube in the Inspector!");
            return;
        }

        activated = true;

        // Save original positions
        Vector3 playerPosition = player.position;
        Vector3 cubePosition = cube.position;

        // Get Character Controller
        CharacterController controller =
            player.GetComponent<CharacterController>();

        // Disable controller before teleporting
        if (controller != null)
            controller.enabled = false;

        // Move player to cube's original position
        player.position = cubePosition
            + Vector3.up * playerHeightOffset;

        // Re-enable controller
        if (controller != null)
            controller.enabled = true;

        // Get cube Rigidbody
        Rigidbody cubeRb = cube.GetComponent<Rigidbody>();

        // Calculate cube's new position
        Vector3 newCubePosition = playerPosition
            + Vector3.up * cubeHeightOffset;

        if (cubeRb != null)
        {
            // Only reset velocity if cube is NOT kinematic
            if (!cubeRb.isKinematic)
            {
                cubeRb.linearVelocity = Vector3.zero;
                cubeRb.angularVelocity = Vector3.zero;
            }

            // Move cube
            cubeRb.position = newCubePosition;
        }
        else
        {
            cube.position = newCubePosition;
        }

        // Update physics
        Physics.SyncTransforms();

        Debug.Log("Player and cube swapped positions!");
    }
}
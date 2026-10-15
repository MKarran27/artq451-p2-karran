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

        activated = true;
        SwapPlayerAndCube();
    }

    private void SwapPlayerAndCube()
    {
        if (player == null || cube == null)
        {
            Debug.LogError("Assign Player and Cube in the Inspector!");
            activated = false;
            return;
        }

        // Save original positions
        Vector3 playerPosition = player.position;
        Vector3 cubePosition = cube.position;

        // Move player to cube's original position
        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.position = cubePosition
            + Vector3.up * playerHeightOffset;

        if (controller != null)
            controller.enabled = true;

        // Move cube to player's original position,
        // adding height to keep it above the platform
        Rigidbody cubeRb = cube.GetComponent<Rigidbody>();

        Vector3 newCubePosition = playerPosition
            + Vector3.up * cubeHeightOffset;

        if (cubeRb != null)
        {
            cubeRb.linearVelocity = Vector3.zero;
            cubeRb.angularVelocity = Vector3.zero;
            cubeRb.position = newCubePosition;
        }
        else
        {
            cube.position = newCubePosition;
        }

        Physics.SyncTransforms();

        Debug.Log("Player and cube swapped positions!");
    }
}
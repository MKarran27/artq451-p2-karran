using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [Header("Dash")]
    [Range(0f, 40f)]
    [SerializeField] float dashSpeed = 20f;

    [Tooltip("How long the dash lasts, in seconds")]
    [Range(0.05f, 1f)]
    [SerializeField] float dashTime = 0.2f;

    float dashLeft;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) dashLeft = dashTime;
        if (dashLeft <= 0f) return;
        dashLeft -= Time.deltaTime;
        Vector3 step = transform.forward * dashSpeed;
        controller.Move(step * Time.deltaTime);
    }
}
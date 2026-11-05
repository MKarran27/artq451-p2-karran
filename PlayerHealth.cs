
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] int maxHealth = 2;
    [SerializeField] float safeTime = 3f;
    [SerializeField] CharacterController controller;

    [Header("Extras")]
    [SerializeField] TMP_Text healthText;
    [SerializeField] HurtFlash flash;

    private int health;
    private float safeLeft;
    private Vector3 respawnPoint;

    void Start()
    {
        health = maxHealth;
        ShowHealth();
    }

    void Update()
    {
        if (safeLeft > 0f)
            safeLeft -= Time.deltaTime;
    }

    public void TakeDamage(int amount)
    {
        if (safeLeft > 0f) return;

        // Don't take damage after the game ends.
        if (GameManager.Instance == null ||
            GameManager.Instance.state != GameManager.State.Playing)
            return;

        health -= amount;
        health = Mathf.Max(health, 0);

        safeLeft = safeTime;

        if (flash != null)
            flash.Flash();

        ShowHealth();

        // If health reaches zero, show Game Over.
        if (health <= 0)
        {
            GameManager.Instance.Lose();
        }
    }

    public void SetCheckpoint(Vector3 point)
    {
        respawnPoint = point;
    }

    void ShowHealth()
    {
        if (healthText != null)
            healthText.text = "Health: " + health;
    }
}

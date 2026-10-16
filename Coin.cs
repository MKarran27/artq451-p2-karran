using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] GameManager manager;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        manager.AddCoin();
        gameObject.SetActive(false);

    }
}
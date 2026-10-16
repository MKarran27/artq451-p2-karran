using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum State { Playing, Won }
    public State state = State.Playing;

    [SerializeField] int coinsToWin = 3;
    int coins;

    [SerializeField] TMP_Text coinText;

    public void AddCoin()
    {
        coins++;
        Debug.Log("Coins:" + coins);
        coinText.text = "Coins: " + coins;
        if (coins >= coinsToWin) state = State.Won;
    }
}
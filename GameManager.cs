using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public enum State { Playing, Won, Lost }
    public State state = State.Playing;

    [SerializeField] int coinsToWin = 3;
    int coins;

    [Header("Kitchen Items")]
    [SerializeField] int itemsToWin = 5;
    int items;

    // Keeps track of items already counted.
    HashSet<int> countedItems = new HashSet<int>();

    [Header("UI")]
    [SerializeField] TMP_Text coinText;
    [SerializeField] TMP_Text itemText;

    [Header("Week 7")]
    [SerializeField] int lives = 3;
    [SerializeField] TMP_Text livesText;
    [SerializeField] GameObject endPanel;
    [SerializeField] TMP_Text endText;
    [SerializeField] string titleScene = "Title";
    [SerializeField] StarterAssetsInputs input;
    [SerializeField] PlayerInput playerInput;

    [Header("Week 8")]
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioClip coinSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip loseSound;
    [SerializeField] TMP_Text timeText;
    [SerializeField] float slowMotion = 0.25f;
    public UnityEvent onWon;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;

        ShowLives();

        if (coinText != null)
            coinText.text = "Coins: " + coins;

        if (itemText != null)
            itemText.text = "Kitchen Items: " + items + " / " + itemsToWin;
    }

    public void AddCoin()
    {
        if (state != State.Playing) return;

        coins++;

        Debug.Log("Coins: " + coins);

        if (coinText != null)
            coinText.text = "Coins: " + coins;

        PlaySound(coinSound);

        CheckWinCondition();
    }

    // Called when an item is placed in its correct spot.
    public void AddItem(GameObject item)
    {
        if (state != State.Playing) return;

        if (item == null) return;

        // Make sure the same item isn't counted twice.
        int itemID = item.GetInstanceID();

        if (countedItems.Contains(itemID)) return;

        countedItems.Add(itemID);

        items = countedItems.Count;

        // Update the counter.
        if (itemText != null)
        {
            itemText.text = "Kitchen Items: " + items + " / " + itemsToWin;
        }

        Debug.Log("Correctly placed items: " + items + " / " + itemsToWin);

        CheckWinCondition();
    }

    // Check if both coins and kitchen items are complete.
    void CheckWinCondition()
    {
        if (state != State.Playing) return;

        if (coins >= coinsToWin && items >= itemsToWin)
        {
            Win();
        }
    }

    public void RemoveItem(GameObject item)
    {
        if (state != State.Playing) return;

        if (item == null) return;

        int itemID = item.GetInstanceID();

        if (!countedItems.Contains(itemID)) return;

        countedItems.Remove(itemID);

        items = countedItems.Count;

        if (itemText != null)
        {
            itemText.text = "Kitchen Items: " + items + " / " + itemsToWin;
        }
    }

    public void LoseLife()
    {
        if (state != State.Playing) return;

        lives--;

        ShowLives();

        if (lives <= 0)
            Lose();
    }

    void Win()
    {
        if (state != State.Playing) return;

        state = State.Won;

        PlaySound(winSound);

        onWon.Invoke();

        StartCoroutine(WinSlowly());
    }

    IEnumerator WinSlowly()
    {
        float runTime = Time.timeSinceLevelLoad;

        Time.timeScale = slowMotion;

        yield return new WaitForSecondsRealtime(1.5f);

        Time.timeScale = 1f;

        ShowEnd("You win");

        ShowTime(runTime);
    }

    public void Lose()
    {
        if (state != State.Playing) return;

        state = State.Lost;

        Time.timeScale = 0f;

        ShowEnd("GAME OVER!");
    }

    void ShowEnd(string message)
    {
        if (endPanel == null) return;

        endPanel.SetActive(true);

        if (endText != null)
            endText.text = message;

        StopPlayer();
    }

    void ShowTime(float runTime)
    {
        float best = PlayerPrefs.GetFloat("bestTime", 0f);

        if (best == 0f || runTime < best)
        {
            best = runTime;

            PlayerPrefs.SetFloat("bestTime", best);

            PlayerPrefs.Save();
        }

        string message = "Time " + runTime.ToString("0.0") +
                         "   Best " + best.ToString("0.0");

        if (timeText != null)
            timeText.text = message;
    }

    public void StopPlayer()
    {
        if (playerInput != null)
            playerInput.enabled = false;

        if (input != null)
        {
            input.move = Vector2.zero;
            input.look = Vector2.zero;
            input.cursorLocked = false;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumePlayer()
    {
        if (playerInput != null)
            playerInput.enabled = true;

        if (input != null)
            input.cursorLocked = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToTitle()
    {
        SceneManager.LoadScene(titleScene);
    }

    void ShowLives()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    void PlaySound(AudioClip clip)
    {
        if (sfx != null && clip != null)
            sfx.PlayOneShot(clip);
    }
}
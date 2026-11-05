
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] GameObject pausePanel;

    [Header("Sound Button")]
    [SerializeField] TMP_Text soundButtonText;

    private bool paused = false;
    private bool soundOn = true;

    void Start()
    {
        paused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = false;

        UpdateSoundText();
    }

    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            // Do not pause after winning or losing.
            if (GameManager.Instance == null ||
                GameManager.Instance.state != GameManager.State.Playing)
                return;

            if (paused)
                Resume();
            else
                Pause();
        }
    }

    // =========================
    // PAUSE
    // =========================

    public void Pause()
    {
        if (paused) return;

        if (GameManager.Instance == null ||
            GameManager.Instance.state != GameManager.State.Playing)
            return;

        paused = true;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Time.timeScale = 0f;
        AudioListener.pause = true;

        GameManager.Instance.StopPlayer();
    }

    // =========================
    // RESUME
    // =========================

    public void Resume()
    {
        if (GameManager.Instance == null ||
            GameManager.Instance.state != GameManager.State.Playing)
            return;

        paused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Time.timeScale = 1f;
        AudioListener.pause = !soundOn;

        GameManager.Instance.ResumePlayer();
    }

    // =========================
    // RESTART
    // =========================

    public void Restart()
    {
        paused = false;

        Time.timeScale = 1f;
        AudioListener.pause = false;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    // =========================
    // SOUND ON/OFF
    // =========================

    public void ToggleSound()
    {
        soundOn = !soundOn;

        AudioListener.pause = !soundOn || paused;

        UpdateSoundText();
    }

    void UpdateSoundText()
    {
        if (soundButtonText != null)
        {
            soundButtonText.text = soundOn
                ? "SOUND: ON"
                : "SOUND: OFF";
        }
    }

    // =========================
    // QUIT TO TITLE
    // =========================

    public void Quit()
    {
        paused = false;

        Time.timeScale = 1f;
        AudioListener.pause = false;

        GameManager.Instance.ToTitle();
    }
}
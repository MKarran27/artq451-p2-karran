using UnityEngine;                    // Unity's tools: MonoBehaviour, Cursor and the rest.
using UnityEngine.SceneManagement;    // SceneManager, for loading scenes, lives here.

// The class name must match the file name. MonoBehaviour makes it a component.
public class TitleMenu : MonoBehaviour
{   // Start of the TitleMenu class.
    [SerializeField] string gameScene = "Game";   // Your level's scene name, spelled exactly.

    // Unity runs Start once, when the title scene starts.
    void Start()
    {   // Start of Start.
        // Coming back from the game, the mouse can still be locked. Free it for the buttons.
        Cursor.lockState = CursorLockMode.None;
        // And show it.
        Cursor.visible = true;
    }   // End of Start.

    // The Play button calls this. Public, so it shows in the button's On Click list.
    public void Play()
    {   // Start of Play.
        // Load your level by its name.
        SceneManager.LoadScene(gameScene);
    }   // End of Play.
}   // End of the TitleMenu class.

using UnityEngine;      // Unity's tools: MonoBehaviour, Vector3, Time and the rest.
using UnityEngine.UI;   // Image lives here.

// HurtFlash.cs, on a red Image that fills the screen. In first person you can't see your own body,
// so a hit flashes the screen red and fades out.
// The class name must match the file name. MonoBehaviour makes it a component.
public class HurtFlash : MonoBehaviour
{
    [SerializeField] Image image;              // The red Image that fills the screen.
    [Range(0f, 1f)]                            // A slider from 0 to 1 in the Inspector.
    [SerializeField] float strength = 0.45f;   // Red at the start: 0 is clear, 1 is solid.
    [SerializeField] float fadeTime = 0.4f;    // Seconds to fade out.

    float left;                // Seconds of fade left. 0 means no red.

    // Public, so PlayerHealth can call it on a hit.
    public void Flash()
    {
        // Start the fade from the top.
        left = fadeTime;
    }

    // Unity runs Update every frame.
    void Update()
    {
        // Count the fade down.
        if (left > 0f) left -= Time.deltaTime;
        // Copy the Image's color, so we can change it.
        Color c = image.color;
        // a is how solid it is. left / fadeTime goes from 1 to 0, so the red fades out.
        c.a = strength * Mathf.Clamp01(left / fadeTime);
        // Put the color back on the Image.
        image.color = c;
    }
}
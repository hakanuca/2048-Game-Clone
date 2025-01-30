using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour
{
    public Text fpsText;
    private int frameCount = 0;
    private float elapsedTime = 0f;
    private float fps = 0f;
    
    void Update()
    {
        frameCount++;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= 0.5f) // Update every 1 second
        {
            fps = frameCount / elapsedTime;
            fpsText.text = $"FPS: {Mathf.Round(fps)}";

            // Reset counters
            frameCount = 0;
            elapsedTime = 0f;
        }
    }
}
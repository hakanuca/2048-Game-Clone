using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject cutScene;
    [SerializeField] private float scaleDuration = 1.5f; // Duration of the scaling animation
    [SerializeField] private Vector3 targetScale = new Vector3(2f, 2f, 2f); // Target scale size

    // This method will be linked to the Play button
    public void OnPlayButtonClick()
    {
        if (cutScene != null)
        {
            // Start the coroutine to scale up the cutscene before transitioning scenes
            StartCoroutine(ScaleCutsceneAndLoadScene());
        }
        else
        {
            // Load the next scene immediately if no cutscene is assigned
            LoadNextScene();
        }
    }

    private IEnumerator ScaleCutsceneAndLoadScene()
    {
        Vector3 initialScale = cutScene.transform.localScale;
        float elapsedTime = 0f;

        // Smoothly scale the cutscene over time
        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / scaleDuration;
            cutScene.transform.localScale = Vector3.Lerp(initialScale, targetScale, progress);
            yield return null; // Wait for the next frame
        }

        // Ensure the final scale is exactly the target scale
        cutScene.transform.localScale = targetScale;

        // Load the next scene after scaling
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // This method will be linked to the Quit button
    public void OnQuitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // This method will be linked to the Credits button
    public void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

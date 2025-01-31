using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class MainMenu : MonoBehaviour
{
    // This method will be linked to the Play button
    public void OnPlayButtonClick()
    {
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Load the next scene by incrementing the index
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    // This method will be linked to the Quit button
    public void OnQuitButtonClick()
    {
        // Quit the application
        Application.Quit();

        // If running in the Unity Editor, stop playing
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    // This method will be linked to the Credits button
    public void OnCreditsButtonClick()
    {
        // Load the scene named "Credits"
        SceneManager.LoadScene("Credits");
    }
    
    public void OnBackButtonClick()
    {
        // Load the scene named "Credits"
        SceneManager.LoadScene("MainMenu");
    }
}
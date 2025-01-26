using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class PlayButtonScript : MonoBehaviour
{
    // This method will be linked to the Play button
    public void OnPlayButtonClick()
    {
        // Get the index of the current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        
        // Load the next scene by incrementing the index
        SceneManager.LoadScene(currentSceneIndex + 1);
    }
}
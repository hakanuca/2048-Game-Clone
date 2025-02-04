using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject logo2N;
    [SerializeField] private GameObject buttons2N;
    [SerializeField] private GameObject panel2N;

    [SerializeField] private float fadeDuration = 0.5f;  // Duration of fade effect
    [SerializeField] private float scaleDuration = 0.5f; // Duration of scaling effect
    
    void Start()
    {
        Application.targetFrameRate = 120; 
    }
    
    // This method will be linked to the Play button
    public void OnPlayButtonClick()
    {
        if (logo2N != null && buttons2N != null && panel2N != null)
        {
            StartCoroutine(CutSceneAnimation());
        }
        else
        {
            LoadNextScene();
        }
    }

    private IEnumerator CutSceneAnimation()
    {
        // Get SpriteRenderer components
        SpriteRenderer logoSprite = logo2N.GetComponent<SpriteRenderer>();
        SpriteRenderer buttonsSprite = buttons2N.GetComponent<SpriteRenderer>();

        if (logoSprite != null && buttonsSprite != null)
        {
            // Fade out logo2N and buttons2N
            yield return StartCoroutine(FadeOutSprite(logoSprite));
            yield return StartCoroutine(FadeOutSprite(buttonsSprite));
        }

        // Scale up panel2N
        yield return StartCoroutine(ScaleUpObject(panel2N));

        // Load the next scene after the animations
        LoadNextScene();
    }

    private IEnumerator FadeOutSprite(SpriteRenderer spriteRenderer)
    {
        float elapsedTime = 0f;
        Color startColor = spriteRenderer.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, newAlpha);
            yield return null;
        }

        // Ensure the sprite is fully transparent
        spriteRenderer.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    private IEnumerator ScaleUpObject(GameObject obj)
    {
        Vector3 startScale = obj.transform.localScale;
        Vector3 targetScale = startScale * 2f; // Scale to 2x

        float elapsedTime = 0f;

        while (elapsedTime < scaleDuration)
        {
            elapsedTime += Time.deltaTime;
            obj.transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / scaleDuration);
            yield return null;
        }

        obj.transform.localScale = targetScale; // Ensure it reaches full scale
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void OnQuitButtonClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

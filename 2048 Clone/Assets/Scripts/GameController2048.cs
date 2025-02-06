using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController2048 : MonoBehaviour
{
    public static GameController2048 instance;
    public static int ticker;
    [SerializeField] GameObject fillPrefab;
    [SerializeField] Cell2048[] allCells;
    public static Action<string> slide;
    public int myScore;
    [SerializeField] Text scoreDisplay;
    int isGameOver;
    [SerializeField] GameObject gameOverPanel;
    public Color[] fillColors;
    [SerializeField] private int winningScore;
    [SerializeField] GameObject winningPanel;
    [SerializeField] GameObject restartPanel;
    private bool hasWon;
    private Vector2 startTouchPosition, endTouchPosition;
    private float minSwipeDistance = 50f;
    public bool blockMoved = false;

    // Undo feature
    private Stack<int[,]> previousStates = new Stack<int[,]>();
    private Stack<int> previousScores = new Stack<int>();

    // Cutscene fade
    [SerializeField] private GameObject cutScenePanel;
    private CanvasGroup cutSceneCanvasGroup;
    [SerializeField] private float fadeDuration = 1f;
    
    // Screenshot Variables
    [SerializeField] private Button screenshotButton;
    [SerializeField] private Text feedbackText;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Application.targetFrameRate = 120;
        StartSpawnFill();
        StartSpawnFill();

        // Cutscene fade-out
        if (cutScenePanel != null)
        {
            cutSceneCanvasGroup = cutScenePanel.GetComponent<CanvasGroup>();
            if (cutSceneCanvasGroup != null)
            {
                StartCoroutine(FadeOutCutscene());
            }
        }
        
        // Assign screenshot button click event
        if (screenshotButton != null)
        {
            screenshotButton.onClick.AddListener(TakeScreenshot);
        }
    }

    void Update()
    {
        if (Application.isEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            HandlePCInput();
        }
        else
        {
            DetectSwipe();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnFill();
        }
    }
    
    private IEnumerator FadeOutCutscene()
    {
        float elapsedTime = 0f;
        float startAlpha = cutSceneCanvasGroup.alpha;
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            cutSceneCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            yield return null;
        }

        cutSceneCanvasGroup.alpha = 0f;
        cutSceneCanvasGroup.interactable = false;
        cutSceneCanvasGroup.blocksRaycasts = false;
    }

    private void HandlePCInput()
    {
        if (Input.GetKeyDown(KeyCode.W)) TriggerSlide("w");
        if (Input.GetKeyDown(KeyCode.D)) TriggerSlide("d");
        if (Input.GetKeyDown(KeyCode.S)) TriggerSlide("s");
        if (Input.GetKeyDown(KeyCode.A)) TriggerSlide("a");

        if (Input.GetKeyDown(KeyCode.U)) // Undo
        {
            UndoMove();
        }
    }

    private void DetectSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                endTouchPosition = touch.position;
                Vector2 swipeDelta = endTouchPosition - startTouchPosition;

                if (swipeDelta.magnitude > minSwipeDistance)
                {
                    swipeDelta.Normalize();
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        TriggerSlide(swipeDelta.x > 0 ? "d" : "a");
                    }
                    else
                    {
                        TriggerSlide(swipeDelta.y > 0 ? "w" : "s");
                    }
                }
            }
        }
    }

    private void TriggerSlide(string direction)
    {
        SaveState();
        ticker = 0;
        isGameOver = 0;
        blockMoved = false;
        slide(direction);

        if (blockMoved) 
        {
            SpawnFill();
        }
    }

    public void SpawnFill()
    {
        bool isFull = true;
        foreach (var cell in allCells)
        {
            if (cell.fill == null)
            {
                isFull = false;
            }
        }
        if (isFull) return;

        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);
        if (allCells[whichSpawn].transform.childCount != 0)
        {
            SpawnFill();
            return;
        }

        int value = UnityEngine.Random.Range(0f, 1f) < 0.8f ? 2 : 4;
        GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
        Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
        allCells[whichSpawn].fill = tempFillComp;
        tempFillComp.FillValueUpdate(value);
    }

    public void StartSpawnFill()
    {
        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);
        if (allCells[whichSpawn].transform.childCount != 0)
        {
            SpawnFill();
            return;
        }

        GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
        Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
        allCells[whichSpawn].fill = tempFillComp;
        tempFillComp.FillValueUpdate(2);
    }

    public void ScoreUpdate(int scoreIn)
    {
        myScore += scoreIn;
        scoreDisplay.text = myScore.ToString();
    }

    public void GameOverCheck()
    {
        isGameOver++;
        if (isGameOver >= 16)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void RestartCheck()
    {
        restartPanel.SetActive(true);
    }

    public void RestartPanelClose()
    {
        restartPanel.SetActive(false);
    }

    public void WinningCheck(int highScore)
    {
        if (hasWon) return;

        if (highScore == winningScore)
        {
            winningPanel.SetActive(true);
            hasWon = true;
        }
    }

    public void KeepPlaying()
    {
        winningPanel.SetActive(false);
    }

    private void SaveState()
    {
        int[,] gridState = new int[4, 4];
        for (int i = 0; i < allCells.Length; i++)
        {
            int x = i / 4;
            int y = i % 4;
            gridState[x, y] = allCells[i].fill != null ? allCells[i].fill.value : 0;
        }
        previousStates.Push(gridState);
        previousScores.Push(myScore);
    }

    public void UndoMove()
    {
        if (previousStates.Count > 0)
        {
            int[,] lastState = previousStates.Pop();
            myScore = previousScores.Pop();
            scoreDisplay.text = myScore.ToString();

            // Restore board state
            for (int i = 0; i < allCells.Length; i++)
            {
                int x = i / 4;
                int y = i % 4;

                if (lastState[x, y] == 0)
                {
                    // If the previous state had no tile, destroy the existing tile if present
                    if (allCells[i].fill != null)
                    {
                        Destroy(allCells[i].fill.gameObject);
                        allCells[i].fill = null;
                    }
                }
                else
                {
                    // If there was a tile in the previous state, ensure it exists now
                    if (allCells[i].fill == null)
                    {
                        GameObject tempFill = Instantiate(fillPrefab, allCells[i].transform);
                        Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
                        allCells[i].fill = tempFillComp;
                        tempFillComp.FillValueUpdate(lastState[x, y]);
                    }
                    else
                    {
                        // Update the existing tile value
                        allCells[i].fill.FillValueUpdate(lastState[x, y]);
                    }
                }
            }
        }
    }


    private void TakeScreenshot()
    {
        StartCoroutine(CaptureScreenshotAndSaveToGallery());
    }

    private IEnumerator CaptureScreenshotAndSaveToGallery()
    {
        yield return new WaitForEndOfFrame();

        string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        string filename = $"Screenshot_{timestamp}.png";

        string galleryPath = Path.Combine(Application.persistentDataPath, filename);
        string finalPath = Path.Combine("/storage/emulated/0/DCIM/Screenshots/", filename);

        ScreenCapture.CaptureScreenshot(filename);
        yield return new WaitForSeconds(1f);

        try
        {
            if (File.Exists(galleryPath))
            {
                File.Move(galleryPath, finalPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving screenshot: " + e.Message);
            if (feedbackText != null)
            {
                feedbackText.text = "Failed to save screenshot!";
            }
            yield break;
        }

        AndroidJavaClass mediaScan = new AndroidJavaClass("android.media.MediaScannerConnection");
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        mediaScan.CallStatic("scanFile", currentActivity, new string[] { finalPath }, null, null);

        if (feedbackText != null)
        {
            feedbackText.text = "Screenshot saved to Gallery!";
            StartCoroutine(HideFeedbackText());
        }
    }

    private IEnumerator HideFeedbackText()
    {
        yield return new WaitForSeconds(2f);
        feedbackText.text = "";
    }
}

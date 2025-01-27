using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private bool hasWon;

    private Vector2 startTouchPosition, endTouchPosition;
    private float minSwipeDistance = 50f; // Minimum swipe distance to detect a valid swipe

    public bool blockMoved = false; // Track if any block moved

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        StartSpawnFill();
        StartSpawnFill();
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

    private void HandlePCInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            TriggerSlide("w");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            TriggerSlide("d");
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            TriggerSlide("s");
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            TriggerSlide("a");
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
                        if (swipeDelta.x > 0)
                        {
                            TriggerSlide("d"); // Swipe right
                        }
                        else
                        {
                            TriggerSlide("a"); // Swipe left
                        }
                    }
                    else
                    {
                        if (swipeDelta.y > 0)
                        {
                            TriggerSlide("w"); // Swipe up
                        }
                        else
                        {
                            TriggerSlide("s"); // Swipe down
                        }
                    }
                }
            }
        }
    }

    private void TriggerSlide(string direction)
    {
        ticker = 0;
        isGameOver = 0;
        blockMoved = false; // Reset the movement tracker
        slide(direction);

        if (blockMoved) // Spawn only if a block moved
        {
            SpawnFill();
        }
    }

    public void SpawnFill()
    {
        bool isFull = true;
        for (int i = 0; i < allCells.Length; i++)
        {
            if (allCells[i].fill == null)
            {
                isFull = false;
            }
        }
        if (isFull)
        {
            return;
        }
        int whichSpawn = UnityEngine.Random.Range(0, allCells.Length);
        if (allCells[whichSpawn].transform.childCount != 0)
        {
            SpawnFill();
            return;
        }
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance < .8f)
        {
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(2);
        }
        else
        {
            GameObject tempFill = Instantiate(fillPrefab, allCells[whichSpawn].transform);
            Fill2048 tempFillComp = tempFill.GetComponent<Fill2048>();
            allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
            tempFillComp.FillValueUpdate(4);
        }
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
        allCells[whichSpawn].GetComponent<Cell2048>().fill = tempFillComp;
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
        SceneManager.LoadScene(0);
    }

    public void WinningCheck(int highScore)
    {
        if (hasWon)
        {
            return;
        }

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
}

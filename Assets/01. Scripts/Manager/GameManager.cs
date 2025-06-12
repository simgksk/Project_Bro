using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("In-Game UI")]
    public TextMeshProUGUI scoreText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI gameOverBestScoreText;

    [Header("Pause UI")]
    public GameObject pausePanel;
    public GameObject pauseButton;

    [Header("Countdown UI")]
    public TextMeshProUGUI countdownText;

    [Header("Sound Settings")]
    public AudioSource audioSource;
    public AudioClip inGameClip;
    public AudioClip gameOverClip;

    private int score = 0;
    private int bestScore = 0;
    private bool isGameOver = false;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreUI();
        gameOverPanel?.SetActive(false);
        pausePanel?.SetActive(false);
        scoreText?.gameObject.SetActive(true);
        pauseButton?.SetActive(true);
        countdownText?.gameObject.SetActive(false);

        if (audioSource != null && inGameClip != null)
        {
            audioSource.clip = inGameClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && !isGameOver)
        {
            TogglePause();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver || isPaused) return;

        score += amount;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        isGameOver = false;
        isPaused = false;
        UpdateScoreUI();
        gameOverPanel?.SetActive(false);
        pausePanel?.SetActive(false);
        scoreText?.gameObject.SetActive(true);
        pauseButton?.SetActive(true);
        Time.timeScale = 1f;

        if (audioSource != null && inGameClip != null)
        {
            audioSource.clip = inGameClip;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverPanel?.SetActive(true);
        scoreText?.gameObject.SetActive(false);
        pauseButton?.SetActive(false);

        if (audioSource != null && gameOverClip != null)
        {
            audioSource.Stop();
            audioSource.clip = gameOverClip;
            audioSource.loop = false;
            audioSource.Play();
        }

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        UpdateScoreUI();
        UpdateGameOverUI();
    }

    public void PauseGame()
    {
        if (isGameOver) return;

        isPaused = true;
        Time.timeScale = 0f;
        pausePanel?.SetActive(true);
    }

    public void ResumeGame()
    {
        if (isGameOver) return;

        pausePanel?.SetActive(false);
        StartCoroutine(ResumeWithCountdown());
    }

    private IEnumerator ResumeWithCountdown()
    {
        scoreText?.gameObject.SetActive(false);
        pauseButton?.SetActive(false);
        countdownText?.gameObject.SetActive(true);

        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSecondsRealtime(1f);
            count--;
        }

        countdownText.text = "Go!";
        yield return new WaitForSecondsRealtime(1f);

        countdownText.gameObject.SetActive(false);

        scoreText?.gameObject.SetActive(true);
        pauseButton?.SetActive(true);

        isPaused = false;
        Time.timeScale = 1f;
    }


    public void TogglePause()
    {
        if (isPaused) ResumeGame();
        else PauseGame();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void UpdateGameOverUI()
    {
        if (gameOverScoreText != null)
            gameOverScoreText.text = score.ToString();

        if (gameOverBestScoreText != null)
            gameOverBestScoreText.text = bestScore.ToString();
    }

    public int GetScore() => score;
    public int GetBestScore() => bestScore;
    public bool IsPaused() => isPaused;
}

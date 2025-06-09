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

    private int score = 0;
    private int bestScore = 0;
    private bool isGameOver = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreUI();
        gameOverPanel?.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;

        score += amount;
        UpdateScoreUI();
    }

    public void ResetScore()
    {
        score = 0;
        isGameOver = false;
        UpdateScoreUI();
        gameOverPanel?.SetActive(false);
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverPanel?.SetActive(true);
        scoreText.gameObject.SetActive(false);

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        UpdateScoreUI();
        UpdateGameOverUI();
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
}

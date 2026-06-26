using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;
    public GameObject pausePanel;

    [Header("Timer")]
    public TMP_Text timerText;
    public float timeLimit = 45f;

    private float timeRemaining;
    private bool timerRunning = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        gameOverPanel?.SetActive(false);
        winPanel?.SetActive(false);
        timeRemaining = timeLimit;
        timerRunning = true;
    }

    void Update()
    {
        if (!timerRunning) return;

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            timerRunning = false;
            GameManager.Instance.PlayerDied();
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        int seconds = Mathf.CeilToInt(timeRemaining);
        timerText.text = $"{seconds}";

        if (seconds <= 10)
            timerText.color = Color.red;
        else
            timerText.color = Color.white;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ShowGameOver()
    {
        timerRunning = false;
        gameOverPanel?.SetActive(true);
    }

    public void ShowWin()
    {
        timerRunning = false;
        winPanel?.SetActive(true);
    }

    public void ShowPauseMenu(bool show)
    {
        pausePanel?.SetActive(show);
        if (show) timerRunning = false;
        else timerRunning = true;
    }
}
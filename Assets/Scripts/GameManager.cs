using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayerDied()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowGameOver();
    }

    public void CheckWin()
    {
        BubbleController[] bubbles = FindObjectsByType<BubbleController>(FindObjectsSortMode.None);
        if (bubbles.Length == 0)
        {
            Time.timeScale = 0f;
            UIManager.Instance.ShowWin();
        }
    }

    public void RestartGame()
    {
        AudioManager.Instance?.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance?.PlayButtonClick();
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        UIManager.Instance.ShowPauseMenu(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        UIManager.Instance.ShowPauseMenu(false);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    public void Pause()
    {
        if (Time.timeScale == 0f) return;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        GameSession.ResetRun();
        SceneManager.LoadScene(1);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverListener : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject gameOverPanel;
    private bool isGameOver;

    private void OnEnable()
    {
        playerHealth.Died += HandlePlayerDied;
    }

    private void OnDisable()
    {
        playerHealth.Died -= HandlePlayerDied;
    }

    private void Update()
    {
        if (!isGameOver) return;
        if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
        {
            Restart();
        }
    }

    private void HandlePlayerDied()
    {
        isGameOver = true;
        Time.timeScale = 0f;                    //게임속 속도
        SoundManager.Instance.Play(Sfx.GameOver);
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);               //GetActiveScene().buildIndex ->지금 열려있는 씬의 번호
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}

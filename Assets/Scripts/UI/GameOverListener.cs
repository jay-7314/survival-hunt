using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverListener : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI lostCoinText;
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
        bool normal = !GameSession.isDaily && !GameSession.isTutorial;
        int lost = GameSession.RewardFor(GameSession.stage - 1);
        if (normal && lost > 0)
        {
            lostCoinText.text = $"정산되지 않은 {lost}코인을 잃었습니다.";
            lostCoinText.gameObject.SetActive(true);
        }
        else
        {
            lostCoinText.gameObject.SetActive(false);
        }
            gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        GameSession.ResetRun();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);               //GetActiveScene().buildIndex ->지금 열려있는 씬의 번호
    }

    public void GoTitle()
    {
        GameSession.ResetRun();
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}

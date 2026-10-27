using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameOverListener : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
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
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);               //GetActiveScene().buildIndex ->지금 열려있는 씬의 번호
        }
    }

    private void HandlePlayerDied()
    {
        isGameOver = true;
        Debug.Log("게임오버 - R 키로 재시작");
        Time.timeScale = 0f;                    //게임속 속도
    }
}

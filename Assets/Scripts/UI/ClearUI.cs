using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUI : MonoBehaviour
{
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI timeTexe;

    public void Show(float elapsed)
    {
        int minutes = (int)(elapsed / 60f);
        int seconds = (int)(elapsed % 60f);
        timeTexe.text = $"{minutes:00}:{seconds:00}";
        clearPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

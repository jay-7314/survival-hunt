using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUI : MonoBehaviour
{
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestText;

    private string FormatTime(float t)
    {
        int minutes = (int)(t / 60f);
        int seconds = (int)(t % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public void Show(float elapsed)
    {
        SoundManager.Instance.Play(Sfx.Clear);
        timeText.text = FormatTime(elapsed);

        if (GameSession.isDaily)
        {
            bool isNew = DailyRecord.TrySave(GameSession.dailySeed, elapsed);
            float best = DailyRecord.GetBest(GameSession.dailySeed);

            if (isNew) bestText.text = $"신기록! :{FormatTime(best)}";
            else bestText.text = $"오늘의 최고기록 : {FormatTime(best)}";

            bestText.gameObject.SetActive(true);
        }
        else
        {
            bestText.gameObject.SetActive(false);
        }
        clearPanel.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}

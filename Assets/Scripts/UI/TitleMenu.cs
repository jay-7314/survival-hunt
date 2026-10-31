using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject dailyPanel;
    [SerializeField] private Fader fader;
    [SerializeField] private int gameSceneIndex = 2;
    [SerializeField] private CharacterData[] dailyCharacters;
    [SerializeField] private TextMeshProUGUI dailyBestText;

    public void OnStartClicked()
    {
        GameSession.isDaily = false;
        StartCoroutine(ShowPanel(selectPanel));

    }

    public void OnDailyClicked()
    {
        GameSession.isDaily = true;
        int seed = GameSession.TodaySeed();
        GameSession.dailySeed = seed;
        GameSession.selectedCharacter = dailyCharacters[seed % dailyCharacters.Length];

        float best = DailyRecord.GetBest(seed);
        if (best < 0) dailyBestText.text = "오늘의 기록 없음";
        else dailyBestText.text = $"오늘의 최고기록 {DailyRecord.FormatTime(best)}";
        StartCoroutine(ShowPanel(dailyPanel));
    }

    public void OnBackClicked()
    {
        StartCoroutine(ShowPanel(titlePanel));
    }

    public void OnPlayClicked()
    {
        StartCoroutine(LoadGame());
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    private IEnumerator ShowPanel(GameObject target)
    {
        yield return fader.FadeOut();
        titlePanel.SetActive(false);
        selectPanel.SetActive(false);
        dailyPanel.SetActive(false);
        target.SetActive(true);
        yield return fader.FadeIn();
    }

    private IEnumerator LoadGame()
    {
        yield return fader.FadeOut();
        SceneManager.LoadScene(gameSceneIndex);
    }

}

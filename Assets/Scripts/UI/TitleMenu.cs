using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject dailyPanel;
    [SerializeField] private Fader fader;
    [SerializeField] private int gameSceneIndex = 2;
    [SerializeField] private CharacterData[] dailyCharacters;
    [SerializeField] private TextMeshProUGUI dailyBestText;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private Image tutorialIcon;
    [SerializeField] private Sprite tutorialOnSprite;
    [SerializeField] private Sprite tutorialOffSprite;

    public void OnStartClicked()
    {
        GameSession.isDaily = false;
        GameSession.isTutorial = GameSession.TutorialOn;
        StartCoroutine(ShowPanel(selectPanel));
        GameSession.ResetRun();
    }

    public void OnTutorialToggleClicked()
    {
        GameSession.TutorialOn = !GameSession.TutorialOn;
        RefreshTutorialIcon();
    }

    private void RefreshTutorialIcon()
    {
        if (tutorialIcon == null) return;
        tutorialIcon.sprite = GameSession.TutorialOn ? tutorialOnSprite : tutorialOffSprite;
    }

    private void Start()
    {
        if (Application.isMobilePlatform)
        {
            quitButton.SetActive(false);
        }
        RefreshTutorialIcon();
    }

    public void OnDailyClicked()
    {
        GameSession.isDaily = true;
        GameSession.isTutorial = false;
        int seed = GameSession.TodaySeed();
        GameSession.dailySeed = seed;
        GameSession.selectedCharacter = dailyCharacters[seed % dailyCharacters.Length];

        float best = DailyRecord.GetBest(seed);
        if (best < 0) dailyBestText.text = "오늘의 기록 없음";
        else dailyBestText.text = $"오늘의 최고기록 {DailyRecord.FormatTime(best)}";
        StartCoroutine(ShowPanel(dailyPanel));
        GameSession.ResetRun();
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
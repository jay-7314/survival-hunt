using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearUI : MonoBehaviour
{
    [SerializeField] private CardManager cardManager;
    [SerializeField] private Health playerHealth;
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestText;
    [SerializeField] private TextMeshProUGUI rewardText;
    [SerializeField] private GameObject startGameButton;
    [SerializeField] private GameObject settleButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject titleButton;
    [SerializeField] private Fader fader;

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

        bool normal = !GameSession.isDaily && !GameSession.isTutorial;

        if (GameSession.isDaily)
        {
            bool isNew = DailyRecord.TrySave(GameSession.dailySeed, elapsed);
            float best = DailyRecord.GetBest(GameSession.dailySeed);

            if (isNew)
            {
                CoinWallet.Add(1);
                bestText.text = $"신기록! :{FormatTime(best)}\n코인 +1";
            }
            else
            {
                bestText.text = $"오늘의 최고기록 : {FormatTime(best)}";
            }
            bestText.gameObject.SetActive(true);
        }
        else
        {
            bestText.gameObject.SetActive(false);
        }

        startGameButton.SetActive(GameSession.isTutorial);

        settleButton.SetActive(normal);
        continueButton.SetActive(normal);
        restartButton.SetActive(!normal);
        titleButton.SetActive(!normal);
        rewardText.gameObject.SetActive(normal);

        if (normal)
        {
            int now = GameSession.RewardFor(GameSession.stage);
            int next = GameSession.RewardFor(GameSession.stage + 1);
            rewardText.text = $"{GameSession.stage}판 클리어!\n지금 정산하면 +{now}코인\n계속하면 다음 판에서 +{next}코인 (죽으면 0)";
        }

        clearPanel.SetActive(true);
    }

    public void Settle()
    {
        int reward = GameSession.RewardFor(GameSession.stage);
        CoinWallet.Add(reward);
        GameSession.ResetRun();

        rewardText.text = $"정산 완료! +{reward}코인";
        settleButton.SetActive(false);
        continueButton.SetActive(false);

        StartCoroutine(GoTitleRoutine());
    }

    private IEnumerator GoTitleRoutine()
    {
        yield return new WaitForSecondsRealtime(1.5f);      //결과 글자를 잠깐 보여 줌
        if (fader != null) yield return fader.FadeOut();    //화면을 어둡게
        GoTitle();                                          //타이틀로
    }

    public void Continue()
    {
        cardManager.SaveToSession();
        GameSession.carryHealth = playerHealth.Current;
        GameSession.hasCarryOver = true;

        GameSession.stage++;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartRealGame()
    {
        StartCoroutine(StartRealGameRoutine());
    }

    private IEnumerator StartRealGameRoutine()
    {
        GameSession.isTutorial = false;
        if (fader != null) yield return fader.FadeOut();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }
}
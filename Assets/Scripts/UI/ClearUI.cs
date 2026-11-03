using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.LevelPlay;

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
    [SerializeField] private GameObject adSettleButton;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject restartButton;
    [SerializeField] private GameObject titleButton;
    [SerializeField] private Fader fader;

    private bool adReady;
    private bool settled;

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

        settled = false;

        if (normal)
        {
            int now = GameSession.RewardFor(GameSession.stage);
            int next = GameSession.RewardFor(GameSession.stage + 1);
            rewardText.text = $"{GameSession.stage}판 클리어!\n지금 정산하면 +{now}코인\n계속하면 다음 판에서 +{next}코인 (죽으면 0)";

            adSettleButton.GetComponentInChildren<TextMeshProUGUI>().text = $"광고 보고 {now * 2}코인 정산";
        }
        adSettleButton.SetActive(normal && adReady);

        clearPanel.SetActive(true);
    }

    public void Settle()
    {
        FinishSettle(GameSession.RewardFor(GameSession.stage));
    }

    private void FinishSettle(int reward)
    {
        if (settled) return;                 //두 번 정산되는 것 방지
        settled = true;

        CoinWallet.Add(reward);
        GameSession.ResetRun();

        rewardText.text = $"정산 완료! +{reward}코인";
        settleButton.SetActive(false);
        continueButton.SetActive(false);
        adSettleButton.SetActive(false);

        StartCoroutine(GoTitleRoutine());
    }

    public void OnAdLoaded(LevelPlayAdInfo info)
    {
        adReady = true;                      //광고가 준비됐음을 기억
    }

    public void OnAdRewarded(LevelPlayAdInfo info, LevelPlayReward reward)
    {
        FinishSettle(GameSession.RewardFor(GameSession.stage) * 2);   //끝까지 봤을 때만 2배
    }

    private IEnumerator GoTitleRoutine()
    {
        yield return new WaitForSecondsRealtime(2.5f);      //결과 글자를 잠깐 보여 줌
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

#if UNITY_EDITOR
    [ContextMenu("Test Show Clear")]
    private void TestShow()
    {
        Show(12f);
        Time.timeScale = 0f;
    }
#endif
}
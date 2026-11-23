using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialGuide : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform goal;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private float freezeTime = 0.5f;
    [SerializeField] private float messageTime = 3f;
    [SerializeField] private float closeDistance = 2.5f;
    [SerializeField] private float keyNearDistance = 5f;
    [SerializeField] private float goalNearDistance = 5f;

    private bool shownEnemy;
    private bool shownClose;
    private bool shownKey;
    private bool shownGoal;
    private bool busy;

    private void Start()
    {
        hintText.gameObject.SetActive(false);
        if (!GameSession.isTutorial)
        {
            enabled = false;
            return;
        }
        // 튜토리얼을 시작했으니 다음부터는 자동으로 끈다.
        GameSession.TutorialOn = false;
    }

    private void Update()
    {
        if (busy || Time.timeScale == 0f) return;
        if (!shownEnemy && Enemy.All.Count > 0)
        {
            shownEnemy = true;
            StartCoroutine(Show(Application.isMobilePlatform ? "적이 나타났어요! 원거리 공격으로 공격해보세요" : "적이 나타났어요! Space키로 원거리 공격을 해보세요"));
            return;
        }

        if (shownEnemy && !shownClose && NearestEnemyDistance() < closeDistance)
        {
            shownClose = true;
            StartCoroutine(Show(Application.isMobilePlatform ? "적이 가까이 왔어요! 근접 공격으로 공격하세요" : "적이 가까이 왔어요! F키로 근접 공격을 하세요"));
            return;
        }

        if (!shownKey && IsKeyNear())
        {
            shownKey = true;
            StartCoroutine(Show("열쇠를 먹으면 깃발이 나타납니다."));
            return;
        }

        if (!shownGoal && IsGoalNear())
        {
            shownGoal = true;
            StartCoroutine(Show("깃발에 들어가세요"));
            return;
        }
    }

    private IEnumerator Show(string message)
    {
        busy = true;
        hintText.text = message;
        hintText.gameObject.SetActive(true);

        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(freezeTime);
        Time.timeScale = 1f;

        yield return new WaitForSecondsRealtime(messageTime);
        hintText.gameObject.SetActive(false);
        busy = false;
    }

    private float NearestEnemyDistance()
    {
        float best = float.MaxValue;
        for (int i = 0; i < Enemy.All.Count; i++)
        {
            float d = Vector2.Distance(player.position, Enemy.All[i].transform.position);
            if (d < best) best = d;
        }
        return best;
    }

    private bool IsKeyNear()
    {
        if (KeyManager.instance == null) return false;
        Transform key = KeyManager.instance.CurrentTarget;
        if (key == null) return false;
        return Vector2.Distance(player.position, key.position) < keyNearDistance;
    }

    private bool IsGoalNear()
    {
        if (KeyManager.instance == null) return false;
        if (KeyManager.instance.Total == 0 || KeyManager.instance.CurrentTarget != null) return false;
        if (!goal.gameObject.activeInHierarchy) return false;
        return Vector2.Distance(player.position, goal.position) < goalNearDistance;
    }
}
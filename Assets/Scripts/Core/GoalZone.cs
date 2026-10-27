using UnityEngine;

public class GoalZone : MonoBehaviour
{
    [SerializeField] private GameTimer timer;
    [SerializeField] private ClearUI clearUI;
    private bool reached;       //이미 도착했는지 기억하는 변수

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (reached) return;
        if (other.GetComponent<PlayerMove>() == null) return;       //적이나 투사체가 닿아도 클리어가 되지 않게 함.

        reached = true;
        clearUI.Show(timer.Elapsed);
        timer.Stop();
        Time.timeScale = 0f;
        Debug.Log($"도착! 클리어시간 {timer.Elapsed:F1}");
    }
}

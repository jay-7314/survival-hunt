using UnityEngine;
using UnityEngine.UI;

public class MinimapGoalArrow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform goal;
    [SerializeField] private float radius = 125f;
    [SerializeField] private float hideDistance = 7.5f;

    private RectTransform rect;
    private Image arrowImage;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        arrowImage = GetComponent<Image>();
    }

    private void LateUpdate()
    {
        if (player == null || goal == null) return;

        Vector2 toGoal = goal.position - player.position;
        bool farAway = toGoal.magnitude > hideDistance;
        arrowImage.enabled = farAway;
        if (!farAway) return;

        Vector2 direction = toGoal.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rect.anchoredPosition = direction * radius;
        rect.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}

using UnityEngine;
using UnityEngine.UI;

public class OverheadHealthBar : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Transform target;
    [SerializeField] private Image fill;
    [SerializeField] private Vector3 offset = new Vector3(0f, 1.2f, 0f);

    private void OnEnable()
    {
        health.Changed += Refresh;
    }

    private void OnDisable()
    {
        health.Changed -= Refresh;
    }

    private void Start()
    {
        Refresh(health.Current, health.Max);
    }

    private void Refresh(int current, int max)
    {
        if (max <= 0) return;
        fill.fillAmount = Mathf.Clamp01((float)current / max);
    }

    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position + offset;
        transform.rotation = Quaternion.identity;
    }
}
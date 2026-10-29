using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI label;

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
        current = Mathf.Max(0, current);
        fillImage.fillAmount = max>0 ?(float) current / max : 0;
        label.text = $"{current}/ {max}";
    }
}

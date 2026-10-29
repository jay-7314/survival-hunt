using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpBarUI : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI label;

    private void OnEnable()
    {
        playerLevel.ExpChanged += Refresh;
    }

    private void OnDisable()
    {
        playerLevel.ExpChanged -= Refresh;
    }

    private void Start()
    {
        Refresh(playerLevel.CurrentExp, playerLevel.ExpToNext);
    }
    private void Refresh(int current, int needed)
    {
        fillImage.fillAmount = needed > 0 ? (float)current / needed : 0f;
        label.text = $"Lv {playerLevel.Level}";
    }
}

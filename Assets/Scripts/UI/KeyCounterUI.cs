using TMPro;
using UnityEngine;

public class KeyCounterUI : MonoBehaviour
{
    [SerializeField] private KeyManager keyManager;
    [SerializeField] private TextMeshProUGUI text;

    private void OnEnable()
    {
        if (keyManager != null) keyManager.OnProgress += Refresh;
    }

    private void OnDisable()
    {
        if (keyManager != null) keyManager.OnProgress -= Refresh;

    }

    private void Start()
    {
        if (keyManager != null) Refresh(keyManager.Collected, keyManager.Total);
    }

    private void Refresh(int collected, int total)
    {
        text.text = $"{collected} / {total}";
    }
}

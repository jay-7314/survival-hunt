using TMPro;
using UnityEngine;

public class CoinText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        if (text == null) text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        CoinWallet.Changed += Refresh;
        Refresh(CoinWallet.Coins);
    }

    private void OnDisable()
    {
        CoinWallet.Changed -= Refresh;
    }

    private void Refresh(int coins)
    {
        text.text = $"코인 {coins}";
    }

#if UNITY_EDITOR
    [ContextMenu("Add 50 Coins")]
    private void DebugAdd()
    {
        CoinWallet.Add(50);
    }
    [ContextMenu("Reset Coins")]
    private void DebugReset()
    {
        PlayerPrefs.DeleteKey("coins");
        Refresh(0);
    }
#endif
}

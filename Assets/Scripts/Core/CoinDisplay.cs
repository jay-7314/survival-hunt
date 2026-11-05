using TMPro;
using UnityEngine;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;

    private void OnEnable()
    {
        CoinWallet.Changed += Show;
        Show(CoinWallet.Coins);
    }

    private void OnDisable()
    {
        CoinWallet.Changed -= Show;
    }

    private void Show(int coins)
    {
        label.text = coins.ToString("N0");
    }

    [ContextMenu("Add 100 Coins(test)")]
    private void AddTest() { CoinWallet.Add(100); }
}

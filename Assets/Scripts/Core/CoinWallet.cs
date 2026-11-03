using System;
using UnityEngine;

public class CoinWallet : MonoBehaviour
{
    private const string Key = "coins";
    public static event Action<int> Changed;
    public static int Coins => PlayerPrefs.GetInt(Key, 0);
    public static void Add(int amount)
    {
        Set(Coins + amount);
    }

    public static bool TrySpend(int amout)
    {
        if (Coins < amout) return false;
        Set(Coins - amout);
        return true;
    }

    private static void Set(int value)
    {
        PlayerPrefs.SetInt(Key, value);
        PlayerPrefs.Save();
        Changed?.Invoke(value);
    }
}


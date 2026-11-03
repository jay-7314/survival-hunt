using UnityEngine;

public static class CharacterUnlocks
{
    private static string Key(CharacterData c)
    {
        return "unlocked_" + c.name;
    }

    public static bool IsUnlocked(CharacterData c)
    {
        if (c.UnlockCost <= 0) return true;
        return PlayerPrefs.GetInt(Key(c), 0) == 1;
    }

    public static bool TryUnlock(CharacterData c)
    {
        if(IsUnlocked(c)) return true;
        if(!CoinWallet.TrySpend(c.UnlockCost)) return false;

        PlayerPrefs.SetInt(Key(c), 1);
        PlayerPrefs.Save();
        return true;
    }
}
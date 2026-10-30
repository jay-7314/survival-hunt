using UnityEngine;

public static class DailyRecord
{
    private static string Key(int seed)
    {
        return "DailyBest_" + seed;
    }

    public static string FormatTime(float t)
    {
        int minutes = (int)(t / 60f);
        int seconds = (int)(t % 60f);
        return $"{minutes:00}:{seconds:00}";
    }

    public static float GetBest(int seed)
    {
        return PlayerPrefs.GetFloat(Key(seed) ,-1f);
    }
    public static bool TrySave(int seed, float time)
    {
        float best = GetBest(seed);

        if(best <0f || time < best)
        {
            PlayerPrefs.SetFloat(Key(seed), time);
            PlayerPrefs.Save();
            return true;
        }
        return false;
    }
}


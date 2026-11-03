using UnityEngine;

public static class Difficulty
{
    private static int Stage
    {
        get
        {
            if (GameSession.isDaily || GameSession.isTutorial) return 1;
            return GameSession.stage;
        }
    }

    public static float HealthMultiplier => 1f + 0.3f * (Stage - 1);
    public static float SpeedMultiplier => 1f + 0.08f * (Stage - 1);
    public static float SpawnIntervalMultiplier => Mathf.Pow(0.9f, Stage - 1);
}
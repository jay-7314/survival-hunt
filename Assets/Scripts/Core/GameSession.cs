using UnityEngine;

public static class GameSession
{
    public static CharacterData selectedCharacter;
    public static int stage = 1;
    public static bool hasCarryOver;
    public static int carryHealth;
    public static CardData[] carryCards = new CardData[CardManager.SlotCount];
    public static int[] carryLevels = new int[CardManager.SlotCount];

    public static bool isDaily;
    public static int dailySeed;
    public static bool isTutorial;

    public static int TodaySeed()
    {
        System.DateTime now = System.DateTime.Now;
        return now.Year * 10000 + now.Month * 100 + now.Day;
    }

    public static void ResetRun()
    {
        stage = 1;
        hasCarryOver = false;
    }

    public static int RewardFor(int clearedStages)
    {
        if(clearedStages <= 0) return 0;
        return 5 + 2 * (clearedStages - 1);
    }
}
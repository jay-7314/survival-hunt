using UnityEngine;

public static class GameSession
{
    public static CharacterData selectedCharacter;

    public static bool isDaily;
    public static int dailySeed;

    public static int TodaySeed()
    {
        System.DateTime now = System.DateTime.Now;
        return now.Year * 10000 + now.Month * 100 + now.Day;

    }
}
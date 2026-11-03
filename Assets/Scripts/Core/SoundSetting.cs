using System;
using UnityEngine;

public class SoundSetting : MonoBehaviour
{
    private const string BgmKey = "bgm_on";
    private const string SfxKey = "sfx_on";

    public static event Action Changed;

    public static bool BgmOn
    {
        get => PlayerPrefs.GetInt(BgmKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(BgmKey, value ? 1 : 0);
            PlayerPrefs.Save();
            Changed?.Invoke();
        }
    }

    public static bool SfxOn
    {
        get => PlayerPrefs.GetInt(SfxKey, 1) == 1;
        set
        {
            PlayerPrefs.SetInt(SfxKey, value ? 1 : 0);
            PlayerPrefs.Save();
            Changed?.Invoke();
        }
    }
}

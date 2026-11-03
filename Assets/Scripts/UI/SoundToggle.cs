using TMPro;
using UnityEngine;

public class SoundToggle : MonoBehaviour
{
   public enum Kind { Bgm, Sfx}
    [SerializeField] private Kind kind;
    [SerializeField] private TextMeshProUGUI label;

    private void OnEnable()
    {
        SoundSetting.Changed += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        SoundSetting.Changed -= Refresh;
    }

    public void OnClick()
    {
        if(kind == Kind.Bgm) SoundSetting.BgmOn = !SoundSetting.BgmOn;
        else SoundSetting.SfxOn = !SoundSetting.SfxOn;
    }

    private void Refresh()
    {
        bool on = kind == Kind.Bgm?SoundSetting.BgmOn : SoundSetting.SfxOn;
        string name = kind == Kind.Bgm ? "배경음" : "효과음";
        label.text = $"{name} {(on ? "On":"OFF")}";
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoundToggle : MonoBehaviour
{
    public enum Kind { Bgm, Sfx }
    [SerializeField] private Kind kind;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Image icon;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

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
        if (kind == Kind.Bgm) SoundSetting.BgmOn = !SoundSetting.BgmOn;
        else SoundSetting.SfxOn = !SoundSetting.SfxOn;
    }

    private void Refresh()
    {
        bool on = kind == Kind.Bgm ? SoundSetting.BgmOn : SoundSetting.SfxOn;
        if (label != null)
        {
            string name = kind == Kind.Bgm ? "배경음" : "효과음";
            label.text = $"{name} {(on ? "On" : "OFF")}";
        }

        if (icon != null)
        {
            icon.sprite = on? onSprite : offSprite;
        }
    }
}

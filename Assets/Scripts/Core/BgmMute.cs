using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BgmMute : MonoBehaviour
{
    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        SoundSetting.Changed += Apply;
        Apply();
    }

    private void OnDisable()
    {
        SoundSetting.Changed -= Apply;
    }

    private void Apply()
    {
        source.mute = !SoundSetting.BgmOn;
    }
}

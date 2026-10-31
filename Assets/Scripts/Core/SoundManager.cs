using UnityEngine;

public enum Sfx
{
    Clear,
    Enemy_Die,
    GameOver,
    Hit,
    LevelUp,
    Player_Hurt,
    Shoot,
    Swing
}

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] sfxClips;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(Sfx sfx)
    {
        int index = (int)sfx;
        if (index >= sfxClips.Length) return;
        if (sfxClips[index] == null) return;
        sfxSource.PlayOneShot(sfxClips[index]);
    }
}

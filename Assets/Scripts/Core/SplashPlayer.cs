using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class SplashPlayer : MonoBehaviour
{
    [SerializeField] private int nextSceneIndex = 1;

    private VideoPlayer videoPlayer;
    private bool loading;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    private void Update()
    {
        if (loading) return;
        Pointer pointer = Pointer.current;
        if (pointer != null && pointer.press.wasPressedThisFrame)
        {
            GoNext();
        }
    }
    private void OnVideoEnd(VideoPlayer source)
    {
        GoNext();
    }

    private void GoNext()
    {
        if (loading) return;
        loading = true;
        SceneManager.LoadScene(nextSceneIndex);
    }
}

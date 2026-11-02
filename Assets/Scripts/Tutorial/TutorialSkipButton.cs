using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSkipButton : MonoBehaviour
{
    [SerializeField] private GameObject skipButton;
    [SerializeField] private Fader fader;

    private void Start()
    {
        if (skipButton != null) skipButton.SetActive(GameSession.isTutorial);
    }

    public void Skip()
    {
        StartCoroutine(skipRoutine());
    }

    private IEnumerator skipRoutine()
    {
        GameSession.isTutorial = false;
        if (fader != null) yield return fader.FadeOut();
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

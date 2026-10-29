using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float duration = 0.4f;

    private void Start()
    {
        group.alpha = 1f;
        StartCoroutine(Fade(1f, 0f));
    }

    public IEnumerator FadeOut()
    {
        return Fade(0f, 1f);
    }

    public IEnumerator FadeIn()
    {
        return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        group.blocksRaycasts = true;
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;                //지난시간 더하기. 게임이 멈춰있어도 흐르닌시간.
            group.alpha = Mathf.Lerp(from, to, t/duration);
            yield return null;
        }

        group.alpha = to;
        group.blocksRaycasts = to > 0f;
    }
}

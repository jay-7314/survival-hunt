using System.Collections;
using UnityEngine;

public class PlayerHitEffect : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashTime = 0.5f;

    private void OnEnable()
    {
        health.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        health.Damaged -= OnDamaged;
    }

    private void OnDamaged()
    {
        SoundManager.Instance.Play(Sfx.Player_Hurt);
        StopAllCoroutines();
        StartCoroutine(Flash());
    }

    private IEnumerator Flash()
    {
        spriteRenderer.color = new Color(1f, 0.4f, 0.4f);
        yield return new WaitForSeconds(flashTime);
        spriteRenderer.color = Color.white;
    }
}

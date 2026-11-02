using UnityEngine;

public class SwordSlashEffect : MonoBehaviour
{
    [SerializeField] private float showTime = 0.15f;
    [SerializeField] private float sweepAngle = 30f;

    private SpriteRenderer spriteRenderer;
    private Color baseColor;
    private float baseAngle;
    private bool flip;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        spriteRenderer.enabled = false;
    }

    public void Play(Vector2 direction)
    {
        baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        flip = !flip;
        timer = showTime;
        spriteRenderer.enabled = true;
        Apply(0f);
    }

    private void Update()
    {
        if (timer <= 0f) return;            //쉬고 있을때 아무일도 하지 않는 장치
        timer -= Time.deltaTime;
        float t = 1f - Mathf.Clamp01(timer / showTime);
        Apply(t);
        if (timer <= 0f)
        {
            spriteRenderer.enabled = false;
        }
    }

    private void Apply(float t)
    {
        float start = flip ? -sweepAngle / 2 : sweepAngle / 2f;
        float angle = baseAngle + Mathf.Lerp(start, -start, t);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        Color c = baseColor;
        c.a = baseColor.a * (1f - t);
        spriteRenderer.color = c;
    }
}

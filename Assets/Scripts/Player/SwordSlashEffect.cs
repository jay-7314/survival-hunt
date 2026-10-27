using UnityEngine;

public class SwordSlashEffect : MonoBehaviour
{
    [SerializeField] private float showTime = 0.15f;

    private SpriteRenderer spriteRenderer;
    private float timer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public void Play(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        spriteRenderer.enabled = true;
        timer = showTime;
    }

    private void Update()
    {
        if (timer <= 0f) return;            //쉬고 있을때 아무일도 하지 않는 장치
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            spriteRenderer.enabled = false;
        }
    }
}

using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;          // 기본 속도 (프리팹 설정, 바뀌지 않는다)
    [SerializeField] private float lifeTime = 5f;        // 안전망: 사거리 검사가 놓쳐도 이 시간이 지나면 사라진다
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Sprite waterSprite;
    [SerializeField] private Sprite earthSprite;
    [SerializeField] private Sprite windSprite;

    private int wallMask;
    private SpriteRenderer sr;
    private Element element;
    private ObjectPool<Projectile> pool;
    private Vector3 direction;
    private float timer;
    private Vector3 startPosition;
    private float maxRange = 5f;
    private float currentSpeed;                          // 이번 발사에서 실제로 쓰는 속도
    private bool released;

    public void SetPool(ObjectPool<Projectile> ownerPool)
    {
        sr = GetComponent<SpriteRenderer>();
        pool = ownerPool;
        wallMask = LayerMask.GetMask("Wall");
    }

    public void Launch(Vector3 dir, int newDamage, Element newElement, float newRange = 5f, float speedRatio = 1f)
    {
        direction = dir.normalized;
        damage = newDamage;
        element = newElement;
        Sprite s = SpriteOf(newElement);
        if (sr != null && s != null) sr.sprite = s;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        maxRange = newRange;
        // 너무 빨라지면 벽을 뚫고 지나갈 수 있어서 2배까지만 허용한다
        currentSpeed = speed * Mathf.Min(speedRatio, 2f);
        startPosition = transform.position;
        timer = 0f;
        released = false;
    }

    private void Update()
    {
        Vector3 previous = transform.position;
        transform.position += direction * currentSpeed * Time.deltaTime;

        // 날아간 거리가 사거리를 넘으면 반납한다
        if(Physics2D.Linecast(previous, transform.position, wallMask))
        {
            Despawn();
            return;
        }

        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Despawn();
        }
    }

    private Sprite SpriteOf(Element e)
    {
        switch (e)
        {
            case Element.Fire: return fireSprite;
            case Element.Water: return waterSprite;
            case Element.Earth: return earthSprite;
            case Element.Wind: return windSprite;
            default: return null;
        }
    }

    private void Despawn()
    {
        if (released) return;
        released = true;
        if (pool != null) pool.Release(this);
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        //{
        //    Despawn();
        //    return;
        //}
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;
        Health health = other.GetComponent<Health>();
        if (health != null)
        {
            float multiplier = ElementChart.GetMultiplier(element, enemy.Element);
            int finalDamage = Mathf.Max(1, Mathf.RoundToInt(damage * multiplier));

            health.TakeDamage(finalDamage);
        }
        if (hitEffectPrefab != null)
        {
            SoundManager.Instance.Play(Sfx.Hit);
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        Despawn();
    }
}
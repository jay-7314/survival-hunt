using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject hitEffectPrefab;

    private Element element;
    private ObjectPool<Projectile> pool;
    private Vector3 direction;
    private float timer;
    private bool released;

    public void SetPool(ObjectPool<Projectile> ownerPool)
    {
        pool = ownerPool;
    }

    public void Launch(Vector3 dir, int newDamage, Element newElement)
    {
        direction = dir.normalized;
        damage = newDamage;
        element = newElement;
        timer = 0f;
        released = false;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        timer += Time.deltaTime;
        if(timer>= lifeTime)
        {
            Despawn();
        }
    }

    private void Despawn()
    {
        if (released) return;
        released=true;
        if (pool != null) pool.Release(this);
        else Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Despawn();
            return;
        }
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy == null) return;
        Health health = other.GetComponent<Health>();
        if(health != null)
        {
            float multiplier = ElementChart.GetMultiplier(element, enemy.Element);
            int finalDamage = Mathf.Max(1, Mathf.RoundToInt(damage * multiplier));

            health.TakeDamage(finalDamage);
        }
        if(hitEffectPrefab != null)
        {
            SoundManager.Instance.Play(Sfx.Hit);
            Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
        }
        Despawn();
  
    }
}

using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;

    private ObjectPool<Projectile> pool;
    private Vector3 direction;
    private float timer;

    public void SetPool(ObjectPool<Projectile> ownerPool)
    {
        pool = ownerPool;
    }

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
        timer = 0f;
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
        if (pool != null) pool.Release(this);
        else Destroy(gameObject);
    }
}

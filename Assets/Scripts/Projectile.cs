using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;

    private ObjectPool<Projectile> pool;
    private Vector3 direction;
    private float timer;
    private bool released;

    public void SetPool(ObjectPool<Projectile> ownerPool)
    {
        pool = ownerPool;
    }

    public void Launch(Vector3 dir)
    {
        direction = dir.normalized;
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
        if (other.GetComponent<Enemy>() == null) return;

        Debug.Log("명중 : " + other.name);
        Despawn();
    }
}

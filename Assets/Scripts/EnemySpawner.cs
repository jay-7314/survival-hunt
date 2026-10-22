using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy zombiePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistance = 10f;

    private ObjectPool<Enemy> pool;
    private float timer;

    private void Awake()
    {
        pool = new ObjectPool<Enemy>(
            createFunc: () =>
            {
                Enemy zombie = Instantiate(zombiePrefab);
                zombie.GetComponent<EnemyChase>().SetTarget(player);
                zombie.SetPool(pool);
                return zombie;
            },
            //꺼낼때 켠다.
            actionOnGet: zombie => zombie.gameObject.SetActive(true),
            //반납할떄 끈다.
            actionOnRelease: zombie => zombie.gameObject.SetActive(false),
            //보관한도 넘으면 진짜 삭제한다.
            actionOnDestroy:zombie => Destroy(zombie.gameObject),
            defaultCapacity : 20,
            maxSize : 100
            );
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>= spawnInterval)
        {
            timer = 0f;
            Spawn();
        }
    }

    private void Spawn()
    {
        Vector2 offset = Random.insideUnitCircle.normalized * spawnDistance;
        Enemy zombie = pool.Get();
        zombie.transform.position = player.position + (Vector3)offset;
    }

}

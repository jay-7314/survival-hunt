using UnityEngine;
using UnityEngine.Pool;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy zombiePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private float minSpawnInterval = 0.5f;     //간격의 하한선
    [SerializeField] private float intervalDecreasePerSecond = 0.02f;           //1초가 지날때 간격이 줄어드는 양
    private float elapsed;      //게임시작후 흐른시간

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
        elapsed += Time.deltaTime;
        float currentInterval = Mathf.Max(minSpawnInterval, spawnInterval - elapsed * intervalDecreasePerSecond);

        timer += Time.deltaTime;
        if(timer>= currentInterval)
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

using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy zombiePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDistance = 10f;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private float spawnCheckRadius = 0.4f;
    [SerializeField] private int maxSpawnTries = 10;
    [SerializeField] private float minSpawnInterval = 0.5f;     //간격의 하한선
    [SerializeField] private float intervalDecreasePerSecond = 0.02f;           //1초가 지날때 간격이 줄어드는 양
    [SerializeField] private FlowField flowField;
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
                zombie.GetComponent<EnemyChase>().SetFlowField(flowField);
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
        for(int i = 0; i< maxSpawnTries; i++)
        {
            Vector2 pos = (Vector2)player.position + Random.insideUnitCircle.normalized * spawnDistance;
            Vector3Int cell = floorTilemap.WorldToCell(pos);
            if (!floorTilemap.HasTile(cell)) continue;
            if (Physics2D.OverlapCircle(pos, spawnCheckRadius, wallMask)) continue;

            Enemy zombie = pool.Get();
            zombie.transform.position = pos;
            return;
        }
    }

}

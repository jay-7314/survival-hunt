using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TutorialEnemies : MonoBehaviour
{
    [SerializeField] private EnemySpawner zombieSpawnwer;
    [SerializeField] private EnemySpawner robotSpawner;
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private GameObject waveText;
    [SerializeField] private int zombieCount = 2;
    [SerializeField] private int robotCount = 2;

    private IEnumerator Start()
    {
        if (!GameSession.isTutorial) yield break;
        zombieSpawnwer.enabled = false;
        robotSpawner.enabled = false;
        if(waveText != null) waveText.SetActive(false);

        yield return null;

        SpawnMany(zombieSpawnwer, zombieCount);
        SpawnMany(robotSpawner, robotCount);

    }

    private void SpawnMany(EnemySpawner spawner, int count)
    {
        Bounds area = floorTilemap.localBounds;
        int spawned = 0;
        int tries = 0;
        while(spawned < count && tries < 100)
        {
            tries++;
            if(spawner.SpawnInArea(area)!= null) spawned++;
        }
    }
}

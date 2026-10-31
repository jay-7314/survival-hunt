using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateWave : MonoBehaviour
{
    [SerializeField] private GateController gate;
    [SerializeField] private int enemyCount = 10;
    [SerializeField] private Collider2D area;

    private EnemySpawner[] spawners;
    private TextMeshProUGUI remainText;

    private bool started;
    private int waveTotal;
    public static event System.Action Cleared;

    private readonly List<Enemy> waveEnemies = new List<Enemy>();

    public void Init(EnemySpawner[] spawnerList, TextMeshProUGUI text)
    {
        spawners = spawnerList;
        remainText = text;
        if(remainText != null) remainText.gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        Enemy.Killed += HandleKilled;
    }

    private void OnDisable()
    {
        Enemy.Killed -= HandleKilled;
    }

    private void UpdateText()
    {
        if (remainText == null) return;
        remainText.text = $"Enemies {waveEnemies.Count}/{waveTotal}";
    }

    public void StartWave()
    {
        if (started) return;
        started = true;
        gate.Close();
        SpawnWave();
    }

    private void SpawnWave()
    {
        for (int i = 0; i < enemyCount * 5; i++)
        {
            if (waveEnemies.Count >= enemyCount) break;
            EnemySpawner picked = spawners[Random.Range(0, spawners.Length)];
            Enemy e = picked.SpawnInArea(area.bounds);
            if (e != null) waveEnemies.Add(e);
        }
        Debug.Log($"웨이브 적 {waveEnemies.Count}마리 소환");
        waveTotal = waveEnemies.Count;
        if (remainText != null) remainText.gameObject.SetActive(true);
        UpdateText();
        if (waveEnemies.Count == 0) gate.Open();
    }

    private void HandleKilled(Enemy enemy)
    {
        if (!waveEnemies.Remove(enemy)) return;
        Debug.Log($"웨이브 남은적 : {waveEnemies.Count}");
        UpdateText();

        if(waveEnemies.Count == 0)
        {
            gate.Open();
            if (remainText != null) remainText.gameObject.SetActive(false);
            Debug.Log("문이 열렸다.");
            Cleared?.Invoke();
        }
        
    }
}

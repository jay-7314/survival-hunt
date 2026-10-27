using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public static readonly List<Enemy> All = new List<Enemy>();             //readonly사용이유 -> 안에 추가하고 빼는건 가능하지만, 바꿔치기하는건 안됨.
    private ObjectPool<Enemy> pool;
    public static event Action<Enemy> Killed;       //적이 죽었다는 방송채널

    [SerializeField] private int expReward = 1;
    [SerializeField] private Element element;

    public Element Element => element;

    public int ExpReward => expReward;      //읽기만 하게 열어두는것, 밖에서도 읽을수는 있지만 수정은 못함.

    public void OnKilled()
    {
        Killed?.Invoke(this);
        Despawn();
    }

    private void OnEnable()
    {
        All.Add(this);
    }

    private void OnDisable()
    {
        All.Remove(this);
    }

    public void SetPool(ObjectPool<Enemy> ownerPool)
    {
        pool = ownerPool;
    }

    public void Despawn()
    {
        if (pool != null) pool.Release(this);
        else gameObject.SetActive(false);
    }
}

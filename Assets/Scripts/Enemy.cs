using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    public static readonly List<Enemy> All = new List<Enemy>();             //readonly사용이유 -> 안에 추가하고 빼는건 가능하지만, 바꿔치기하는건 안됨.
    private ObjectPool<Enemy> pool;
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

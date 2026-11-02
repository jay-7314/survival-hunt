using System;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public static KeyManager instance { get; private set; }

    [SerializeField] private List<KeyPickup> keys = new List<KeyPickup> ();
    [SerializeField] private GameObject goal;
    public event Action<int, int> OnProgress;
    public event Action OnAllCollected;

    private int collected;

    public int Collected => collected;
    public int Total => keys.Count;
    public Transform CurrentTarget => collected < keys.Count ? keys[collected].transform : null;


    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        KeyPickup.OnCollected += HandleCollected;
    }

    private void OnDisable()
    {
        KeyPickup.OnCollected -= HandleCollected;
    }

    private void Start()
    {
        for(int i = 0; i<keys.Count; i++)
        {
            keys[i].gameObject.SetActive(i == 0);
        }
        if (goal != null) goal.SetActive(keys.Count == 0);

        OnProgress?.Invoke(collected, keys.Count);
    }

    private void HandleCollected(KeyPickup key)
    {
        if (collected >= keys.Count || keys[collected] != key) return;

        collected++;
        OnProgress?.Invoke(collected, keys.Count);

        if(collected < keys.Count)
        {
            keys[collected].gameObject.SetActive(true);
        }
        else
        {
            if (goal != null) goal.SetActive(true);
            OnAllCollected?.Invoke();
        }
    }

    public void SetKeys(List<KeyPickup> newKeys)
    {
        keys = newKeys;
        collected = 0;
    }
}

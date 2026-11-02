using System;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public static event Action<KeyPickup> OnCollected;
    private bool collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        if (other.GetComponent<PlayerMove>() == null) return;

        collected = true;
        OnCollected?.Invoke(this);
        Debug.Log("열쇠 획득");
        gameObject.SetActive(false);
    }
}

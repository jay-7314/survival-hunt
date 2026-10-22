using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= amount;
        Debug.Log($"{name} 체력 {currentHealth}");

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Enemy enemy = GetComponent<Enemy>();
        if(enemy != null) enemy.OnKilled();
        else gameObject.SetActive(false);
    }
}

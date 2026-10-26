using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    public event Action Died;
    private int currentHealth;
    private int bonusMaxHealth;


    private void OnEnable()
    {
        currentHealth = maxHealth+bonusMaxHealth;
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
        Died?.Invoke();
        Enemy enemy = GetComponent<Enemy>();
        if(enemy != null) enemy.OnKilled();
        else gameObject.SetActive(false);
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        Debug.Log($"최대체력 : {maxHealth}");
    }

    public void SetMaxHealthBonus(int bonus)
    {
        int diff = bonus - bonusMaxHealth;
        bonusMaxHealth = bonus;

        if (diff > 0) currentHealth += diff;
        else currentHealth = Mathf.Min(currentHealth, maxHealth + bonusMaxHealth);

        Debug.Log($"최대체력: {maxHealth+ bonusMaxHealth} /현재체력 {currentHealth}");
    }

    public void ApplyCharacter(CharacterData data)
    {
        maxHealth = data.MaxHealth;
        currentHealth = maxHealth + bonusMaxHealth;
        Debug.Log($"케릭터 적용 : 최대체력 {maxHealth + bonusMaxHealth}");
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth + bonusMaxHealth);
        Debug.Log($"체력 회복 : {currentHealth} / {maxHealth + bonusMaxHealth}");
    }
}

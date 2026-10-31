using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    [SerializeField] private float invincibleTime = 0f;
    private float lastHitTime = -999f;
    public event Action Damaged;
    public event Action Died;
    public event Action<int, int> Changed;


    private int currentHealth;
    private int bonusMaxHealth;

    public int Current => currentHealth;
    public int Max => maxHealth + bonusMaxHealth;

    private void OnEnable()
    {
        currentHealth = maxHealth+bonusMaxHealth;
        lastHitTime = -000f;
        NotifyChanged();
    }
    
    private void NotifyChanged()
    {
        Changed?.Invoke(currentHealth, Max);
    }

    public void TakeDamage(int amount)
    {
        if (currentHealth <= 0) return;
        if (Time.time - lastHitTime < invincibleTime) return;

        lastHitTime = Time.time;
        currentHealth -= amount;
        Debug.Log($"{name} 체력 {currentHealth}");
        NotifyChanged();
        Damaged?.Invoke();
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
        NotifyChanged();
    }

    public void SetMaxHealthBonus(int bonus)
    {
        int diff = bonus - bonusMaxHealth;
        bonusMaxHealth = bonus;

        if (diff > 0) currentHealth += diff;
        else currentHealth = Mathf.Min(currentHealth, maxHealth + bonusMaxHealth);

        Debug.Log($"최대체력: {maxHealth+ bonusMaxHealth} /현재체력 {currentHealth}");
        NotifyChanged();
    }

    public void ApplyCharacter(CharacterData data)
    {
        maxHealth = data.MaxHealth;
        currentHealth = maxHealth + bonusMaxHealth;
        Debug.Log($"케릭터 적용 : 최대체력 {maxHealth + bonusMaxHealth}");
        NotifyChanged();
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth + bonusMaxHealth);
        Debug.Log($"체력 회복 : {currentHealth} / {maxHealth + bonusMaxHealth}");
        NotifyChanged();
    }

    [ContextMenu("Take 1 Damage")]
    private void TestDamage()
    {
        TakeDamage(1);
    }
}

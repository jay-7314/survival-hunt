using System;
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
        NotifyChanged();
    }

    public void SetMaxHealthBonus(int bonus)
    {
        int diff = bonus - bonusMaxHealth;
        bonusMaxHealth = bonus;

        if (diff > 0) currentHealth += diff;
        else currentHealth = Mathf.Min(currentHealth, maxHealth + bonusMaxHealth);

        NotifyChanged();
    }

    public void ApplyCharacter(CharacterData data)
    {
        maxHealth = data.MaxHealth;
        currentHealth = maxHealth + bonusMaxHealth;
        NotifyChanged();
    }

    public void Heal(int amount)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth + bonusMaxHealth);
        NotifyChanged();
    }

    public void ScaleMaxHealth(float muluiplier)
    {
        int bouns = Mathf.RoundToInt(maxHealth * (muluiplier - 1f));
        SetMaxHealthBonus(bouns);
    }
    public void SetCurrent(int value)
    {
        currentHealth = Mathf.Clamp(value, 1, maxHealth + bonusMaxHealth);
        NotifyChanged();
    }

    [ContextMenu("Take 1 Damage")]
    private void TestDamage()
    {
        TakeDamage(1);
    }
}

using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float attactInterval = 1f;

    private float nextAttackTimer;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (Time.time < nextAttackTimer) return;                            //Time.time => 현재시간

        Health health = other.GetComponent<Health>();
        if (health == null) return;

        health.TakeDamage(damage);
        nextAttackTimer = Time.time + attactInterval;
    }
}

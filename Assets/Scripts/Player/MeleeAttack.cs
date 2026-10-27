using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private float radius = 2f;     //검이 닿는 거리
    [SerializeField] private float arcAngle = 90f;      //부채꼴의 전체각도
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float cooldown = 0.8f;
    [SerializeField] private AutoTargeting targeting;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private SwordSlashEffect slashEffect;
    [SerializeField] private LayerMask wallMask;

    private float cooldownTimer;
    private InputAction meleeAction;

    private void Awake()
    {
        meleeAction = new InputAction("Melee", InputActionType.Button);
        meleeAction.AddBinding("<Keyboard>/f");
        meleeAction.AddBinding("<Gamepad>/buttonWest");
    }

    private void OnEnable()
    {
        meleeAction.Enable();  
    }

    private void OnDisable()
    {
        meleeAction.Disable();
    }

    private void Update()
    {
        cooldownTimer = Mathf.Max(0f, cooldownTimer - Time.deltaTime);
        if(meleeAction.IsPressed() && cooldownTimer <= 0f)
        {
            Swing();
        }
    }

    private bool HasLineOfSight(Vector3 targetPosition)
    {

        return !Physics2D.Linecast(transform.position, targetPosition, wallMask);
    }
    private bool IsInArc(Vector2 forward, Vector2 toTarget)
    {
        float angle = Vector2.Angle(forward, toTarget);
        return angle <= arcAngle / 2f;
    }

    private List<Enemy> FindTargets(Vector2 forward)
    {
        List<Enemy> result = new List<Enemy>();         //맞은적의 list

        for(int i = 0; i<Enemy.All.Count; i++)
        {
            Enemy enemy = Enemy.All[i];
            Vector2 toTarget = enemy.transform.position - transform.position;

            if(toTarget.magnitude <= radius && IsInArc(forward, toTarget) && HasLineOfSight(enemy.transform.position))
            {
                result.Add(enemy);
            }
        }
        return result;
    }

    [ContextMenu("Swing")]
    public void Swing()
    {
        Enemy target = targeting.Current;
        cooldownTimer = cooldown;
        if (target == null) return;

        Vector2 forward = target.transform.position - transform.position;
        slashEffect.Play(forward);
        List<Enemy> targets = FindTargets(forward);

        for(int i =0; i< targets.Count; i++)
        {
            Vector2 away = targets[i].transform.position- transform.position;       //나에게 적으로 향하는 방향,
            EnemyChase chase = targets[i].GetComponent<EnemyChase>();
            if (chase != null) chase.Knockback(away, knockbackForce);
            Health health = targets[i].GetComponent<Health>();
            if(health == null) continue;

            float multiplier = ElementChart.GetMultiplier(playerAttack.CurrentElement, targets[i].Element);
            int finalDamage = Mathf.Max(1, Mathf.RoundToInt(playerAttack.CurrentDamage * multiplier));
            health.TakeDamage(finalDamage);
        }
        Debug.Log($"검공격 :{targets.Count}마리");
    }
}

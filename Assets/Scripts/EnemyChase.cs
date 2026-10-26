using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.6f;
    [SerializeField] private float knockbackDecay = 12f;        //1초에 얼마나 줄어드는지
    private Vector3 knockbackVelocity;              // 지금 밀려나는 속도


    private void Update()
    {
        if(knockbackVelocity.sqrMagnitude > 0.01f)
        {
            transform.position += knockbackVelocity * Time.deltaTime;
            knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, knockbackDecay * Time.deltaTime);
            return;
        }

        if (target == null) return; //플레이어가 사라져도 오류가 나지 않게 하는 방어코드
        Vector3 toTarget = target.position - transform.position;

        if (toTarget.sqrMagnitude <= stoppingDistance * stoppingDistance) return;

        Vector3 direction = toTarget.normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
    public void Knockback(Vector2 direction, float force)
    {
        knockbackVelocity = direction.normalized * force;
    }
    private void OnEnable()
    {
        knockbackVelocity = Vector3.zero;
    }
}

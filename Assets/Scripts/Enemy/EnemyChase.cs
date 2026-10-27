using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.6f;
    [SerializeField] private float knockbackDecay = 12f;        //1초에 얼마나 줄어드는지
    private Vector3 knockbackVelocity;              // 지금 밀려나는 속도
    private Rigidbody2D rb;
    private FlowField flowField;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if(knockbackVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = knockbackVelocity;
            knockbackVelocity = Vector3.MoveTowards(knockbackVelocity, Vector3.zero, knockbackDecay * Time.fixedDeltaTime);
            return;
        }

        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;             //플레이어가 사라져도 오류가 나지 않게 하는 방어코드
        }
        Vector2 toTarget = target.position - transform.position;

        if (toTarget.sqrMagnitude <= stoppingDistance * stoppingDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = toTarget.normalized;            //플레이어를 향한 직선 방향

        if(flowField != null)
        {
            Vector2 flowDirection = flowField.GetDirection(transform.position);
            if (flowDirection != Vector2.zero)
            {
                direction = flowDirection;
            }
        }

        rb.linearVelocity = direction * moveSpeed;
    }

    public void SetFlowField(FlowField field)
    {
        flowField = field;
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
        rb.linearVelocity = Vector2.zero ;
    }
}

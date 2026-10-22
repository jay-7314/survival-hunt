using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stoppingDistance = 0.6f;


    private void Update()
    {
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
}

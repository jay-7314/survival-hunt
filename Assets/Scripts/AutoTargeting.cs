using UnityEngine;

public class AutoTargeting : MonoBehaviour
{
    [SerializeField] private float range = 8f;
    private float bonusRange;

    public Enemy Current { get; private set; }

    private void Update()
    {
        Current = FindNearest();
        if (Current != null)
        {
            Debug.DrawLine(transform.position, Current.transform.position, Color.yellow);
        }
    }

    private Enemy FindNearest()
    {
        Enemy nearest = null;
        float currentRange = range + bonusRange;
        float bestSqr = currentRange * currentRange;
        Vector3 myPos = transform.position;

        for (int i = 0; i < Enemy.All.Count; i++)
        {
            float sqr = (Enemy.All[i].transform.position - myPos).sqrMagnitude;
            if (sqr < bestSqr)                   //가까운 적이 등장했을때 후보군 교체
            {
                bestSqr = sqr;
                nearest = Enemy.All[i];
            }
        }
        return nearest;
    }
    public void SetRangeBonus(float bonus)
    {
        bonusRange = bonus;
        Debug.Log($"사거리 :{range + bonusRange}");
    }
}

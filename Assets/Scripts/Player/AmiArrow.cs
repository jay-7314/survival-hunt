using UnityEngine;

public class AmiArrow : MonoBehaviour
{
    [SerializeField] private AutoTargeting targeting;
    [SerializeField] private SpriteRenderer arrowRenderer;
    [SerializeField] private float distance = 1.6f;

    private void LateUpdate()
    {
        Enemy target = targeting.Current;

        if(target == null)
        {
            arrowRenderer.enabled = false;
            return;
        }

        Vector3 direction = (target.transform.position - targeting.transform.position).normalized;

        arrowRenderer.enabled = true;
        transform.position = targeting.transform.position + direction * distance;
        transform.right = direction;
    }
}

using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 pos = target.position;
        pos.z = transform.position.z;
        transform.position = pos;
    }
}

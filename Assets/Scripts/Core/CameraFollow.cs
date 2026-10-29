using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector2 minBonus;
    [SerializeField] private Vector2 maxBonus;
    [SerializeField] private float margin = 12f;
    private Camera cam;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (target == null) return;             //플레이어 사망시 카메라 작동 안함.

        float halfHeight = cam.orthographicSize;
        float halfWidth = halfHeight * cam.aspect;

        
        Vector3 pos = target.position;      //플레이어 위치 저장
        pos.x = Mathf.Clamp(pos.x, minBonus.x + halfWidth - margin, maxBonus.x - halfWidth + margin);
        pos.y = Mathf.Clamp(pos.y, minBonus.y + halfHeight - margin, maxBonus.y - halfHeight + margin);
        pos.z = transform.position.z;       //z만 카메라 원래값으로 되돌림
        transform.position = pos;           //카메라를 옮김
    }

    public void SetBounds(Vector2 newMin, Vector2 newMax)
    {
        minBonus = newMin;
        maxBonus = newMax;
    }
}

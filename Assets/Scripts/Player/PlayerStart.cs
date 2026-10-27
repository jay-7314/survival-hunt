using UnityEngine;

public class PlayerStart : MonoBehaviour
{
    [SerializeField] private Transform startPoint;              //출발점의 위치를 받음

    private void Start()                //update로 쓰면 계속 끌려감
    {
        if (startPoint == null) return;
        transform.position = startPoint.position;
    }
}

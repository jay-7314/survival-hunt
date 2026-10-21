using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");       //static readonly -> 모든 오브젝트가 하나를 함께 쓰고 바꿀수 없다.

    [SerializeField] private Animator anim;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector3 lastPosition;

    private void Awake()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = transform.position - lastPosition;
        lastPosition = transform.position;      //이것이 없으면 영원히 걷는 모습으로 나옴

        bool moving = delta.sqrMagnitude > 0.00001f;
        anim.SetBool(IsMovingHash, moving);

        if (Mathf.Abs(delta.x) > 0.0001f)
        {
            spriteRenderer.flipX = delta.x < 0f;
        }
    }
}

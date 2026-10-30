using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    private Vector3 lastPosition;           //지난 프레임에 내가 있던곳

    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");
    private static readonly int MoveXHash = Animator.StringToHash("MoveX");
    private static readonly int MoveYHash = Animator.StringToHash("MoveY");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = transform.position - lastPosition;              //이번 프레임에서 얼마나 어느쪽으로 움직였나?
        lastPosition = transform.position;

        bool isMoving = delta.sqrMagnitude > 0.00001f;
        animator.SetBool(IsMovingHash, isMoving);

        if (!isMoving) return;
        float absX = Mathf.Abs(delta.x);
        float absY = Mathf.Abs(delta.y);

        bool wasHorizontal = animator.GetFloat(MoveXHash) != 0f;

        bool goHorizontal;
        if (wasHorizontal)
        {
            goHorizontal = absX * 1.5f > absY;
        }
        else
        {
            goHorizontal = absX > absY * 1.5f;
        }
        
        if (goHorizontal)
        {
            animator.SetFloat(MoveXHash, delta.x> 0? 1f:-1f);
            animator.SetFloat(MoveYHash,0f);
        }

        else
        {
            animator.SetFloat(MoveXHash, 0f);
            animator.SetFloat(MoveYHash, delta.y > 0 ? 1f:-1f);
        }
    }
}

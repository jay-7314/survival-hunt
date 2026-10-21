using UnityEngine;

public class SimpleSpriteAnimator : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite[] walkSprites;          //걷기 그림을 넣는 배열
    [SerializeField] private float framesPerSecond = 6f;

    private Vector3 lastPosition;       //지난 프레임 기준

    private void Awake()
    {
        lastPosition = transform.position;
    }

    private void LateUpdate()
    {
        Vector3 delta = transform.position - lastPosition;      //움직임에 대한 비율
        lastPosition = transform.position;

        bool moving = delta.sqrMagnitude > 0.000001f;           //진짜 조금만 움직여도 움직였다고 표현하기

        if (moving)
        {
            int frame = (int)(Time.time * framesPerSecond) % walkSprites.Length;
            spriteRenderer.sprite = walkSprites[frame];

            if(Mathf.Abs(delta.x) > 0.0001f)
            {
                spriteRenderer.flipX = delta.x < 0f;
            }
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
        }
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private VirtualJoyStick joyStick;
    private float bonusMoveSpeed;
    private Rigidbody2D rb;
    private Vector2 input;
    private InputAction moveAction;     //결과값을 vector2로 하나로 모인다.

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector").With("Up", "<Keyboard>/w").With("Down", "<Keyboard>/s").With("Left", "<Keyboard>/a").With("Right", "<Keyboard>/d");      //대각선은 더 빠르지 않게 설정
        moveAction.AddBinding("<Gamepad>/leftstick");
    }

    private void OnEnable()
    {
        moveAction.Enable();
    }
    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void Update()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        if (joyStick != null && joyStick.Direction != Vector2.zero)
        {
            move = joyStick.Direction;
        }
        input = move;
    }


    private void FixedUpdate()
    {
        rb.linearVelocity = input * (moveSpeed + bonusMoveSpeed);
    }



    public void SetMoveSpeedBonus(float Bonus)
    {
        bonusMoveSpeed = Bonus;
        Debug.Log($"이동속도 : {moveSpeed + bonusMoveSpeed}");
    }
   
    public void ApplyCharacter(CharacterData data)
    {
        moveSpeed = data.MoveSpeed;
        Debug.Log($"케릭터 적용 : 이동속도 {moveSpeed + bonusMoveSpeed}");

    }
}

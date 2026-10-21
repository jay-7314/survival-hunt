using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private InputAction moveAction;     //결과값을 vector2로 하나로 모인다.

    private void Awake()
    {
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
        Vector2 input = moveAction.ReadValue<Vector2>();
        transform.position += (Vector3)(input * moveSpeed * Time.deltaTime);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

// 이 스크립트를 붙이면 AutoTargeting도 자동으로 붙고, 지우지 못하게 막는다
[RequireComponent(typeof(AutoTargeting))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefabs;
    [SerializeField] private int baseDamage = 1;

    private AutoTargeting targeting;
    private InputAction attackAction;
    private ObjectPool<Projectile> pool;
    private int bonusDamage;

    private void Awake()
    {
        // 같은 오브젝트의 AutoTargeting을 한 번만 찾아 둔다 (매 프레임 찾으면 비싸다)
        targeting = GetComponent<AutoTargeting>();

        // 눌렀다/뗐다 두 상태만 있는 버튼 입력
        attackAction = new InputAction("Attack", InputActionType.Button);
        attackAction.AddBinding("<Keyboard>/space");
        attackAction.AddBinding("<Gamepad>/buttonSouth");

        pool = new ObjectPool<Projectile>(
            // 풀에 쉬고 있는 총알이 없을 때만 새로 만든다
            createFunc: () =>
            {
                Projectile p = Instantiate(projectilePrefabs);
                // 수명이 끝났을 때 스스로 반납하려면 풀의 주소를 알아야 한다
                p.SetPool(pool);
                return p;
            },
            // 꺼낼 때 켠다
            actionOnGet: p => p.gameObject.SetActive(true),
            // 반납할 때 끈다 (파괴하지 않고 보관)
            actionOnRelease: p => p.gameObject.SetActive(false),
            // 보관 한도를 넘었을 때만 진짜 파괴한다
            actionOnDestroy: p => Destroy(p.gameObject),
            // 처음에 20개를 예상해 자리를 잡아 둔다 (넘으면 알아서 늘어난다)
            defaultCapacity: 20,
            // 최대 보관 개수
            maxSize: 100);
    }

    private void OnEnable() { attackAction.Enable(); }
    private void OnDisable() { attackAction.Disable(); }

    private void Update()
    {
        // 누른 순간에만 참이라 꾹 눌러도 한 번만 발사된다
        if (attackAction.WasPressedThisFrame())
        {
            TryFire();
        }
    }

    // 나중에 화면의 공격 버튼(UI)도 이 함수를 부르게 할 것이라 public으로 분리
    public void TryFire()
    {
        Enemy target = targeting.Current;
        // 사거리 안에 적이 없으면 아무 일도 하지 않는다
        if (target == null) return;

        Vector3 direction = target.transform.position - transform.position;

        Projectile bullet = pool.Get();
        // 재사용된 총알은 이전 위치에 남아 있어서 위치를 다시 정해 준다
        bullet.transform.position = transform.position;
        bullet.Launch(direction, baseDamage + bonusDamage);
    }

    public void SetDamageBonus(int bonus)
    {
        bonusDamage = bonus;
        Debug.Log($"공격력 :{baseDamage+bonusDamage}");
    }
}
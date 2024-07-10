using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(Rigidbody2D), typeof(Animator))]
public class MonkfishLightball : PoolObject
{
    [SerializeField, Tooltip("true일 시 일정 시간 후 사라진다. 아귀 대가리에 달려 있는 불은 false로 둔다.")]
    private bool isProjectile;
    [SerializeField]
    private float lifeTime = 3f;

    [SerializeField]
    private float speed;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        GetComponent<Rigidbody2D>().velocity = MyCalculator.Deg2Vec(angle) * speed;
    }
    private void OnEnable() // 아귀 대가리에 있는 불은 Init()이 되지 않으므로 OnEnable()을 통해 두 상황 다 실행되도록 한다.
    {
        if (isProjectile)
        {
            Invoke("BlinkStart", lifeTime);
        }
    }
   private void BlinkStart()
    {
        if (gameObject.activeInHierarchy)
        {
            GetComponent<Animator>().SetTrigger(EntityAnimHash.Pattern);
        }
    }

    public static event System.Action PlayerDetected = () => { Debug.Log("아귀에게 발각됨!"); };
    private void OnDestroy()
    {
        PlayerDetected = () => { Debug.Log("아귀에게 발각됨!"); }; // 파괴될 때(풀 들어갈 떄 말고) 감지 이벤트 초기화
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<PlayerInteract>(out _)) // 플레이어를 감지했을 경우
        {
            PlayerDetected.Invoke();
        }
    }
}

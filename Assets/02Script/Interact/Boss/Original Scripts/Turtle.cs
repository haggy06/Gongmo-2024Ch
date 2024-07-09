using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Turtle : BossBase
{
    [Space(10)]
    [SerializeField]
    private Vector3 initialPosition;

    [Header("Fin Attack")]
    [SerializeField]
    private Collider2D finCollider;

    [Header("Spread Projectile")]
    [SerializeField]
    private PoolObject projectile;

    [SerializeField]
    private Transform pivotPosition;
    [SerializeField]
    private Transform[] projectilePosition;

    [Header("Launch Tornado")]
    [SerializeField]
    private PoolObject tornado;
    [SerializeField]
    private Transform[] tornadoPosition;

    [Header("Spin Turtle")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private int collisionCount;
    [SerializeField]
    private float comebackSpeed;
    [SerializeField]
    private bool spining = false;

    private CircleCollider2D physicalBox;
    protected override void Awake()
    {
        base.Awake();

        physicalBox = GetComponent<CircleCollider2D>();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        physicalBox.enabled = false;
        StartCoroutine("MoveToInitialPosition");
    }
    protected override void HalfHP()
    {
        spining = true;
        anim.SetInteger(EntityAnimHash.Pattern, 3);
    }

    protected override void MoribundHP()
    {

    }

    /* 거북 패턴
     * 1. 탄막 흩뿌리기
     * 2. 회오리 날리기
     * 3. 돌기
     */
    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {
        if (!spining) // 회전 중이 아닐 경우
        {
            anim.SetInteger(EntityAnimHash.Pattern, caseNumber + 1);
        }
    }
    public void SpreadProjectile()
    {
        foreach (Transform projPosition in projectilePosition)
        {
            parentPool.GetPoolObject(projectile).Init(projPosition.position, MyCalculator.Vec2Deg(projPosition.transform.position - pivotPosition.position));
        }
    }
    public void LaunchTornado()
    {
        foreach (Transform projPosition in tornadoPosition)
        {
            parentPool.GetPoolObject(tornado).Init(projPosition.position, MyCalculator.Vec2Deg(PlayerController.Player.transform.position - projPosition.transform.position));
        }
    }

    private int curCollisionCount = 0;
    public void TurtleSpin()
    {
        enemyInteract.damageResistance = 0.75f;

        curCollisionCount = 0;

        physicalBox.enabled = true;
        rigid2D.velocity = (PlayerController.Player.transform.position - transform.position).normalized * speed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boarder"))
        {
            curCollisionCount++;
            if (curCollisionCount >= collisionCount) // 벽에 충분히 튕겼을 경우
            {
                rigid2D.velocity = Vector2.zero;
                StartCoroutine("MoveToInitialPosition");
            }
        }
    }

    private IEnumerator MoveToInitialPosition()
    {
        while (MyCalculator.Distance(initialPosition, transform.position) > 0.1f) // 원래 위치와의 오차가 0.5 이하가 될 때까지 반복 
        {
            print("위치로 이동중");

            transform.position += (initialPosition - transform.position).normalized * Time.deltaTime * comebackSpeed;

            yield return null;
        }

        if (spining) // 돌고 있었을 경우
        {
            enemyInteract.damageResistance = 0f;

            curCollisionCount = 0;
            physicalBox.enabled = false;

            StabilizePattern();
        }
    }
}

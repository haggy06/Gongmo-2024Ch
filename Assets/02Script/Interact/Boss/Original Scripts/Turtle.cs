using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D), typeof(HorizontalRepeatMove))]
public class Turtle : BossBase
{
    [Space(10)]
    [SerializeField]
    private Vector3 initialPosition;

    [Header("Fin Attack")]
    [SerializeField]
    private Collider2D finCollider;

    [Header("Single Projectile")]
    [SerializeField]
    private PoolObject singleProjectile;
    [SerializeField]
    private Transform singleProjectilePosition;

    [Header("Spread Projectile")]
    [SerializeField]
    private PoolObject spreadProjectile;

    [SerializeField]
    private Transform pivotPosition;
    [SerializeField]
    private Transform[] spreadProjectilePosition;

    [Header("Launch Tornado")]
    [SerializeField]
    private PoolObject tornado;

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
    private HorizontalRepeatMove repeatMove;
    protected override void Awake()
    {
        base.Awake();

        physicalBox = GetComponent<CircleCollider2D>();
        repeatMove = GetComponent<HorizontalRepeatMove>();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        repeatMove.moving = false;
        physicalBox.enabled = false;

        StartCoroutine("MoveToInitialPosition");
    }
    protected override void HalfHP()
    {
        spining = true;
        repeatMove.moving = false;
        anim.SetInteger(EntityAnimHash.Pattern, 4);
    }

    protected override void MoribundHP()
    {

    }

    /* �ź� ����
     * 1. ź�� ��Ѹ���
     * 2. ȸ���� ������
     * 3. ����
     */
    protected override void Pattern(int caseNumber, bool isListPattern = false)
    {
        if (!spining) // ȸ�� ���� �ƴ� ���
        {
            anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
        }
    }
    public void SingleProjectile()
    {
        parentPool.GetPoolObject(singleProjectile).Init(singleProjectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Player.transform.position - singleProjectilePosition.position));
    }
    public void SpreadProjectile()
    {
        foreach (Transform projPosition in spreadProjectilePosition)
        {
            parentPool.GetPoolObject(spreadProjectile).Init(projPosition.position, MyCalculator.Vec2Deg(projPosition.position - pivotPosition.position));
        }
    }
    public void LaunchTornado()
    {
        parentPool.GetPoolObject(tornado).Init(singleProjectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Player.transform.position - singleProjectilePosition.position));
    }

    private int curCollisionCount = 0;
    public void TurtleSpin()
    {
        enemyInteract.damageResistance = 0.75f;

        curCollisionCount = 0;

        physicalBox.enabled = true;
        rigid2D.velocity = (PlayerController.Player.transform.position - transform.position).normalized * speed;
        StartCoroutine("TurtleSpinCor");
    }
    public void TurtleSpinEnd()
    {
        repeatMove.moving = true;
        spining = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boarder"))
        {
            curCollisionCount++;
            rigid2D.velocity = (PlayerController.Player.transform.position - transform.position).normalized * speed;
            if (curCollisionCount >= collisionCount) // ���� ����� ƨ���� ���
            {
                rigid2D.velocity = Vector2.zero;
                StartCoroutine("MoveToInitialPosition");
            }
        }
    }
    private IEnumerator TurtleSpinCor()
    {
        while (spining)
        {
            transform.Rotate(Vector3.forward * 360f * Time.deltaTime);

            yield return null;
        }

        transform.eulerAngles = Vector3.zero;
    }

    private IEnumerator MoveToInitialPosition()
    {
        while (MyCalculator.Distance(initialPosition, transform.position) > 0.1f) // ���� ��ġ���� ������ 0.5 ���ϰ� �� ������ �ݺ� 
        {
            print("��ġ�� �̵���");

            transform.position += (initialPosition - transform.position).normalized * Time.deltaTime * comebackSpeed;

            yield return null;
        }

        if (spining) // ���� �־��� ���
        {
            enemyInteract.damageResistance = 0f;

            curCollisionCount = 0;
            physicalBox.enabled = false;
            transform.eulerAngles = Vector3.zero;

            StopCoroutine("TurtleSpinCor");
            StabilizePattern();
        }
        repeatMove.moving = true;
    }
}

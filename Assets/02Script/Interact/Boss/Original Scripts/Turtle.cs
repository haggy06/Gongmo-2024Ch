using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Turtle : BossBase
{
    [Space(5)]
    [SerializeField]
    private AudioClip attackSound;

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
    private AudioClip shellSound;
    [SerializeField]
    private AudioClip collisionSound;

    [Space(5)]
    [SerializeField]
    private float speed;
    [SerializeField]
    private int collisionCount;
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

        patternTerm = 2f;
        enemyInteract.damageResistance = 0f;

        repeatMove.moving = false;
        spining = false;

        physicalBox.enabled = false;
    }
    protected override void HalfHP()
    {
        StopCoroutine("PatternCor");
        patternTerm = 1.5f;

        spining = true;
        repeatMove.moving = false;

        anim.SetInteger(EntityAnimHash.Pattern, 4);
    }

    /* �ź� ����
     * 1. ź�� ��Ѹ���
     * 2. ȸ���� ������
     * 3. ����
     */
    protected override void Pattern(int caseNumber)
    {
        if (!spining) // ȸ�� ���� �ƴ� ���
        {
            anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
        }
    }
    public void SingleProjectile()
    {
        AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(singleProjectile).Init(singleProjectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - singleProjectilePosition.position));

        StabilizePattern();
    }
    public void SpreadProjectile()
    {
        AudioManager.Inst.PlaySFX(attackSound);

        foreach (Transform projPosition in spreadProjectilePosition)
        {
            parentPool.GetPoolObject(spreadProjectile).Init(projPosition.position, MyCalculator.Vec2Deg(projPosition.position - pivotPosition.position));
        }

        StabilizePattern();
    }
    public void LaunchTornado()
    {
        AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(tornado).Init(singleProjectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - singleProjectilePosition.position));

        StabilizePattern();
    }

    private int curCollisionCount = 0;
    public void TurtleSpin()
    {
        anim.SetInteger(EntityAnimHash.Pattern, 4); // �������� �ٷ� ���� �Ͼ ���� �ֱ淡 Ȯ�λ����� Pattern�� 4�� �� �� �� ����
        AudioManager.Inst.PlaySFX(shellSound);

        enemyInteract.damageResistance = 0.75f;

        curCollisionCount = 0;

        physicalBox.enabled = true;
        rigid2D.velocity = (PlayerController.Inst.transform.position - transform.position).normalized * speed;
        StartCoroutine("TurtleSpinCor");
    }
    public void TurtleSpinEnd()
    {
        enemyInteract.damageResistance = 0f;

        AudioManager.Inst.PlaySFX(shellSound);

        repeatMove.moving = true;
        spining = false;

        StartCoroutine("PatternCor");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Boarder"))
        {
            AudioManager.Inst.PlaySFX(collisionSound);

            curCollisionCount++;
            rigid2D.velocity = (PlayerController.Inst.transform.position - transform.position).normalized * speed;
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

    protected override void InitialPositionArrive()
    {
        base.InitialPositionArrive();

        if (spining) // ���� �־��� ���
        {
            curCollisionCount = 0;
            physicalBox.enabled = false;
            transform.eulerAngles = Vector3.zero;

            StopCoroutine("TurtleSpinCor");
            StabilizePattern();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScrollWithBackground))]
public class Octopus : EnemyBase
{
    [Header("Suprise Attack")]
    [SerializeField]
    private bool hiding;
    /*
    [SerializeField]
    private ExplosionObject inkAttack;

    [Header("Tentacle Scratch")]
    [SerializeField]
    private float tentacleReach = 5f;
    [SerializeField]
    private ExplosionObject tentacleAttack;

    [Header("Spread Projectile")]
    [SerializeField]
    private AudioClip spreadSound;
    [SerializeField]
    private Transform spreadPosition;
    [SerializeField]
    private PoolObject OctopusProjectile;
    [SerializeField]
    private int projectileNumber = 8;
    */
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        hiding = true;
        usePattern = false;
    }

    protected override void HalfHP()
    {
        base.HalfHP();

        if (hiding) // ������ �ẹ ���̾��� ���
        {
            Appear();
        }
    }
    private void FixedUpdate()
    {
        if (hiding) // �ẹ ���� ���
        {
            if (PatternCheck.ShortDistance(transform.position, detectionRadius)) // �÷��̾� ����
            {
                Appear();
            }
        }
    }
    private void Appear()
    {
        if (hiding) // �ẹ ���� ���
        {
            hiding = false;
            usePattern = true;

            anim.SetInteger(EntityAnimHash.Pattern, 3); // ����
        }        
    }
    /*
    protected override void Pattern(int caseNumber)
    {        
        if (!hiding) // �ẹ ���� �ƴ� ���
        {
            if (PatternCheck.ShortDistance(transform.position, tentacleReach)) // �÷��̾� ����
            {
                anim.SetInteger(EntityAnimHash.Pattern, 1); // ��������
            }
            else
            {
                anim.SetInteger(EntityAnimHash.Pattern, 2); // ���Ÿ� ����
            }
        }
    }

    public void SupriseAttack() // ���
    {
        parentPool.GetPoolObject(inkAttack).Init(spreadPosition.position, 0f);
        SpreadProjectile();
    }

    public void TentacleAttack() // ���� ����
    {
        parentPool.GetPoolObject(tentacleAttack).Init(spreadPosition.position, 0f);

        StabilizePattern();
    }
    public void SpreadProjectile() // ��ź �߻�
    {
        AudioManager.Inst.PlaySFX(spreadSound);

        float angleDiff = 360f / projectileNumber;
        float initialAngle = Random.Range(0f, 360f);
        for (int i = 0; i < projectileNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(OctopusProjectile);
            proj.Init(spreadPosition.position, initialAngle + (angleDiff * i));
        }

        StabilizePattern();
    }
    */
}

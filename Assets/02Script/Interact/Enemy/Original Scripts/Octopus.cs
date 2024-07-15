using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScrollWithBackground))]
public class Octopus : EnemyBase
{
    [Header("Suprise Attack")]
    [SerializeField]
    private ExplosionObject inkAttack;
    [SerializeField]
    private float supriseRadius;
    [SerializeField]
    private bool hiding;

    [Header("Tentacle Scratch")]
    [SerializeField]
    private float tentacleReach = 5f;
    [SerializeField]
    private ExplosionObject tentacleAttack;

    [Header("Spread Projectile")]
    [SerializeField]
    private Transform spreadPosition;
    [SerializeField]
    private PoolObject OctopusProjectile;
    [SerializeField]
    private int projectileNumber = 8;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        hiding = true;
    }

    protected override void HalfHP()
    {
        if (hiding) // ������ �ẹ ���̾��� ���
        {
            Appear();
        }
    }
    protected override void MoribundHP()
    {

    }

    private void FixedUpdate()
    {
        if (hiding) // �ẹ ���� ���
        {
            if (PatternCheck.ShortDistance(transform.position, supriseRadius)) // �÷��̾� ����
            {
                Appear();
            }
        }
    }
    private void Appear()
    {
        hiding = false;
        anim.SetTrigger("Suprise");

        StartCoroutine("PatternRepeat");
    }

    protected override void Pattern(int caseNumber, bool isListPattern = false)
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
    }
    public void SpreadProjectile() // ��ź �߻�
    {
        float angleDiff = 360f / projectileNumber;
        float initialAngle = Random.Range(0f, 360f);
        for (int i = 0; i < projectileNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(OctopusProjectile);
            proj.Init(spreadPosition.position, initialAngle + (angleDiff * i));
        }
    }
}

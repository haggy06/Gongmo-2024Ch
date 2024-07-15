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
        if (hiding) // 아직도 잠복 중이었을 경우
        {
            Appear();
        }
    }
    protected override void MoribundHP()
    {

    }

    private void FixedUpdate()
    {
        if (hiding) // 잠복 중일 경우
        {
            if (PatternCheck.ShortDistance(transform.position, supriseRadius)) // 플레이어 감지
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
        if (!hiding) // 잠복 중이 아닐 경우
        {
            if (PatternCheck.ShortDistance(transform.position, tentacleReach)) // 플레이어 감지
            {
                anim.SetInteger(EntityAnimHash.Pattern, 1); // 근접공격
            }
            else
            {
                anim.SetInteger(EntityAnimHash.Pattern, 2); // 원거리 공격
            }
        }
    }

    public void SupriseAttack() // 기습
    {
        parentPool.GetPoolObject(inkAttack).Init(spreadPosition.position, 0f);
        SpreadProjectile();
    }

    public void TentacleAttack() // 근접 공격
    {
        parentPool.GetPoolObject(tentacleAttack).Init(spreadPosition.position, 0f);
    }
    public void SpreadProjectile() // 산탄 발사
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ScrollWithBackground))]
public class SeaAnemone : EnemyBase
{
    [Header("Tentacle Scratch")]
    [SerializeField]
    private float tentacleReach = 3f;
    [SerializeField]
    private ExplosionObject tentacleAttack;

    [Header("Spread Projectile")]
    [SerializeField]
    private AudioClip spreadSound;
    [SerializeField]
    private Transform spreadPosition;
    [SerializeField]
    private PoolObject anemoneProjectile;
    [SerializeField]
    private int projectileNumber = 8;

    protected override void Awake()
    {
        base.Awake();
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
    }
    protected override void HalfHP()
    {
        
    }

    protected override void MoribundHP()
    {
        
    }

    /* 말미잘 공격 패턴
     * 1. 촉수 할퀴기 (근접)
     * 2. 산탄 발사 (원거리)
     */
    protected override void Pattern(int caseNumber, bool isListPattern = false) // 말미잘은 리스트를 안 쓰니 패스
    {
        if (PatternCheck.ShortDistance(spreadPosition.position, tentacleReach)) // 근접 공격이 가능할 경우
        {
            anim.SetInteger(EntityAnimHash.Pattern, 1); // 촉수 할퀴기
        }
        else // 근접 공격이 불가능할 경우
        {
            anim.SetInteger(EntityAnimHash.Pattern, 2); // 산탄 발사
        }
        /*
        switch (caseNumber)
        {
            case 0: // 촉수 할퀴기
                if (PatternCheck.shortDistance(transform.position, tentacleReach)) // 근접 공격이 가능할 경우
                {
                    anim.SetInteger(EntityAnimHash.Pattern, 1);
                }
                else // 근접 공격이 불가능할 경우
                {
                    anim.SetInteger(EntityAnimHash.Pattern, 2);
                    //Pattern(1); // 산탄 발사 실행
                }
                break;

            case 1: // 산탄 발사
                anim.SetInteger(EntityAnimHash.Pattern, 2);
                break;
        }
        */
    }

     // 실실적인 공격은 연결된 Animator에서 실행한다. (타이밍 맞추기 위해)
    public void TentacleAttack() // 촉수 할퀴기
    {
        parentPool.GetPoolObject(tentacleAttack).Init(spreadPosition.position, 0f);

        StabilizePattern();
    }
    public void SpreadProjectile() // 산탄 발사
    {
        AudioManager.Inst.PlaySFX(spreadSound);

        float angleDiff = 360f / projectileNumber;
        float initialAngle = Random.Range(0f, 360f);
        for (int i = 0; i < projectileNumber; i++)
        {
            PoolObject proj = parentPool.GetPoolObject(anemoneProjectile);
            proj.Init(spreadPosition.position, initialAngle + (angleDiff * i));
        }

        StabilizePattern();
    }
}

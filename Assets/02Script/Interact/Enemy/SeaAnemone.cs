using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaAnemone : EnemyBase
{
    [SerializeField]
    private float scrollSpeed = 4f;

    [Header("Tentacle Scratch")]
    [SerializeField]
    private Collider2D tenacleAttack;
    [SerializeField]
    private ParticleSystem scratchEffect;

    [Header("Spread Projectile")]
    [SerializeField]
    private PoolObject anemoneProjectile;
    [SerializeField]
    private int projectileNumber = 8;

    protected override void Awake()
    {
        base.Awake();
        GameManager.BossEvent += (isOn) => rigid2D.velocity = isOn ? Vector2.zero : Vector2.down * scrollSpeed; // 보스 등장 시엔 스크롤이 멈추므로 말미잘도 멈추게 함
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        rigid2D.velocity = Vector2.down * scrollSpeed;
    }

    protected override void Dead(AttackBase attack)
    {
        
    }

    protected override void HalfHP()
    {
        
    }

    protected override void MoribundHP()
    {
        
    }

    protected override void Pattern(int caseNumber, bool isListPattern = false) // 말미잘은 리스트를 안 쓰니 패스
    {
        switch (caseNumber)
        {
            case 0: // 촉수 할퀴기
                if (PatternCheck.shortDistance(transform.position, 3f)) // 근접 공격이 가능할 경우
                {
                    tenacleAttack.enabled = true;
                    scratchEffect.Play();
                }
                else // 근접 공격이 물가능할 경우
                {
                    goto ProjPattern;
                    //Pattern(1); // 산탄 발사 실행
                }
                break;

            case 1: // 산탄 발사
            ProjPattern:
                float angleDiff = 360f / projectileNumber;
                for (int i = 0; i < projectileNumber; i++)
                {
                    PoolObject proj = parentPool.GetPoolObject(anemoneProjectile);
                    proj.Init(transform.position, 90f + (angleDiff * i));
                }
                break;
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crayfish : BossBase
{
    [Header("Normal Pattern")]
    [SerializeField]
    private Transform projectilePosition;

    [Space(5)]
    [SerializeField]
    private PoolObject projectile;
    [Space(5)]
    [SerializeField]
    private PoolObject shokewaveBall;
    [Space(5)]
    [SerializeField]
    private PoolObject smashExplosion;
    [SerializeField]
    private float explosionRadius;

    [Header("Throw Boomerang")]
    [SerializeField]
    private Collider2D leftTongsCollider;
    [SerializeField]
    private SpriteRenderer boomerangHand;

    [Space(5)]
    [SerializeField]
    private Transform boomerangPosition;
    [SerializeField]
    private PoolObject boomerang;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        patternTerm = 1.5f;
        enemyInteract.damageResistance = 0f;

        boomerangHand.color = Color.white;
        leftTongsCollider.enabled = true;
    }

    protected override void HalfHP()
    {
        patternTerm = 1f; // 패턴 시전 속도가 빨라짐
        anim.SetInteger(EntityAnimHash.Pattern, 4); // 부메랑 던지기 실행
        enemyInteract.damageResistance = 0.5f;
        StopCoroutine("PatternCor");
    }

    protected override void Pattern(int caseNumber)
    {
        anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
    }

    public void SpreadProj()
    {
        AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(projectile).Init(projectilePosition.position, -90f);

        StabilizePattern();
    }
    public void Shokewave()
    {
        AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(shokewaveBall).Init(projectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - projectilePosition.position));

        StabilizePattern();
    }
    public void Smash()
    {
        AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(smashExplosion).Init(projectilePosition.position, 0f);

        StabilizePattern();
    }

    public void ThrowBoomerang()
    {
        enemyInteract.damageResistance = 0f;
        StartCoroutine("PatternCor");

        PoolObject bmr = parentPool.GetPoolObject(boomerang);
        bmr.Init(boomerangPosition.position, -90f); // 부메랑 발사
        bmr.GetComponent<Rigidbody2D>().velocity = Vector2.down * 10f; // 부메랑 초기속도 설정


        boomerangHand.color = new Color(1f, 1f, 1f, 0f);
        leftTongsCollider.enabled = false;

        StabilizePattern();
    }
    protected override void Dead(EntityType killer)
    {
        base.Dead(killer);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crayfish : BossBase
{
    /*
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
    */
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
        enemyInteract.DamageResistance = 0f;

        boomerangHand.color = Color.white;
        leftTongsCollider.enabled = true;
    }

    protected override void HalfHP()
    {
        base.HalfHP();

        patternTerm = 1f; // ���� ���� �ӵ��� ������
        enemyInteract.DamageResistance = 0.5f;
    }
    /*
    protected override void Pattern(int caseNumber)
    {
        anim.SetInteger(EntityAnimHash.Pattern, caseNumber);
    }

    public void SpreadProj()
    {
        //AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(projectile).Init(projectilePosition.position, -90f);

        StabilizePattern();
    }
    public void Shokewave()
    {
        //AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(shokewaveBall).Init(projectilePosition.position, MyCalculator.Vec2Deg(PlayerController.Inst.transform.position - projectilePosition.position));

        StabilizePattern();
    }
    public void Smash()
    {
        //AudioManager.Inst.PlaySFX(attackSound);

        parentPool.GetPoolObject(smashExplosion).Init(projectilePosition.position, 0f);

        StabilizePattern();
    }
    */

    public void ThrowBoomerang()
    {
        enemyInteract.DamageResistance = 0f;

        PoolObject bmr = parentPool.GetPoolObject(boomerang);
        bmr.Init(boomerangPosition.position, -90f); // �θ޶� �߻�
        //bmr.GetComponent<Rigidbody2D>().velocity = Vector2.down * 10f; // �θ޶� �ʱ�ӵ� ����

        boomerangHand.color = new Color(1f, 1f, 1f, 0f);
        leftTongsCollider.enabled = false;

        StabilizePattern();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyInteract : HitBase
{
    [Header("Enemy Info")]
    [SerializeField]
    private float skillPerHit = 0.25f;

    [Space(5)]
    [SerializeField]
    private float skillPerDead = 5f;
    [SerializeField]
    private int expPerDead = 20;

    private SpriteRenderer sprite;
    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void HitBy(AttackBase attack)
    {
        base.HitBy(attack);

        if (curHP <= 0) // 사망 시
            return;

        StopCoroutine("HitBlink");
        StartCoroutine("HitBlink");

        if (attack.Owner == EntityType.Player) // 플레이어에게 맞았을 경우
        {
            GameManager.Skill += skillPerHit;
        }
    }
    private IEnumerator HitBlink()
    {
        sprite.color = Color.red;

        yield return YieldReturn.WaitForSeconds(0.1f);

        sprite.color = Color.white;
    }

    protected override void DeadBy(AttackBase attack)
    {
        if (attack.Owner == EntityType.Player) // 플레이어에게 죽었을 경우
        {
            GameManager.Skill += skillPerDead;
            GameManager.EXP += expPerDead;
        }
        GetComponentInParent<PoolObject>().ReturnToPool();
        //Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemyInteract : HitBase
{
    [Header("Enemy Info")]
    [SerializeField, Range(0f, 1f), Tooltip("받은 대미지의 몇 퍼센트를 스킬로 치환해줄 지 정하는 필드")]
    private float skillEff = 0.1f;

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
            GameManager.Skill += (attack.Damage * damageScope / GameManager.CurWeapon.skillGauge) * 100f * skillEff; // 딜량/요구 딜량 * 100 * 대미지 효율
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
    }
    protected override void HalfHP()
    {
        
    }

    protected override void MoribundHP()
    {
        
    }
}

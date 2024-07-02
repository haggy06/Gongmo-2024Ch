using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : HitBase
{
    [SerializeField, Range(0f, 1f), Tooltip("막은 대미지의 몇 퍼센트를 받아들일지 정함")]
    protected float damageResistance = 0f;

    public override void HitBy(AttackBase attack)
    {
        damageScope = attack.Owner == EntityType.Player ? GameManager.DamageScope : 1f; // 플레이어의 공격일 경우 대미지 배율 적용
        damageScope *= (1f - damageResistance); // 대미지 감소율 대입

        curHP = Mathf.Clamp(curHP - (int)Mathf.Round(attack.Damage * damageScope), 0, maxHP);

        if (curHP <= 0) // HP가 0이 되었을 때
        {
            DeadBy(attack); // 사망 처리
        }
        else if (curHP <= maxHP / 4f) // HP가 1/4이하가 되었을 때
        {

        }
        else if (curHP <= maxHP / 2f) // HP가 절반 이하가 되었을 때
        {

        }        
    }

    protected override void DeadBy(AttackBase attack)
    {
         if (TryGetComponent<PoolObject>(out PoolObject poolObject)) // PoolObject일 경우
        {
            poolObject.ReturnToPool();
        }
        else // PoolObject가 아닐 경우
        {
            Destroy(gameObject);
        }
    }
}

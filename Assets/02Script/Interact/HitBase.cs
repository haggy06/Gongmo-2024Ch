using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitBase : MonoBehaviour
{
    [SerializeField]
    protected EntityType entityType = EntityType.Enemy;
    public EntityType EntityType => entityType;

    [SerializeField]
    protected bool invincible = false;
    public bool Invincible => invincible;

    [Space(5)]
    [SerializeField]
    protected int maxHP = 10;
    [SerializeField]
    protected int curHP = 0;
    private void OnEnable()
    {
        Init();
    }

    public virtual void Init()
    {
        invincible = false;

        curHP = maxHP;
    }

    public virtual void HitBy(AttackBase attack)
    {
        float damageScope = attack.Owner == EntityType.Player ? GameManager.DamageScope : 1f; // 플레이어의 공격일 경우 대미지 배율 적용

        curHP = Mathf.Clamp(curHP - (int)(attack.Damage * damageScope), 0, maxHP);

        if (curHP <= 0) // HP가 0이 되었을 때
        {
            DeadBy(attack); // 사망 처리
        }
    }

    protected abstract void DeadBy(AttackBase attack);
}

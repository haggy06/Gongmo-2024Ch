using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

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
    protected float maxHP = 10;
    [SerializeField]
    protected float curHP = 0;

    [Space(5)]
    [Range(0f, 1f)]
    protected float damageResistance = 0f;
    public float DamageResistance { get => damageResistance; set => damageResistance = value; }

    protected virtual void Awake()
    {
        HalfHPEvent += HalfHP;
        DeadEvent += DeadBy;
    }

    protected bool halfHPInvoked, moribundHPInvoked, alive;
    public bool Alive => alive;
    public virtual void Init()
    {
        invincible = false;

        halfHPInvoked = moribundHPInvoked = false;
        curHP = maxHP;
        alive = true;
    }

    protected float damageScope;
    public virtual void Hit(EntityType victim, float damage, bool isSkill = false)
    {
        if (!alive) // 이미 사망했을 경우 패스
            return;

        damageScope = 1f - damageResistance;
        damageScope *= victim == EntityType.Player ? GameManager.DamageScope : 1f; // 플레이어의 공격일 경우 대미지 배율 적용
        
        curHP = Mathf.Clamp(curHP - damage * damageScope, 0f, maxHP);

        if (curHP <= 0) // HP가 0이 되었을 때
        {
            alive = false;
            DeadEvent.Invoke(victim); // 사망 처리
        }
        else if (curHP <= maxHP / 4f && !moribundHPInvoked) // HP가 1/4이하가 되었을 때
        {
            moribundHPInvoked = true;
        }
        else if (curHP <= maxHP / 2f && !halfHPInvoked) // HP가 절반 이하가 되었을 때
        {
            halfHPInvoked = true;
            HalfHPEvent.Invoke();
        }
    }
    public virtual void InstantKill(EntityType entity)
    {
        curHP = 0f;
        DeadEvent.Invoke(entity); // 사망 처리
    }

    /// <summary> 반피 이하가 되었을 떄 실행 </summary>
    public event Action HalfHPEvent;
    protected abstract void HalfHP();

    /// <summary> 사망했을 시 실행 </summary>
    public event Action<EntityType> DeadEvent;
    protected abstract void DeadBy(EntityType killer);
}
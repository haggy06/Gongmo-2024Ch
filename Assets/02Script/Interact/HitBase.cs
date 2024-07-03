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
    protected int maxHP = 10;
    [SerializeField]
    protected int curHP = 0;

    [Space(5)]
    [SerializeField, Range(0f, 1f)]
    protected float damageResistance = 0f;
    private void Awake()
    {
        HalfHPEvent += HalfHP;
        MoribundHPEvent += MoribundHP;
        DeadEvent += DeadBy;
    }

    private void OnEnable()
    {
        Init();
    }

    public virtual void Init()
    {
        invincible = false;

        curHP = maxHP;
    }

    protected float damageScope;
    public virtual void HitBy(AttackBase attack)
    {
        damageScope = 1f - damageResistance;
        damageScope *= attack.Owner == EntityType.Player ? GameManager.DamageScope : 1f; // 플레이어의 공격일 경우 대미지 배율 적용

        curHP = Mathf.Clamp(curHP - (int)Mathf.Round(attack.Damage * damageScope), 0, maxHP);

        if (curHP <= 0) // HP가 0이 되었을 때
        {
            DeadEvent.Invoke(attack); // 사망 처리
        }
        else if (curHP <= maxHP / 4f) // HP가 1/4이하가 되었을 때
        {
            MoribundHPEvent.Invoke();
        }
        else if (curHP <= maxHP / 2f) // HP가 절반 이하가 되었을 때
        {
            HalfHPEvent.Invoke();
        }
    }

    /// <summary> 반피 이하가 되었을 떄 실행 </summary>
    public event Action HalfHPEvent;
    protected abstract void HalfHP();

    /// <summary> 1/4피 이하가 되었을 떄 실행 </summary>
    public event Action MoribundHPEvent;
    protected abstract void MoribundHP();

    /// <summary> 사망했을 시 실행 </summary>
    public event Action<AttackBase> DeadEvent;
    protected abstract void DeadBy(AttackBase attack);
}
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
    public float damageResistance = 0f;
    protected virtual void Awake()
    {
        HalfHPEvent += HalfHP;
        MoribundHPEvent += MoribundHP;
        DeadEvent += DeadBy;
    }

    private void OnEnable()
    {
        Init();
    }

    protected bool halfHPInvoked, moribundHPInvoked, alive;
    public virtual void Init()
    {
        invincible = false;

        halfHPInvoked = moribundHPInvoked = false;
        curHP = maxHP;
        alive = true;
    }

    protected float damageScope;
    public virtual void Hit(EntityType victim, float damage)
    {
        if (!alive) // �̹� ������� ��� �н�
            return;

        damageScope = 1f - damageResistance;
        damageScope *= victim == EntityType.Player ? GameManager.DamageScope : 1f; // �÷��̾��� ������ ��� ����� ���� ����

        curHP = Mathf.Clamp(curHP - damage * damageScope, 0f, maxHP);

        if (curHP <= 0) // HP�� 0�� �Ǿ��� ��
        {
            alive = false;
            DeadEvent.Invoke(victim); // ��� ó��
        }
        else if (curHP <= maxHP / 4f && !moribundHPInvoked) // HP�� 1/4���ϰ� �Ǿ��� ��
        {
            moribundHPInvoked = true;
            MoribundHPEvent.Invoke();
        }
        else if (curHP <= maxHP / 2f && !halfHPInvoked) // HP�� ���� ���ϰ� �Ǿ��� ��
        {
            halfHPInvoked = true;
            HalfHPEvent.Invoke();
        }
    }
    public virtual void InstantKill(EntityType entity)
    {
        curHP = 0f;
        DeadEvent.Invoke(entity); // ��� ó��
    }

    /// <summary> ���� ���ϰ� �Ǿ��� �� ���� </summary>
    public event Action HalfHPEvent;
    protected abstract void HalfHP();

    /// <summary> 1/4�� ���ϰ� �Ǿ��� �� ���� </summary>
    public event Action MoribundHPEvent;
    protected abstract void MoribundHP();

    /// <summary> ������� �� ���� </summary>
    public event Action<EntityType> DeadEvent;
    protected abstract void DeadBy(EntityType killer);
}
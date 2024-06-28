using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HitBase : MonoBehaviour
{
    [SerializeField]
    protected EntityType entityType;
    public EntityType EntityType => entityType;

    [SerializeField]
    protected bool invincible = false;
    public bool Invincible => invincible;

    [Space(5)]
    [SerializeField]
    private float maxHP = 10f;
    [SerializeField]
    private float curHP = 0f;
    private void Awake()
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
        float damageScope = attack.Owner == EntityType.Player ? GameManager.DamageScope : 1f; // �÷��̾��� ������ ��� ����� ���� ����

        curHP = Mathf.Clamp(curHP - attack.Damage * damageScope, 0f, maxHP);

        if (Mathf.Approximately(curHP, 0f)) // HP�� 0�� �Ǿ��� ��
        {
            DeadBy(attack); // ��� ó��
        }
    }

    protected abstract void DeadBy(AttackBase attack);
}

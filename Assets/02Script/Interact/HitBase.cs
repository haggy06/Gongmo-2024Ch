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

    protected float damageScope;
    public virtual void HitBy(AttackBase attack)
    {
        damageScope = attack.Owner == EntityType.Player ? GameManager.DamageScope : 1f; // �÷��̾��� ������ ��� ����� ���� ����

        curHP = Mathf.Clamp(curHP - (int)Mathf.Round(attack.Damage * damageScope), 0, maxHP);

        if (curHP <= 0) // HP�� 0�� �Ǿ��� ��
        {
            DeadBy(attack); // ��� ó��
        }
    }

    protected abstract void DeadBy(AttackBase attack);
}

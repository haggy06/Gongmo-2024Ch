using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using System;

[RequireComponent(typeof(Collider2D))]
public class AttackBase : MonoBehaviour
{
    [SerializeField, Tooltip("��Ʈ ������� Ȱ��ȭ�� ��� damage�� �ʴ� ���ذ� �ȴ�.")]
    protected bool isDotDamage = false;
    [SerializeField]
    protected EntityType owner = EntityType.Enemy;
    public EntityType Owner => owner;

    [SerializeField]
    protected EntityType target = EntityType.Player;
    public EntityType Target => target;

    [SerializeField]
    protected int damage = 1;
    public int Damage => damage;

    public bool canAttack = true;

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (canAttack && !isDotDamage) // ������ �����ϰ� ��Ʈ���� �ƴ� ���
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision�� HitBase�� �ְ� ��ǥ Ÿ���̸� ���� ���°� �ƴ� ���
            {
                hitBase.Hit(owner, damage);
                AttackSuccessEvent.Invoke(hitBase);
            }
        }
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (canAttack && isDotDamage) // ������ �����ϰ� ��Ʈ���� ���
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision�� HitBase�� �ְ� ��ǥ Ÿ���̸� ���� ���°� �ƴ� ���
            {
                hitBase.Hit(owner, (int)(damage * Time.fixedDeltaTime));
                AttackSuccessEvent.Invoke(hitBase);
            }
        }
    }

    public event Action<HitBase> AttackSuccessEvent = (_) => { };
}
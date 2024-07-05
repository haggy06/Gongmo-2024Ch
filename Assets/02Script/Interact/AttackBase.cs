using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using System;

[RequireComponent(typeof(Collider2D))]
public class AttackBase : MonoBehaviour
{
    [SerializeField, Tooltip("도트 대미지를 활성화할 경우 damage는 초당 피해가 된다.")]
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
        if (canAttack && !isDotDamage) // 공격이 가능하고 도트딜이 아닐 경우
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision에 HitBase가 있고 목표 타입이며 무적 상태가 아닐 경우
            {
                hitBase.Hit(owner, damage);
                AttackSuccessEvent.Invoke(hitBase);
            }
        }
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (canAttack && isDotDamage) // 공격이 가능하고 도트딜일 경우
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision에 HitBase가 있고 목표 타입이며 무적 상태가 아닐 경우
            {
                hitBase.Hit(owner, (int)(damage * Time.fixedDeltaTime));
                AttackSuccessEvent.Invoke(hitBase);
            }
        }
    }

    public event Action<HitBase> AttackSuccessEvent = (_) => { };
}
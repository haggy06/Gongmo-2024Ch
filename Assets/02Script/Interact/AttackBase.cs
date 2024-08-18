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
    protected int damage = 1;
    public int Damage => damage;

    [Space(10)]
    [SerializeField]
    private bool infinityAttack = true;
    [SerializeField]
    private int attackableNumber = 1;
    [SerializeField]
    private bool isSkill = false;

    [SerializeField]
    protected EntityType owner = EntityType.Enemy;
    public EntityType Owner => owner;

    [SerializeField]
    protected EntityType target = EntityType.Player;
    public EntityType Target => target;

    public bool canAttack = true;

    private float cAttackCount = 0f;
    private void OnEnable()
    {
        Init();
    }
    public void Init()
    {
        cAttackCount = 0f;
        isRunOut = false;
    }

    private bool isRunOut = false;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (canAttack && !isDotDamage && !isRunOut) // 공격이 가능하고 도트딜이 아닐 경우 (+공격 횟수가 남아 있을 경우 )
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision에 HitBase가 있고 목표 타입이며 무적 상태가 아닐 경우
            {
                cAttackCount++;

                hitBase.Hit(owner, damage, isSkill);

                isRunOut = !(infinityAttack || cAttackCount < attackableNumber);
                AttackSuccessEvent.Invoke(hitBase, isRunOut);
            }
        }
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (canAttack && isDotDamage && !isRunOut) // 공격이 가능하고 도트딜일 경우 (+공격 횟수가 남아 있을 경우 )
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision에 HitBase가 있고 목표 타입이며 무적 상태가 아닐 경우
            {
                cAttackCount += Time.fixedDeltaTime;

                hitBase.Hit(owner, damage * Time.fixedDeltaTime, isSkill);

                isRunOut = !(infinityAttack || cAttackCount < attackableNumber);
                AttackSuccessEvent.Invoke(hitBase, isRunOut);
            }
        }
    }

    public event Action<HitBase, bool> AttackSuccessEvent = (_, _) => { };
}
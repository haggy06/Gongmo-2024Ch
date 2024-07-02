using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

using System;

[RequireComponent(typeof(Collider2D))]
public class AttackBase : MonoBehaviour
{
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
    [SerializeField]
    protected float attackTerm = 0.25f;

    protected void OnTriggerStay2D(Collider2D collision)
    {
        if (canAttack) // 공격이 가능할 경우
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision에 HitBase가 있고 목표 타입이며 무적 상태가 아닐 경우
            {
                hitBase.HitBy(this);
                AttackSuccessEvent.Invoke(hitBase);

                if (gameObject.activeSelf) // 오브젝트가 사라지지 않았을 시
                    StartCoroutine("AttackRecharge");
            }
        }
    }
    private IEnumerator AttackRecharge()
    {
        canAttack = false;

        yield return YieldReturn.WaitForSeconds(attackTerm);

        canAttack = true;
    }

    public event Action<HitBase> AttackSuccessEvent = (_) => { };
}
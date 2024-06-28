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

    protected virtual void Awake()
    {
        AttackSuccessEvent += AttackSuccess;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && !hitBase.Invincible ) // collision에 HitBase가 있고 무적 상태가 아닐 경우
        {
            hitBase.HitBy(this);

            AttackSuccess(hitBase);
        }
    }

    public event Action<HitBase> AttackSuccessEvent;
    protected virtual void AttackSuccess(HitBase hitBase)
    {

    }
}
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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareEntity(hitBase.EntityType, target) && !hitBase.Invincible ) // collision�� HitBase�� �ְ� ��ǥ Ÿ���̸� ���� ���°� �ƴ� ���
        {
            hitBase.HitBy(this);

            AttackSuccessEvent.Invoke(hitBase);
        }
    }

    public event Action<HitBase> AttackSuccessEvent = (_) => { };
}
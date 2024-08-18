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
        if (canAttack && !isDotDamage && !isRunOut) // ������ �����ϰ� ��Ʈ���� �ƴ� ��� (+���� Ƚ���� ���� ���� ��� )
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision�� HitBase�� �ְ� ��ǥ Ÿ���̸� ���� ���°� �ƴ� ���
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
        if (canAttack && isDotDamage && !isRunOut) // ������ �����ϰ� ��Ʈ���� ��� (+���� Ƚ���� ���� ���� ��� )
        {
            if (collision.TryGetComponent<HitBase>(out HitBase hitBase) && MyCalculator.CompareFlag((int)hitBase.EntityType, (int)target) && !hitBase.Invincible) // collision�� HitBase�� �ְ� ��ǥ Ÿ���̸� ���� ���°� �ƴ� ���
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
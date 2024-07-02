using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBase))]
public class Projectile : PoolObject
{
    [Header("Bullet Setting")]
    [SerializeField]
    protected int attackableCount = 1;
    protected int nowAttackCount = 0;
    [SerializeField]
    protected float lifeTime = 5f;
    public float LifeTime => lifeTime;
    [SerializeField]
    protected float speed = 10f;

    private AttackBase attack;
    protected virtual void Awake()
    {
        attack = GetComponent<AttackBase>();
        attack.AttackSuccessEvent += AttackSuccess;
    }
    protected virtual void AttackSuccess(HitBase hitBase)
    {
        nowAttackCount++;
        if (nowAttackCount >= attackableCount)
        {
            ReturnToPool();
        }
    }

    protected virtual void FixedUpdate()
    {
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime); // ������Ʈ ���� ���� ��(������)���� �����̵�
    }

    protected virtual void OnEnable()
    {
        attack.canAttack = true;

        nowAttackCount = 0;
        StartCoroutine("AutoReturn");
    }

    private IEnumerator AutoReturn()
    {
        yield return YieldReturn.WaitForSeconds(lifeTime);

        if (gameObject.activeSelf)
        {
            ReturnToPool();
        }
    }
}

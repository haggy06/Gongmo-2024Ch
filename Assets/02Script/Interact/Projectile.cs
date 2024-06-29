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
    [SerializeField]
    protected float speed = 10f;

    protected virtual void Awake()
    {
        AttackBase attack = GetComponent<AttackBase>();
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
        transform.Translate(Vector2.right * speed * Time.fixedDeltaTime); // 오브젝트 방향 기준 앞(오른쪽)으로 평행이동
    }

    protected virtual void OnEnable()
    {
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

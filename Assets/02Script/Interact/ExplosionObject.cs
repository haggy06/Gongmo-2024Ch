using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackBase))]
public class ExplosionObject : PoolObject
{
    [SerializeField]
    private float attackTime;
    [SerializeField]
    private float lifeTime;

    private AttackBase attack;

    protected virtual void Awake()
    {
        attack = GetComponent<AttackBase>();
    }

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        attack.canAttack = true;

        StartCoroutine("AutoReturn");
    }
    protected virtual IEnumerator AutoReturn()
    {
        yield return YieldReturn.WaitForSeconds(attackTime);

        attack.canAttack = false;

        yield return YieldReturn.WaitForSeconds(lifeTime - attackTime);

        if (gameObject.activeInHierarchy)
        {
            ReturnToPool();
        }
    }
    public override void ReturnToPool()
    {
        StopCoroutine("AutoReturn");
        base.ReturnToPool();
    }
}

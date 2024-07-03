using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : HitBase
{ 
    protected override void HalfHP()
    {
        
    }

    protected override void MoribundHP()
    {
        
    }
    protected override void DeadBy(AttackBase attack)
    {
        if (TryGetComponent<PoolObject>(out PoolObject poolObject)) // PoolObject�� ���
        {
            poolObject.ReturnToPool();
        }
        else // PoolObject�� �ƴ� ���
        {
            Destroy(gameObject);
        }
    }
}

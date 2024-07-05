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
    protected override void DeadBy(EntityType killer)
    {
        if (TryGetComponent<PoolObject>(out PoolObject poolObject)) // PoolObject일 경우
        {
            poolObject.ReturnToPool();
        }
        else // PoolObject가 아닐 경우
        {
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBase : EnemyBase
{
    protected override void ObjectReturned()
    {
        base.ObjectReturned();
        GameManager.Inst.BossDisappear();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : PoolObject // 회오리 같이 혼자 생성될 수 없는 장애물을 소환해 주는 역할
{
    [SerializeField]
    private PoolObject loadedObject;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        parentPool.GetPoolObject(loadedObject).Init(transform.position, -90f);

        ReturnToPool();
    }
}

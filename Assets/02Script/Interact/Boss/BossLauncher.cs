using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLauncher : PoolObject
{
    [SerializeField]
    private PoolObject loadedBoss;
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        PopupManager.Inst.BossAppear();
        Invoke("LaunchBoss", PopupManager.BossWarningTime);
    }

    private void LaunchBoss()
    {
        GameManager.Inst.BossAppear();
        parentPool.GetPoolObject(loadedBoss).Init(transform.position, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadProjectile : Projectile
{
    [Header("Sub Projectile Setting")]
    [SerializeField]
    protected SpreadTiming spreadTiming;
    [SerializeField]
    protected Projectile subProjectile;

    [Space(5)]
    [SerializeField, Tooltip("���� �߻�ü�� �� ����. 1 �Է� �� ����ü �¿쿡 ���� �߻�ü�� �ϳ��� ��ȯ��.")]
    protected int projPair = 1;
    [SerializeField]
    protected float angleDiff;

    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);

        if (MyCalculator.CompareFlag((int)spreadTiming, (int)SpreadTiming.Fire))
        {
            SpawnSubProj();
        }
    }
    protected override void ObjectReturned()
    {
        base.ObjectReturned();

        if (MyCalculator.CompareFlag((int)spreadTiming, (int)SpreadTiming.Dead))
        {
            SpawnSubProj();
        }
    }

    private void SpawnSubProj()
    {
        for (int i = 1; i < projPair + 1; i++) // 1 ~ projPair������ ��
        {
            PoolObject projectile;
            projectile = parentPool.GetPoolObject(subProjectile);
            projectile.Init(transform.position, transform.eulerAngles.z - angleDiff * i);

            projectile = parentPool.GetPoolObject(subProjectile);
            projectile.Init(transform.position, transform.eulerAngles.z + angleDiff * i);
        }
    }
}

[System.Flags]
public enum SpreadTiming
{
    Nothing = 1 << 0,

    Fire = 1 << 1,
    Dead = 1 << 2,

}
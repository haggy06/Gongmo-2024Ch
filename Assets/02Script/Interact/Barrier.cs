using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : HitBase
{
    [Header("Barrier Setting")]
    [SerializeField]
    private AudioClip hitSound;
    [Space(5)]
    [SerializeField]
    private AudioClip destorySound;
    [SerializeField]
    private ExplosionObject destroyExplosion;
    protected override void HalfHP()
    {
        
    }
    protected override void DeadBy(EntityType killer)
    {
        AudioManager.Inst.PlaySFX(destorySound);

        if (TryGetComponent<PoolObject>(out PoolObject poolObject)) // PoolObject일 경우
        {
            poolObject.parentPool.GetPoolObject(destroyExplosion).Init(transform.position, 0f);

            poolObject.ReturnToPool();
        }
        else // PoolObject가 아닐 경우
        {
            Destroy(gameObject);
        }
    }

    public override void Hit(EntityType victim, float damage, bool isSkill = false)
    {
        base.Hit(victim, damage, isSkill);

        AudioManager.Inst.PlaySFX(hitSound);
    }
}

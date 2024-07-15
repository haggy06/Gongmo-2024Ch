using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPustule : Coral
{
    [SerializeField]
    private ParticleSystem crackParticle;
    [SerializeField]
    private ExplosionObject pustuleExplosion;
    public override void Splint()
    {
        if (enemyInteract.Alive) // »ì¾Æ ÀÖÀ» °æ¿ì(= ¹ÝÇÇ ÀÌº¥Æ®)
        {
            print("Ç÷À¯¿¡ ±ÝÀÌ °¨");
            crackParticle.Play();
        }
        else // Á×¾úÀ» °æ¿ì
        {
            parentPool.GetPoolObject(pustuleExplosion).Init(transform.position, 0f); // Ç÷À¯ Æø¹ß

            for (int i = 0; i < splintNumber; i++) // µ¹¸æÀÌ Æ¢±è
            {
                PoolObject proj = parentPool.GetPoolObject(splinter);
                proj.Init(transform.position + Vector3.one * Random.Range(positionOffset.x, positionOffset.y), 90f + Random.Range(angleOffset.x, angleOffset.y));
                proj.GetComponent<SpriteRenderer>().color = enemyInteract.originalColor;
            }
        }
    }
}

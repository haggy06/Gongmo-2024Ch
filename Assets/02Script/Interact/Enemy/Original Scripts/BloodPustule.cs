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
        if (enemyInteract.Alive) // 살아 있을 경우(= 반피 이벤트)
        {
            print("혈유에 금이 감");
            crackParticle.Play();
        }
        else // 죽었을 경우
        {
            parentPool.GetPoolObject(pustuleExplosion).Init(transform.position, 0f); // 혈유 폭발

            if (splinter) // 파편이 있을 경우
            {
                for (int i = 0; i < splintNumber; i++)
                {
                    PoolObject proj = parentPool.GetPoolObject(splinter);
                    proj.Init(transform.position + Vector3.one * Random.Range(positionOffset.x, positionOffset.y), 90f + Random.Range(angleOffset.x, angleOffset.y));
                    proj.GetComponent<SpriteRenderer>().color = enemyInteract.OriginalColor;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPustule : Coral
{
    [SerializeField]
    private ParticleSystem crackParticle;
    public override void Splint()
    {
        if (enemyInteract.Alive) // 살아 있을 경우(= 반피 이벤트)
        {
            print("혈유에 금이 감");
            crackParticle.Play();
        }
        else // 죽었을 경우
        {
            parentPool.GetPoolObject(splinter).Init(transform.position, 0f);
        }
    }
}

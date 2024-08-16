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
        if (enemyInteract.Alive) // ��� ���� ���(= ���� �̺�Ʈ)
        {
            print("������ ���� ��");
            crackParticle.Play();
        }
        else // �׾��� ���
        {
            parentPool.GetPoolObject(pustuleExplosion).Init(transform.position, 0f); // ���� ����

            if (splinter) // ������ ���� ���
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

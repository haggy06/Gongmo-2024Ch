using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodPustule : Coral
{
    [SerializeField]
    private ParticleSystem crackParticle;
    public override void Splint()
    {
        if (enemyInteract.Alive) // ��� ���� ���(= ���� �̺�Ʈ)
        {
            print("������ ���� ��");
            crackParticle.Play();
        }
        else // �׾��� ���
        {
            parentPool.GetPoolObject(splinter).Init(transform.position, 0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaAnemone : EnemyBase
{
    [SerializeField]
    private float scrollSpeed = 4f;

    [Header("Tentacle Scratch")]
    [SerializeField]
    private Collider2D tenacleAttack;
    [SerializeField]
    private ParticleSystem scratchEffect;

    [Header("Spread Projectile")]
    [SerializeField]
    private PoolObject anemoneProjectile;
    [SerializeField]
    private int projectileNumber = 8;

    protected override void Awake()
    {
        base.Awake();
        GameManager.BossEvent += (isOn) => rigid2D.velocity = isOn ? Vector2.zero : Vector2.down * scrollSpeed; // ���� ���� �ÿ� ��ũ���� ���߹Ƿ� �����ߵ� ���߰� ��
    }
    public override void Init(Vector2 position, float angle)
    {
        base.Init(position, angle);
        rigid2D.velocity = Vector2.down * scrollSpeed;
    }

    protected override void Dead(AttackBase attack)
    {
        
    }

    protected override void HalfHP()
    {
        
    }

    protected override void MoribundHP()
    {
        
    }

    protected override void Pattern(int caseNumber, bool isListPattern = false) // �������� ����Ʈ�� �� ���� �н�
    {
        switch (caseNumber)
        {
            case 0: // �˼� ������
                if (PatternCheck.shortDistance(transform.position, 3f)) // ���� ������ ������ ���
                {
                    tenacleAttack.enabled = true;
                    scratchEffect.Play();
                }
                else // ���� ������ �������� ���
                {
                    goto ProjPattern;
                    //Pattern(1); // ��ź �߻� ����
                }
                break;

            case 1: // ��ź �߻�
            ProjPattern:
                float angleDiff = 360f / projectileNumber;
                for (int i = 0; i < projectileNumber; i++)
                {
                    PoolObject proj = parentPool.GetPoolObject(anemoneProjectile);
                    proj.Init(transform.position, 90f + (angleDiff * i));
                }
                break;
        }
    }

}